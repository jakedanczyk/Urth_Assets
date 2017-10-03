﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Thermometer : MonoBehaviour {

    public float temperature;
    public float radiatorTemp;
    public WeatherControl weatherSystem;
    public List<HeatZone> heatZones;
    public List<HeatRadiator> heatRadiators;
    public Collider col;

	// Use this for initialization
	void Start ()
    {
        for(int i = 0; i < heatRadiators.Count; i++)
        {
            float dist = Vector3.Distance(heatRadiators[i].gameObject.transform.position, this.transform.position);
            temperature = Mathf.Max(heatRadiators[i].power / (dist * dist), heatRadiators[i].coreTemp);
        }
        float altitude = this.transform.position.y;
        temperature += weatherSystem.currentWeather.globalTemperature - (.0016f * (altitude - 8000)) + heatZones.Sum(temp => temp.temperatureDifference);
        InvokeRepeating("ReadTemp", 6, 6);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float ReadTemp()
    {
        for (int i = 0; i < heatRadiators.Count; i++)
        {
            float dist = Vector3.Distance(heatRadiators[i].gameObject.transform.position, this.transform.position);
            temperature = Mathf.Max(heatRadiators[i].power / (dist * dist), heatRadiators[i].coreTemp);
        }
        float altitude = this.transform.position.y;
        return temperature += weatherSystem.currentWeather.globalTemperature + heatZones.Sum(temp => temp.temperatureDifference);
    }
}
