using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

//[System.Serializable]
public class WeatherControl : MonoBehaviour {
    public static GameObject manager;

    [SerializeField]
    float localTemperature;
    [SerializeField]
    float areaTemperature;
    public List<Weather> weathers;
    public List<GameObject> weatherFX;
    public List<Material> skyboxMaterials;
    public BodyManager_Human_Player player_bodyManager;

    public int weatherDice;

    public Light sun;
    public Light moon;
    public GameObject player;
    public int currentWeatherIndex;
    public GameObject currentWeatherFX;
    public int currentDuration, currentStart;
    public WorldTime time;
    public ShelterCheck shelterCheck;

    public int yearDay;
    public float dayHour, monthDay;
    public float precipRate, windSpeed;
    public bool night, dawn, day, dusk;

    public List<int> schedule;
    public List<int> durations;

    //Clouds
    public LazyClouds lazyCloudsCirrus;
    public SCS cumulus;
    public GameObject lowClouds;

    private void Awake()
    {
        manager = this.gameObject;

        if (LevelSerializer.IsDeserializing) return;

        currentWeatherIndex = Random.Range(0, 4);

        currentStart = 0;
        currentDuration = 300 + Random.Range(0, 36000);
        precipRate = weathers[currentWeatherIndex].precipRate;
        windSpeed = weathers[currentWeatherIndex].windSpeed;
        if (weathers[currentWeatherIndex].areLowClouds)
        {
            lazyCloudsCirrus.gameObject.SetActive(false);
            lowClouds.SetActive(true);
        }
        else
        {
            lazyCloudsCirrus.LS_CloudScattering = weathers[currentWeatherIndex].LS_CloudScattering;
            lowClouds.SetActive(false);
        }
        RenderSettings.fogDensity = weathers[currentWeatherIndex].globalFog;
    }

    // Use this for initialization
    void Start ()
    {
        player = BodyManager_Human_Player.playerObject;
        if (LevelSerializer.IsDeserializing) return;
        if (currentWeatherFX == null)
            currentWeatherFX = Instantiate<GameObject>(weathers[currentWeatherIndex].weatherFX, player.transform);

        //{
        //    if (currentWeatherFX != null)
        //    {
        //        Destroy(currentWeatherFX);
        //    }
        //    if (player == null)
        //    {
        //        player = GameObject.FindGameObjectWithTag("Player").transform.root.gameObject;
        //    }
        //    currentWeather = schedule[0];
        //    print("Weather control Player: " + player.name + "||weather" + currentWeather.name + "    fx||" + currentWeather.weatherFX.name);
        //    currentWeatherFX = Instantiate(currentWeather.weatherFX, player.transform);
        //    return;
        //}

        schedule.Add(Random.Range(0, 4));
        schedule.Add(Random.Range(0, 4));
        schedule.Add(Random.Range(0, 4));
        schedule.Add(Random.Range(0, 4));
        schedule.Add(Random.Range(0, 4));
        schedule.Add(Random.Range(0, 4));
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
    }

    // Update is called once per frame
    void Update () {
        if(currentWeatherFX == null)
            currentWeatherFX = Instantiate<GameObject>(weathers[currentWeatherIndex].weatherFX, player.transform);
        dayHour = time.hours % 24;
        UpdateSun();
        UpdateMoon();
        if (day && dayHour > 18)
        {
            RenderSettings.skybox = skyboxMaterials[1];
            day = false;
            night = true;
        }
        else if (night && dayHour > 6 && dayHour < 17.5)
        {
            RenderSettings.skybox = skyboxMaterials[0];
            day = true;
            night = false;
        }
        if(time.totalGameSeconds > (currentStart + currentDuration))
        {
            NextWeather();
        }
        if (shelterCheck.roof && shelterCheck.walls)
        {
            currentWeatherFX.SetActive(false);
            precipRate = windSpeed = 0;
        }
        else if (!shelterCheck.roof && !shelterCheck.walls && !currentWeatherFX.activeSelf)
        {
            currentWeatherFX.SetActive(true);
            precipRate = weathers[schedule[0]].precipRate;
            windSpeed = weathers[schedule[0]].windSpeed;
        }
	}

    public float Temperature
    {
        get { return localTemperature; }
        set { localTemperature = value; }
    }

    void NextWeather()
    {
        if(currentWeatherFX != null)
            Destroy(currentWeatherFX);
        currentWeatherIndex = schedule[0];
        currentWeatherFX = Instantiate<GameObject>(weathers[schedule[0]].weatherFX, player.transform);
        currentStart =(int) time.totalGameSeconds;
        currentDuration = durations[0];
        precipRate = weathers[schedule[0]].precipRate;
        windSpeed = weathers[schedule[0]].windSpeed;
        if (weathers[currentWeatherIndex].areLowClouds)
        {
            lazyCloudsCirrus.gameObject.SetActive(false);
            lowClouds.SetActive(true);
        }
        else
        {
            lazyCloudsCirrus.LS_CloudScattering = weathers[currentWeatherIndex].LS_CloudScattering;
            lowClouds.SetActive(false);
        }
        schedule.RemoveAt(0);
        durations.RemoveAt(0);
        schedule.Add(Random.Range(0, 4));
        durations.Add(300 + Random.Range(0, 36000));
        RenderSettings.fogDensity = weathers[currentWeatherIndex].globalFog;
    }

    void UpdateSun()
    {
        sun.transform.eulerAngles = new Vector3((dayHour - 6) / 12 * 180, 90, 0);
        if (dayHour > 18 || dayHour < 6)
        {
            sun.intensity = 0;
        }
        else sun.intensity = 1;
    }

    void UpdateMoon()
    {
        //moon.transform.eulerAngles = new Vector3((monthDay) / 1.02f *  180, 90, 0);
        moon.transform.eulerAngles = new Vector3((dayHour - 6) / 13 * 180, 90, 0);
    }

    void SunPosition()
    {
        float pi = 3.14159265f;
        float y = (2 * pi / 365) * (yearDay - 1 + (dayHour - 12) / 24);
        float eqtime = 229.18f * (0.000075f + 0.001868f * Mathf.Cos(y) - 0.032077f * Mathf.Sin(y) - 0.014615f * Mathf.Cos(2 * y) - 0.040849f * Mathf.Sin(2 * y));
        float declin = 0.006918f - 0.399912f * Mathf.Cos(y) + 0.070257f * Mathf.Sin(y) - 0.006758f * Mathf.Cos(2 * y) + 0.000907f * Mathf.Sin(2 * y) - 0.002697f * Mathf.Cos(3 * y) + 0.00148f * Mathf.Sin(3 * y);
    }

    public float sunriseTime;
    void SunriseTime() { }

    void SunsetTime() { }

    void ClearDay() { }

    void OverCastDay() { }

    void StormDay() { }

    void ClearDusk() { }

    void OverCastDusk() { }

    void StormDusk() { }

    void ClearNight() { }

    void OvercastNight() { }

    void ClearDawn() { }

    void OvercastDawn() { }

    void StormDawn() { }

    void OnDeserialize()
    {
        currentWeatherFX = Instantiate<GameObject>(weathers[currentWeatherIndex].weatherFX, player.transform);
    }
}
