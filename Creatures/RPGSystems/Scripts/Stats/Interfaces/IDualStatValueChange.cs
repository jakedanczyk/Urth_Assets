using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Used to indicate when the stat's value changes
/// </summary>
public interface IDualStatValueChange
{
    event EventHandler OnValueChange_1;
    event EventHandler OnValueChange_2;
}