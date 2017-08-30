using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGDualStatLinkerLinear : RPGDualStatLinker {

    // a,b
    private float _ratio;

    /// <summary>
    /// returns the ratio of the linked stat as the linker's value
    /// </summary> 
    public override int Value
    {
        get { return (int)(_ratio * LinkedStat_1.StatBaseValue * LinkedStat_2.StatBaseValue); }
    }


    /// <summary>
    /// Constructor that takes the linked stat and the ratio to use
    /// </summary>
    public RPGDualStatLinkerLinear(RPGStat stat_1, RPGStat stat_2, float ratio)
        : base(stat_1, stat_2)
    {
        _ratio = ratio;
    }
}