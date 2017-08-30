using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Stack : Item {

    public int weightPerItem;
    public int numItems;

    void Start()
    {
        WeightCalc();
    }

    public void ChangeNum(int num)
    {
        numItems += num;
        if(numItems > 0)
        WeightCalc();
        else DestroyObject(this.gameObject);
    }

    public void WeightCalc()
    {
        itemWeight = numItems * weightPerItem;
    }
}
