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

    public float LS_CloudTimeScale = 2;
    public float LS_CloudScale = 4;
    public float LS_CloudScattering = 0.6f;
    public float LS_CloudIntensity = 4;
    public float LS_CloudSharpness = 0.75f;
    public float LS_CloudThickness = 1.0f;
    public float LS_ShadowScale = 0.75f;
    public float LS_DistScale = 10.0f;
    public Vector3 LS_CloudColor = new Vector3(1, 0.9f, 0.95f);
}
