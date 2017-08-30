using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// logarithmic stat linker: y = a*ln(x)+k
/// </summary>
public class RPGStatLinkerLog : RPGStatLinker {
    /// <summary>
    /// ratio a, constant k
    /// </summary>
    private float _ratio;
    private float _constant;

    /// <summary>
    /// returns the ratio of the linked stat as the linker's value
    /// </summary>
    public override int Value
    {
        get { return (int)((Mathf.Log(LinkedStat.StatValue) * _ratio) + _constant); }
    }

    /// <summary>
    /// Constructor that takes the linked stat, the ratio a, and the constant k
    /// </summary>
    public RPGStatLinkerLog(RPGStat stat, float ratio, float constant)
        : base(stat) {
        _ratio = ratio;
        _constant = constant;
    }
}
