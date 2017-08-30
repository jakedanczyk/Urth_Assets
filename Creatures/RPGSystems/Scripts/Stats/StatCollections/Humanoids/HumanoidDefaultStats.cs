using UnityEngine;
using System.Collections;

public class HumanoidDefaultStats : RPGStatCollection
{
    protected override void ConfigureStats()
    {
    //attributes
        //physical
        var agility = CreateOrGetStat<RPGAttribute>(RPGStatType.Agility);
        agility.StatName = "Agility";
        agility.StatBaseValue = 200;
        agility.Aptitude = 100;

        var dexterity = CreateOrGetStat<RPGAttribute>(RPGStatType.Dexterity);
        dexterity.StatName = "Dexterity";
        dexterity.StatBaseValue = 200;
        dexterity.Aptitude = 100;
               
        var endurance = CreateOrGetStat<RPGAttribute>(RPGStatType.Endurance);
        endurance.StatName = "Endurance";
        endurance.StatBaseValue = 200;
        endurance.Aptitude = 100;

        var speed = CreateOrGetStat<RPGAttribute>(RPGStatType.Speed);
        speed.StatName = "Speed";
        speed.StatBaseValue = 200;
        speed.Aptitude = 100;

        var strength = CreateOrGetStat<RPGAttribute>(RPGStatType.Strength);
        strength.StatName = "Strength";
        strength.StatBaseValue = 200;
        strength.Aptitude = 100;

        var toughness = CreateOrGetStat<RPGAttribute>(RPGStatType.Toughness);
        toughness.StatName = "Toughness";
        toughness.StatBaseValue = 200;
        toughness.Aptitude = 100;

        //mental
        var attunement = CreateOrGetStat<RPGAttribute>(RPGStatType.Attunement);
        attunement.StatName = "Attunement";
        attunement.StatBaseValue = 200;
        attunement.Aptitude = 100;

        var creativity = CreateOrGetStat<RPGAttribute>(RPGStatType.Creativity);
        creativity.StatName = "Creativity";
        creativity.StatBaseValue = 200;
        creativity.Aptitude = 100;
        
        var intelligence = CreateOrGetStat<RPGAttribute>(RPGStatType.Intelligence);
        intelligence.StatName = "Intelligence";
        intelligence.StatBaseValue = 200;
        intelligence.Aptitude = 100;

        var perception = CreateOrGetStat<RPGAttribute>(RPGStatType.Perception);
        perception.StatName = "Perception";
        perception.StatBaseValue = 200;
        perception.Aptitude = 100;

        var thirdEye = CreateOrGetStat<RPGAttribute>(RPGStatType.ThirdEye);
        thirdEye.StatName = "Third Eye";
        thirdEye.StatBaseValue = 200;
        thirdEye.Aptitude = 100;

        var willpower = CreateOrGetStat<RPGAttribute>(RPGStatType.Willpower);
        willpower.StatName = "Willpower";
        willpower.StatBaseValue = 200;
        willpower.Aptitude = 100;

        //emotional
        var artistry = CreateOrGetStat<RPGAttribute>(RPGStatType.Artistry);
        artistry.StatName = "Artistry";
        artistry.StatBaseValue = 200;
        artistry.Aptitude = 100;

        var confidence = CreateOrGetStat<RPGAttribute>(RPGStatType.Confidence);
        confidence.StatName = "Confidence";
        confidence.StatBaseValue = 200;
        confidence.Aptitude = 100;

        var courage = CreateOrGetStat<RPGAttribute>(RPGStatType.Courage);
        courage.StatName = "Courage";
        courage.StatBaseValue = 200;
        courage.Aptitude = 100;

        var empathy = CreateOrGetStat<RPGAttribute>(RPGStatType.Empathy);
        empathy.StatName = "Empathy";
        empathy.StatBaseValue = 200;
        empathy.Aptitude = 100;

        var openness = CreateOrGetStat<RPGAttribute>(RPGStatType.Openness);
        openness.StatName = "Openness";
        openness.StatBaseValue = 200;
        openness.Aptitude = 100;
                
        var warmth = CreateOrGetStat<RPGAttribute>(RPGStatType.Warmth);
        warmth.StatName = "Warmth";
        warmth.StatBaseValue = 200;
        warmth.Aptitude = 100;

        //derived stats
        var health = CreateOrGetStat<RPGVital>(RPGStatType.Health);
        health.StatName = "Health";
        health.StatBaseValue = 100;
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
        magicka.StatBaseValue = 100;
        magicka.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Attunement), .5f, -200f));
        magicka.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Creativity), .05f, -200f));
        magicka.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Intelligence), .5f, -200f));
        magicka.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Willpower), .05f, -200f));
        magicka.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Confidence), .2f, -400f));
        magicka.UpdateLinkers();
        magicka.SetCurrentValueToMax();

        var height = CreateOrGetStat<RPGAttribute>(RPGStatType.Height);
        height.StatName = "Height";
        height.StatBaseValue = 1800; //mm



        var muscle = CreateOrGetStat<RPGVital>(RPGStatType.Muscle); //9kg per 100 str @ 1800mm height
        muscle.StatName = "Muscle";
        muscle.StatBaseValue = 1000; //g

        muscle.AddDualLinker(new RPGDualStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Strength), CreateOrGetStat<RPGAttribute>(RPGStatType.Height), .05f));
        muscle.UpdateDualLinkers();
        muscle.SetCurrentValueToMax();

        var bodyfat = CreateOrGetStat<RPGAttribute>(RPGStatType.Bodyfat);
        bodyfat.StatName = "Bodyfat";
        bodyfat.StatBaseValue = 1000; //g

        var weight = CreateOrGetStat<RPGVital>(RPGStatType.Weight);
        weight.StatName = "Weight";
        weight.StatBaseValue = 0; //g
        weight.AddLinker(new RPGStatLinkerQuadratic(CreateOrGetStat<RPGAttribute>(RPGStatType.Height), .000012f, 0));
        weight.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Bodyfat), 1f, 0));
        weight.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Muscle), 1f, 0));
        weight.UpdateLinkers();
        weight.SetCurrentValueToMax();

        var carryWeight = CreateOrGetStat<RPGVital>(RPGStatType.CarryWeight);
        carryWeight.StatName = "Max Carry Weight";
        carryWeight.StatBaseValue = 1000; //g
        carryWeight.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Strength), 500f, 20f));
        carryWeight.AddLinker(new RPGStatLinkerLinear(CreateOrGetStat<RPGAttribute>(RPGStatType.Weight), 2.2f, -34000f));
        carryWeight.UpdateLinkers();
        carryWeight.SetCurrentValueToMax();


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
        restingHeartRate.StatBaseValue = 60;
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

        var parry = CreateOrGetStat<RPGSkill>(RPGStatType.Parry);
        parry.StatName = "Parry";
        parry.StatBaseValue = 1;

        var shield = CreateOrGetStat<RPGSkill>(RPGStatType.Shield);
        shield.StatName = "Shield";
        shield.StatBaseValue = 1;


        var armStrike = CreateOrGetStat<RPGSkill>(RPGStatType.ArmStrike);
        armStrike.StatName = "Arm Strike";
        armStrike.StatBaseValue = 1;

        var legStrike = CreateOrGetStat<RPGSkill>(RPGStatType.LegStrike);
        legStrike.StatName = "Leg Strike";
        legStrike.StatBaseValue = 1;

        var axe = CreateOrGetStat<RPGSkill>(RPGStatType.Axe);
        axe.StatName = "Axe";
        axe.StatBaseValue = 1;

        var blunt = CreateOrGetStat<RPGSkill>(RPGStatType.Blunt);
        blunt.StatName = "Blunt";
        blunt.StatBaseValue = 1;

        var pick = CreateOrGetStat<RPGSkill>(RPGStatType.Pick);
        pick.StatName = "Pick";
        pick.StatBaseValue = 1;

        var shortBlade = CreateOrGetStat<RPGSkill>(RPGStatType.ShortBlade);
        shortBlade.StatName = "ShortBlade";
        shortBlade.StatBaseValue = 1;

        var longBlade = CreateOrGetStat<RPGSkill>(RPGStatType.LongBlade);
        longBlade.StatName = "LongBlade";
        longBlade.StatBaseValue = 1;

        var lash = CreateOrGetStat<RPGSkill>(RPGStatType.Lash);
        lash.StatName = "Lash";
        lash.StatBaseValue = 1;

        var lightPoleArm = CreateOrGetStat<RPGSkill>(RPGStatType.LightPoleArm);
        lightPoleArm.StatName = "LightPoleArm";
        lightPoleArm.StatBaseValue = 1;

        var heavyPoleArm = CreateOrGetStat<RPGSkill>(RPGStatType.HeavyPoleArm);
        heavyPoleArm.StatName = "HeavyPoleArm";
        heavyPoleArm.StatBaseValue = 1;

        var atlatl = CreateOrGetStat<RPGSkill>(RPGStatType.Atlatl);
        atlatl.StatName = "Atlatl";
        atlatl.StatBaseValue = 1;

        var bow = CreateOrGetStat<RPGSkill>(RPGStatType.Bow);
        bow.StatName = "Bow";
        bow.StatBaseValue = 1;

        var crossBow = CreateOrGetStat<RPGSkill>(RPGStatType.CrossBow);
        crossBow.StatName = "CrossBow";
        crossBow.StatBaseValue = 1;

        var sling = CreateOrGetStat<RPGSkill>(RPGStatType.Sling);
        sling.StatName = "Sling";
        sling.StatBaseValue = 1;

        var throwing = CreateOrGetStat<RPGSkill>(RPGStatType.Throwing);
        throwing.StatName = "Throwing";
        throwing.StatBaseValue = 1;

        //skills
        var stoneWorking = CreateOrGetStat<RPGSkill>(RPGStatType.StoneWorking);
        stoneWorking.StatName = "StoneWorking";
        stoneWorking.StatBaseValue = 1;

        var woodWorking = CreateOrGetStat<RPGSkill>(RPGStatType.WoodWorking);
        woodWorking.StatName = "WoodWorking";
        woodWorking.StatBaseValue = 1;


        var fireMaking = CreateOrGetStat<RPGSkill>(RPGStatType.FireMaking);
        fireMaking.StatName = "FireMaking";
        fireMaking.StatBaseValue = 1;

        var gathering = CreateOrGetStat<RPGSkill>(RPGStatType.Gathering);
        gathering.StatName = "Gathering";
        gathering.StatBaseValue = 1;

        var farming = CreateOrGetStat<RPGSkill>(RPGStatType.Farming);
        farming.StatName = "Farming";
        farming.StatBaseValue = 1;

        var mining = CreateOrGetStat<RPGSkill>(RPGStatType.Mining);
        mining.StatName = "Mining";
        mining.StatBaseValue = 1;

        var medicine = CreateOrGetStat<RPGSkill>(RPGStatType.Medicine);
        medicine.StatName = "Medicine";
        medicine.StatBaseValue = 1;

        var animalTraining = CreateOrGetStat<RPGSkill>(RPGStatType.AnimalTraining);
        animalTraining.StatName = "AnimalTraining";
        animalTraining.StatBaseValue = 1;

        var tracking = CreateOrGetStat<RPGSkill>(RPGStatType.Tracking);
        tracking.StatName = "Tracking";
        tracking.StatBaseValue = 1;

        var pottery = CreateOrGetStat<RPGSkill>(RPGStatType.Pottery);
        pottery.StatName = "Pottery";
        pottery.StatBaseValue = 1;

        var fishing = CreateOrGetStat<RPGSkill>(RPGStatType.Fishing);
        fishing.StatName = "Fishing";
        fishing.StatBaseValue = 1;

        var butchery = CreateOrGetStat<RPGSkill>(RPGStatType.Butchery);
        butchery.StatName = "Butchery";
        butchery.StatBaseValue = 1;

        var cooking = CreateOrGetStat<RPGSkill>(RPGStatType.Cooking);
        cooking.StatName = "Cooking";
        cooking.StatBaseValue = 1;

        var tailoring = CreateOrGetStat<RPGSkill>(RPGStatType.Tailoring);
        tailoring.StatName = "Tailoring";
        tailoring.StatBaseValue = 1;

        var weatherSense = CreateOrGetStat<RPGSkill>(RPGStatType.WeatherSense);
        weatherSense.StatName = "WeatherSense";
        weatherSense.StatBaseValue = 1;

        //bodyparts
        var head = CreateOrGetStat<RPGBodyPart>(RPGStatType.Head);
        head.StatName = "Head";
        head.StatBaseValue = 100;
        head.bodyPartType = BodyPartType.Head;
        head.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var neck = CreateOrGetStat<RPGBodyPart>(RPGStatType.Neck);
        neck.StatName = "Neck";
        neck.StatBaseValue = 100;
        neck.bodyPartType = BodyPartType.Neck;
        neck.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var chest = CreateOrGetStat<RPGBodyPart>(RPGStatType.Chest);
        chest.StatName = "Chest";
        chest.StatBaseValue = 100;
        chest.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var stomach = CreateOrGetStat<RPGBodyPart>(RPGStatType.Stomach);
        stomach.StatName = "Stomach";
        stomach.StatBaseValue = 100;
        stomach.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var pelvis = CreateOrGetStat<RPGBodyPart>(RPGStatType.Pelvis);
        pelvis.StatName = "Pelvis";
        pelvis.StatBaseValue = 100;
        pelvis.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var upperBack = CreateOrGetStat<RPGBodyPart>(RPGStatType.UpperBack);
        upperBack.StatName = "Upper Back";
        upperBack.StatBaseValue = 100;
        upperBack.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var lowerBack = CreateOrGetStat<RPGBodyPart>(RPGStatType.LowerBack);
        lowerBack.StatName = "Lower Back";
        lowerBack.StatBaseValue = 100;
        lowerBack.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var leftShoulder = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftShoulder);
        leftShoulder.StatName = "Left Shoulder";
        leftShoulder.StatBaseValue = 100;
        leftShoulder.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var rightShoulder = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightShoulder);
        rightShoulder.StatName = "Right Shoulder";
        rightShoulder.StatBaseValue = 100;
        rightShoulder.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var leftUpperArm = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftUpperArm);
        leftUpperArm.StatName = "Left Upper Arm";
        leftUpperArm.StatBaseValue = 100;
        leftUpperArm.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var rightUpperArm = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightUpperArm);
        rightUpperArm.StatName = "Right Upper Arm";
        rightUpperArm.StatBaseValue = 100;
        rightUpperArm.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var leftForearm = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftForearm);
        leftForearm.StatName = "Left Forearm";
        leftForearm.StatBaseValue = 100;
        leftForearm.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var rightForearm = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightForearm);
        rightForearm.StatName = "Right Forearm";
        rightForearm.StatBaseValue = 100;
        rightForearm.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var leftHand = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftHand);
        leftHand.StatName = "Left Hand";
        leftHand.StatBaseValue = 100;
        leftHand.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var rightHand = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightHand);
        rightHand.StatName = "Right Hand";
        rightHand.StatBaseValue = 100;
        rightHand.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var leftThigh = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftThigh);
        leftThigh.StatName = "Left Thigh";
        leftThigh.StatBaseValue = 100;
        leftThigh.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var rightThigh = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightThigh);
        rightThigh.StatName = "Right Thigh";
        rightThigh.StatBaseValue = 100;
        rightThigh.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var leftCalf = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftCalf);
        leftCalf.StatName = "Left Calf";
        leftCalf.StatBaseValue = 100;
        leftCalf.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var rightCalf = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightCalf);
        rightCalf.StatName = "Right Calf";
        rightCalf.StatBaseValue = 100;
        rightCalf.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var leftFoot = CreateOrGetStat<RPGBodyPart>(RPGStatType.LeftFoot);
        leftFoot.StatName = "Left Foot";
        leftFoot.StatBaseValue = 100;
        leftFoot.protection = new int[] { 0, 0, 0, 0, 0, 0 };

        var rightFoot = CreateOrGetStat<RPGBodyPart>(RPGStatType.RightFoot);
        rightFoot.StatName = "Right Foot";
        rightFoot.StatBaseValue = 100;
        rightFoot.protection = new int[] { 0, 0, 0, 0, 0, 0 };
    }
}