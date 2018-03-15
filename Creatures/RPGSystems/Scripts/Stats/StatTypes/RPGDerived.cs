using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// RPGStat that inherits from RPGStatModifiable and implements IStatScalable, IStatLinkable, and IStatCurrentValueChange.
/// Derived from attributes, skills, and active effects
/// </summary>
public class RPGDerived : RPGStatModifiable, IStatScalable, IStatLinkable, IStatCurrentValueChange
{
    /// <summary>
    /// Used By StatLevelValue Property
    /// </summary>
    private int _statLevelValue;

    /// <summary>
    /// Used By StatLinker Value Property
    /// </summary>
    private int _statLinkerValue;

    /// <summary>
    /// List of all stat linkers applied to the stat
    /// </summary>
    private List<RPGStatLinker> _statLinkers;

    private RPGStatLinkerBodySurfaceArea _bodySurfaceAreaLinker;

    /// <summary>
    /// Add a linker to the stat and listen to it's valueChange event
    /// </summary>
    public void RPGStatLinkerBodySurfaceArea(RPGStatLinkerBodySurfaceArea linker)
    {
        _bodySurfaceAreaLinker = linker;
        linker.OnValueChange += OnLinkerValueChange;
    }

    /// <summary>
    /// Update the StatLinkerValue based of the currently applied stat linkers
    /// </summary>
    public void UpdateBodySurfaceArea()
    {
        _statLinkerValue = 0;
        _statLinkerValue += _bodySurfaceAreaLinker.Value;
        TriggerValueChange();
    }

    private int _aptitude;
    private int _training;

    /// <summary>
    /// The value gained by the ScaledStat method
    /// </summary>
    public int StatLevelValue
    {
        get { return _statLevelValue; }
    }

    /// <summary>
    /// the value gained from all applied stat linkers
    /// </summary>
    public int StatLinkerValue
    {
        get { return _statLinkerValue; }
    }

    /// <summary>
    /// Gets the stat base value with the StatLevelValue and StatLinkerValue added
    /// </summary>
    public override int StatBaseValue
    {
        get { return base.StatBaseValue + StatLevelValue + StatLinkerValue; }
    }

    /// <summary>
    /// Overridable method that scales the class based off the passed value
    /// Triggers the stat's Value Change event
    /// </summary>
    public virtual void ScaleStat(int level)
    {
        _statLevelValue = level;
        TriggerValueChange();
    }

    /// <summary>
    /// Add a linker to the stat and listen to it's valueChange event
    /// </summary>
    public void AddLinker(RPGStatLinker linker)
    {
        _statLinkers.Add(linker);
        linker.OnValueChange += OnLinkerValueChange;
    }

    /// <summary>
    /// Removes a linker from the stat and stops listening to the value change event
    /// </summary>
    /// <param name="linker"></param>
    public void RemoveLinker(RPGStatLinker linker)
    {
        _statLinkers.Remove(linker);
        linker.OnValueChange -= OnLinkerValueChange;
    }

    /// <summary>
    /// Removes all linkers from the stat
    /// </summary>
    public void ClearLinkers()
    {
        foreach (var linker in _statLinkers)
        {
            linker.OnValueChange -= OnLinkerValueChange;
        }
        _statLinkers.Clear();
    }

    /// <summary>
    /// Update the StatLinkerValue based of the currently applied stat linkers
    /// </summary>
    public void UpdateLinkers()
    {
        _statLinkerValue = 0;
        foreach (RPGStatLinker link in _statLinkers)
        {
            _statLinkerValue += link.Value;
        }
        TriggerValueChange();
    }

    /// <summary>
    /// Basic Constructor
    /// </summary>
    public RPGDerived()
    {
        _statLinkers = new List<RPGStatLinker>();
        _statCurrentValue = 0;
    }

    /// <summary>
    /// Listens to the attached StatLinkers and Updates the StatLinkerValue if
    /// a stat linker value changes
    /// </summary>
    private void OnLinkerValueChange(object linker, EventArgs args)
    {
        UpdateLinkers();
    }

    public int TrainingValue
    {
        get { return _training; }
        set { _training = value; }
    }

    public int Aptitude
    {
        get { return _aptitude; }
        set { _aptitude = value; }
    }


    /// <summary>
    /// Used by the StatCurrentValue Property
    /// </summary>
    private int _statCurrentValue;

    /// <summary>
    /// Event called when the StatCurrentValue changes
    /// </summary>
    public event EventHandler OnCurrentValueChange;

    /// <summary>
    /// The current value of the stat. When set will trigger the OnCurrentValueChange event.
    /// </summary>
    public int StatCurrentValue
    {
        get
        {
            return _statCurrentValue;
        }
        set
        {
            if (_statCurrentValue != value)
            {
                _statCurrentValue = value;
                TriggerCurrentValueChange();
            }
        }
    }


    /// <summary>
    /// Sets the StatCurrentValue to StatValue
    /// </summary>
    public void SetCurrentValueToMax()
    {
        StatCurrentValue = StatValue;
    }

    /// <summary>
    /// Triggers the OnCurrentValueChange Event
    /// </summary>
    private void TriggerCurrentValueChange()
    {
        if (OnCurrentValueChange != null)
        {
            OnCurrentValueChange(this, null);
        }
    }
}
