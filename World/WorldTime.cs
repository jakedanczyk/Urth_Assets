using UnityEngine;
using System.Collections;

[System.Serializable]
public class WorldTime : MonoBehaviour
{
    public static GameObject thisObject;

    public MessageLog messageLog;
    public DataWindow dataWindow;
    public float totalGameSeconds;

    public int yearDay = 0;

    public float seconds;
    public float minutes;
    public float hours;
    public float days;
    public float months;
    public float years;

    private int secondsPerSecond;

    public DayNightController dayNightController;

    private void Awake()
    {
        thisObject = this.gameObject;
    }

    void Start()
    {
        dataWindow = DataWindow.dataWindowGameObject.GetComponent<DataWindow>();
        messageLog = MessageLog.messageLogGameObject.GetComponent<MessageLog>();
        secondsPerSecond = 1;
        totalGameSeconds += secondsPerSecond * Time.deltaTime;
    }


    void Update()
    {
        totalGameSeconds += secondsPerSecond * Time.deltaTime;

        seconds = totalGameSeconds;
        minutes = totalGameSeconds / 60;
        hours = minutes / 60;
        days = hours / 24;
        months = days / (360 / 12);
        years = months / 12;
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            if (secondsPerSecond == 1) { }
            else if (secondsPerSecond == 5)
                secondsPerSecond = 1;
            else if (secondsPerSecond == 30)
                secondsPerSecond = 5;
            else if (secondsPerSecond == 120)
                secondsPerSecond = 30;
            else if (secondsPerSecond == 360)
                secondsPerSecond = 120;
            else if (secondsPerSecond == 720)
                secondsPerSecond = 360;
            else if (secondsPerSecond == 1200)
                secondsPerSecond = 720;
            dayNightController.daySpeedMultiplier = secondsPerSecond / 3600;
            dataWindow.SetClockSpeed(secondsPerSecond);
        }
        if (Input.GetKeyDown(KeyCode.Quote))
        {
            if (secondsPerSecond == 1)
                secondsPerSecond = 5;
            else if (secondsPerSecond == 5)
                secondsPerSecond = 30;
            else if (secondsPerSecond == 30)
                secondsPerSecond = 120;
            else if (secondsPerSecond == 120)
                secondsPerSecond = 360;
            else if (secondsPerSecond == 360)
                secondsPerSecond = 720;
            else if (secondsPerSecond == 720)
                secondsPerSecond = 1200;
            else if (secondsPerSecond == 1200)
            { }
            dayNightController.daySpeedMultiplier = secondsPerSecond / 3600;
            dataWindow.SetClockSpeed(secondsPerSecond);
        }
    }

    bool isGUIActive = false;
    public bool IsGUIActive
    {
        get { return isGUIActive; }
        set { isGUIActive = value; }
    }
    void OnGUI()
    {
        if (isGUIActive)
        {
            GUI.Label(new Rect(0, 200, 500, 500), "Total Seconds: " + totalGameSeconds);
            GUI.Label(new Rect(0, 225, 500, 500), "Seconds Per Second: " + secondsPerSecond);

            GUI.Label(new Rect(0, 250, 500, 500), "Second: " + (int)seconds % 60);
            GUI.Label(new Rect(0, 275, 500, 500), "Minute: " + (int)minutes % 60);
            GUI.Label(new Rect(0, 300, 500, 500), "Hour: " + (int)hours % 24);
            GUI.Label(new Rect(0, 325, 500, 500), "Day: " + (int)days % (360 / 12));
            GUI.Label(new Rect(0, 350, 500, 500), "Month: " + (int)months % 12);
            GUI.Label(new Rect(0, 375, 500, 500), "Year: " + (int)years);
        }
    }


}