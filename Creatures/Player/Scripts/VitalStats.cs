using UnityEngine;
using System.Collections;

public class VitalStats : MonoBehaviour {

    float localTemperature;
    [Range(0f,50f)]
    public float coreTemp;
    float coreTempBase = 37f;
    public WeatherControl weatherSystem;
    public Weather currentWeather;
    // public Equipment playerEquipment;
    public RPGPlayer rpgPlayer;

    
    


	// Use this for initialization
	void Start () {

        coreTemp = coreTempBase;
        currentWeather = weatherSystem.currentWeather;
    }

    // Update is called once per frame
    void Update () {
        currentWeather = weatherSystem.currentWeather;

        LocalTemperature();
        CoreTemp();
	}


    public float LocalTemperature()
    {
        float altitude = this.transform.position.y;
        localTemperature = currentWeather.globalTemperature - (.0016f * (altitude - 8000));
        return localTemperature;
        
    }

    public float CoreTemp()
    {
        float coolingFactor = coreTemp * (1+0.001f*(coreTemp - localTemperature + 10));
        float heatingFactor = coreTemp * (1 + 0.001f * (coreTemp - localTemperature + 5));
        if (localTemperature < 15)
        { coreTemp = coreTemp + coolingFactor;
            return coreTemp;
        }

        if (localTemperature > 32)
        { coreTemp = coreTemp + heatingFactor;
            return coreTemp;
        }
        else return coreTemp;

    }
    void OnGUI()
    {

        GUI.Label(new Rect(0, 400, 500, 500), "Core Temp: " + coreTemp);
        GUI.Label(new Rect(0, 425, 500, 500), "Local Temp: " + localTemperature);
    }
}
