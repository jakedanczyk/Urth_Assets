using UnityEngine;
using System.Collections;

public class HorseStats : CreatureStats
{
    protected override void ConfigureStats()
    {
        //attributes
        //physical
        var agility = CreateOrGetStat<RPGAttribute>(RPGStatType.Agility);
        agility.StatName = "Agility";
        agility.StatBaseValue = 100;
        agility.Aptitude = 100;

        var dexterity = CreateOrGetStat<RPGAttribute>(RPGStatType.Dexterity);
        dexterity.StatName = "Dexterity";
        dexterity.StatBaseValue = 1;
        dexterity.Aptitude = 0;

        var endurance = CreateOrGetStat<RPGAttribute>(RPGStatType.Endurance);
        endurance.StatName = "Endurance";
        endurance.StatBaseValue = 300;
        endurance.Aptitude = 300;

        var speed = CreateOrGetStat<RPGAttribute>(RPGStatType.Speed);
        speed.StatName = "Speed";
        speed.StatBaseValue = 600;
        speed.Aptitude = 200;

        var strength = CreateOrGetStat<RPGAttribute>(RPGStatType.Strength);
        strength.StatName = "Strength";
        strength.StatBaseValue = 600;
        strength.Aptitude = 100;

        var toughness = CreateOrGetStat<RPGAttribute>(RPGStatType.Toughness);
        toughness.StatName = "Toughness";
        toughness.StatBaseValue = 150;
        toughness.Aptitude = 50;

        //mental
        var attunement = CreateOrGetStat<RPGAttribute>(RPGStatType.Attunement);
        attunement.StatName = "Attunement";
        attunement.StatBaseValue = 1;
        attunement.Aptitude = 1;

        var creativity = CreateOrGetStat<RPGAttribute>(RPGStatType.Creativity);
        creativity.StatName = "Creativity";
        creativity.StatBaseValue = 5;
        creativity.Aptitude = 5;

        var intelligence = CreateOrGetStat<RPGAttribute>(RPGStatType.Intelligence);
        intelligence.StatName = "Intelligence";
        intelligence.StatBaseValue = 10;
        intelligence.Aptitude = 10;

        var perception = CreateOrGetStat<RPGAttribute>(RPGStatType.Perception);
        perception.StatName = "Perception";
        perception.StatBaseValue = 200;
        perception.Aptitude = 100;

        var thirdEye = CreateOrGetStat<RPGAttribute>(RPGStatType.ThirdEye);
        thirdEye.StatName = "Third Eye";
        thirdEye.StatBaseValue = 5;
        thirdEye.Aptitude = 5;

        var willpower = CreateOrGetStat<RPGAttribute>(RPGStatType.Willpower);
        willpower.StatName = "Willpower";
        willpower.StatBaseValue = 100;
        willpower.Aptitude = 50;

        //emotional
        var artistry = CreateOrGetStat<RPGAttribute>(RPGStatType.Artistry);
        artistry.StatName = "Artistry";
        artistry.StatBaseValue = 1;
        artistry.Aptitude = 1;

        var confidence = CreateOrGetStat<RPGAttribute>(RPGStatType.Confidence);
        confidence.StatName = "Confidence";
        confidence.StatBaseValue = 200;
        confidence.Aptitude = 100;

        var courage = CreateOrGetStat<RPGAttribute>(RPGStatType.Courage);
        courage.StatName = "Courage";
        courage.StatBaseValue = 50;
        courage.Aptitude = 25;

        var empathy = CreateOrGetStat<RPGAttribute>(RPGStatType.Empathy);
        empathy.StatName = "Empathy";
        empathy.StatBaseValue = 50;
        empathy.Aptitude = 50;

        var openness = CreateOrGetStat<RPGAttribute>(RPGStatType.Openness);
        openness.StatName = "Openness";
        openness.StatBaseValue = 50;
        openness.Aptitude = 50;

        var warmth = CreateOrGetStat<RPGAttribute>(RPGStatType.Warmth);
        warmth.StatName = "Warmth";
        warmth.StatBaseValue = 200;
        warmth.Aptitude = 100;

        //derived stats
        var health = CreateOrGetStat<RPGVital>(RPGStatType.Health);
        health.StatName = "Health";
        health.StatBaseValue = 400;
        health.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Endurance), .05f, -200f));
        health.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Strength), .1667f, -200f));
        health.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Toughness), .1667f, -200f));
        health.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Willpower), .015f, -200f));
        health.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.ThirdEye), .015f, -200f));
        health.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Confidence), .015f, -200f));
        health.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Courage), .015f, -200f));
        health.UpdateLinkers();
        health.SetCurrentValueToMax();

        var magicka = CreateOrGetStat<RPGVital>(RPGStatType.Magicka);
        magicka.StatName = "Magicka";
        magicka.StatBaseValue = 0;

        var height = CreateOrGetStat<RPGAttribute>(RPGStatType.Height);
        height.StatName = "Withers Height";
        height.StatBaseValue = 1300; //mm


        var weight = CreateOrGetStat<RPGVital>(RPGStatType.Weight);
        weight.StatName = "Weight";
        weight.StatBaseValue = 272000; //g

        var carryWeight = CreateOrGetStat<RPGVital>(RPGStatType.CarryWeight);
        carryWeight.StatName = "Max Carry Weight";
        carryWeight.StatBaseValue = 108800; //g

        var stamina = CreateOrGetStat<RPGVital>(RPGStatType.Stamina);
        stamina.StatName = "Stamina";
        stamina.StatBaseValue = 1000;
        stamina.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Endurance), 15f, -200f));
        stamina.UpdateLinkers();
        stamina.SetCurrentValueToMax();

        var jumpHeight = CreateOrGetStat<RPGDerived>(RPGStatType.JumpHeight);
        jumpHeight.StatName = "Jump Height";
        jumpHeight.StatBaseValue = 20;
        jumpHeight.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Agility), .01f, -200f));
        jumpHeight.UpdateLinkers();
        jumpHeight.SetCurrentValueToMax();

        var attackSpeed = CreateOrGetStat<RPGDerived>(RPGStatType.AttackSpeed);
        attackSpeed.StatName = "Attack Speed";
        attackSpeed.StatBaseValue = 10;
        jumpHeight.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Agility), .01f, -200f));
        jumpHeight.UpdateLinkers();
        jumpHeight.SetCurrentValueToMax();

        var restingHeartRate = CreateOrGetStat<RPGDerived>(RPGStatType.RestingHeartRate);
        restingHeartRate.StatName = "Resting Heart Rate";
        restingHeartRate.StatBaseValue = 40;
        restingHeartRate.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Endurance), -.075f, -400f));
        restingHeartRate.UpdateLinkers();
        restingHeartRate.SetCurrentValueToMax();

        var restingOxygenRate = CreateOrGetStat<RPGDerived>(RPGStatType.RestingOxygenRate);
        restingOxygenRate.StatName = "Resting Oxygen Rate"; //
        restingOxygenRate.StatBaseValue = 60;
        restingOxygenRate.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Stamina), -.075f, -400f));

        //skills
        //var sneak = CreateOrGetStat<RPGSkill>(RPGStatType.Sneak); 
        //sneak.StatName = "Sneak";
        //sneak.StatBaseValue = 1;
        //combat skills
        var armor = CreateOrGetStat<RPGSkill>(RPGStatType.Armor);
        armor.StatName = "Armor";
        armor.StatBaseValue = 1;

        var dodge = CreateOrGetStat<RPGSkill>(RPGStatType.Dodge);
        dodge.StatName = "Dodge";
        dodge.StatBaseValue = 1;

        var deflect = CreateOrGetStat<RPGSkill>(RPGStatType.Deflect);
        deflect.StatName = "Deflect";
        deflect.StatBaseValue = 1;

        var absorb = CreateOrGetStat<RPGSkill>(RPGStatType.Absorb);
        absorb.StatName = "Absorb";
        absorb.StatBaseValue = 1;

        var legStrike = CreateOrGetStat<RPGSkill>(RPGStatType.LegStrike);
        legStrike.StatName = "Leg Strike";
        legStrike.StatBaseValue = 1;


        //bodyparts
        var head = CreateOrGetStat<RPGBodyPart>(RPGStatType.Head);
        head.StatName = "Head";
        head.StatBaseValue = 100;
        head.bodyPartType = BodyPartType.Head;
        head.protection = new int[] { 1, 5, 3, 1, 1, 1 };
        head.damageModifer = 3;

        var neck = CreateOrGetStat<RPGBodyPart>(RPGStatType.Neck);
        neck.StatName = "Neck";
        neck.StatBaseValue = 100;
        neck.bodyPartType = BodyPartType.Neck;
        neck.protection = new int[] { 1, 5, 3, 1, 1, 1 };
        neck.damageModifer = 2;

        var torso = CreateOrGetStat<RPGBodyPart>(RPGStatType.Torso);
        torso.StatName = "Torso";
        torso.StatBaseValue = 100;
        torso.bodyPartType = BodyPartType.Torso;
        torso.protection = new int[] { 1, 5, 3, 1, 1, 1 };
        torso.damageModifer = 1;

        var leg = CreateOrGetStat<RPGBodyPart>(RPGStatType.Leg);
        leg.StatName = "Leg";
        leg.StatBaseValue = 100;
        leg.bodyPartType = BodyPartType.Leg;
        leg.protection = new int[] { 1, 5, 3, 1, 1, 1 };
        leg.damageModifer = 1;

        //var stomach = CreateOrGetStat<RPGBodyPart>(RPGStatType.Stomach);
        //stomach.StatName = "Stomach";
        //stomach.StatBaseValue = 100;
        //stomach.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var pelvis = CreateOrGetStat<RPGBodyPart>(RPGStatType.Pelvis);
        //pelvis.StatName = "Pelvis";
        //pelvis.StatBaseValue = 100;
        //pelvis.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var upperBack = CreateOrGetStat<RPGBodyPart>(RPGStatType.UpperBack);
        //upperBack.StatName = "Upper Back";
        //upperBack.StatBaseValue = 100;
        //upperBack.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var lowerBack = CreateOrGetStat<RPGBodyPart>(RPGStatType.LowerBack);
        //lowerBack.StatName = "Lower Back";
        //lowerBack.StatBaseValue = 100;
        //lowerBack.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var leftShoulder = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftShoulder);
        //leftShoulder.StatName = "Left Shoulder";
        //leftShoulder.StatBaseValue = 100;
        //leftShoulder.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var rightShoulder = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightShoulder);
        //rightShoulder.StatName = "Right Shoulder";
        //rightShoulder.StatBaseValue = 100;
        //rightShoulder.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var leftUpperArm = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftUpperArm);
        //leftUpperArm.StatName = "Left Upper Arm";
        //leftUpperArm.StatBaseValue = 100;
        //leftUpperArm.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var rightUpperArm = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightUpperArm);
        //rightUpperArm.StatName = "Right Upper Arm";
        //rightUpperArm.StatBaseValue = 100;
        //rightUpperArm.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var leftForearm = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftForearm);
        //leftForearm.StatName = "Left Forearm";
        //leftForearm.StatBaseValue = 100;
        //leftForearm.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var rightForearm = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightForearm);
        //rightForearm.StatName = "Right Forearm";
        //rightForearm.StatBaseValue = 100;
        //rightForearm.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var leftHand = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftHand);
        //leftHand.StatName = "Left Hand";
        //leftHand.StatBaseValue = 100;
        //leftHand.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var rightHand = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightHand);
        //rightHand.StatName = "Right Hand";
        //rightHand.StatBaseValue = 100;
        //rightHand.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var leftThigh = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftThigh);
        //leftThigh.StatName = "Left Thigh";
        //leftThigh.StatBaseValue = 100;
        //leftThigh.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var rightThigh = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightThigh);
        //rightThigh.StatName = "Right Thigh";
        //rightThigh.StatBaseValue = 100;
        //rightThigh.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var leftCalf = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftCalf);
        //leftCalf.StatName = "Left Calf";
        //leftCalf.StatBaseValue = 100;
        //leftCalf.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var rightCalf = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightCalf);
        //rightCalf.StatName = "Right Calf";
        //rightCalf.StatBaseValue = 100;
        //rightCalf.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var leftFoot = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftFoot);
        //leftFoot.StatName = "Left Foot";
        //leftFoot.StatBaseValue = 100;
        //leftFoot.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        //var rightFoot = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightFoot);
        //rightFoot.StatName = "Right Foot";
        //rightFoot.StatBaseValue = 100;
        //rightFoot.protection = new int[] { 0, 0, 0, 0, 0, 0 };
    }
}