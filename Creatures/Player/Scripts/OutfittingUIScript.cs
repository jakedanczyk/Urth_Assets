using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OutfittingUIScript : MonoBehaviour {

    public BodyManager body;
    public RPGStatCollection stats;
    public GameObject thisPanel;
    public GameObject fontObject;
    public List<RPGStatType> combatSkillsList;
    public bool panelActive = false;
    public List<RPGStatType> combatSkillsList2; //= new List<RPGStatType> { RPGStatType.Armor, RPGStatType.Defense, RPGStatType.Shield, RPGStatType.ArmStrike, RPGStatType.LegStrike, RPGStatType.Axe };
    public List<RPGStatType> skillsList;

    void Awake()
    {
        //combatSkillsList2 = new List<RPGStatType> { RPGStatType.Armor, RPGStatType.Defense, RPGStatType.Shield, RPGStatType.ArmStrike, RPGStatType.LegStrike, RPGStatType.Axe };
    }
    // Use this for initialization
    void Start()
    {
        combatSkillsList2 = new List<RPGStatType>{ RPGStatType.Dodge,
                                                    RPGStatType.Deflect,
                                                    RPGStatType.Absorb,
                                                    RPGStatType.Shield,
                                                    RPGStatType.Parry,
                                                   // RPGStatType.OffHandWeapon,
                                                    RPGStatType.Armor,
                                                    RPGStatType.ArmStrike,
                                                    RPGStatType.LegStrike,
                                                    RPGStatType.Throwing,
                                                    RPGStatType.Axe,
                                                    RPGStatType.Blunt,
                                                    RPGStatType.Pick,
                                                    RPGStatType.ShortBlade,
                                                    RPGStatType.LongBlade,
                                                    RPGStatType.Lash,
                                                    RPGStatType.LightPoleArm,
                                                    RPGStatType.HeavyPoleArm,
                                                    RPGStatType.CrossBow,
                                                    RPGStatType.Bow,
                                                    RPGStatType.Sling,
                                                    RPGStatType.Atlatl,
                                                    };
        skillsList = new List<RPGStatType> { RPGStatType.StoneWorking, RPGStatType.WoodWorking, RPGStatType.FireMaking, RPGStatType.Gathering, RPGStatType.Farming, RPGStatType.Mining,
                                                RPGStatType.Medicine, RPGStatType.AnimalTraining, RPGStatType.Tracking, RPGStatType.Pottery, RPGStatType.Fishing, RPGStatType.Butchery,
                                                    RPGStatType.Cooking, RPGStatType.Tailoring, RPGStatType.WeatherSense };
    }

    // Update is called once per frame
    void Update() {

    }


    void OnGUI()
    {
        Font myFont = (Font)Resources.Load("MorrisRoman-Black", typeof(Font));
        GUIStyle myStyle = new GUIStyle(GUI.skin.label);
        myStyle.font = myFont;
        
        //health, bodyparts
        GUI.Label(new Rect(700, 200, 500, 500), "Health: " + stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue + " / " + stats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue, myStyle);

        GUI.Label(new Rect(500, 155, 500, 500), "Head armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.Head).protection, x => x.ToString())),myStyle);
        GUI.Label(new Rect(500, 170, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.Head).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.Head).StatValue,myStyle);

        GUI.Label(new Rect(500, 250, 500, 500), "Neck armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.Neck).protection, x => x.ToString())),myStyle);
        GUI.Label(new Rect(500, 265, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.Neck).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.Neck).StatValue, myStyle);

        GUI.Label(new Rect(450, 355, 500, 500), "Chest armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.Chest).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(450, 370, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.Chest).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.Chest).StatValue, myStyle);

        GUI.Label(new Rect(450, 405, 500, 500), "Stomach armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(450, 415, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.Stomach).StatValue, myStyle);

        GUI.Label(new Rect(500, 450, 500, 500), "Pelvis armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(500, 460, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.Pelvis).StatValue, myStyle);

        GUI.Label(new Rect(550, 370, 500, 500), "Upper Back armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(550, 385, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.UpperBack).StatValue, myStyle);

        GUI.Label(new Rect(550, 420, 500, 500), "Lower Back armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(550, 430, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.LowerBack).StatValue, myStyle);

        GUI.Label(new Rect(300, 330, 500, 500), "R Shoulder armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(300, 345, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.RightShoulder).StatValue, myStyle);

        GUI.Label(new Rect(300, 390, 500, 500), "R Upper Arm armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.RightUpperArm).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(300, 405, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.RightUpperArm).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.RightUpperArm).StatValue, myStyle);

        GUI.Label(new Rect(300, 435, 500, 500), "R Forearm armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(300, 445, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.RightForearm).StatValue, myStyle);

        GUI.Label(new Rect(300, 480, 500, 500), "R Hand armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(300, 495, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.RightHand).StatValue, myStyle);

        GUI.Label(new Rect(700, 330, 500, 500), "L Shoulder armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(700, 345, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftShoulder).StatValue, myStyle);

        GUI.Label(new Rect(700, 390, 500, 500), "L Upper Arm armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.LeftUpperArm).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(700, 405, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftUpperArm).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftUpperArm).StatValue, myStyle);

        GUI.Label(new Rect(700, 435, 500, 500), "L Forearm armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(700, 450, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftForearm).StatValue, myStyle);

        GUI.Label(new Rect(700, 480, 500, 500), "L Hand armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(700, 495, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftHand).StatValue, myStyle);

        GUI.Label(new Rect(400, 505, 500, 500), "R Thigh armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(400, 520, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.RightThigh).StatValue, myStyle);

        GUI.Label(new Rect(400, 580, 500, 500), "R Calf armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(400, 595, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.RightCalf).StatValue, myStyle);

        GUI.Label(new Rect(400, 630, 500, 500), "R Foot armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(400, 645, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.RightFoot).StatValue, myStyle);

        GUI.Label(new Rect(600, 515, 500, 500), "L Thigh armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(600, 525, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftThigh).StatValue, myStyle);

        GUI.Label(new Rect(600, 580, 500, 500), "L Calf armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(600, 595, 500, 500), "LeftCalf health: " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftCalf).StatValue, myStyle);

        GUI.Label(new Rect(600, 630, 500, 500), "L Foot armor: " + string.Join(",", Array.ConvertAll(stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).protection, x => x.ToString())), myStyle);
        GUI.Label(new Rect(600, 645, 500, 500), "Health: " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).StatCurrentValue + " / " + stats.GetStat<RPGBodyPart>(RPGStatType.LeftFoot).StatValue, myStyle);

        //attributes
        int ySpace = 15;
        int yPos = 250;
        GUI.Label(new Rect(875, yPos, 500, 500), "Attribute | Value | CurrentValue", myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Agility).StatName + "   "
                                                + stats.GetStat<RPGAttribute>(RPGStatType.Agility).StatBaseValue + "    "
                                                + stats.GetStat<RPGAttribute>(RPGStatType.Agility).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).StatName + "   "
                                                + stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).StatBaseValue + "    "
                                                + stats.GetStat<RPGAttribute>(RPGStatType.Dexterity).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Endurance).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Endurance).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Endurance).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Speed).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Speed).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Speed).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Strength).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Toughness).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Toughness).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Toughness).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Attunement).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Attunement).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Attunement).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Creativity).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Creativity).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Creativity).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Intelligence).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Intelligence).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Intelligence).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Perception).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Perception).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Perception).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.ThirdEye).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.ThirdEye).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.ThirdEye).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Willpower).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Willpower).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Willpower).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Artistry).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Artistry).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Artistry).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Confidence).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Confidence).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Confidence).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Courage).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Courage).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Courage).StatValue, myStyle);
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Empathy).StatName + "   "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Empathy).StatBaseValue + "    "
                                        + stats.GetStat<RPGAttribute>(RPGStatType.Empathy).StatValue, myStyle);
        //openness??
        yPos = yPos + ySpace;
        GUI.Label(new Rect(875, yPos, 500, 500), stats.GetStat<RPGAttribute>(RPGStatType.Warmth).StatName + "   "
                                + stats.GetStat<RPGAttribute>(RPGStatType.Warmth).StatBaseValue + "    "
                                + stats.GetStat<RPGAttribute>(RPGStatType.Warmth).StatValue, myStyle);

        //combat skills
        int ySpaceCombatSkills = 15;
        int yPosCombatSkills = 530;
        GUI.Label(new Rect(875, yPosCombatSkills, 500, 500), "Skill | Value | CurrentValue", myStyle);
        yPosCombatSkills = yPosCombatSkills + ySpaceCombatSkills;
        foreach(RPGStatType stat in combatSkillsList)
        {
            GUI.Label(new Rect(875, yPosCombatSkills, 500, 500), stats.GetStat<RPGSkill>(stat).StatName + "   "
                              + stats.GetStat<RPGSkill>(stat).StatBaseValue + "    "
                              + stats.GetStat<RPGSkill>(stat).StatValue, myStyle);
            yPosCombatSkills = yPosCombatSkills + ySpaceCombatSkills;
        }


        //skill skills
        int xPosSkills = 1100;
        int ySpaceSkills = 15;
        int yPosSkills = 250;

        GUI.Label(new Rect(xPosSkills, yPosSkills, 500, 500), "Skill | Value | CurrentValue", myStyle);
        yPosSkills = yPosSkills + ySpaceSkills;
        foreach (RPGStatType stat in skillsList)
        {
            GUI.Label(new Rect(xPosSkills, yPosSkills, 500, 500), stats.GetStat<RPGSkill>(stat).StatName + "   "
                              + stats.GetStat<RPGSkill>(stat).StatBaseValue + "    "
                              + stats.GetStat<RPGSkill>(stat).StatValue, myStyle);
            yPosSkills = yPosSkills + ySpaceSkills;
        }
    }
}
