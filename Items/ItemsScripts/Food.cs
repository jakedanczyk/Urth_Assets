using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Food {

    [HideInInspector]
    [SerializeField]
    float Calories
    {
        get;
        set;
    }

    [HideInInspector]
    [SerializeField]
    float Protein
    {
        get;
        set;
    }
    [HideInInspector]
    [SerializeField]
    float Fat
    {
        get;
        set;
    }
    [HideInInspector]
    [SerializeField]
    float Carb
    {
        get;
        set;
    }
    [HideInInspector]
    [SerializeField]
    float Water
    {
        get;
        set;
    }
}
