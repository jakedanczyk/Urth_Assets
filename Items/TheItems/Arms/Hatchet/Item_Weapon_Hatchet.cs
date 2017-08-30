﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item_Weapon_Hatchet : Item_Weapon {

    public Item_HatchetHead component_axeHead;
    public Item_Haft component_Haft;

    public Collider edgeCollider;

    //public List<Item> componentNeeds = new List<Item> { Item_HatchetHead, Item_ShortHaft}

    void Awake()
    {

        //attackList.Add(new Item_Attack("Chop", 3, 0, 3));
        //print(attackList[0].attackName);
        //governingSkill = RPGStatType.Axe;
    }


    // Use this for initialization
    void Start ()
    {
        gripAdjust = new Vector3(-.35f, .3f, 0);
        gripOrientation = new Vector3(98.36f, 104.03f, 25f);
        primaryMaterialType = component_axeHead.primaryMaterialType;
        //attackList.Add(new Item_Attack("Hammer", 3, 0, 3));
        //print(attackList[1].attackName);

    }

    // Update is called once per frame
    void Update () {
		
	}

    void Chop() 
    {
        edgeCollider.isTrigger = true;

    }
}
