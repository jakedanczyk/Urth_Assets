using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeAll]
public class SkyControl : MonoBehaviour {

    public static GameObject SkyControlObject;

    public GameObject gasGiant, urthOrbit, urth, moon, youAreHere;
    public WorldTime worldTime;
    float prevTime;
    public Light planetLight, sunLight, moonLight;
    public Material stars;


    [SerializeField]
    float localTemperature;
    public List<Weather> weathers;
    public List<GameObject> weatherFX;
    public List<Material> skyboxMaterials;
    public BodyManager_Human_Player player_bodyManager;

    public int weatherDice;

    public Light sun;
    public Light planet;
    public GameObject player;
    public int currentWeatherIndex;
    public GameObject currentWeatherFX, starSphere;
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
    public CloudsToy cloudsToy;

    private void Awake()
    {
        SkyControlObject = gameObject;
    }

    // Use this for initialization
    void Start () {
        lowCloudsMat = lowClouds.GetComponent<Renderer>().material;
        highCloudsMat = highClouds.GetComponent<Renderer>().material;
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
        durations.Add(1);
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
        durations.Add(300 + Random.Range(0, 36000));
    }

    void Update()
    {
        if (LevelSerializer.IsDeserializing) return;

        urthOrbit.transform.eulerAngles = new Vector3(urthOrbit.transform.eulerAngles.x, worldTime.totalGameSeconds / 14400, urthOrbit.transform.eulerAngles.z);

        //urthOrbit.transform.Rotate(urthOrbit.transform.up * (worldTime.totalGameSeconds - prevTime) / 14400);
        urth.transform.eulerAngles = new Vector3(urth.transform.eulerAngles.x, 170 + worldTime.totalGameSeconds / 240, urth.transform.eulerAngles.z);
        //urth.transform.Rotate(Vector3.up);
        sunLight.transform.rotation = Quaternion.LookRotation(youAreHere.transform.InverseTransformDirection(youAreHere.transform.position - transform.position));

        float sunAngle = Vector3.Angle(youAreHere.transform.up, transform.position - youAreHere.transform.position);
        sunLight.intensity = Mathf.Min((95 - sunAngle) / 6, 1) * 2f;
        if (sunAngle < 95)
        {
            sunLight.enabled = true;
        }
        else
            sunLight.enabled = false;


        planetLight.transform.rotation = Quaternion.LookRotation(youAreHere.transform.InverseTransformDirection(youAreHere.transform.position - gasGiant.transform.position));
        float planetAngle = Vector3.Angle(youAreHere.transform.up, gasGiant.transform.position - youAreHere.transform.position);
        planetLight.intensity = Mathf.Min((95 - planetAngle) / 6, 1) * 0.3f;
        if (planetAngle < 95)
        {
            planetLight.enabled = true;
        }
        else
            planetLight.enabled = false;
        prevTime = worldTime.totalGameSeconds;
        if (currentWeatherFX == null)
            currentWeatherFX = Instantiate<GameObject>(weathers[currentWeatherIndex].weatherFX, player.transform);
        dayHour = time.hours % 24f;

        float ambientIntensity = 1;
        //UpdateSkyLights();

        if (weathers[currentWeatherIndex].areLowClouds)
        {
            ambientIntensity = sunLight.intensity * 0.4f;
            sunLight.intensity = 0.01f * sunLight.intensity;
            planetLight.intensity = 0.01f * planetLight.intensity;
        }
        else
        {
            ambientIntensity = sunLight.intensity;
        }

        float rgb = ambientIntensity * .5f;
        Color color = new Color(rgb, rgb, rgb);
        RenderSettings.ambientSkyColor = color;
        RenderSettings.fogColor = color;
        lowCloudsMat.SetColor("_CloudColor", color);
        highCloudsMat.SetColor("_CloudColor", new Color(ambientIntensity,ambientIntensity,ambientIntensity));


        if (day && dayHour > 18)
        {
            StartCoroutine(FadeInStars());
            //RenderSettings.skybox = skyboxMaterials[1];
            starSphere.SetActive(true);
            day = false;
            night = true;
            dawn = false;
        }
        else if (night)
        {
            if (dayHour < 17.5 && dayHour > 5 && !dawn)
            {
                dawn = true;
                StartCoroutine(FadeOutStars());
            }
            if (dayHour > 7 && dayHour < 17.5)
            {
                //RenderSettings.skybox = skyboxMaterials[0];
                starSphere.SetActive(false);
                day = true;
                night = false;
                dawn = false;
            }
        }
        if (time.totalGameSeconds > (currentStart + currentDuration))
        {
            NextWeather();
        }
        if (shelterCheck.roof && shelterCheck.walls)
        {
            RenderSettings.ambientSkyColor = new Color(.01f, .01f, .01f);
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
    public Material lowCloudsMat, highCloudsMat;
    public GameObject closeClouds;
    public GameObject lowClouds;
    public GameObject highClouds;
    public Color lowCloudsColor, highCloudsColor, clearDayAmbient, cloudyDayAmbient, cloudyMoonNightAmbient, clearMoonNightAmbient, clearNightAmbient, cloudyNightAmbient;

    void UpdateSkyLights()
    {
        sun.transform.eulerAngles = new Vector3((dayHour - 6) / 12 * 180, 90, 0);
        if (dayHour > 18 || dayHour < 6)
        {
            RenderSettings.ambientSkyColor = clearNightAmbient;
            sun.intensity = 0;
        }
        else
        {
            if (weathers[currentWeatherIndex].areLowClouds)
            {
                sun.intensity = 0.01f;
                RenderSettings.ambientSkyColor = cloudyDayAmbient;
            }
            else
            {
                RenderSettings.ambientSkyColor = clearDayAmbient;
                sun.intensity = 2f;
            }
        }
    }
    void SunPosition()
    {
        float pi = 3.14159265f;
        float y = (2 * pi / 365) * (yearDay - 1 + (dayHour - 12) / 24);
        float eqtime = 229.18f * (0.000075f + 0.001868f * Mathf.Cos(y) - 0.032077f * Mathf.Sin(y) - 0.014615f * Mathf.Cos(2 * y) - 0.040849f * Mathf.Sin(2 * y));
        float declin = 0.006918f - 0.399912f * Mathf.Cos(y) + 0.070257f * Mathf.Sin(y) - 0.006758f * Mathf.Cos(2 * y) + 0.000907f * Mathf.Sin(2 * y) - 0.002697f * Mathf.Cos(3 * y) + 0.00148f * Mathf.Sin(3 * y);
    }
    IEnumerator FadeInStars()
    {
        bool fading = true;
        while (fading)
        {
            float c = 1 - 0.5f * (dayHour - 18);
            if (c <= .02f)
            {
                c = .02f;
                fading = false;
            }
            stars.SetFloat("_Cutoff", c);

            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator FadeOutStars()
    {
        bool fading = true;
        while (fading)
        {
            float c = .02f + 0.5f * (dayHour - 5);
            if (c >= 1f)
            {
                c = 1f;
                fading = false;
            }
            stars.SetFloat("_Cutoff", c);

            yield return new WaitForSeconds(1);
        }
    }

    public float Temperature
    {
        get { return localTemperature; }
        set { localTemperature = value; }
    }

    void NextWeather()
    {
        if (currentWeatherFX != null)
            Destroy(currentWeatherFX);
        currentWeatherIndex = schedule[0];
        currentWeatherFX = Instantiate<GameObject>(weathers[schedule[0]].weatherFX, player.transform);
        currentStart = (int)time.totalGameSeconds;
        currentDuration = durations[0];
        precipRate = weathers[schedule[0]].precipRate;
        windSpeed = weathers[schedule[0]].windSpeed;
        if (weathers[currentWeatherIndex].areLowClouds)
        {
            lowClouds.SetActive(true);
            lowClouds.GetComponent<Renderer>().material.SetFloat("_Density", weathers[currentWeatherIndex].lowCloudDensity);
        }
        else
        {
            lowClouds.SetActive(false);
        }
        if (weathers[currentWeatherIndex].areHighClouds)
        {
            highClouds.SetActive(true);
            highClouds.GetComponent<Renderer>().material.SetFloat("_Density", weathers[currentWeatherIndex].highCloudDensity);
        }
        else
        {
            highClouds.SetActive(false);
        }
        if (weathers[currentWeatherIndex].areCirrusClouds)
        {
            lazyCloudsCirrus.gameObject.SetActive(true);
            lazyCloudsCirrus.LS_CloudScattering = weathers[currentWeatherIndex].LS_CloudScattering;
        }
        else
        {
            lazyCloudsCirrus.gameObject.SetActive(false);
        }
        if (weathers[currentWeatherIndex].areCloseClouds)
        {
            if (closeClouds.activeSelf) { }
            else
            {
                closeClouds.SetActive(true);
                //SetLayerRecursively(closeClouds, 30);
            }
        }
        else
        {
            closeClouds.SetActive(false);
        }

        schedule.RemoveAt(0);
        durations.RemoveAt(0);
        schedule.Add(Random.Range(0, 4));
        durations.Add(300 + Random.Range(0, 36000));
        RenderSettings.fogDensity = weathers[currentWeatherIndex].globalFog;
        RenderSettings.fogStartDistance = weathers[currentWeatherIndex].fogStart;
        RenderSettings.fogEndDistance = weathers[currentWeatherIndex].fogEnd;
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
