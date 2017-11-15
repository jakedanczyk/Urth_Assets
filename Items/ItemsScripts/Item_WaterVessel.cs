using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_WaterVessel : Item, WaterVessel {

    public float capacity;
    public float content;
    public bool full;

    public float Capacity
    {
        get
        {
            return capacity;
        }
        set
        {
            capacity = value;
        }
    }

    public float Content
    {
        get
        {
            return content;
        }
        set
        {
            content = value;
        }
    }

    public bool Full
    {
        get
        {
            return full;
        }
        set
        {
            full = value;
        }
    }

    public void Fill(int mlFluid)
    {
        content = content + mlFluid;
        if (content >= capacity)
        {
            content = capacity;
            full = true;
        }
        else
        {
            full = false;
            if (content < 0)
                content = 0;
        }
        itemName = itemName.Substring(0, 10) + content + "mL";
        itemUIElementScript.GetComponentInChildren<Text>().text = itemName;
    }
}
