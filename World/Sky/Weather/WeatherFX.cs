using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherFX : MonoBehaviour {

    Quaternion iniRot;
 
    void Start()
    {
        iniRot = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = iniRot;
    }
}
