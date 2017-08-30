﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_Garment : Item {

    public List<RPGStatType> bodyPartCoverage;
    public int[] protection = new int[] { 0, 0, 0, 0, 0, 0 }; // Protection: blunt, cut, pierce, insulation, water, wind 
    public bool hasModel;
    public string modelID;
    public bool equipped;

    void Wear(BodyManager_Human bodyManager)
    {
        bodyManager.outfit.Add(this);
    }

}
