using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Stack : Item {

    public int weightPerItem, volumePerItem;
    public int numItems;

    void Start()
    {
        TotalProperties();
    }

    public void ChangeNum(int num)
    {
        numItems += num;
        if(numItems > 0)
        TotalProperties();
        else DestroyObject(this.gameObject);
    }

    public void TotalProperties()
    {
        itemWeight = numItems * weightPerItem;
        itemVolume = numItems * volumePerItem;
    }
}
