using UnityEngine;
using System.Collections;

/// <summary>
/// power function of a RPGStatLinker. y = c2*(c1*x)^n
/// y, x : linked stats
/// c1: preRatio
/// c2: postRatio
/// n: exponent
/// </summary>
public class RPGStatLinkerPower : RPGStatLinker
{
    // n
    private float _preRatio;
    private float _postRatio;
    private float _power;

    /// <summary>
    /// returns the ratio of the linked stat as the linker's value
    /// </summary>
    public override int Value
    {
        get { return (int)(_postRatio * Mathf.Pow(_preRatio * LinkedStat.StatValue, _power)); }
    }

    /// <summary>
    /// Constructor that takes the linked stat and the ratio to use
    /// </summary>
    public RPGStatLinkerPower(RPGStat stat, float preRatio, float postRatio, float power)
        : base(stat)
    {
        _preRatio = preRatio;
        _postRatio = postRatio;
        _power = power;
    }
}
