using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Stack_GreenBoughs : Item_Stack {

	// Use this for initialization
	void Start () {
        weightPerItem = 50;
        WeightCalc();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public Item_Stack_GreenBoughs(int num)
    {
        numItems = num;
        WeightCalc();
    }


}
