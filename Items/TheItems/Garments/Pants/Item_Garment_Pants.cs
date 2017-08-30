using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Garment_Pants : Item_Garment {

	// Use this for initialization
	void Start () {
        protection = new int[] { 3, 3, 3, 5, 5, 5};
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
