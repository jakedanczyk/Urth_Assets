using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public BodyManager shooter;
    public Item_Ammo ammo;
    
    public Projectile(BodyManager_Human x) { shooter = x; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Terrain")
        {

            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (other.transform.tag == "BodyPart")
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.SetParent(other.gameObject);
            BodyPartColliderScript hitBodyPart = other.GetComponent<BodyPartColliderScript>();
            int[] d = new int[] { 0, 0, 0 };
            d = hitBodyPart.parentBody.SendArmorNumbers(hitBodyPart.bodyPartType);

            int[] o = new int[] { ammo.baseBlunt, ammo.baseCut, ammo.basePierce };

            AttackResolution(hitBodyPart, hitBodyPart.parentBody.stats, d, o);
        }
    }

    void AttackResolution(BodyPartColliderScript bp, CreatureStats targetStats, int[] d, int[] o)
    {
        print(bp.bodyPartType);
        print(targetStats.name);
        print(d);
        print(o);
        float dodgeChance = (targetStats.GetStat(RPGStatType.Dodge).StatValue * 3 + (targetStats.GetStat(RPGStatType.Agility).StatValue)) / (1 + 3 * bp.parentBody.encumbrance);
        print(3);
        float deflectChance = targetStats.GetStat(RPGStatType.Deflect).StatValue * 3 + targetStats.GetStat(RPGStatType.Agility).StatValue + targetStats.GetStat(RPGStatType.Strength).StatValue + d[2] * targetStats.GetStat(RPGStatType.Armor).StatValue;
        float absorbChance = targetStats.GetStat(RPGStatType.Absorb).StatValue * 3 + targetStats.GetStat(RPGStatType.Agility).StatValue + 2 * targetStats.GetStat(RPGStatType.Strength).StatValue + d[2] * targetStats.GetStat(RPGStatType.Armor).StatValue + targetStats.GetStat(RPGStatType.Weight).StatValue;
        float hitChance = shooter.stats.GetStat<RPGSkill>(RPGStatType.Bow).StatValue * 5 + shooter.stats.GetStat(RPGStatType.Agility).StatValue + shooter.stats.GetStat(RPGStatType.Strength).StatValue;
        float criticalChance = 0.001f * shooter.stats.GetStat<RPGSkill>(RPGStatType.Bow).StatValue * shooter.stats.GetStat<RPGSkill>(RPGStatType.Bow).StatValue + shooter.stats.GetStat(RPGStatType.Agility).StatValue + shooter.stats.GetStat(RPGStatType.Strength).StatValue;
        print(4);

        float sum = dodgeChance + deflectChance + absorbChance + hitChance + criticalChance;
        print(5);

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
            shooter.stats.GetStat<RPGSkill>(RPGStatType.Bow).GainXP(10);
            return;
        }
        else
        {
            print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
            bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue -= 4 * (int)(bp.parentBody.stats.GetStat<RPGBodyPart>(bp.bodyPartType).damageModifer * ((o[0] / d[0]) + (o[1] / d[1]) + (o[2] / d[2])));
        }
        print(bp.parentBody.stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue);
        //defense: defense skills, armor, encumbrance, stamina, energy, agility, strength, toughness
        enabled = false;
    }
}