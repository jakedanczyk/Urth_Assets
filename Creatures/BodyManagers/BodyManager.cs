using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class BodyManager : MonoBehaviour {

    public List<Item_Garment> outfit;
    public Inventory inventory; // all carried items
    public Transform aimPoint;

    public Animator anim;
    public Weapon mainWeapon;
    public CreatureStats stats;

    public List<Collider> collisionList;

    //status bools
    public bool attacking, guardRaised, sneaking;
    public bool alive = true;
    public bool butchered = false;
    

    //Attack pseudo-code
    //calc damage, speed from:(weapon, attributes, status, skill)
    //Damage:
    //Speed:
    //run animation, use rays and colliders to build collisionList
    //if collision
    //      object type... Terrain, Construction, Item, SmallPlant, Tree, BodyPart ?
    //Terrain... Hard, Compact, Loose

    void Start()
    {
        attacking = guardRaised = sneaking = true;
        var health = stats.GetStat<RPGVital>(RPGStatType.Health);
        print(stats.GetStat<RPGVital>(RPGStatType.Health).StatName);
        health.OnCurrentValueChange += OnStatValueChange;
    }

    void Update()
    {

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
        }
    }

    public abstract void RemoveGarment(Item_Garment aGarment);

    public void PickupItem(Item anItem)
    {
        inventory.AddItem(anItem);
        //anItem.loose = false;
        //        anItem.transform.SetParent(transform.parent);
        //anItem.gameObject.SetActive(false);
        //Destroy(hit.collider.gameObject);
    }

    public void DropItem(Item anItem)
    {
        print(1);
        if(anItem is Item_Garment)
        {
            RemoveGarment((Item_Garment)anItem);
        }
        inventory.selectedItem = null;
        inventory.RemoveItem(anItem);
        anItem.loose = true;
        anItem.gameObject.SetActive(true);
        anItem.transform.position = this.transform.position;
        anItem.transform.parent = null;
    }

    public int[] SendArmorNumbers(RPGStatType bodyPart)
    {
        int[] z = new int[] { 0, 0, 0 };
        z[0] = stats.GetStat<RPGBodyPart>(bodyPart).protection[0];
        z[1] = stats.GetStat<RPGBodyPart>(bodyPart).protection[1];
        z[2] = stats.GetStat<RPGBodyPart>(bodyPart).protection[2];
        return z;
   }

    public float encumbrance = 0; //percent 
    public float carryWeight;
    void Encumbrance()
    {
        encumbrance = ((outfit.Select(c => c.itemWeight).ToList().Sum() * .25f) + inventory.SumWeight()) / stats.GetStat<RPGAttribute>(RPGStatType.CarryWeight).StatValue;
        carryWeight = stats.GetStat<RPGAttribute>(RPGStatType.CarryWeight).StatValue;
    }

    public void Dodge() { }

    public void Deflect() { }

    public void Absorb() { }

    public List<GameObject> butcheringReturns; // Outside in. Feathers, skin/exoskeloton, fat, muscle, organs, bones
    public int butcherTime; // seconds
    public int butcherSkillFactor; // how variable is quality?
    public abstract void ProcessThisBody(); 
}
