using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Event Args displaying a gain in experience
/// </summary>
public class RPGRestingEventArgs : EventArgs
{
    /// <summary>
    /// The gain in experience, can be positive or negative.
    /// </summary>
    public int TrainingLevel { get; private set; }
    public int RestQuantity { get; private set; }

    /// <summary>
    /// Basic Constructor takes the experienced gained
    /// </summary>
    public RPGRestingEventArgs(int trainingLevel, int restQuantity)
    {
        TrainingLevel = trainingLevel;
        RestQuantity = restQuantity;
    }
}

