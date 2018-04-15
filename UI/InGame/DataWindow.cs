using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class DataWindow : MonoBehaviour {

    public static GameObject dataWindowGameObject;
    public Text gaitText, timeOfDayText, dayOfYearText, airTempText, bodyTempText, thirstText, hungerText, clockSpeed, gameSpeed;
    public WorldTime time;
    public BodyManager_Human_Player playerBodyManager;

    private void Awake()
    {
        dataWindowGameObject = gameObject;
    }

    private void Start()
    {
        StartCoroutine(Refresh());
    }

    public void SetGaitText(string newGait)
    {
        gaitText.text = "Gait: " + newGait;
    }

    public void SetClockSpeed(float newSpeed)
    {
        clockSpeed.text = "Clock rate: " + (int)newSpeed + "x";
    }

    public void SetGameSpeed(float newSpeed)
    {
        if (newSpeed >= 1)
            gameSpeed.text = "Game speed: " + (int)newSpeed + "x";
        else if (newSpeed > .4)
            gameSpeed.text = "Game speed: 0.5x";
        else if (newSpeed > .2)
            gameSpeed.text = "Game speed: 0.25x";
        else
            gameSpeed.text = "Game speed: " + newSpeed.ToString("N4") + "x";
    }

    IEnumerator Refresh()
    {
        int i = 6, j = 0;
        while(true)
        {
            AirTemperature temp = AirTemperature.Moderate;
            if (playerBodyManager.thermometer.temperature < -20)
                temp = AirTemperature.BitterCold;
            else if (playerBodyManager.thermometer.temperature < -1)
                temp = AirTemperature.BelowFreezing;
            else if (playerBodyManager.thermometer.temperature < 1)
                temp = AirTemperature.Freezing;
            else if (playerBodyManager.thermometer.temperature < 7)
                temp = AirTemperature.Cold;
            else if (playerBodyManager.thermometer.temperature < 13)
                temp = AirTemperature.Cool;
            else if (playerBodyManager.thermometer.temperature < 20)
                temp = AirTemperature.Moderate;
            else if (playerBodyManager.thermometer.temperature < 27)
                temp = AirTemperature.Warm;
            else if (playerBodyManager.thermometer.temperature < 35)
                temp = AirTemperature.Hot;
            else if (playerBodyManager.thermometer.temperature < 45)
                temp = AirTemperature.VeryHot;
            else
                temp = AirTemperature.BurningHot;
            if(temp != airTemp)
            {
                airTemp = temp;
                airTempText.text = "Air temperature: " + GetEnumDescription(airTemp);
            }
            BodyTemperature btemp = BodyTemperature.Neutral;
            if (playerBodyManager.coreTemp > 36.5 && playerBodyManager.coreTemp < 37.5)
                btemp = BodyTemperature.Neutral;
            else if (playerBodyManager.coreTemp < 36.5)
            {
                if (playerBodyManager.coreTemp > 36)
                    btemp = BodyTemperature.Cool;
                else if (playerBodyManager.coreTemp > 35.5)
                    btemp = BodyTemperature.Cold;
                else if (playerBodyManager.coreTemp > 32)
                    btemp = BodyTemperature.VeryCold;
                else if (playerBodyManager.coreTemp > 29)
                    btemp = BodyTemperature.PartiallyNumb;
                else
                    btemp = BodyTemperature.FullyNumb;
                //else if (playerBodyManager.coreTemp > 26)
                //    btemp = BodyTemperature.FullyNumb;
                //else
                //    btemp = BodyTemperature.PainfullyHot;
            }
            else
            {
                if (playerBodyManager.coreTemp < 38)
                    btemp = BodyTemperature.Warm;
                else if (playerBodyManager.coreTemp < 38.5)
                    btemp = BodyTemperature.Hot;
                else
                    btemp = BodyTemperature.PainfullyHot;
            }
            if (btemp != bodyTemp)
            {
                bodyTemp = btemp;
                bodyTempText.text = "Body temperature: " + GetEnumDescription(bodyTemp);
            }
            ThirstLevel tempThirst = ThirstLevel.Bit;
            if (playerBodyManager.hydration > 60)
            {
                thirstText.color = Color.blue;
                tempThirst = ThirstLevel.Slaked;
            }
            else if (playerBodyManager.hydration > 57.5)
            {
                thirstText.color = new Color(173, 216, 230);
                tempThirst = ThirstLevel.Bit;
            }
            else if (playerBodyManager.hydration > 55.5)
            {
                thirstText.color = new Color(255, 204, 203);
                tempThirst = ThirstLevel.Thirsty;
            }
            else if (playerBodyManager.hydration > 53)
            {
                thirstText.color = new Color(230, 0, 38);
                tempThirst = ThirstLevel.Parched;
            }
            else if (playerBodyManager.hydration > 48)
            {
                thirstText.color = new Color(150, 0, 24);
                tempThirst = ThirstLevel.Intense;
            }
            else
            {
                thirstText.color = new Color(150, 0, 24);
                tempThirst = ThirstLevel.Dying;
            }
            if (tempThirst != thirstLevel)
            {
                thirstLevel = tempThirst;
                thirstText.text = GetEnumDescription(thirstLevel);
            }
            Hunger tempHunger = Hunger.Hungry;
            if(playerBodyManager.calories > 3000)
            {
                if (playerBodyManager.calories > 6000)
                    tempHunger = Hunger.OverFed;
                else
                    tempHunger = Hunger.WellFed;
            }
            else if(playerBodyManager.calories < 0)
            {
                if (playerBodyManager.calories > -9000)
                    tempHunger = Hunger.VeryHungry;
                else if(playerBodyManager.calories > -45000)
                    tempHunger = Hunger.WeakFrom;
                else if (playerBodyManager.calories > -90000)
                    tempHunger = Hunger.Wracked;
                else if (playerBodyManager.calories > -93000)
                    tempHunger = Hunger.Comatose;
                else
                    tempHunger = Hunger.Dead;
            }
            StomachFill tempStomachFill = StomachFill.Empty;
            if (playerBodyManager.StomachFillLevel() > 0f)
            {
                if (playerBodyManager.StomachFillLevel() < .25f)
                    tempStomachFill = StomachFill.NotEmpty;
                else if (playerBodyManager.StomachFillLevel() < .5f)
                    tempStomachFill = StomachFill.Full;
                else if (playerBodyManager.StomachFillLevel() < .9f)
                    tempStomachFill = StomachFill.VeryFull;
                else
                    tempStomachFill = StomachFill.Painful;
            }
            if (tempStomachFill != stomachFill || tempHunger != hunger)
            {
                hungerText.text = (GetEnumDescription(hunger) + ", stomach " + GetEnumDescription(stomachFill));
            }

            if (i >= 6)
            {
                i = 0;
                if (time.hours < 5)
                {
                    if (tod != TimeOfDay.Night)
                    {
                        tod = TimeOfDay.Night;
                        timeOfDayText.text = "Night";
                    }
                }
                else if (time.hours < 7)
                {
                    if (tod != TimeOfDay.Dawn)
                    {
                        tod = TimeOfDay.Dawn;
                        timeOfDayText.text = "Dawn";
                    }
                }
                else if (time.hours < 9)
                {
                    if (tod != TimeOfDay.EarlyMorning)
                    {
                        tod = TimeOfDay.EarlyMorning;
                        timeOfDayText.text = "Early Morning";
                    }
                }
                else if (time.hours < 11)
                {
                    if (tod != TimeOfDay.LateMorning)
                    {
                        tod = TimeOfDay.LateMorning;
                        timeOfDayText.text = "Late Morning";
                    }
                }
                else if (time.hours < 13)
                {
                    if (tod != TimeOfDay.MidDay)
                    {
                        tod = TimeOfDay.MidDay;
                        timeOfDayText.text = "Mid Day";
                    }
                }
                else if (time.hours < 15)
                {
                    if (tod != TimeOfDay.EarlyAfternoon)
                    {
                        tod = TimeOfDay.EarlyAfternoon;
                        timeOfDayText.text = "Early Afternoon";
                    }
                }
                else if (time.hours < 17)
                {
                    if (tod != TimeOfDay.LateAfternoon)
                    {
                        tod = TimeOfDay.LateAfternoon;
                        timeOfDayText.text = "Late Afternoon";
                    }
                }
                else if (time.hours < 19)
                {
                    if (tod != TimeOfDay.Dusk)
                    {
                        tod = TimeOfDay.Dusk;
                        timeOfDayText.text = "Dusk";
                    }
                }
                else
                {
                    if (tod != TimeOfDay.Night)
                    {
                        tod = TimeOfDay.Night;
                        timeOfDayText.text = "Night";
                    }
                }
            }
            i++;
            if(j >= 40)
            {
                j = 0;
                if((time.days + 1) % 360 < 91)
                {
                    dayOfYearText.text = NumberToWords(((int)time.days % 360) + 1) + " day of Spring";
                }
                else if((time.days + 1) % 360 < 181)
                {
                    dayOfYearText.text = NumberToWords(((int)time.days % 360) - 89) + " day of Summer";
                }
                else if ((time.days + 1) % 360 < 271)
                {
                    dayOfYearText.text = NumberToWords(((int)time.days % 360) - 180) + " day of Autumn";
                }
                else if ((time.days + 1) % 360 < 361)
                {
                    dayOfYearText.text = NumberToWords((((int)time.days - 1) % 360) - 269) + " day of Winter";
                }
                else { }
            }
            j++;
            yield return new WaitForSeconds(1.5f);
        }
    }
    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToString();
    }
    private enum StomachFill
    {
        [Description("painfully full")]
        Painful,
        [Description("very full")]
        VeryFull,
        [Description("full")]
        Full,
        [Description("not empty")]
        NotEmpty,
        [Description("empty")]
        Empty,
    }
    StomachFill stomachFill = StomachFill.Empty;
    private enum Hunger
    {
        [Description("Starved to death")]
        Dead,
        Comatose,
        [Description("Sever hunger pain")] //15 days without food
        Wracked,
        [Description("Weak from hunger")] //3 days without food
        WeakFrom,
        [Description("Very hungry")]
        VeryHungry, //<0 food being digested
        Hungry, //0 to 1 days worth of food being digested
        [Description("Well fed")]
        WellFed, //1 days worth of food being digested
        [Description("Over fed")] 
        OverFed //2 days worth of food being digested
    }
    Hunger hunger = Hunger.Hungry;
    private enum ThirstLevel
    {
        [Description("Dying of thirst")]
        Dying,
        [Description("Intense thirst")]
        Intense,
        Parched,
        Thirsty,
        [Description("A bit thirsty")]
        Bit,
        Slaked,
    }
    ThirstLevel thirstLevel = ThirstLevel.Bit;
    private enum AirTemperature
    {
        [Description("Bitter Cold")]
        BitterCold, //<-20C
        [Description("Below Freezing")]
        BelowFreezing, //-20C to -1C
        Freezing, //-1C to 1C
        Cold, //1C to 7C
        Cool, //7C to 13C
        Moderate, //15 to 20C
        Warm, //20C to 27C
        Hot, //27 to 35
        [Description("Very Hot")]
        VeryHot, //35 to 45
        [Description("Burning Hot")]
        BurningHot, //>45
    }
    AirTemperature airTemp = AirTemperature.Moderate;
    private enum BodyTemperature
    {
        [Description("Fully Numb")]
        FullyNumb,
        [Description("Partially Numb")]
        PartiallyNumb,
        [Description("Very Cold")]
        VeryCold,
        Cold,
        Cool,
        Neutral,
        Warm,
        Hot,
        [Description("Painfully Hot")]
        PainfullyHot,
    }
    BodyTemperature bodyTemp = BodyTemperature.Neutral;
    TimeOfDay tod = TimeOfDay.Night;
    private enum TimeOfDay
    {
        Night,
        Dawn,
        EarlyMorning,
        LateMorning,
        MidDay,
        EarlyAfternoon,
        LateAfternoon,
        Dusk    
    }
    private enum Season
    {
        Spring,Summer,Autumn,Winter
    }
    Season season;
    public string NumberToWords(int number)
    {
        if(number > 10 && number < 20)
        {
            return (number + "th");
        }
        else
        {
            int ones = number % 10;
            if (ones == 1)
                return (number + "st");
            else if (ones == 2)
                return (number + "nd");
            else if (ones == 3)
                return (number + "rd");
            else if (ones == 4)
                return (number + "st");
            else
                return (number + "th");
        }
    }
}

