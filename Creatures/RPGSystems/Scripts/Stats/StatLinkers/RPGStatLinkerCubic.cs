using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// cubic polynomial implementation of a RPGStatLinker. y = a*x^3+b*x^2+c*x+d
/// y, x : linked stats
/// a,b,c,d: vars
/// </summary>
public class RPGStatLinkerCubic : RPGStatLinker {

    // a,b,c,d
    private float _ratio_a, _ratio_b, _ratio_c, _offset;

    /// <summary>
    /// returns the ratio of the linked stat as the linker's value
    /// </summary>
    public override int Value
    {
        get { return (int)(_ratio_a * (LinkedStat.StatValue * LinkedStat.StatValue * LinkedStat.StatValue) + _offset); }
    }

    /// <summary>
    /// Constructor that takes the linked stat and the ratio to use
    /// </summary>
    public RPGStatLinkerCubic(RPGStat stat, float ratio_a, float ratio_b, float ratio_c, float offset)
        : base(stat)
    {
        _ratio_a = ratio_a;
        _ratio_b = ratio_b;
        _ratio_c = ratio_c;
        _offset = offset;
    }
}