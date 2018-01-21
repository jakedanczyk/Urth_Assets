﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class WorldTime : MonoBehaviour
{



    public float totalGameSeconds;

    public int yearDay = 0;

    public float seconds;
    public float minutes;
    public float hours;
    public float days;
    public float months;
    public float years;

    private float secondsPerSecond;

    public DayNightController dayNightController;

    void Start()
    {

        secondsPerSecond = 1;
        totalGameSeconds += secondsPerSecond * Time.deltaTime;


    }


    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            secondsPerSecond = 1;
            dayNightController.daySpeedMultiplier = secondsPerSecond / 3600;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            secondsPerSecond = 60;
            dayNightController.daySpeedMultiplier = secondsPerSecond / 3600;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            secondsPerSecond = 3600;
            dayNightController.daySpeedMultiplier = secondsPerSecond / 3600;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            secondsPerSecond = 86400;
            dayNightController.daySpeedMultiplier = secondsPerSecond / 3600;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            secondsPerSecond = 2629743;
            dayNightController.daySpeedMultiplier = secondsPerSecond / 3600;
        }

        totalGameSeconds += secondsPerSecond * Time.deltaTime;

        seconds = totalGameSeconds;
        minutes = totalGameSeconds / 60;
        hours = minutes / 60;
        days = hours / 24;
        months = days / (365 / 12);
        years = months / 12;
    }


    void OnGUI()
    {

        GUI.Label(new Rect(0, 200, 500, 500), "Total Seconds: " + totalGameSeconds);
        GUI.Label(new Rect(0, 225, 500, 500), "Seconds Per Second: " + secondsPerSecond);

        GUI.Label(new Rect(0, 250, 500, 500), "Second: " + (int)seconds % 60);
        GUI.Label(new Rect(0, 275, 500, 500), "Minute: " + (int)minutes % 60);
        GUI.Label(new Rect(0, 300, 500, 500), "Hour: " + (int)hours % 24);
        GUI.Label(new Rect(0, 325, 500, 500), "Day: " + (int)days % (365 / 12));
        GUI.Label(new Rect(0, 350, 500, 500), "Month: " + (int)months % 12);
        GUI.Label(new Rect(0, 375, 500, 500), "Year: " + (int)years);

    }


}