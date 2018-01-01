using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_Garment : Item {

    [SerializeField]
    public List<RPGStatType> bodyPartCoverage;
    public int[] protection = new int[] { 0, 0, 0, 0, 0, 0 }; // Protection: blunt, cut, pierce, insulation, water, wind 
    public bool hasModel;
    public string modelID;
    public bool equipped;
    public Material material;
    public Texture texture;

    void Wear(BodyManager_Human bodyManager)
    {
        bodyManager.outfit.Add(this);
    }

}
