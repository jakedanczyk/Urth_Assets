using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon_PickMattock : Item_Weapon {

    public Collider edgeCollider;

    //public List<Item> componentNeeds = new List<Item> { Item_HatchetHead, Item_ShortHaft}

    // Use this for initialization
    void Start()
    {
        gripAdjust = new Vector3(.077f, -.02f, 0.09f);
        gripOrientation = new Vector3(82.28f, 27.9f, -13.7f);
        //attackList.Add(new Item_Attack("Hammer", 3, 0, 3));
        //print(attackList[1].attackName);

    }
}
