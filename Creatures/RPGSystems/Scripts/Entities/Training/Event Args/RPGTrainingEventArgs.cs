using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Event Args displaying a gain in experience
/// </summary>
public class RPGTrainingEventArgs : EventArgs
{
    /// <summary>
    /// The gain in experience, can be positive or negative.
    /// </summary>
    public int TrainingLevelsGained { get; private set; }

    /// <summary>
    /// Basic Constructor takes the experienced gained
    /// </summary>
    public RPGTrainingEventArgs(int trainingLevelsGained)
    {
        TrainingLevelsGained = trainingLevelsGained;
    }
}

