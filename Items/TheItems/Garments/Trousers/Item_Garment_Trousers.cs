using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Garment_Trousers : Item_Garment {

	// Use this for initialization
	void Start () {
        bodyPartCoverage.Add(RPGStatType.Pelvis);
        bodyPartCoverage.Add(RPGStatType.LeftThigh);
        bodyPartCoverage.Add(RPGStatType.RightThigh);
        bodyPartCoverage.Add(RPGStatType.LeftCalf);
        bodyPartCoverage.Add(RPGStatType.RightCalf);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
