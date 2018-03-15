using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// custom linker for humanoid body surface area. y = c1*((c2*x)^n1)*((c3*x)^n2)
/// y, x : linked stats
/// c1,c2,c3,n1,n2 : vars
/// </summary>
public class RPGStatLinkerBodySurfaceArea : IStatValueChange
{
    /// <summary>
    /// Trigger when the Value of the linkers change
    /// </summary>
    public event EventHandler OnValueChange;
    //public event EventHandler OnValueChange;

    /// <summary>
    /// The RPGStats linked to
    /// </summary>
    public RPGStat LinkedStat1 { get; private set; }
    public RPGStat LinkedStat2 { get; private set; }

    private float _c1,_c2,_c3,_n1,_n2;

    /// <summary>
    /// returns the ratio of the linked stat as the linker's value
    /// </summary>
    public int Value
    {
        get { return (int)(_c1 * Mathf.Pow(_c2 * LinkedStat1.StatValue, _n1) * Mathf.Pow(_c3 * LinkedStat2.StatValue, _n2)); }
    }

    /// <summary>
    /// Constructor that takes the linked stat and the ratio to use
    /// </summary>
    public RPGStatLinkerBodySurfaceArea(RPGStat stat1, RPGStat stat2, float c1, float c2, float c3, float n1, float n2)
    {
        LinkedStat2 = stat2;
        LinkedStat1 = stat1;

        IStatValueChange iValueChange1 = LinkedStat1 as IStatValueChange;
        IStatValueChange iValueChange2 = LinkedStat2 as IStatValueChange;
        if (iValueChange1 != null)
        {
            iValueChange1.OnValueChange += OnLinkedStatValueChange1;
        }
        if (iValueChange2 != null)
        {
            iValueChange2.OnValueChange += OnLinkedStatValueChange2;
        }

        _c1 = c1; _c2 = c2; _c3 = c3; _n1 = n1; _n2 = n2;
    }

    private void OnLinkedStatValueChange1(object stat, EventArgs args)
    {
        if (OnValueChange != null)
        {
            OnValueChange(this, null);
        }
    }
    private void OnLinkedStatValueChange2(object stat, EventArgs args)
    {
        if (OnValueChange != null)
        {
            OnValueChange(this, null);
        }
    }
}
