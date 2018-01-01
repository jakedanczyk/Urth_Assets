using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;
using MORPH3D;
using System.Threading;

//[System.Serializable]
public class BodyManager_Human_Player : BodyManager {

    public static GameObject playerObject;

    public BodyManager_Human_Player thisManager;
    public PlayerControls controls;
    public WorldTime worldTime;
    StaticBuildingSystem buildingSystem;
    public Weather currentWeather;
    public WeatherControl weatherSystem;
    public Transform camTransform;
    public RectTransform outfittingUI;

    public Collider rHandCollider; // headCollider, neckCollider, chestCollider, stomachCollider, pelvisCollider, upperBackCollider, lowerBackCollider,
    //                leftThighCollider, rightThighCollider, leftLowerLegCollider, rightLowerLegCollider, leftFootCollider, rightFootCollider,
    //                lShoulderCollider, rShoulderCollider, lUpperArmCollider, rUpperArmCollider, lForearmCollider, rForearmCollider, lHandCollider, rHandCollider;

    public bool meleeWeaponDrawn, rangedWeaponDrawn, weaponReadied;
    public Transform rHandTransform, lHandTransform;
    public Item_Weapon rHandWeapon;
    public Item_Weapon lHandWeapon;
    public Item_Ammo currentAmmo;
    public GameObject currentAmmoPrefab;
    public Item_Weapon_Fist fist;

    public AudioSource audioSource;
    public AudioClip swing;
    public PlayerAudio_Manager playerAudioManager;

    public Item rHandItem, lHandItem;
    //carried items
    public Inventory rHandGrasp;
    public Inventory lHandGrasp;
    public Inventory pockets;
    public Inventory[] bags;
    public M3DCharacterManager mcs, mcsUI;
    public List<Item_WaterVessel> waterVessels;
    public new PlayerInventory baseInventory;

    public bool crouching;
    public bool inTask;

    public bool gathering, treeFelling, treeProcessing;
    public String chararacterName, race;
    //[SerializeField]
    //public GameObject weatherFX;

    //attach points... special slots for weapons and equipment. each boot, small of back, weapons belt, strap, back sling, etc...
    private void Awake()
    {
        //mcs = GetComponent<M3DCharacterManager>();
    }

    public int test;

    private float sprintTime;

    void Start()
    {
        playerObject = this.gameObject;
        weatherSystem = WeatherControl.manager.GetComponent<WeatherControl>();
        currentWeather = weatherSystem.currentWeather;
        speed = 1.25F + 1.25f * gait;

        InvokeRepeating("UpdateHeartRate", 6f, 6f);
        InvokeRepeating("ReadWeather", 1.5f, 300f);
        InvokeRepeating("UpdateCoreTemp", 7f, 6f);
        InvokeRepeating("Encumbrance", 8f, 6f);
        InvokeRepeating("CalorieBurn", 9f, 6f);
        InvokeRepeating("Hydration", 10f, 6f);
        InvokeRepeating("SleepDebt", 11f, 2f);
        InvokeRepeating("StaminaUpdate", 9.5f, 6f);
        sleepMod.OnValueChange += SleepOnValueChange;
        stats.GetStat<RPGAttribute>(RPGStatType.Strength).AddModifier(sleepMod);
        stats.GetStat<RPGAttribute>(RPGStatType.Agility).AddModifier(sleepMod);
        stats.GetStat<RPGAttribute>(RPGStatType.Attunement).AddModifier(sleepMod);
        stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).AddModifier(sleepMod);
        stats.GetStat<RPGAttribute>(RPGStatType.Endurance).AddModifier(sleepMod);
        stats.GetStat<RPGAttribute>(RPGStatType.Intelligence).AddModifier(sleepMod);
        stats.GetStat<RPGAttribute>(RPGStatType.Perception).AddModifier(sleepMod);
        stats.GetStat<RPGAttribute>(RPGStatType.Willpower).AddModifier(sleepMod);
        stats.GetStat<RPGAttribute>(RPGStatType.Stamina).AddModifier(sleepMod);

        if (LevelSerializer.IsDeserializing) return;


        anim = GetComponentInChildren<Animator>();
        gait = 0;
        stamina = maxStamina = stats.GetStat(RPGStatType.Endurance).StatValue*10;
        heartRate = stats.GetStat(RPGStatType.RestingHeartRate).StatValue;
        UpdateWeatherProtection();        
        var health = stats.GetStat<RPGVital>(RPGStatType.Health);
        health.OnCurrentValueChange += OnStatValueChange;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(aimPoint.position, aimPoint.forward, out hit, 540))
        {
            Debug.DrawLine(aimPoint.position, hit.point, Color.green);
        }
        if (inTask)
        {
            controls.enabled = false;
            if (Input.GetKeyDown(KeyCode.W)) { StopTasks(); controls.enabled = true; }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionList.Add(other);
    }

    void OnStatValueChange(object sender, EventArgs args)
    {
        print("stat change");
        RPGVital vital = (RPGVital)sender;
        if (vital != null)
        {
            print(string.Format("Vital {0}'s OnStatValueChange event was triggered", vital.StatName));
            print(vital.StatCurrentValue);
        }
        if (vital.StatCurrentValue <= 0)
        {
            alive = false;
            anim.SetBool("isAlive", false);
            anim.SetBool("isDead", true);
            controls.enabled = false;
        }
    }

    public override void SheatheWeapon(Item_Weapon aWeapon)
    {
        if (!aWeapon.wielded)
            return;
        aWeapon.wielded = false;
        Debug.LogWarning("Sheathe weapon: " + aWeapon.itemName);
        aWeapon.GetComponent<Rigidbody>().isKinematic = false;
        aWeapon.GetComponent<Rigidbody>().useGravity = true;
        aWeapon.gameObject.SetActive(false);
        anim.SetBool("ArmsRaised", false);
        if (aWeapon is Item_Weapon_Bow)
        {
            wieldingBow = false;
            rangedWeaponDrawn = false;
            lHandWeapon = null;
        }
        else
        {
            meleeWeaponDrawn = false;
            rHandWeapon = null;
        }
    }

    public void DrawWeapon(Item_Weapon drawnWeapon)
    {
        if (rHandWeapon != null)
        {
            Item_Weapon droppingItem = rHandWeapon;
            SheatheWeapon(rHandWeapon);
            DropItem(droppingItem);
        }
            
        drawnWeapon.wielded = true;
        Debug.LogWarning("Draw weapon: " + drawnWeapon.itemName);
        drawnWeapon.GetComponent<Rigidbody>().isKinematic = true;
        drawnWeapon.GetComponent<Rigidbody>().useGravity = false;
        drawnWeapon.gameObject.SetActive(true);
        if (drawnWeapon is Item_Weapon_Bow)
        {
            wieldingBow = true;
            rangedWeaponDrawn = true;
            lHandWeapon = drawnWeapon;
            drawnWeapon.gameObject.transform.SetParent(lHandTransform, false);
            drawnWeapon.gameObject.transform.position = lHandTransform.position;
        }
        else
        {
            meleeWeaponDrawn = true;
            rHandWeapon = drawnWeapon;
            drawnWeapon.gameObject.transform.SetParent(rHandTransform, false);
            //drawnWeapon.gameObject.transform.position = rHandTransform.position;
            drawnWeapon.gameObject.transform.localPosition = Vector3.zero;
        }
        drawnWeapon.gameObject.transform.localRotation = Quaternion.Euler(drawnWeapon.gripOrientation);
        //drawnWeapon.gameObject.transform.localRotation.Equals(drawnWeapon.gripOrientation);
        drawnWeapon.gameObject.transform.localPosition = drawnWeapon.gripAdjust;
        anim.SetBool("ArmsRaised", true);
    }

    public void OffHandDrawWeapon(Item_Weapon drawnWeapon)
    {
        if (lHandWeapon != null)
        {
            Item_Weapon droppingItem = rHandWeapon;
            SheatheWeapon(rHandWeapon);
            DropItem(droppingItem);
        }

        drawnWeapon.wielded = true;
        Debug.LogWarning("OffHandDraw weapon: " + drawnWeapon.itemName);
        drawnWeapon.GetComponent<Rigidbody>().isKinematic = true;
        drawnWeapon.GetComponent<Rigidbody>().useGravity = false;
        drawnWeapon.gameObject.SetActive(true);
        if (drawnWeapon is Item_Weapon_Bow)
        {
            wieldingBow = true;
            rangedWeaponDrawn = true;
            lHandWeapon = drawnWeapon;
            drawnWeapon.gameObject.transform.SetParent(lHandTransform, false);
            drawnWeapon.gameObject.transform.position = lHandTransform.position;
        }
        else
        {
            meleeWeaponDrawn = true;
            lHandWeapon = drawnWeapon;
            drawnWeapon.gameObject.transform.SetParent(lHandTransform, false);
            drawnWeapon.gameObject.transform.position = lHandTransform.position;
        }
        drawnWeapon.gameObject.transform.localRotation.Equals(0);
        drawnWeapon.gameObject.transform.localPosition.Equals(0);
        drawnWeapon.gameObject.transform.Rotate(drawnWeapon.gripOrientation);
        drawnWeapon.gameObject.transform.Translate(drawnWeapon.gripAdjust);
        anim.SetBool("ArmsRaised", true);
    }

    void RaiseGuard()
    {

        if (guardRaised) { guardRaised = false; }
        else { guardRaised = true; }
    }

    void LowerGuard()
    {
        anim.SetBool("ArmsRaised", false);

    }

    void RaiseWeapons()
    {
        anim.SetBool("ArmsRaised", true);
        weaponReadied = !weaponReadied;
        //anim.RaiseWeapons
        //bodyStatus.weaponsStatus = raised
    }

    [DoNotSerialize]
    public Dictionary<WeaponType, Tuple<string,RPGStatType>> mainAttackDict = new Dictionary<WeaponType, Tuple<string, RPGStatType>>
    {
        { WeaponType.Axe_1H, new Tuple<string,RPGStatType>("AxeChop",RPGStatType.Axe)}, { WeaponType.Axe_2H, new Tuple<string,RPGStatType>("AxeChop",RPGStatType.Axe) },
        { WeaponType.Arm, new Tuple<string,RPGStatType>("RightPunch",RPGStatType.ArmStrike) }, { WeaponType.Bow, new Tuple<string,RPGStatType>("FireBow",RPGStatType.Bow) },
        { WeaponType.Pick, new Tuple<string,RPGStatType>("PickSwing",RPGStatType.Pick) }
    };
       
    public void MainAttack()
    {
        if (attacking) { return; }
        audioSource.PlayOneShot(playerAudioManager.swing);
        collisionList.Clear();
        if (rHandWeapon == null)
            Invoke("RightPunch", 0);
        else
            Invoke(mainAttackDict[rHandWeapon.weaponType].first, 0);
    }

    private IEnumerator WeaponSwing()
    {
        print("Start");
        yield return new WaitForSeconds(0.5f);
        if (collisionList.Count > 0)
        {
            print("collision");
            print(collisionList[0].transform.tag);
            int blunt = rHandWeapon.baseBlunt + rHandWeapon.itemWeight * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
            int cut = rHandWeapon.baseCut * (1 + (stats.GetStat(mainAttackDict[rHandWeapon.weaponType].second).StatValue / 10));
            int pierce = rHandWeapon.basePierce * (1 + (stats.GetStat(mainAttackDict[rHandWeapon.weaponType].second).StatValue / 10));

            if (collisionList[0].transform.root.tag == "Tree")
            {
                print(collisionList[0].transform.root.GetComponent<Tree>().health);
                collisionList[0].transform.root.GetComponent<Tree>().health -= (cut * blunt + 10);
                print(collisionList[0].transform.root.GetComponent<Tree>().health);
            }
            else if (collisionList[0].transform.tag == "BodyPart")
            {
                print(1);

                BodyPartColliderScript bp = collisionList[0].GetComponent<BodyPartColliderScript>();
                int[] z = new int[] { 0, 0, 0 }; int[] o = new int[] { blunt, cut, pierce };
                z = bp.parentBody.SendArmorNumbers(bp.bodyPartType);
                bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((pierce / z[0]) + (blunt / z[1]));
                print(2);
                print(o[0] + ","+ o[1] + "," + o[2]);
                print(0);
                AttackResolution(bp, bp.parentBody.stats, stats.GetStat<RPGSkill>(mainAttackDict[rHandWeapon.weaponType].second), z, o);
                print(4);
            }
        }
        print("turning off trigger");
        rHandWeapon.collList[0].isTrigger = false;
        rHandCollider.isTrigger = false;
    }

    void AttackResolution(BodyPartColliderScript bp, CreatureStats targetStats, RPGSkill attackSkill, int[] d, int[] o)
    {
        float dodgeChance = 0.1f * (targetStats.GetStat(RPGStatType.Dodge).StatValue * 3 + (targetStats.GetStat(RPGStatType.Agility).StatValue)) / (1 + 3 * bp.parentBody.encumbrance);
        float deflectChance = 0.1f * targetStats.GetStat(RPGStatType.Deflect).StatValue * 3 + targetStats.GetStat(RPGStatType.Agility).StatValue + targetStats.GetStat(RPGStatType.Strength).StatValue + d[2] * targetStats.GetStat(RPGStatType.Armor).StatValue;
        float absorbChance = 0.1f * targetStats.GetStat(RPGStatType.Absorb).StatValue * 3 + targetStats.GetStat(RPGStatType.Agility).StatValue + 2 * targetStats.GetStat(RPGStatType.Strength).StatValue + d[2] * targetStats.GetStat(RPGStatType.Armor).StatValue + Mathf.Log(targetStats.GetStat(RPGStatType.Weight).StatValue);
        float hitChance = 10f * attackSkill.StatValue * 5 + stats.GetStat(RPGStatType.Agility).StatValue + stats.GetStat(RPGStatType.Strength).StatValue;
        float criticalChance = 0.01f * attackSkill.StatValue * attackSkill.StatValue + stats.GetStat(RPGStatType.Agility).StatValue + stats.GetStat(RPGStatType.Strength).StatValue;
        float sum = dodgeChance + deflectChance + absorbChance + hitChance + criticalChance;
        //dodgeChance = dodgeChance / sum;
        //deflectChance = deflectChance / sum;
        //absorbChance = absorbChance / sum;
        //hitChance = hitChance / sum;
        //criticalChance = criticalChance / sum;
        float roll = UnityEngine.Random.Range(0, sum);
        print(roll + " / " + sum);
        print(dodgeChance + " , " + deflectChance + " , " + absorbChance + " , " + hitChance);

        if (roll < dodgeChance) { bp.parentBody.Dodge(); print("dodge"); return; }
        else if (roll < (dodgeChance + deflectChance)) { bp.parentBody.Deflect(); print("deflect"); return; }
        else if (roll < (dodgeChance + deflectChance + absorbChance)) { bp.parentBody.Absorb(); print("absorb"); return; }
        else if (roll < (dodgeChance + deflectChance + absorbChance + hitChance))
        {
            print(bp.name);
            print("hit");
            //bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((o[0] / d[0]) + ( o[1] / d[1]) + (o[2] / d[2]));
            bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= (int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2])));
            print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
            print((int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2]))));
            attackSkill.GainXP(10);
            return;
        }
        else
        {
            print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
            bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= 4 * (int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2])));
        }
        print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
        collisionList.Clear();
        //defense: defense skills, armor, encumbrance, stamina, energy, agility, strength, toughness
    }

    void RightPunch()
    {
        print("punch");
        rHandWeapon = fist;
        rHandCollider.isTrigger = true;
        anim.SetTrigger("AxeChop"); //punch trigger
        StartCoroutine(WeaponSwing());
    }

    void AxeChop()
    {
        print("chop");

        rHandWeapon.collList[0].isTrigger = true;
        anim.SetTrigger("AxeChop");
        StartCoroutine(WeaponSwing());
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo.length);
    }

    void PickSwing()
    {
        rHandWeapon.collList[0].isTrigger = true;
        anim.SetTrigger("PickSwing");
        RaycastHit hit;
        if (Physics.Raycast(aimPoint.position, aimPoint.forward, out hit, 240f))
        {
            print(hit.transform.tag);

            if (hit.transform.gameObject.layer == 19)
            {
                stats.GetStat<RPGSkill>(RPGStatType.Mining).GainXP(10);
                stats.GetStat<RPGAttribute>(RPGStatType.Strength).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Toughness).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Endurance).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).TrainingValue += 10;
                int pierce = rHandWeapon.basePierce * (1 + (stats.GetStat(RPGStatType.Pick).StatValue / 10));
                int blunt = (rHandWeapon.baseBlunt * 5 + rHandWeapon.itemWeight) * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
                Block block = EditTerrain.GetBlock(hit);
                if (block is BlockAir)
                    print("air");
                print(EditTerrain.GetBlockPos(hit).x + "," + EditTerrain.GetBlockPos(hit).y + "," + EditTerrain.GetBlockPos(hit).z);
                if (block is BlockGrass)
                {
                    EditTerrain.HitBlock(hit, 100*(pierce + (blunt / 2)));
                    print(pierce + (blunt / 2));
                    return;
                }

                else if (block is Block)
                {
                    audioSource.PlayOneShot(playerAudioManager.metalOnStone);
                    EditTerrain.HitBlock(hit, pierce * blunt);
                    print(pierce * blunt);
                }
            }
            else if (hit.collider.transform.tag == "BodyPart")
            {
                print(0);
                int pierce = rHandWeapon.basePierce * (1 + (stats.GetStat(RPGStatType.Pick).StatValue / 10));
                int blunt = (rHandWeapon.baseBlunt * 5 + rHandWeapon.itemWeight) * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
                print(1);

                BodyPartColliderScript bp = hit.collider.GetComponent<BodyPartColliderScript>();
                int[] z = new int[] { 0, 0, 0 }; int[] o = new int[] { 0, blunt, pierce };
                z = bp.parentBody.SendArmorNumbers(bp.bodyPartType);
                bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((pierce / z[0]) + (blunt / z[1]));
                print(2);

                AttackResolution(bp, bp.parentBody.stats, stats.GetStat<RPGSkill>(RPGStatType.Pick) , z, o);
                print(4);

            }
        }
    }

  



    //bow and arrow stuff
    public bool wieldingBow, arrowKnocked, drawingBow, bowDrawn;
    public float startTime, nowTime;
    public float drawTime = 3f;
    public void KnockArrow()
    {
        if(!drawingBow && !(currentAmmo == null))
        arrowKnocked = true;
    }
    public void DrawBow()
    {
        anim.SetBool("DrawingBow", true);
        drawingBow = true;
        StartCoroutine(BowPull());
        startTime = Time.time;
    }
    private IEnumerator BowPull()
    {
        print("Start");
        yield return new WaitForSeconds(3);
        print("done");
        if(drawingBow)
        bowDrawn = true;
    }
    public void ReleaseBow()
    {
        anim.SetBool("DrawingBow", false);
        drawingBow = false;
        bowDrawn = false;

        //StartCoroutine(BowRelease());
    }
    private IEnumerator BowRelease()
    {
        print("Start");
        yield return new WaitForSeconds(1f);
        print("done");
        bowDrawn = false;
    }

    public Rigidbody arrowRigid;
    public void FireBow()
    {
        if (!arrowKnocked)
        {
            KnockArrow();
            return;
        }

        if (drawingBow && bowDrawn && arrowKnocked)
        {
            bowDrawn = false;
            drawingBow = arrowKnocked = false;
            anim.SetTrigger("FireBow");
            anim.SetBool("DrawingBow", false);
            GameObject clone;
            clone = Instantiate(currentAmmoPrefab, aimPoint) as GameObject;
            Item_Ammo_Arrow thisArrow = clone.GetComponent<Item_Ammo_Arrow>();
            clone.SetActive(true);
            clone.transform.parent = null;
            clone.transform.position = new Vector3(aimPoint.position.x + .14f, aimPoint.position.y - 0.04f, aimPoint.position.z + .177f);
            clone.GetComponent<Rigidbody>().AddForce(aimPoint.TransformDirection(Vector3.forward * 2000));
            //clone.GetComponent<Rigidbody>().velocity = aimPoint.TransformDirection(Vector3.forward * 50);
            thisArrow.projectile.shooter = thisManager;
            thisArrow.attackCollider.isTrigger = true;
            thisArrow.numItems = 1;
            currentAmmo.ChangeNum(-1);
            return;
        }
        //if (drawingBow && arrowKnocked)
        //{
        //    bowDrawn = false;
        //    drawingBow = arrowKnocked = false;
        //    nowTime = Time.time;
        //    float speedFraction = (nowTime - startTime) / drawTime;
        //    anim.SetTrigger("FireBow");
        //    anim.SetBool("DrawingBow", false);
        //    GameObject clone;
        //    clone = Instantiate(currentAmmo.GetComponent<Item_Ammo>().itemPrefab, aimPoint) as GameObject;
        //    clone.transform.localPosition = Vector3.zero;
        //    clone.transform.parent = null;
        //    clone.GetComponent<Rigidbody>().velocity = aimPoint.TransformDirection(Vector3.forward * 100 * speedFraction);
        //    clone.GetComponent<Item_Ammo>().numItems = 1;
        //    currentAmmo.GetComponent<Item_Ammo>().ChangeNum(-1);
        //    return;
        //}
    }

    public void PlaceInBag(Item_Weapon weapon, Inventory bag) { }


    public void EquipWearable(Item_Garment newWearable)
    {
        if (!newWearable.equipped)
        {
            outfit.Add(newWearable);
            newWearable.equipped = true;

            foreach (RPGStatType partCovered in newWearable.bodyPartCoverage)
            {
                RPGBodyPart bodyPart = stats.GetStat<RPGBodyPart>(partCovered);
                int[] myArr = new int[6];
                for (int i = 0; i < 5; i++)
                {
                    bodyPart.protection[i] = newWearable.protection[i] + stats.GetStat<RPGBodyPart>(partCovered).protection[i];
                }
            }
            UpdateWeatherProtection();
            if (newWearable.hasModel)
            {
                newWearable.material.SetTexture("_MainTex", newWearable.texture);
                mcs.SetClothingVisibility(newWearable.modelID, true);
                mcsUI.SetClothingVisibility(newWearable.modelID, true);
            }
        }
    }

    public override void RemoveGarment(Item_Garment thisWearable)
    {
        if (!thisWearable.equipped)
            return;
        foreach (RPGStatType partCovered in thisWearable.bodyPartCoverage)
        {
            RPGBodyPart bodyPart = stats.GetStat<RPGBodyPart>(partCovered);
            int[] myArr = new int[6];
            for (int i = 0; i < 5; i++)
            {
                bodyPart.protection[i] = bodyPart.protection[i] - thisWearable.protection[i];
            }
        }
        if (thisWearable.hasModel)
        {
            mcs.SetClothingVisibility(thisWearable.modelID, false);
            mcsUI.SetClothingVisibility(thisWearable.modelID, false);
        }
        outfit.Remove(thisWearable);
        thisWearable.equipped = false;
    }

    public int[] SendArmorNumbers(RPGStatType bodyPart)
    {
        int b = 0, c = 0, p = 0;
        int[] z = new int[] { 0, 0, 0 };
        z[0] = stats.GetStat<RPGBodyPart>(bodyPart).protection[0];
        z[1] = stats.GetStat<RPGBodyPart>(bodyPart).protection[1];
        z[2] = stats.GetStat<RPGBodyPart>(bodyPart).protection[2];
        return z;
    }

    public int[] SendArmorNumbersID(int targetColliderID)
    {
        int b = 0, c = 0, p = 0;
        int[] z = new int[] { 0, 0, 0 };
        switch (targetColliderID)
        {
            case 0:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.Head).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.Head).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.Head).protection[2];
                    break; }
            case 1:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.Neck).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.Neck).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.Neck).protection[2];
                    break; }
            case 2:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.Chest).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.Chest).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.Chest).protection[2];
                    break; }
            case 3:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).protection[2];
                    break;
                }
            case 4:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).protection[2];
                    break;
                }
            case 5:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).protection[2];
                    break;
                }
            case 6:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).protection[2];
                    break;
                }
            case 7:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).protection[2];
                    break;
                }
            case 8:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).protection[2];
                    break;
                }
            case 9:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.LeftUpperArm).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.LeftUpperArm).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.LeftUpperArm).protection[2];
                    break;
                }
            case 10:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.RightUpperArm).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.RightUpperArm).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.RightUpperArm).protection[2];
                    break;
                }
            case 11:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).protection[2];
                    break;
                }
            case 12:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).protection[2];
                    break;
                }
            case 13:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).protection[2];
                    break;
                }
            case 14:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).protection[2];
                    break;
                }
            case 15:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).protection[2];
                    break;
                }
            case 16:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).protection[2];
                    break;
                }
            case 17:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).protection[2];
                    break;
                }
            case 18:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).protection[2];
                    break;
                }
            case 19:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).protection[2];
                    break;
                }
            case 20:
                {
                    b = stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).protection[0];
                    c = stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).protection[1];
                    p = stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).protection[2];
                    break;
                }
            default: { break; }
        }
        z[0] = b;
        z[1] = c;
        z[2] = p;
        return z;
    }

    public float heartRate;

    public bool moving, sprinting;
    public int gait;
    public int[] gaits = new int[] { 0, 1, 2, 3 }; //0=slow walk, 1=walk, 2=jog, 3=run
    public float speed;

    public float SprintTime
    {
        get { return sprintTime; }
        set { sprintTime = value; }
    }

    public void NextGait()
    {
        if (gait < 3) { gait++; }
        else gait = 0;
        speed = 1.25F + 1.25F * gait;
    }

    float lastHeartCheckTime = 0;
    void UpdateHeartRate()
    {
        test = stats.GetStat(RPGStatType.RestingHeartRate).StatValue;
        float target;
        target = stats.GetStat(RPGStatType.RestingHeartRate).StatValue + (moving ? 0 : (gait * 30 * (crouching ? 1:1.5f))) + (.001f * (maxStamina - stamina)) + (shivering ? 30 : 0);
        target += encumbrance * 60;
        float diff = target - heartRate;
        float now = worldTime.totalGameSeconds;
        heartRate = Math.Min(heartRate + (now - lastHeartCheckTime) / 6 * ((.0005f * diff * diff) + (.01f * diff)),200);
        heartRate = Math.Max(10, heartRate);
        lastHeartCheckTime = 0;
    }

    public float coreTempBase = 37f;
    public Thermometer thermometer;
    public ShelterCheck shelterCheck;
    public float localTemperature = 0;
    public float precipRate = 0;
    public float windSpeed = 0;
    public float humidity = 0;
    public float coreTemp = 37f;

    public float totalInsulation = 0;
    public float totalWaterCover = 0;
    public float totalWindCover = 0;
    [SerializeField]
    float lastTempCheckTime = 0;

    void UpdateCoreTemp()
    {
        ReadWeather();
        float heatProduction = 0.005f * stats.GetStat(RPGStatType.Weight).StatValue * heartRate * heartRate / (stats.GetStat(RPGStatType.RestingHeartRate).StatValue * stats.GetStat(RPGStatType.RestingHeartRate).StatValue);
        float heatLoss = ((coreTemp - localTemperature) * (.1f+humidity) + localTemperature * localTemperature * humidity) * stats.GetStat(RPGStatType.Height).StatValue / totalInsulation;
        //print(coreTemp + ", " + localTemperature + ", " + heartRate);
        //float coolingFactor = coreTemp * (0.000001f * (coreTemp - localTemperature + 10 - (5*heartRate/60)));
        //float heatingFactor = coreTemp * (0.000001f * (coreTemp - localTemperature + 5 - (5 * heartRate / 60)));
        //if (localTemperature < 15)
        //{
        //    coreTemp = coreTemp - (coolingFactor / (1 + totalInsulation));
        //}

        //if (localTemperature > 32)
        //{
        //    coreTemp = (coreTemp - coreTempBase) + (heatingFactor * (1 + totalInsulation));
        //}
        float now = worldTime.totalGameSeconds;
        coreTemp += (now - lastTempCheckTime) / 6 * (heatProduction - heatLoss) / stats.GetStat(RPGStatType.Weight).StatValue;

        if(coreTemp > (coreTempBase+1))
        {
            Sweat((now - lastTempCheckTime)/6);
        }
        lastTempCheckTime = now;
        if (coreTemp < (coreTempBase - 1))
        {
            shivering = true;
        }
        else shivering = false;
    }



    void UpdateWeatherProtection()
    {
        totalWaterCover = (stats.GetStat<RPGBodyPart>(RPGStatType.Head).protection[4] * 5 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.Neck).protection[4] +
                                stats.GetStat<RPGBodyPart>(RPGStatType.Chest).protection[4] * 3 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).protection[4] * 3 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).protection[4] * 5 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).protection[4] * 3 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).protection[4] * 2 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).protection[4] * 2 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).protection[4] * 2 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).protection[4] +
                                stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).protection[4] +
                                stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).protection[4] +
                                stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).protection[4] +
                                stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).protection[4] * 3 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).protection[4] * 3 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).protection[4] +
                                stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).protection[4] +
                                stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).protection[4] * 2 +
                                stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).protection[4] * 2);

        totalInsulation = (stats.GetStat<RPGBodyPart>(RPGStatType.Head).protection[3] * 5 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.Neck).protection[3] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.Chest).protection[3] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).protection[3] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).protection[3] * 5 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).protection[3] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).protection[3] * 2 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).protection[3] * 2 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).protection[3] * 2 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).protection[3] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).protection[3] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).protection[3] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).protection[3] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).protection[3] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).protection[3] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).protection[3] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).protection[3] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).protection[3] * 2 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).protection[3] * 2);

        totalWindCover = (stats.GetStat<RPGBodyPart>(RPGStatType.Head).protection[5] * 5 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.Neck).protection[5] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.Chest).protection[5] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).protection[5] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).protection[5] * 5 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).protection[5] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).protection[5] * 2 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).protection[5] * 2 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).protection[5] * 2 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).protection[5] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).protection[5] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).protection[5] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).protection[5] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).protection[5] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).protection[5] * 3 +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).protection[5] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).protection[5] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).protection[5] +
                        stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).protection[5]);
        ReadWeather();
        humidity = Mathf.Clamp(humidity + precipRate / totalWaterCover, 0,1);
    }

    int printPos = 375;
    //void OnGUI()
    //{

    //    GUI.Label(new Rect(0, 400, 500, 500), "Core Temp: " + coreTemp);
    //    GUI.Label(new Rect(0, 425, 500, 500), "Local Temp: " + localTemperature);
    //    GUI.Label(new Rect(0, 450, 500, 500), "Heart Rate: " + heartRate);
    //    GUI.Label(new Rect(0, 475, 500, 500), "Hydration: " + hydration + "%");
    //    GUI.Label(new Rect(0, 500, 500, 500), "Sleep Debt: " + sleepDebt + " hrs");
    //    GUI.Label(new Rect(0, 525, 500, 500), "Calories: " + calories);
    //    GUI.Label(new Rect(0, 550, 500, 500), "Encumbrance: " + encumbrance + "%");
    //    GUI.Label(new Rect(0, 575, 500, 500), "Stamina Max: " + maxStamina);
    //    GUI.Label(new Rect(0, 600, 500, 500), "Stamina: " + stamina);

    //    for (int j = 0; j < outfit.Count; j++)
    //    {
    //        GUI.Label(new Rect(1100, printPos, 500, 500), outfit[j].itemName);
    //        printPos = printPos + 25;
    //    }
    //}

    public void ReadWeather()
    {
        float altitude = this.transform.position.y;
        localTemperature = thermometer.temperature;
        if (shelterCheck.roof) { precipRate = 0; }
        else { precipRate = weatherSystem.currentWeather.precipRate; }
        if (shelterCheck.walls) { windSpeed = 0; }
        else { windSpeed = weatherSystem.currentWeather.windSpeed; }
    }

    //public float encumbrance = 0; //percent 
    //public float carryWeight;
    //void Encumbrance()
    //{
    //    encumbrance = ((outfit.Select(c => c.itemWeight).ToList().Sum()*.25f) + baseInventory.SumWeight())/stats.GetStat<RPGAttribute>(RPGStatType.CarryWeight).StatValue;
    //    carryWeight = stats.GetStat<RPGAttribute>(RPGStatType.CarryWeight).StatValue;
    //}

    public float calories = 2000f;
    public float calorieBurn = 0;
    [SerializeField]
    float calorieTime,hydrationTime,sleepTime;
    void CalorieBurn()
    {
        float now = worldTime.totalGameSeconds;
        calorieBurn = stats.GetStat(RPGStatType.Weight).StatValue * (Math.Max(heartRate-50, 0) * .00000068481f + 0.000001175f) * (now - calorieTime)/6; // TODO
        calorieTime = now;
        calories -= calorieBurn; 
    }

    public bool shivering;
    [SerializeField]
    float waterContent;
    public float hydration = 66f; // body water %
    void Hydration()
    {
        hydration -= calorieBurn * .000042f * Math.Max(localTemperature*.1f, 4);
    }

    void Sweat(float amount)
    {
        hydration -= .001f * amount;
    }

    public float sleepDebt = 1f; // hours sleep debt
    [SerializeField]
    RPGStatModTotalPercent sleepMod = new RPGStatModTotalPercent(0);
    void SleepDebt()
    {
        float now = worldTime.totalGameSeconds;
        sleepDebt += ((now - sleepTime) - (3*sleepLength))/ 7200; // .01 if invoke period is 72
        sleepTime = now;
        float sleepModValue = Mathf.Max(-1,(Mathf.Min(0,(16 - sleepDebt)) + Mathf.Min(0, (24 - sleepDebt)) * sleepDebt)/400f);
        sleepMod.Value = sleepModValue < 100 ? sleepModValue : 100;
    }

    private void SleepOnValueChange(object mod, System.EventArgs args)
    {
        //Debug.Log("On Value Changed Called For Modifier: " + mod.GetType().ToString());
        stats.GetStat<RPGAttribute>(RPGStatType.Strength).UpdateModifiers();
        stats.GetStat<RPGAttribute>(RPGStatType.Agility).UpdateModifiers();
        stats.GetStat<RPGAttribute>(RPGStatType.Attunement).UpdateModifiers();
        stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).UpdateModifiers();
        stats.GetStat<RPGAttribute>(RPGStatType.Endurance).UpdateModifiers();
        stats.GetStat<RPGAttribute>(RPGStatType.Intelligence).UpdateModifiers();
        stats.GetStat<RPGAttribute>(RPGStatType.Perception).UpdateModifiers();
        stats.GetStat<RPGAttribute>(RPGStatType.Willpower).UpdateModifiers();
        stats.GetStat<RPGAttribute>(RPGStatType.Stamina).UpdateModifiers();
    }
    public float oxygenConsumptionRate = 0; // ml per min
    public float oxygenSupplyRate = 0;
    public float stamina,maxStamina;

    void StaminaUpdate()
    {
        oxygenConsumptionRate = (3.5f + gait*10) * stats.GetStat(RPGStatType.Weight).StatValue * (1 + 20 * encumbrance * encumbrance);
        oxygenSupplyRate = .00003f * heartRate * stats.GetStat(RPGStatType.Endurance).StatValue * stats.GetStat(RPGStatType.Weight).StatValue * Math.Max((heartRate - stats.GetStat(RPGStatType.RestingHeartRate).StatValue),1);
        stamina = stamina + ((oxygenSupplyRate - oxygenConsumptionRate) * .0001f);
        if (stamina > maxStamina )
        {
            stamina = maxStamina;
        }
        if (stamina < 0 )
        {
            stamina = 0;
        }
    }


    //Tasks
    private IEnumerator coroutine;
    public float taskStartTime, taskTimeNeeded;

    public float treeDamage;
    public Tree targetTree;
    public void FellTree(Tree aTree, Item_Weapon_Axe anAxe)
    {
        targetTree = aTree;
        treeDamage = 0;
        taskStartTime = worldTime.totalGameSeconds;
        treeFelling = inTask = true;
        anim.SetBool("inTask", true);
        anim.SetBool("isFallingTree", true);
        print("falling");
        float chopRate = (anAxe.baseBlunt + anAxe.baseCut + anAxe.woodChopBonus) * stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatValue * stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).StatValue;
        print(chopRate.ToString());
        coroutine = FellTree(aTree, chopRate);
        StartCoroutine(coroutine);
    }
    
    private IEnumerator FellTree(Tree aTree, float chopRate)
    {
        print("Start");
        while (true)
        {
            print("stage2");
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            treeDamage = chopRate * workTime;
            if (treeDamage >= aTree.health)
            {
                aTree.Fall();
                stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).GainXP((int)(treeDamage));
                stats.GetStat<RPGAttribute>(RPGStatType.Toughness).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Strength).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Endurance).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).TrainingValue += 10;
                treeFelling = inTask =  false;
                anim.SetBool("inTask", false);
                anim.SetBool("isFallingTree", false);
                print("Tree felled in " + workTime / 60 + " minutes.");
                print(treeDamage + "woodworking xp gained");
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator ProcessTree(Tree aTree, float chopRate)
    {
        print("Start chopping downed tree");
        while (true)
        {
            print("stage2");
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            treeDamage = chopRate * workTime;
            if (treeDamage >= aTree.downedHealth)
            {
                aTree.TurnToWoodPile();
                stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).GainXP((int)(treeDamage));
                stats.GetStat<RPGAttribute>(RPGStatType.Toughness).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Strength).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Endurance).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).TrainingValue += 10;
                treeProcessing = inTask = false;
                anim.SetBool("inTask", false);
                anim.SetBool("isFallingTree", false);
                print("Tree felled in " + workTime / 60 + " minutes.");
                print(treeDamage + "woodworking xp gained");
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void ProcessDownedTree(Tree aTree, Item_Weapon_Axe anAxe)
    {
        if (!aTree.standing)
        {
            targetTree = aTree;
            treeDamage = 0;
            taskStartTime = worldTime.totalGameSeconds;
            treeProcessing = inTask = true;
            anim.SetBool("inTask", true);
            anim.SetBool("isFallingTree", true);
            print("harvesting");
            float chopRate = (anAxe.baseBlunt + anAxe.baseCut + anAxe.woodChopBonus) * stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatValue * stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).StatValue;
            print(chopRate.ToString());
            coroutine = ProcessTree(aTree, chopRate);
            StartCoroutine(coroutine);
        }
    }

    //fire making

    public float fireStartingTime;
    [SerializeField]
    float xpGain;
    [SerializeField]
    bool isStartingFire;
    public void StartFire(Fire aFire, float methodFactor)
    {
        isStartingFire = true;
        xpGain = 0;
        inTask = true;
        anim.SetBool("inTask", true);
        anim.SetBool("isStartingFire", true);
        taskStartTime = worldTime.totalGameSeconds;
        fireStartingTime = (weatherSystem.precipRate * methodFactor + methodFactor + 1) / (stats.GetStat<RPGSkill>(RPGStatType.FireMaking).StatValue + localTemperature * .1f);
        print(fireStartingTime);
        coroutine = StartFire(aFire);
        StartCoroutine(coroutine);
    }

    // every 15 seconds try again to start fire
    private IEnumerator StartFire(Fire aFire)
    {
        while (true && !aFire.isLit)
        {
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            xpGain = workTime;
            stats.GetStat<RPGSkill>(RPGStatType.FireMaking).GainXP(10);
            if (workTime > fireStartingTime)
            {
                print("fire lit");
                aFire.LightFire();
                stats.GetStat<RPGSkill>(RPGStatType.FireMaking).GainXP(10 + (int)xpGain);
                isStartingFire = false;
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    //gathering
    public float totalGathered;
    public Perennial targetPlant;
    public void GatherPlant(Perennial plant)
    {
        print(2);

        targetPlant = plant;
        gathering = true;
        totalGathered = 0;
        taskStartTime = worldTime.totalGameSeconds;
        float rate = plant.gatherRate * (1 + stats.GetStat<RPGSkill>(RPGStatType.Gathering).StatValue * .09f) * (stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).StatValue * .005f);
        float timeNeeded = plant.numFruits / rate;
        print(rate); print(timeNeeded);
        anim.SetBool("isGathering", true);

        print("Start Gather: " + totalGathered + " gathered(should be 0)");

        coroutine = Gather(plant, rate);
        StartCoroutine(coroutine);
    }

    // every 15 seconds try again to start fire
    private IEnumerator Gather(Perennial plant, float rate)
    {
        while (true)
        {
            print(3);
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            totalGathered = rate * workTime;
            if (totalGathered >= plant.numFruits)
            {
                totalGathered = (int)Math.Floor(totalGathered);
                if (plant.fruitPrefab.GetComponent<Item_Stack>())
                {
                    GameObject harvest = Instantiate(plant.fruitPrefab);
                    harvest.GetComponent<Item_Stack>().numItems = (int)totalGathered;
                    baseInventory.AddItem(harvest.GetComponent<Item_Stack>());
                }
                else if (plant.fruitPrefab.GetComponent<Item>())
                {
                    for (int i = 0; i < totalGathered; i++)
                    {
                        GameObject harvest = Instantiate(plant.fruitPrefab);
                        baseInventory.AddItem(harvest.GetComponent<Item>());
                    }
                }
                stats.GetStat<RPGSkill>(RPGStatType.Gathering).GainXP((int)(plant.difficulty * totalGathered * 1.1));
                plant.SetHarvested();
                gathering = false;
                print(totalGathered + " " + plant.fruit.itemName + " gathered in " + workTime / 60 + " minutes.");
                print(plant.difficulty * totalGathered + " xp gained");
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    //butchering
    public void ButcherBody(BodyManager body)
    {
        taskStartTime = worldTime.totalGameSeconds;
        taskTimeNeeded = body.butcherTime * (1 + UnityEngine.Random.Range(0, .25f)) / (stats.GetStat(RPGStatType.Butchery).StatValue + rHandWeapon.butcheringAid);
        coroutine = Butcher(body);
        StartCoroutine(coroutine);
    }

    private IEnumerator Butcher(BodyManager body)
    {
        while (true)
        {
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            if (workTime >= taskTimeNeeded)
            {
                for(int i = 0; i < body.butcheringReturns.Count; i++)
                {
                    body.inventory.AddItem(body.butcheringReturns[i].GetComponent<Item>());
                    body.butchered = true;
                    print("finished butchering");
                }
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void Eat(Item_Food foodItem)
    {
        hydration += foodItem.water / stats.GetStat<RPGAttribute>(RPGStatType.Weight).StatValue;
        calories += foodItem.calories;
        if (!foodItem.loose)
            foodItem.itemUIElementScript.parentInventory.DropItem(foodItem);
        Destroy(foodItem.itemUIelement);
        Destroy(foodItem.gameObject);
    }

    public void Drink()
    {

    }

    bool isSleeping;
    float sleepStartTime, sleepLength;
    public void Sleep()
    {
        anim.SetBool("isLayingDown", true);
        print("sleep");
        sleepStartTime = worldTime.totalGameSeconds;
        inTask = true;
        isSleeping = true;

    }

    public void StopSleep()
    {
        anim.SetBool("isLayingDown", false);
        sleepLength = worldTime.totalGameSeconds - sleepStartTime;
        StopTasks();
        controls.enabled = true;
        isSleeping = false;
    }

    public override void ProcessThisBody()
    {
        throw new NotImplementedException();
    }

    public void StopTasks()
    {
        anim.SetBool("inTask", false);
        print("Stop");
        inTask = false;
        if (gathering)
        {
            if (targetPlant.fruitPrefab.GetComponent<Item_Stack>())
            {
                GameObject harvest = Instantiate(targetPlant.fruitPrefab);
                harvest.GetComponent<Item_Stack>().numItems = (int)Math.Floor(totalGathered);
                baseInventory.AddItem(harvest.GetComponent<Item_Stack>());
            }
            else if (targetPlant.fruitPrefab.GetComponent<Item>())
            {
                totalGathered = (int)Math.Floor(totalGathered);
                for (int i = 0; i < totalGathered; i++)
                {
                    GameObject harvest = Instantiate(targetPlant.fruitPrefab);
                    baseInventory.AddItem(harvest.GetComponent<Item>());
                }
            }
            stats.GetStat<RPGSkill>(RPGStatType.Gathering).GainXP((int)(targetPlant.difficulty * totalGathered));
            targetPlant.numFruits -= (int)totalGathered;
            gathering = false;
            anim.SetBool("isGathering", false);
            return;
        }
        if (treeFelling)
        {
            targetTree.TakeDamage(treeDamage);
            treeFelling = inTask =  false;
            stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).GainXP((int)(treeDamage));
            anim.SetBool("inTask", false);
            anim.SetBool("isFallingTree", false);
            print(treeDamage + "woodworking xp gained");
            return;
        }
        if (treeProcessing)
        {
            targetTree.TakeDamage2(treeDamage);
            treeProcessing = inTask = false;
            stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).GainXP((int)(treeDamage));
            anim.SetBool("inTask", false);
            anim.SetBool("isFallingTree", false);
            print(treeDamage + "woodworking xp gained");
            return;
        }
        if (isStartingFire)
        {
            stats.GetStat<RPGSkill>(RPGStatType.FireMaking).GainXP((int)xpGain);
            isStartingFire = false;
            anim.SetBool("isStartingFire", false);
            return;
        }
        if (isSleeping)
        {
            isSleeping = false;
            anim.SetBool("isLayingDown", false);
            sleepLength = worldTime.totalGameSeconds - sleepStartTime;
            return;
        }
        StopAllCoroutines();
    }
}