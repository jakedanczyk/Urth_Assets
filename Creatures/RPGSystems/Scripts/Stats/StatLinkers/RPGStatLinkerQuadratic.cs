using UnityEngine;
using System.Collections;

/// <summary>
/// quadratic polynomial implementation of a RPGStatLinker. y = a*x^2+b
/// y, x : linked stats
/// a,b: vars
/// </summary>
public class RPGStatLinkerQuadratic : RPGStatLinker
{
    // a,b,c
    private float _ratio;
    private float _offset;

    /// <summary>
    /// returns the ratio of the linked stat as the linker's value
    /// </summary>
    public override int Value
    {
        get { return (int)(_ratio * (LinkedStat.StatValue * LinkedStat.StatValue) + _offset); }
    }

    /// <summary>
    /// Constructor that takes the linked stat and the ratio to use
    /// </summary>
    public RPGStatLinkerQuadratic(RPGStat stat, float ratio, float offset)
        : base(stat)
    {
        _ratio = ratio;
        _offset = offset;
    }
}
