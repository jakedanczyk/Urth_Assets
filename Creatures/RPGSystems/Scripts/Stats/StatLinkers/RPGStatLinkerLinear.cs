using UnityEngine;
using System.Collections;

/// <summary>
/// linear implementation of a RPGStatLinker. y = a*(x+b)
/// y, x : linked stats
/// a,b: vars
/// </summary>
public class RPGStatLinkerLinear : RPGStatLinker
{
    // a,b
    private float _ratio;
    private float _offset;

    /// <summary>
    /// returns the ratio of the linked stat as the linker's value
    /// </summary>
    public override int Value
    {
        get { return (int)(_ratio * (LinkedStat.StatValue + _offset)); }
    }

    /// <summary>
    /// Constructor that takes the linked stat and the ratio to use
    /// </summary>
    public RPGStatLinkerLinear(RPGStat stat, float ratio, float offset)
        : base(stat)
    {
        _ratio = ratio;
        _offset = offset;
    }
}
