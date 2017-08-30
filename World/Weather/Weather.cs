using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weather : ScriptableObject {

    public int weatherID;
    public string weatherName;

    [Range(-40f, 50f)]
    public int globalTemperature;
    [Range(0f, 100f)]
    public int windSpeed;
    [Range(0f, 100f)]
    public int precipRate; //mm per hr : <2.5 is light, <7.5 is moderate, <50 is heavy, >50 is violent

    public GameObject weatherFX;
}
