using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterOverview : MonoBehaviour {

    public Text title;
    public BodyManager_Human_Player player_bodyManager;
    public GameObject infoPanel;
    public Font myFont;

    public Text blankTextPrefab;
    public GameObject attributesPanel, skillsPanel;
    public GameObject bodyPartsPanel;
    public List<GameObject> bodyPartsSubPanels;
    public GameObject head, neck, chest, upperBack, abdomen, lowerBack, pelvis, leftShoulder, leftUpperArm, leftLowerArm, leftHand, leftThigh, leftCalf, leftFoot, rightShoulder, rightUpperArm, rightLowerArm, rightHand, rightThigh, rightCalf, rightFoot;

    List<RPGStatType> statTypes = new List<RPGStatType> { RPGStatType.Agility,  RPGStatType.Dexterity,  RPGStatType.Endurance,  RPGStatType.Speed,  RPGStatType.Strength,  RPGStatType.Toughness,  RPGStatType.Attunement,  RPGStatType.Creativity,  RPGStatType.Intelligence,  RPGStatType.Perception,  RPGStatType.ThirdEye,  RPGStatType.Willpower,  RPGStatType.Artistry,  RPGStatType.Confidence,  RPGStatType.Courage,  RPGStatType.Empathy,  RPGStatType.Warmth };
    List<RPGStatType> combatSkillsList = new List<RPGStatType>{ RPGStatType.Dodge,
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
    List<RPGStatType> skillsList = new List<RPGStatType> { RPGStatType.StoneWorking, RPGStatType.WoodWorking, RPGStatType.FireMaking, RPGStatType.Gathering, RPGStatType.Farming,                                                           RPGStatType.Mining,
                                                            RPGStatType.Medicine, RPGStatType.AnimalTraining, RPGStatType.Tracking, RPGStatType.Pottery, RPGStatType.Fishing,                       RPGStatType.Butchery,
                                                            RPGStatType.Cooking, RPGStatType.Tailoring, RPGStatType.WeatherSense };

    // Use this for initialization
    void Start () {
        title.text = player_bodyManager.chararacterName;
        title.text += ", a ";
        title.text += player_bodyManager.race;
        title.resizeTextForBestFit = true;

        UpdateStats();
	}

    private void OnEnable()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        foreach (RectTransform subPanel in bodyPartsPanel.transform)
        {
            var children = new List<GameObject>();
            foreach (Transform child in subPanel) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
            Text txt = Instantiate(blankTextPrefab, subPanel);
            txt.font = myFont;
            txt.resizeTextForBestFit = true;
            txt.text = subPanel.gameObject.name + "\nArmor: ";
            txt.text += "\n";
            RPGStatType statType = (RPGStatType)Enum.Parse(typeof(RPGStatType), subPanel.name);
            txt.text += string.Join(",", Array.ConvertAll(player_bodyManager.stats.GetStat<RPGBodyPart>(statType).protection, x => x.ToString()));
            txt.text += "\n";
            txt.text += "Health: " + player_bodyManager.stats.GetStat<RPGBodyPart>(statType).StatCurrentValue + " / " + player_bodyManager.stats.GetStat<RPGBodyPart>(statType).StatValue;
        }
        var attribs_children = new List<GameObject>();
        foreach (Transform child in attributesPanel.transform) attribs_children.Add(child.gameObject);
        attribs_children.ForEach(child => Destroy(child));
        Text attribs_txt = Instantiate(blankTextPrefab, attributesPanel.transform);
        attribs_txt.font = myFont;
        attribs_txt.resizeTextForBestFit = true;

        attribs_txt.text = "Attribute | Value | CurrentValue\n\n";
        foreach (RPGStatType statType in statTypes)
        {
            attribs_txt.text += player_bodyManager.stats.GetStat<RPGAttribute>(statType).StatName + "   "
                                                + player_bodyManager.stats.GetStat<RPGAttribute>(statType).StatBaseValue + "    "
                                                + player_bodyManager.stats.GetStat<RPGAttribute>(statType).StatValue + "\n";
        }
        var skills_children = new List<GameObject>();
        foreach (Transform child in skillsPanel.transform) skills_children.Add(child.gameObject);
        skills_children.ForEach(child => Destroy(child));
        Text skills_txt = Instantiate(blankTextPrefab, skillsPanel.transform);
        skills_txt.font = myFont;
        skills_txt.resizeTextForBestFit = true;

        skills_txt.text = "Skill | Value | CurrentValue\n\n";
        foreach (RPGStatType statType in combatSkillsList)
        {
            skills_txt.text += player_bodyManager.stats.GetStat<RPGSkill>(statType).StatName + "   "
                                                + player_bodyManager.stats.GetStat<RPGSkill>(statType).StatBaseValue + "    "
                                                + player_bodyManager.stats.GetStat<RPGSkill>(statType).StatValue + "\n";
        }
        skills_txt.text = "\n\nSkill | Value | CurrentValue\n\n";
        foreach (RPGStatType statType in skillsList)
        {
            skills_txt.text += player_bodyManager.stats.GetStat<RPGSkill>(statType).StatName + "   "
                                                + player_bodyManager.stats.GetStat<RPGSkill>(statType).StatBaseValue + "    "
                                                + player_bodyManager.stats.GetStat<RPGSkill>(statType).StatValue + "\n";
        }

    }
}
