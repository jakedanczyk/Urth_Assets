using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodyManager_Horse : BodyManager {

    public Flee fleeScript;
    public GameObject meatPrefab, hidePrefab;

    void Start()
    {
        isAttackingPrimary = guardRaised = sneaking = true;
        var health = stats.GetStat<RPGVital>(RPGStatType.Health);
        print(stats.GetStat<RPGVital>(RPGStatType.Health).StatName);
        health.OnCurrentValueChange += OnStatValueChange;
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
        }
    }

    public void RemoveGarment(Item_Garment garment)
    {

    }

    public override void ProcessThisBody()
    {
    }

    public void SheatheWeapon(Item_Weapon aWeapon)
    {
        if (!aWeapon.isWielded)
            return;
        Debug.LogWarning("Sheate weapon: " + aWeapon.itemName);
        aWeapon.GetComponent<Rigidbody>().isKinematic = false;
        aWeapon.GetComponent<Rigidbody>().useGravity = true;
        aWeapon.gameObject.SetActive(false);
    }

    public override void PrimaryAttack()
    {
        if (isAttackingPrimary) { return; }
        primaryAttackCollisionList.Clear();
        print("main attack");
        Kick();
    }


    void Kick()
    {

    }

    public new void Death()
    {
        base.Death();
        GetComponent<PreyAI>().enabled = false;
        lootInventory.AddItem(Instantiate(hidePrefab).GetComponent<Item>());
        for (int i = 0; i < stats.GetStat(RPGStatType.Weight).StatValue / 5000; i++)
            lootInventory.AddItem(Instantiate(meatPrefab).GetComponent<Item>());
    }
}
