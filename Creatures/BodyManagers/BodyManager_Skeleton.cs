using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.Characters.ThirdPerson;

public class BodyManager_Skeleton : BodyManager {

    public bool weaponDrawn;
    public Transform rHandTransform;
    public Item_Weapon rHandWeapon;
    public Item_Weapon lHandWeapon;
    public Item_Ammo currentAmmo;

    public AICharacterControl aiControl;

    public Collider rHandCollider;

    public float stamina, maxStamina;

    private void Awake()
    {
        //stats = GetComponentInParent<SkeletonStats>();
    }


    void Start()
    {
        attacking = guardRaised = sneaking = false;
        var health = stats.GetStat<RPGVital>(RPGStatType.Health);
        health.OnCurrentValueChange += OnStatValueChange;
        anim = GetComponentInChildren<Animator>();
        stamina = maxStamina = stats.GetStat(RPGStatType.Endurance).StatValue * 10;
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
            aiControl.enabled = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        collisionList.Add(other);
    }

    void SheatheWeapon()
    {
    }

    public void DrawWeapon(Item_Weapon drawnWeapon)
    {
        Debug.LogWarning("4");


        //conditionals upon weapon type(anim.some_DrawWeapon, body.someState)
        //player model attach weapon model
        weaponDrawn = true;
        rHandWeapon = drawnWeapon;
        drawnWeapon.GetComponent<Rigidbody>().isKinematic = true;
        drawnWeapon.GetComponent<Rigidbody>().useGravity = false;
        drawnWeapon.gameObject.SetActive(true);
        drawnWeapon.gameObject.transform.localScale *= .25f;// new Vector3(.25f, .25f, .25f);
        drawnWeapon.gameObject.transform.SetParent(rHandTransform, false);
        drawnWeapon.gameObject.transform.position = rHandTransform.position;
        drawnWeapon.gameObject.transform.localRotation.Equals(0);
        drawnWeapon.gameObject.transform.Rotate(drawnWeapon.gripOrientation);
        drawnWeapon.gameObject.transform.Translate(drawnWeapon.gripAdjust);
    }

    void RaiseGuard()
    {
        if (guardRaised) { guardRaised = false; }
        else { guardRaised = true; }
    }

    void RaiseWeapons()
    {
        //anim.RaiseWeapons
        //bodyStatus.weaponsStatus = raised

    }

    public Dictionary<WeaponType, string> mainAttackDict = new Dictionary<WeaponType, string>
    {
        { WeaponType.Axe_1H, "AxeChop" }, { WeaponType.Axe_2H, "AxeChop" }, { WeaponType.Arm, "RightPunch" }, { WeaponType.Bow, "FireBow" },
        { WeaponType.Pick, "PickSwing" }
    };

    public void MainAttack()
    {
        print(666);
        if (attacking) { return; }
        collisionList.Clear();
        if (rHandWeapon)
        {
            Invoke(mainAttackDict[rHandWeapon.weaponType], 0);
        }
        else
        {
            print(667);
            StartCoroutine(Claw());
            print(668);

        }
    }

    public IEnumerator Claw()
    {
        print(6688);


        anim.SetTrigger("Attack_Claw");
        yield return new WaitForSeconds(.2f);
        rHandCollider.isTrigger = true;

        if (collisionList.Count > 0)
        {
            print(669);
            if (collisionList[0] is CharacterController)
            {
                collisionList.RemoveAt(0);
            }

            if (collisionList[0].tag == "BodyPart")
            {
                print(670);
                int cut = 1 + (stats.GetStat(RPGStatType.ArmStrike).StatValue);
                int blunt = (int)(1 + (stats.GetStat(RPGStatType.ArmStrike).StatValue + stats.GetStat(RPGStatType.Strength).StatValue) * .1f);
                BodyPartColliderScript bp = collisionList[0].GetComponent<BodyPartColliderScript>();
                print(bp.name);
                int[] d = new int[] { 0, 0, 0 }; int[] o = new int[] { cut, blunt, 0 };
                d = bp.parentBody.SendArmorNumbers(bp.bodyPartType);
                AttackResolution(bp, bp.parentBody.stats, stats.GetStat<RPGSkill>(RPGStatType.Pick), d, o);
            }
        }
        yield return new WaitForSeconds(2);
        rHandCollider.isTrigger = false;
    }

    void AxeChop()
    {
        rHandWeapon.collList[0].isTrigger = true;
        anim.SetTrigger("AxeChop");
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo.length);
        if (collisionList.Count >= 0)
        {
            int cut = rHandWeapon.baseCut * (1 + (stats.GetStat(RPGStatType.Axe).StatValue / 10));
            int blunt = rHandWeapon.itemWeight * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
            if (collisionList[0].transform.root.tag == "Tree")
            {
                Debug.LogWarning("2");

                collisionList[0].transform.root.GetComponent<Tree>().health -= (cut * blunt + 10);
            }
        }
    }

    void PickSwing()
    {
        rHandWeapon.collList[0].isTrigger = true;
        anim.SetTrigger("PickSwing");
        RaycastHit hit;
        if (Physics.Raycast(aimPoint.position, aimPoint.forward, out hit, 12f))
        {
            if (hit.transform.gameObject.layer == 19)
            {
                int pierce = rHandWeapon.basePierce * (1 + (stats.GetStat(RPGStatType.Pick).StatValue / 10));
                int blunt = (rHandWeapon.baseBlunt * 5 + rHandWeapon.itemWeight) * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;
                Block block = EditTerrain.GetBlock(hit);

                if (block is BlockGrass)
                {
                    EditTerrain.HitBlock(hit, pierce + (blunt / 2));
                    print(pierce + (blunt / 2));
                    return;
                }

                if (block is Block)
                {
                    EditTerrain.HitBlock(hit, pierce * blunt);
                    print(pierce * blunt);
                }
            }
            else if (hit.transform.tag == "BodyPart")
            {
                int pierce = rHandWeapon.basePierce * (1 + (stats.GetStat(RPGStatType.Pick).StatValue / 10));
                int blunt = (rHandWeapon.baseBlunt * 5 + rHandWeapon.itemWeight) * stats.GetStat(RPGStatType.Dexterity).StatValue * stats.GetStat(RPGStatType.Strength).StatValue;

                BodyPartColliderScript bp = hit.collider.GetComponent<BodyPartColliderScript>();
                int[] z = new int[] { 0, 0, 0 }; int[] o = new int[] { 0, blunt, pierce };
                z = bp.parentBody.SendArmorNumbers(bp.bodyPartType);
                bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= ((pierce / z[0]) + (blunt / z[1]));
                AttackResolution(bp, bp.parentBody.stats, stats.GetStat<RPGSkill>(RPGStatType.Pick), z, o);
            }
        }
    }

    void AttackResolution(BodyPartColliderScript bp, CreatureStats targetStats, RPGSkill attackSkill, int[] d, int[] o)
    {
        float dodgeChance = (targetStats.GetStat(RPGStatType.Dodge).StatValue * 3 + (targetStats.GetStat(RPGStatType.Agility).StatValue)) / (1 + 3 * bp.parentBody.encumbrance);
        float deflectChance = targetStats.GetStat(RPGStatType.Deflect).StatValue * 3 + targetStats.GetStat(RPGStatType.Agility).StatValue + targetStats.GetStat(RPGStatType.Strength).StatValue + d[2] * targetStats.GetStat(RPGStatType.Armor).StatValue;
        float absorbChance = .01f * (targetStats.GetStat(RPGStatType.Absorb).StatValue * 3 + targetStats.GetStat(RPGStatType.Agility).StatValue + 2 * targetStats.GetStat(RPGStatType.Strength).StatValue + d[2] * targetStats.GetStat(RPGStatType.Armor).StatValue + targetStats.GetStat(RPGStatType.Weight).StatValue);
        float hitChance = attackSkill.StatValue * 5 + stats.GetStat(RPGStatType.Agility).StatValue + stats.GetStat(RPGStatType.Strength).StatValue;
        float criticalChance = 0.001f * attackSkill.StatValue * attackSkill.StatValue + stats.GetStat(RPGStatType.Agility).StatValue + stats.GetStat(RPGStatType.Strength).StatValue;
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
            bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= (int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2])));
            print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
            print((int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2]))));

            return;
        }
        else
        {
            print("crit");

            bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).StatCurrentValue -= 4 * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2]));
        }


        //defense: defense skills, armor, encumbrance, stamina, energy, agility, strength, toughness

    }



    //bow and arrow stuff
    public bool wieldingBow, arrowKnocked, drawingBow, bowDrawn;
    public float startTime, nowTime;
    public float drawTime = 3f;
    public void KnockArrow()
    {
        if (!drawingBow && !(currentAmmo == null))
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
        if (drawingBow)
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
        if (drawingBow && bowDrawn && arrowKnocked)
        {
            bowDrawn = false;
            drawingBow = arrowKnocked = false;
            anim.SetTrigger("FireBow");
            anim.SetBool("DrawingBow", false);
            GameObject clone;
            clone = Instantiate(currentAmmo.gameObject, aimPoint) as GameObject;
            clone.transform.localPosition = Vector3.zero;
            clone.transform.parent = null;
            clone.GetComponent<Rigidbody>().velocity = aimPoint.TransformDirection(Vector3.forward * 100);
            clone.GetComponent<Item_Ammo>().numItems = 1;
            currentAmmo.ChangeNum(-1);
            return;
        }
        if (drawingBow && arrowKnocked)
        {
            bowDrawn = false;
            drawingBow = arrowKnocked = false;
            nowTime = Time.time;
            float speedFraction = (nowTime - startTime) / drawTime;
            anim.SetTrigger("FireBow");
            anim.SetBool("DrawingBow", false);
            GameObject clone;
            clone = Instantiate(currentAmmo.gameObject, aimPoint) as GameObject;
            clone.transform.localPosition = Vector3.zero;
            clone.transform.parent = null;
            clone.GetComponent<Rigidbody>().velocity = aimPoint.TransformDirection(Vector3.forward * 100 * speedFraction);
            clone.GetComponent<Item_Ammo>().numItems = 1;
            currentAmmo.ChangeNum(-1);
            return;
        }
    }

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
            if (newWearable.hasModel)
            {

            }
        }
    }

    public override void RemoveGarment(Item_Garment thisWearable)
    {
        foreach (RPGStatType partCovered in thisWearable.bodyPartCoverage)
        {
            RPGBodyPart bodyPart = stats.GetStat<RPGBodyPart>(partCovered);
            int[] myArr = new int[6];
            for (int i = 0; i < 5; i++)
            {
                bodyPart.protection[i] = bodyPart.protection[i] - thisWearable.protection[i];
            }
        }
        outfit.Remove(thisWearable);
        thisWearable.equipped = false;
    }

    public override void ProcessThisBody()
    {
        throw new NotImplementedException();
    }
}
