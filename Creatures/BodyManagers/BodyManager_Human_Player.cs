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
    public HUDManager hud;

    public BodyManager_Human_Player thisManager;
    public PlayerControls controls;
    public WorldTime worldTime;
    StaticBuildingSystem buildingSystem;
    public WeatherControl weatherSystem;
    public Transform camTransform;
    public RectTransform outfittingUI;

    public Collider rHandCollider, lHandCollider; // headCollider, neckCollider, chestCollider, stomachCollider, pelvisCollider, upperBackCollider, lowerBackCollider,
    //                leftThighCollider, rightThighCollider, leftLowerLegCollider, rightLowerLegCollider, leftFootCollider, rightFootCollider,
    //                lShoulderCollider, rShoulderCollider, lUpperArmCollider, rUpperArmCollider, lForearmCollider, rForearmCollider, lHandCollider, rHandCollider;

    public bool meleeWeaponDrawn, rangedWeaponDrawn, weaponsReadied;
    public Transform primaryHandTransform, offHandTransform;
    public Item_Weapon_Fist rightFist, leftFist;
    public Item_Weapon offHandWeapon;
    public Item_Ammo currentAmmo;
    public GameObject currentAmmoPrefab;
    public Item_Weapon_Fist fist;

    public AudioClip swing;

    public Item rHandItem, lHandItem;
    //carried items
    public Inventory rHandGrasp;
    public Inventory lHandGrasp;
    public Inventory pockets;
    public Inventory[] bags;
    public M3DCharacterManager mcs, mcsUI;
    public List<Item_WaterVessel> waterVessels;
    public PlayerInventory baseInventory;

    public bool crouching;
    public bool inTask;

    public bool gathering, treeFelling, treeProcessing;
    public String chararacterName, race;
    MessageLog messageLog;
    IEnumerator weaponSwing,teethChatter;
    //[SerializeField]
    //public GameObject weatherFX;

    //attach points... special slots for weapons and equipment. each boot, small of back, weapons belt, strap, back sling, etc...
    private new void Awake()
    {
        base.Awake();
        weaponSwing = PrimaryWeaponSwing();
        teethChatter = TeethChatter();
        playerObject = this.gameObject;
        //mcs = GetComponent<M3DCharacterManager>();
    }

    public int test;

    private float sprintTime;

    void Start()
    {
        weatherSystem = WeatherControl.manager.GetComponent<WeatherControl>();
        speed = 1.25F + 1.25f * gait;

        messageLog = MessageLog.messageLogGameObject.GetComponent<MessageLog>();
        InvokeRepeating("UpdateHeartRate", 1f, 1f);
        InvokeRepeating("ReadWeather", 1.5f, 300f);
        InvokeRepeating("UpdateCoreTemp", 1.5f, 1f);
        InvokeRepeating("Encumbrance", 2f, 2f);
        InvokeRepeating("CalorieBurn", 1f, 2f);
        InvokeRepeating("Hydration", 3f, 2f);
        InvokeRepeating("SleepDebt", 11f, 2f);
        InvokeRepeating("StaminaUpdate", 1f, 1f);
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


        gait = 0;
        stamina = maxStamina = stats.GetStat(RPGStatType.Endurance).StatValue*10;
        heartRate = stats.GetStat(RPGStatType.RestingHeartRate).StatValue;
        UpdateWeatherProtection();        
        var health = stats.GetStat<RPGVital>(RPGStatType.Health);
        health.OnCurrentValueChange += OnStatValueChange;
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            RaycastHit hit;
            if (Physics.Raycast(aimPoint.position, aimPoint.forward, out hit, 540))
            {
                Debug.DrawLine(aimPoint.position, hit.point, Color.green);
            }
        }
        if (inTask)
        {
            controls.enabled = false;
            if (Input.GetKeyDown(KeyCode.W)) { StopTasks(); controls.enabled = true; }
        }
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
            Death();
            controls.enabled = false;
        }
    }

    public void SheatheWeapon(Item_Weapon aWeapon)
    {
        if (!aWeapon.isWielded)
            return;
        aWeapon.isWielded = false;
        Debug.LogWarning("Sheathe weapon: " + aWeapon.itemName);
        aWeapon.GetComponent<Rigidbody>().isKinematic = false;
        aWeapon.GetComponent<Rigidbody>().useGravity = true;
        aWeapon.gameObject.SetActive(false);
        anim.SetBool("ArmsRaised", false);
        audioSource.PlayOneShot(audioManager.sheathe);
        if (aWeapon is Item_Weapon_Bow)
        {
            wieldingBow = false;
            rangedWeaponDrawn = false;
            offHandWeapon = null;
        }
        else
        {
            meleeWeaponDrawn = false;
            primaryWeapon = null;
        }
    }

    public void SetMainHandWeapon(Item_Weapon weapon)
    {
        primaryWeapon = weapon;
    }

    public void DrawWeapons()
    {
        if (weaponsReadied)
        { //sheathe weapons
            weaponsReadied = false;
            anim.SetBool("RightFistRaised", false);
            anim.SetBool("LeftFistRaised", false);
            anim.SetBool("RightOneHandedRaised", false);
            anim.SetBool("LeftOneHandedRaised", false);
        }
        else
        { //draw weapons
            weaponsReadied = true;
            if (primaryWeapon is Item_Weapon_Fist || !primaryWeapon)
            {
                primaryWeapon = rightFist;
                anim.SetBool("RightFistRaised", true);
            }
            else
            {
                anim.SetBool("RightOneHandedRaised", true);
            }
            if (offHandWeapon is Item_Weapon_Fist || !offHandWeapon)
            {
                offHandWeapon = leftFist;
                anim.SetBool("LeftFistRaised", true);
            }
            else
            {
                anim.SetBool("LeftOneHandedRaised", true);
            }
            primaryWeapon.wielderBodyManager = this;
            offHandWeapon.wielderBodyManager = this;
        }
    }

    public void DrawWeapon(Item_Weapon drawnWeapon)
    {
        bool drawingOffHand;
        if (drawnWeapon.weaponType == WeaponType.Bow)
        {
            if (offHandWeapon != null && !(offHandWeapon is Item_Weapon_Fist))
            {//if another weapon besides fist is already in off hand
                DropItem(offHandWeapon);
            }
            wieldingBow = true;
            rangedWeaponDrawn = true;
            offHandWeapon = drawnWeapon;
            drawnWeapon.gameObject.transform.SetParent(offHandTransform, false);
            drawnWeapon.gameObject.transform.position = offHandTransform.position;
        }
        else if(!drawnWeapon.isTwoHand)
        { //everything except bows and two handers
            if (primaryWeapon == null || (primaryWeapon is Item_Weapon_Fist))
            { // if primary hand is free, draw as primary weapon
                primaryWeapon = drawnWeapon;
                drawnWeapon.gameObject.transform.SetParent(primaryHandTransform, false);
                drawnWeapon.gameObject.transform.position = primaryHandTransform.position;
            }
            else if (offHandWeapon == null || (offHandWeapon is Item_Weapon_Fist))
            { // else if off hand is free, draw as off hand weapon
                offHandWeapon = drawnWeapon;
                drawnWeapon.gameObject.transform.SetParent(offHandTransform, false);
                drawnWeapon.gameObject.transform.position = offHandTransform.position;
            }
            else
            { // if both hands occupied, drop primary weapon and draw this as primary weapon
                DropItem(primaryWeapon);
                drawnWeapon.gameObject.transform.SetParent(primaryHandTransform, false);
                drawnWeapon.gameObject.transform.position = primaryHandTransform.position;
            }
            meleeWeaponDrawn = true;
        }
        else
        { // two handers
            if (drawnWeapon.isTwoHand && (offHandWeapon != null && !(offHandWeapon is Item_Weapon_Fist)))
            { // drop anything in off hand if two handed weapon
                DropItem(offHandWeapon);
            }
        }
        //happens in all cases
        drawnWeapon.isWielded = true;
        drawnWeapon.GetComponent<Rigidbody>().isKinematic = true;
        drawnWeapon.GetComponent<Rigidbody>().useGravity = false;
        audioSource.PlayOneShot(audioManager.draw);
        drawnWeapon.gameObject.transform.localPosition = drawnWeapon.gripAdjust;
        drawnWeapon.gameObject.transform.localRotation = Quaternion.Euler(drawnWeapon.gripOrientation + (offHandWeapon == drawnWeapon ? Vector3.right * 180 : Vector3.zero));
        drawnWeapon.gameObject.SetActive(true);
        anim.SetBool("ArmsRaised", true);
    }

    public void OffHandDrawWeapon(Item_Weapon drawnWeapon)
    {
        if (offHandWeapon != null)
        {
            Item_Weapon droppingItem = primaryWeapon;
            SheatheWeapon(primaryWeapon);
            DropItem(droppingItem);
        }

        drawnWeapon.isWielded = true;
        Debug.LogWarning("OffHandDraw weapon: " + drawnWeapon.itemName);
        drawnWeapon.GetComponent<Rigidbody>().isKinematic = true;
        drawnWeapon.GetComponent<Rigidbody>().useGravity = false;
        drawnWeapon.gameObject.SetActive(true);
        if (drawnWeapon is Item_Weapon_Bow)
        {
            wieldingBow = true;
            rangedWeaponDrawn = true;
            offHandWeapon = drawnWeapon;
            drawnWeapon.gameObject.transform.SetParent(offHandTransform, false);
            drawnWeapon.gameObject.transform.position = offHandTransform.position;
        }
        else
        {
            meleeWeaponDrawn = true;
            offHandWeapon = drawnWeapon;
            drawnWeapon.gameObject.transform.SetParent(offHandTransform, false);
            drawnWeapon.gameObject.transform.position = offHandTransform.position;
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
        weaponsReadied = !weaponsReadied;
        //anim.RaiseWeapons
        //bodyStatus.weaponsStatus = raised
    }

    [DoNotSerialize]
    public Dictionary<WeaponType, Tuple<string,RPGStatType>> mainAttackDict = new Dictionary<WeaponType, Tuple<string, RPGStatType>>
    {
        { WeaponType.Axe, new Tuple<string,RPGStatType>("RightAxeChop",RPGStatType.Axe)}, { WeaponType.Arm, new Tuple<string,RPGStatType>("RightPunch",RPGStatType.ArmStrike) }, { WeaponType.Bow, new Tuple<string,RPGStatType>("FireBow",RPGStatType.Bow) },
        { WeaponType.Pick, new Tuple<string,RPGStatType>("RightPickSwing",RPGStatType.Pick) }
    };
    [DoNotSerialize]
    public Dictionary<WeaponType, Tuple<string, RPGStatType>> offHandAttackDict = new Dictionary<WeaponType, Tuple<string, RPGStatType>>
    {
        { WeaponType.Axe, new Tuple<string,RPGStatType>("LeftAxeChop",RPGStatType.Axe)}, { WeaponType.Arm, new Tuple<string,RPGStatType>("LeftPunch",RPGStatType.ArmStrike) }, { WeaponType.Bow, new Tuple<string,RPGStatType>("FireBow",RPGStatType.Bow) },
        { WeaponType.Pick, new Tuple<string,RPGStatType>("LeftPickSwing",RPGStatType.Pick) }
    };

    public override void PrimaryAttack()
    {
        if (isAttackingPrimary) { return; }
        primaryAttackCollisionList.Clear();
        if (primaryWeapon != null)
        {
            isAttackingPrimary = true;
            primaryWeapon.weaponCollList[0].isActive = true;
            StopCoroutine(OnCompleteMainWeaponAttackAnimation());
            StartCoroutine(OnCompleteMainWeaponAttackAnimation());
            Invoke(mainAttackDict[primaryWeapon.weaponType].first, 0);
            audioSource.PlayOneShot(audioManager.swing);
            StopCoroutine(PrimaryWeaponSwing());
            StartCoroutine(PrimaryWeaponSwing());
        }
    }

    public void OffHandAttack()
    {
        if (isAttackingSecondary) { return; }
        offHandAttackCollisionList.Clear();
        if (offHandWeapon != null)
        {
            isAttackingSecondary = true;
            offHandWeapon.weaponCollList[0].isActive = true;
            StopCoroutine(OnCompleteOffHandWeaponAttackAnimation());
            StartCoroutine(OnCompleteOffHandWeaponAttackAnimation());
            Invoke(offHandAttackDict[offHandWeapon.weaponType].first, 0);
            audioSource.PlayOneShot(audioManager.swing);
            StopCoroutine(OffHandWeaponSwing());
            StartCoroutine(OffHandWeaponSwing());
        }
    }

    int offHandWeaponAnimationLayer = 6;

    public IEnumerator OnCompleteOffHandWeaponAttackAnimation()
    {
        while (anim.GetCurrentAnimatorStateInfo(offHandWeaponAnimationLayer).loop)
            yield return null;
        while (!anim.GetCurrentAnimatorStateInfo(offHandWeaponAnimationLayer).loop)
            yield return null;
        isAttackingSecondary = false;
        offHandWeapon.weaponCollList[0].isActive = false;
        yield return null;
    }

    private IEnumerator PrimaryWeaponSwing(float swingTime = .5f)
    {
        while(isAttackingPrimary && primaryAttackCollisionList.Count == 0)
            yield return null;
        isAttackingPrimary = false;
        foreach (Collision coll in primaryAttackCollisionList)
        {
            if (coll.gameObject.transform.root == this.gameObject)
                continue;
            int blunt = primaryWeapon.baseBlunt + primaryWeapon.itemWeight * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
            int cut = primaryWeapon.baseCut * (1 + (stats.GetStat(mainAttackDict[primaryWeapon.weaponType].second).StatValue / 10));
            int pierce = primaryWeapon.basePierce * (1 + (stats.GetStat(mainAttackDict[primaryWeapon.weaponType].second).StatValue / 10));

            if (coll.collider.transform.tag == "BodyPart")
            {
                BodyPartColliderScript bp = coll.collider.gameObject.GetComponent<BodyPartColliderScript>();
                int[] z = new int[] { 0, 0, 0 }; int[] o = new int[] { blunt, cut, pierce };
                z = bp.parentBody.SendArmorNumbers(bp.bodyPartType);
                bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((pierce / z[0]) + (blunt / z[1]));
                print(o[0] + "," + o[1] + "," + o[2]);
                AttackResolution(bp, bp.parentBody.stats, stats.GetStat<RPGSkill>(mainAttackDict[primaryWeapon.weaponType].second), z, o);
                hud.UpdateTargetHealthBar(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue, bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatBaseValue);
                yield return null;
            }
            else if (coll.transform.root.tag == "Tree")
            {
                Tree hitTree = coll.transform.root.GetComponent<Tree>();
                if (hitTree)
                {
                    float chopRate = primaryWeapon.TreeChopRateFactor() * stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatValue * stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).StatValue;
                    if (hitTree.health > 0)
                    {
                        hitTree.TakeDamage(chopRate);
                        hud.UpdateTargetHealthBar(hitTree.health, hitTree.startinghealth);
                    }
                    else
                    {
                        hitTree.TakeDamage(chopRate);
                        hud.UpdateTargetHealthBar(hitTree.startinghealth + hitTree.health, hitTree.startinghealth);
                    }
                }
                yield return null;
            }
            else if (coll.gameObject.tag == "Terrain")
            {
                if(coll.gameObject.layer == 13)
                {
                    World16 world = World16.worldGameObject.GetComponent<World16>();
                    Block16 block = world.GetBlock16(Mathf.RoundToInt(coll.contacts[0].point.x), Mathf.RoundToInt(coll.contacts[0].point.y), Mathf.RoundToInt(coll.contacts[0].point.z));
                    if(block is Block16Grass || block is Block16RiverGrass)
                    {
                        audioSource.PlayOneShot(audioManager.hitDirt);
                    }
                    else if(block is Block16)
                        audioSource.PlayOneShot(audioManager.metalOnStone);
                }
                yield return null;
            }
        }
        isAttackingPrimary = false;
    }

    private IEnumerator OffHandWeaponSwing(float swingTime = .5f)
    {
        while (isAttackingSecondary && offHandAttackCollisionList.Count == 0)
            yield return null;
        isAttackingSecondary = false;
        foreach (Collision coll in offHandAttackCollisionList)
        {
            if (coll.gameObject.transform.root == this.gameObject)
                continue;
            int blunt = offHandWeapon.baseBlunt + offHandWeapon.itemWeight * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
            int cut = offHandWeapon.baseCut * (1 + (stats.GetStat(mainAttackDict[offHandWeapon.weaponType].second).StatValue / 10));
            int pierce = offHandWeapon.basePierce * (1 + (stats.GetStat(mainAttackDict[offHandWeapon.weaponType].second).StatValue / 10));

            if (coll.collider.transform.tag == "BodyPart")
            {
                BodyPartColliderScript bp = coll.collider.gameObject.GetComponent<BodyPartColliderScript>();
                int[] z = new int[] { 0, 0, 0 }; int[] o = new int[] { blunt, cut, pierce };
                z = bp.parentBody.SendArmorNumbers(bp.bodyPartType);
                bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((pierce / z[0]) + (blunt / z[1]));
                print(o[0] + "," + o[1] + "," + o[2]);
                AttackResolution(bp, bp.parentBody.stats, stats.GetStat<RPGSkill>(mainAttackDict[offHandWeapon.weaponType].second), z, o);
                hud.UpdateTargetHealthBar(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue, bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatBaseValue);
                yield return null;
            }
            else if (coll.transform.root.tag == "Tree")
            {
                Tree hitTree = coll.transform.root.GetComponent<Tree>();
                if (hitTree)
                {
                    float chopRate = offHandWeapon.TreeChopRateFactor() * stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatValue * stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).StatValue;
                    if (hitTree.health > 0)
                    {
                        hitTree.TakeDamage(chopRate);
                        hud.UpdateTargetHealthBar(hitTree.health, hitTree.startinghealth);
                    }
                    else
                    {
                        hitTree.TakeDamage(chopRate);
                        hud.UpdateTargetHealthBar(hitTree.startinghealth + hitTree.health, hitTree.startinghealth);
                    }
                }
                yield return null;
            }
            else if (coll.gameObject.tag == "Terrain")
            {
                if (coll.gameObject.layer == 13)
                {
                    World16 world = World16.worldGameObject.GetComponent<World16>();
                    Block16 block = world.GetBlock16(Mathf.RoundToInt(coll.contacts[0].point.x), Mathf.RoundToInt(coll.contacts[0].point.y), Mathf.RoundToInt(coll.contacts[0].point.z));
                    if (block is Block16Grass || block is Block16RiverGrass)
                    {
                        audioSource.PlayOneShot(audioManager.hitDirt);
                    }
                    else if (block is Block16)
                        audioSource.PlayOneShot(audioManager.metalOnStone);
                }
                yield return null;
            }
        }
        print("turning off trigger");
        isAttackingSecondary = false;
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
            audioSource.PlayOneShot(audioManager.hitBody);
            //bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((o[0] / d[0]) + ( o[1] / d[1]) + (o[2] / d[2]));
            bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= (int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2])));
            attackSkill.GainXP(10);
            return;
        }
        else
        {
            print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
            bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= 4 * (int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2])));
        }
        print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
        primaryAttackCollisionList.Clear();
        //defense: defense skills, armor, encumbrance, stamina, energy, agility, strength, toughness
    }

    void MainPunch()
    {
        anim.SetTrigger("RightPunch");
    }

    void RightPunch()
    {
        anim.SetTrigger("RightPunch");
    }

    void LeftPunch()
    {
        anim.SetTrigger("LeftPunch");
    }

    void RightAxeChop()
    {
        anim.SetTrigger("RightAxeChop");
    }

    void LeftAxeChop()
    {
        anim.SetTrigger("LeftAxeChop");
    }

    void RightPickSwing()
    {
        anim.SetTrigger("RightAxeChop");
        //StartCoroutine(PrimaryWeaponSwing());
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
                int pierce = primaryWeapon.basePierce * (1 + (stats.GetStat(RPGStatType.Pick).StatValue / 10));
                int blunt = (primaryWeapon.baseBlunt * 5 + primaryWeapon.itemWeight) * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
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
                    audioSource.PlayOneShot(audioManager.metalOnStone);
                    EditTerrain.HitBlock(hit, pierce * blunt);
                    print(pierce * blunt);
                }
            }
            else if (hit.collider.transform.tag == "BodyPart")
            {
                audioSource.PlayOneShot(audioManager.hitBody);
                print(0);
                int pierce = primaryWeapon.basePierce * (1 + (stats.GetStat(RPGStatType.Pick).StatValue / 10));
                int blunt = (primaryWeapon.baseBlunt * 5 + primaryWeapon.itemWeight) * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
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

    void LeftPickSwing()
    {
        anim.SetTrigger("LeftAxeChop");
        //StartCoroutine(offHandWeaponSwing());
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
                int pierce = offHandWeapon.basePierce * (1 + (stats.GetStat(RPGStatType.Pick).StatValue / 10));
                int blunt = (offHandWeapon.baseBlunt * 5 + offHandWeapon.itemWeight) * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
                Block block = EditTerrain.GetBlock(hit);
                if (block is BlockAir)
                    print("air");
                print(EditTerrain.GetBlockPos(hit).x + "," + EditTerrain.GetBlockPos(hit).y + "," + EditTerrain.GetBlockPos(hit).z);
                if (block is BlockGrass)
                {
                    EditTerrain.HitBlock(hit, 100 * (pierce + (blunt / 2)));
                    print(pierce + (blunt / 2));
                    return;
                }

                else if (block is Block)
                {
                    audioSource.PlayOneShot(audioManager.metalOnStone);
                    EditTerrain.HitBlock(hit, pierce * blunt);
                    print(pierce * blunt);
                }
            }
            else if (hit.collider.transform.tag == "BodyPart")
            {
                audioSource.PlayOneShot(audioManager.hitBody);
                print(0);
                int pierce = offHandWeapon.basePierce * (1 + (stats.GetStat(RPGStatType.Pick).StatValue / 10));
                int blunt = (offHandWeapon.baseBlunt * 5 + offHandWeapon.itemWeight) * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
                print(1);

                BodyPartColliderScript bp = hit.collider.GetComponent<BodyPartColliderScript>();
                int[] z = new int[] { 0, 0, 0 }; int[] o = new int[] { 0, blunt, pierce };
                z = bp.parentBody.SendArmorNumbers(bp.bodyPartType);
                bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((pierce / z[0]) + (blunt / z[1]));
                print(2);

                AttackResolution(bp, bp.parentBody.stats, stats.GetStat<RPGSkill>(RPGStatType.Pick), z, o);
                print(4);
            }
        }
    }





    //bow and arrow stuff
    public bool wieldingBow, arrowKnocked, drawingBow, bowDrawn;
    public float startTime, nowTime;
    public float drawTime = 3f;
    GameObject loadedProjectile;
    public void KnockArrow()
    {
        if (!drawingBow && !arrowKnocked && currentAmmo != null)
        {
            arrowKnocked = true;
            Item_Weapon_Bow bow = (Item_Weapon_Bow)offHandWeapon;
            if (bow != null)
            {
                loadedProjectile = Instantiate(currentAmmoPrefab, bow.nockPoint) as GameObject;
                loadedProjectile.transform.localPosition = Vector3.zero;
                loadedProjectile.transform.localRotation = Quaternion.identity;
                loadedProjectile.GetComponent<Rigidbody>().isKinematic = true;
                loadedProjectile.GetComponent<Item_Ammo_Arrow>().attackCollider.isTrigger = false;
                loadedProjectile.SetActive(true);
                //loadedProjectile.GetComponent<Item_Ammo_Arrow>().attackCollider.isTrigger = false;
            }
        }
    }
    public void DrawBow()
    {
        anim.SetBool("DrawingBow", true);
        drawingBow = true;
        Item_Weapon_Bow bow = (Item_Weapon_Bow)offHandWeapon;
        if(bow != null)
        {
            bow.Draw();
        }
        StartCoroutine(BowPull());
        startTime = Time.time;
    }
    private IEnumerator BowPull()
    {
        print("Start");
        yield return new WaitForSeconds(1);
        print("done");
        if(drawingBow)
        bowDrawn = true;
    }
    public void ReleaseBow()
    {
        Item_Weapon_Bow bow = (Item_Weapon_Bow)offHandWeapon;
        if (bow != null)
        {
            bow.Release();
        }
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
            Item_Weapon_Bow bow = (Item_Weapon_Bow)offHandWeapon;
            if (bow != null)
            {
                bow.Fire();
            }
            bowDrawn = false;
            drawingBow = arrowKnocked = false;
            anim.SetTrigger("FireBow");
            anim.SetBool("DrawingBow", false);
            Item_Ammo_Arrow thisArrow = loadedProjectile.GetComponent<Item_Ammo_Arrow>();
            loadedProjectile.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 pos = loadedProjectile.transform.position;
            loadedProjectile.transform.parent = null;
            loadedProjectile.transform.position = arrowReleasePoint.position;
            loadedProjectile.GetComponent<Rigidbody>().AddForce(arrowReleasePoint.TransformDirection(Vector3.forward * 10));
            thisArrow.projectile.previousPosition = loadedProjectile.transform.position;
            thisArrow.projectile.enabled = true;
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

    /*
     * Inventory and Outfit 
     */
    public void PlaceInBag(Item_Weapon weapon, Inventory bag) { }

    // Add a garment to the player's outfit. Player protection stats are updated. Character model updated if item has model.
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
                if(newWearable.texture != null)
                    newWearable.material.SetTexture("_MainTex", newWearable.texture);
                mcs.SetClothingVisibility(newWearable.modelID, true);
                mcsUI.SetClothingVisibility(newWearable.modelID, true);
            }
        }
    }

    public void RemoveGarment(Item_Garment thisWearable)
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

    public new int[] SendArmorNumbers(RPGStatType bodyPart)
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

    /*
     *Bodily Needs
     */

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
        gait++;
        if (gait > 3)
        {
            gait = 0;
            messageLog.NewMessage("Gait: Slow Walk");
            anim.SetBool("isRunning", false);
        }
        else if (gait == 3)
        {
            anim.SetBool("isRunning", true);
            messageLog.NewMessage("Gait: Run");
        }
        else if (gait == 2)
        {
            anim.SetBool("isRunning", true);
            messageLog.NewMessage("Gait: Jog");
        }
        else if (gait == 1)
        {
            anim.SetBool("isRunning", false);
            messageLog.NewMessage("Gait: Walk");
        }
        speed = 1.25F + 1.25F * gait;
    }

    float lastHeartCheckTime = 0;
    void UpdateHeartRate()
    {
        test = stats.GetStat(RPGStatType.RestingHeartRate).StatValue;
        float target;
        target = stats.GetStat(RPGStatType.RestingHeartRate).StatValue + (moving ? (gait * 30 * (crouching ? 1:1.5f)) : 0) + (.001f * (maxStamina - stamina)) + (shivering ? 30 : 0);
        target += encumbrance * 60;
        target = Math.Min(target, 200);
        float diff = target - heartRate;
        float now = worldTime.totalGameSeconds;
        heartRate = Math.Min(heartRate + (now - lastHeartCheckTime) * ((.0005f * diff * Math.Abs(diff)) + (.01f * diff)),200);
        heartRate = Math.Max(10, heartRate);
        lastHeartCheckTime = now;
    }

    float coreTempBase = 37f;
    public Thermometer thermometer;
    public ShelterCheck shelterCheck;
    public float localTemperature = 0;
    public float precipRate = 0;
    public float windSpeed = 0;
    public float humidity = 0;
    public float coreTemp = 37f;
    float averageSkinTemp = 33.2f, headSkinTemp,neckSkinTemp, chestSkinTemp, stomachSkinTemp, pelvisSkinTemp, upperBack, lowerBackSkinTemp,
    leftShoulderSkinTemp, rightShoulderSkinTemp, leftUpperArmSkinTemp, rightUpperArmSkinTemp, leftForearmSkinTemp, rightForearmSkinTemp,
    leftHandSkinTemp, rightHandSkinTemp, leftThighSkinTemp, rightThighSkinTemp, leftCalfSkinTemp, rightCalfSkinTemp, leftFootSkinTemp,
    rightFootSkinTemp;

    public float totalInsulation = 0;
    public float totalWaterCover = 0;
    public float totalWindCover = 0;
    
    public float lastTempCheckTime = 28800;

    void UpdateCoreTemp()
    {
        ReadWeather();
        float avgWetness = 0;
        foreach(RPGBodyPart bodyPart in RPGBodyPartsList)
        {
            avgWetness += bodyPart.wetness;
        }
        avgWetness /= RPGBodyPartsList.Count;
        float heatProduction = 0.001f * stats.GetStat(RPGStatType.Weight).StatValue * Mathf.Pow(heartRate / (stats.GetStat(RPGStatType.RestingHeartRate).StatValue),1.285f); // 1w/kg (.001w/g) at rest, 
        float heatLoss = stats.GetStat(RPGStatType.SurfaceArea).StatValue * (averageSkinTemp - localTemperature) * .000075f * (.1f + avgWetness + windSpeed/100) / totalInsulation; // coeff * area * temperature difference

        //(Mathf.Pow(coreTemp / coreTempBase,3) * (coreTemp - localTemperature) * (.1f+humidity) + localTemperature * localTemperature * humidity) * stats.GetStat(RPGStatType.Height).StatValue / totalInsulation;
        if (coreTemp > coreTempBase + .5f)
            averageSkinTemp = coreTemp;
        else
            averageSkinTemp = coreTemp - 4; 

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
        coreTemp += (now - lastTempCheckTime) * (heatProduction - heatLoss) / (4.184f *  stats.GetStat(RPGStatType.Weight).StatValue);

        if (coreTemp < (coreTempBase - 1))
        {
            if (!shivering)
            {
                shivering = true;
                StartCoroutine(teethChatter);
            }
            if (coreTemp < (coreTempBase - 5))
            {
                FrostEffect frost = camTransform.GetComponent<FrostEffect>();
                frost.enabled = true;
                frost.FrostAmount = 0.1f + (Mathf.Pow((coreTempBase - 5f) / coreTemp, 2) - 1);
                if(coreTemp < (coreTempBase - 7))
                {
                    //die
                }
            }
        }
        else
        {
            StopCoroutine(teethChatter);
            shivering = false;
            camTransform.GetComponent<FrostEffect>().enabled = false;
            if (coreTemp > (coreTempBase))
                Sweat(now - lastTempCheckTime);
        }
    }

    IEnumerator TeethChatter()
    {
        while (true)
        {
            audioSource.PlayOneShot(audioManager.teethChatter);
            yield return new WaitForSeconds(15);
        }
    }

    List<RPGBodyPart> RPGBodyPartsList = new List<RPGBodyPart>();

    void UpdateWeatherProtection()
    {
        if(RPGBodyPartsList.Count == 0)
        {
            RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.Head)); 
 			 RPGBodyPartsList.Add( stats.GetStat<RPGBodyPart>(RPGStatType.Neck)); 
 			 RPGBodyPartsList.Add( stats.GetStat<RPGBodyPart>(RPGStatType.Chest)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.Stomach)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.RightHand)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot)); 
 			 RPGBodyPartsList.Add(stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot));
        }
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

        foreach(RPGBodyPart bodyPart in RPGBodyPartsList)
        {
            bodyPart.wetness = Mathf.Clamp01(bodyPart.wetness + (precipRate / totalWaterCover) + (sweating ? Mathf.Pow(Mathf.Clamp01(coreTemp - coreTempBase),2)/10 : 0));
        }
    }

    int printPos = 375;
    void OnGUI()
    {

        GUI.Label(new Rect(0, 400, 500, 500), "Core Temp: " + coreTemp);
        GUI.Label(new Rect(0, 425, 500, 500), "Local Temp: " + localTemperature);
        GUI.Label(new Rect(0, 450, 500, 500), "Heart Rate: " + heartRate);
        GUI.Label(new Rect(0, 475, 500, 500), "Hydration: " + hydration + "%");
        GUI.Label(new Rect(0, 500, 500, 500), "Sleep Debt: " + sleepDebt + " hrs");
        GUI.Label(new Rect(0, 525, 500, 500), "Calories: " + calories);
        GUI.Label(new Rect(0, 550, 500, 500), "Encumbrance: " + encumbrance + "%");
        GUI.Label(new Rect(0, 575, 500, 500), "Stamina Max: " + maxStamina);
        GUI.Label(new Rect(0, 600, 500, 500), "Stamina: " + stamina);

        for (int j = 0; j < outfit.Count; j++)
        {
            GUI.Label(new Rect(1100, printPos, 500, 500), outfit[j].itemName);
            printPos = printPos + 25;
        }
    }

    public void ReadWeather()
    {
        float altitude = this.transform.position.y;
        localTemperature = thermometer.temperature;
        if (shelterCheck.roof) { precipRate = 0; }
        else { precipRate = weatherSystem.weathers[weatherSystem.schedule[0]].precipRate; }
        if (shelterCheck.walls) { windSpeed = 0; }
        else { windSpeed = weatherSystem.weathers[weatherSystem.schedule[0]].windSpeed; }
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

    public bool shivering,sweating;
    [SerializeField]
    float waterContent;
    public float hydration = 66f; // body water %
    void Hydration()
    {
        hydration -= calorieBurn * .000042f * Math.Max(localTemperature*.1f, 4);
    }

    void Sweat(float amount)
    {
        sweating = true;
        hydration -=  Mathf.Pow(Mathf.Clamp01(coreTemp - coreTempBase),2)/100 * amount;
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


    /*
     * Tasks
     */
    private IEnumerator autoTaskCoroutine;
    public float taskStartTime, taskTimeNeeded;

    public float treeDamage;
    public Tree targetTree;
    public void FellTree(Tree aTree)
    {
        targetTree = aTree;
        treeDamage = 0;
        taskStartTime = worldTime.totalGameSeconds;
        treeFelling = inTask = true;
        anim.SetBool("inTask", true);
        anim.SetBool("isFallingTree", true);
        messageLog.NewMessage("Falling tree.");
        float chopRate = primaryWeapon.TreeChopRateFactor() * stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatValue * stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).StatValue; ;
        autoTaskCoroutine = FellTree(aTree, chopRate);
        StartCoroutine(autoTaskCoroutine);
    }
    
    private IEnumerator FellTree(Tree aTree, float chopRate)
    {
        while (true)
        {
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            treeDamage = chopRate * workTime;
            hud.UpdateTargetHealthBar((aTree.startinghealth - treeDamage), aTree.startinghealth);
            if (treeDamage >= aTree.health)
            {
                aTree.Fall();
                int xp = (int)(treeDamage);
                stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).GainXP(xp);
                stats.GetStat<RPGAttribute>(RPGStatType.Toughness).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Strength).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Endurance).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).TrainingValue += 10;
                treeFelling = inTask =  false;
                anim.SetBool("inTask", false);
                anim.SetBool("isFallingTree", false);
                messageLog.NewMessage(xp + " Woodworking XP gained.");
                messageLog.NewMessage("Tree felled in " + workTime / 60 + " minutes.");
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator ProcessTree(Tree aTree, float chopRate)
    {
        float startingHealth = aTree.downedHealth;
        while (true)
        {
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            treeDamage = chopRate * workTime;
            hud.UpdateTargetHealthBar((aTree.startinghealth - treeDamage), aTree.startinghealth);
            if (treeDamage >= aTree.downedHealth)
            {
                aTree.TurnToWoodPile();
                int xp = (int)(treeDamage);
                stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).GainXP(xp);
                stats.GetStat<RPGAttribute>(RPGStatType.Toughness).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Strength).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Endurance).TrainingValue += 10;
                stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).TrainingValue += 10;
                treeProcessing = inTask = false;
                anim.SetBool("inTask", false);
                anim.SetBool("isFallingTree", false);
                messageLog.NewMessage(xp + " Woodworking XP gained.");
                messageLog.NewMessage("Tree processed in " + workTime / 60 + " minutes.");
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void ProcessDownedTree(Tree aTree)
    {
        if (!aTree.standing)
        {
            targetTree = aTree;
            treeDamage = 0;
            taskStartTime = worldTime.totalGameSeconds;
            treeProcessing = inTask = true;
            anim.SetBool("inTask", true);
            anim.SetBool("isFallingTree", true);
            messageLog.NewMessage("Processing downed tree.");
            float chopRate = primaryWeapon.TreeChopRateFactor() * stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatValue * stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).StatValue;
            print(chopRate.ToString());
            autoTaskCoroutine = ProcessTree(aTree, chopRate);
            StartCoroutine(autoTaskCoroutine);
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
        messageLog.NewMessage("Starting fire, " + fireStartingTime + " seconds expected.");
        autoTaskCoroutine = StartFire(aFire);
        StartCoroutine(autoTaskCoroutine);
    }

    // every 15 seconds try again to start fire
    private IEnumerator StartFire(Fire aFire)
    {
        while (true && !aFire.isLit)
        {
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            xpGain = workTime;
            stats.GetStat<RPGSkill>(RPGStatType.FireMaking).GainXP(10);
            hud.UpdateTargetHealthBar(fireStartingTime - workTime, fireStartingTime);
            if (workTime > fireStartingTime)
            {
                print("fire lit");
                aFire.LightFire();
                int xp = 10 + (int)xpGain;
                stats.GetStat<RPGSkill>(RPGStatType.FireMaking).GainXP(xp);
                messageLog.NewMessage(xp + " Firemaking XP gained.");
                isStartingFire = inTask = false;
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
        targetPlant = plant;
        gathering = inTask = true;
        totalGathered = 0;
        taskStartTime = worldTime.totalGameSeconds;
        float rate = plant.gatherRate * (1 + stats.GetStat<RPGSkill>(RPGStatType.Gathering).StatValue * .09f) * (stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).StatValue * .005f);
        float timeNeeded = plant.numFruits / rate;
        print(rate); print(timeNeeded);
        anim.SetBool("isGathering", true);
        messageLog.NewMessage("Gathering berries.");
        autoTaskCoroutine = Gather(plant, rate);
        StartCoroutine(autoTaskCoroutine);
    }

    // every 15 seconds try again to start fire
    private IEnumerator Gather(Perennial plant, float rate)
    {
        while (true)
        {
            print(3);
            float workTime = worldTime.totalGameSeconds - taskStartTime;
            totalGathered = rate * workTime;
            hud.UpdateTargetHealthBar((plant.numFruits) - totalGathered, plant.numFruits);
            if (totalGathered >= plant.numFruits)
            {
                totalGathered = (int)Math.Floor(totalGathered);
                if (plant.fruitPrefab.GetComponent<Item_Stack>())
                {
                    GameObject harvest = Instantiate(plant.fruitPrefab);
                    harvest.GetComponent<Item_Stack>().numItems = (int)totalGathered;
                    PickupItem(harvest.GetComponent<Item_Stack>());
                }
                else if (plant.fruitPrefab.GetComponent<Item>())
                {
                    for (int i = 0; i < totalGathered; i++)
                    {
                        GameObject harvest = Instantiate(plant.fruitPrefab);
                        baseInventory.AddItem(harvest.GetComponent<Item>());
                        PickupItem(harvest.GetComponent<Item>());
                    }
                }
                int xp = (int)(plant.difficulty * totalGathered * 1.1);
                stats.GetStat<RPGSkill>(RPGStatType.Gathering).GainXP(xp);
                messageLog.NewMessage(xp + " Gathering XP gained.");
                plant.SetHarvested();
                gathering = inTask = false;
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
        taskTimeNeeded = body.butcherTime * (1 + UnityEngine.Random.Range(0, .25f)) / (stats.GetStat(RPGStatType.Butchery).StatValue + primaryWeapon.butcheringAid);
        autoTaskCoroutine = Butcher(body);
        StartCoroutine(autoTaskCoroutine);
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

    public void Eat(ItemStackFood foodItem)
    {
        hydration += foodItem.water / stats.GetStat<RPGAttribute>(RPGStatType.Weight).StatValue;
        calories += foodItem.calories;
        foodItem.numItems -= 1;
        if (foodItem.numItems == 0)
        {
            if (!foodItem.loose)
                foodItem.itemUIElementScript.parentInventory.DropItem(foodItem);
            Destroy(foodItem.itemUIelement);
            Destroy(foodItem.gameObject);
        }
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
        messageLog.NewMessage("Stopping task early");
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
            int xp = (int)(targetPlant.difficulty * totalGathered);
            stats.GetStat<RPGSkill>(RPGStatType.Gathering).GainXP(xp);
            messageLog.NewMessage(xp + " Gathering XP gained.");
            targetPlant.numFruits -= (int)totalGathered;
            gathering = false;
            anim.SetBool("isGathering", false);
            return;
        }
        if (treeFelling)
        {
            targetTree.TakeDamage(treeDamage);
            treeFelling = inTask =  false;
            int xp = (int)treeDamage;
            stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).GainXP((int)(treeDamage));
            anim.SetBool("inTask", false);
            anim.SetBool("isFallingTree", false);
            messageLog.NewMessage(xp + " Woodworking XP gained.");
            return;
        }
        if (treeProcessing)
        {
            targetTree.TakeDamage(treeDamage);
            treeProcessing = inTask = false;
            stats.GetStat<RPGSkill>(RPGStatType.WoodWorking).GainXP((int)(treeDamage));
            int xp = (int)treeDamage;
            anim.SetBool("inTask", false);
            anim.SetBool("isFallingTree", false);
            messageLog.NewMessage(xp + " Woodworking XP gained.");
            return;
        }
        if (isStartingFire)
        {
            int xp = (int)xpGain;
            messageLog.NewMessage(xp + " Firemaking XP gained.");
            stats.GetStat<RPGSkill>(RPGStatType.FireMaking).GainXP((int)xpGain);
            isStartingFire = false;
            anim.SetBool("isStartingFire", false);
            return;
        }
        if (isSleeping)
        {
            messageLog.NewMessage("Awake.");
            isSleeping = false;
            anim.SetBool("isLayingDown", false);
            sleepLength = worldTime.totalGameSeconds - sleepStartTime;
            return;
        }
        StopAllCoroutines();
    }
}