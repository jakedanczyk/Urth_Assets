using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item_TerrainBlock : Item {

    public Block1 block;

    public BlockType blockType;

    void Start()
    {
        MassCalc();
    }
}
