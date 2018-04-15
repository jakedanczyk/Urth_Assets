using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Thermometer : MonoBehaviour {

    public float temperature;
    public float radiatorTemp;
    public WeatherControl weatherSystem;
    public List<HeatZone> heatZones = new List<HeatZone>();
    public List<HeatRadiator> heatRadiators;
    public Collider col;

	// Use this for initialization
	void Start ()
    {
        if (LevelSerializer.IsDeserializing) return;
        temperature = 0;
        for(int i = 0; i < heatRadiators.Count; i++)
        {
            float dist = Vector3.Distance(heatRadiators[i].gameObject.transform.position, this.transform.position);
            temperature = Mathf.Max(heatRadiators[i].power / (dist * dist), heatRadiators[i].coreTemp);
        }
        float altitude = this.transform.position.y;
        print("thermo " + temperature);
        temperature += weatherSystem.weathers[weatherSystem.currentWeatherIndex].globalTemperature - (.0064f * (altitude - 2000)) + heatZones.Sum(temp => temp.temperatureDifference);
        InvokeRepeating("ReadTemp", 3, 3);
	}

    public float ReadTemp()
    {
        temperature = 0;
        for (int i = 0; i < heatRadiators.Count; i++)
        {
            float dist = Vector3.Distance(heatRadiators[i].gameObject.transform.position, this.transform.position);
            temperature = Mathf.Min(heatRadiators[i].power / (dist * dist), heatRadiators[i].coreTemp);
        }
        float altitude = this.transform.position.y;
        return temperature += weatherSystem.weathers[weatherSystem.currentWeatherIndex].globalTemperature - (.0064f * (altitude - 2000)) + heatZones.Sum(temp => temp.temperatureDifference);
    }
}
