using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item_Weapon_Hatchet : Item_Weapon {

    public Item_HatchetHead component_axeHead;
    public Item_Haft component_Haft;

    public Collider edgeCollider;
    public int woodChopBonus, woodSplitBonus;

    // Use this for initialization
    void Start ()
    {
        gripAdjust = new Vector3(.11f, -.02f, 0.09f);
        gripOrientation = new Vector3(106.2f, 104.03f, -5.08f);
        primaryMaterialType = component_axeHead.primaryMaterialType;
        //attackList.Add(new Item_Attack("Hammer", 3, 0, 3));
        //print(attackList[1].attackName);

    }

    public override float TreeChopRateFactor()
    {
        return base.TreeChopRateFactor() + woodChopBonus;
    }
}
