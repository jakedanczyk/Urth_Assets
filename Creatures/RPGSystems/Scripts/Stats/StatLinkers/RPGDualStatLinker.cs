using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RPGDualStatLinker : IDualStatValueChange {

    /// <summary>
    /// Trigger when the Values of the linkers change
    /// </summary>
    public event EventHandler OnValueChange_1;
    public event EventHandler OnValueChange_2;


    /// <summary>
    /// The RPGStats linked to by the stat linker
    /// </summary>
    public RPGStat LinkedStat_1 { get; private set; }
    public RPGStat LinkedStat_2 { get; private set; }


    /// <summary>
    /// Gets the value of the stat linker
    /// </summary>
    public abstract int Value{ get; }

    /// <summary>
    /// Basic Constructor. Listens to the Stat's OnValueChange
    /// event if the stat implements IStatValueChange.
    /// </summary>
    public RPGDualStatLinker(RPGStat stat_1, RPGStat stat_2)
    {
        LinkedStat_1 = stat_1;
        LinkedStat_2 = stat_2;


        IStatValueChange iValueChange_1 = LinkedStat_1 as IStatValueChange;
        IStatValueChange iValueChange_2 = LinkedStat_2 as IStatValueChange;

        if (iValueChange_1 != null || iValueChange_2 != null)
        {
            iValueChange_1.OnValueChange += OnLinkedStatValueChange;
            iValueChange_2.OnValueChange += OnLinkedStatValueChange;
        }
    }

    /// <summary>
    /// Listens to the LinkedStat's OnValueChange event if able to
    /// </summary>

    private void OnLinkedStatValueChange(object stat, EventArgs args)
    {
        if (OnValueChange_1 != null)
        {
            OnValueChange_1(this, null);
        }
        else if (OnValueChange_2 != null)
        {
            OnValueChange_2(this, null);
        }
    }
}
