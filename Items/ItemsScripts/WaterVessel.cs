using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WaterVessel  {

    float Capacity
    {
        get;
        set;
    }

    float Content
    {
        get;
        set;
    }

    bool Full
    {
        get;
        set;
    }

}
