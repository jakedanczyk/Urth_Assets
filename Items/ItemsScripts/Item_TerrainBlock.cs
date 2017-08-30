using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item_TerrainBlock : Item {

    public Block block;

    void Start()
    {
        matDict = FindObjectOfType<ItemMaterialsDictionary>();
        MassCalc();
    }
}
