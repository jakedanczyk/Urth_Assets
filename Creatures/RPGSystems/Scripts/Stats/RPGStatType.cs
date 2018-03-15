using UnityEngine;
using System.Collections;

/// <summary>
/// Used to list off all stats that can be used
/// within the RPGStatCollection class
/// </summary>
public enum RPGStatType {
    None = 0,
        
    //stats
    Health = 1,
    Magicka = 2,
    Stamina = 3,
    Height = 4, //mm
    Weight = 5, //g
    Muscle = 6, //g
    Bodyfat = 7, //g
    CarryWeight = 8, //g
    SurfaceArea = 9,

    JumpHeight = 10,
    SprintSpeed = 11,
    RunSpeed = 12,
    JogSpeed = 13,
    WalkSpeed = 14,
    CrouchSpeed = 15,

    AttackSpeed = 20,
    AttackSpeedEmpty = 21,
    AttackSpeedHeavy = 22,

    RestingHeartRate = 30,
    RestingOxygenRate = 31,

    
    //maybes
    Wisdom = 99,

    //attributes
    Agility = 100,
    Dexterity = 101,
    Endurance = 102,
    Speed = 103,
    Strength = 104,
    Toughness = 105,

    Attunement = 110,
    Creativity = 111,
    Intelligence = 112,
    Willpower = 113,
    ThirdEye = 114,
    Perception = 115,

    Artistry = 120,
    Confidence = 121,
    Courage = 122,
    Empathy = 123,
    Openness = 124,
    Warmth = 125,

    //skills
    //magic



    //combat skills

    Dodge = 546,
    Deflect = 547,
    Absorb = 548,
    Shield = 549,
    Parry = 550,  //should turn this into dodge, deflect, absorb, parry so people can specialize...each being naturally suited to pair with light, medium, heavy armor.
    //OffHandWeapon = 551, //not implemented
    Armor = 552,

    ArmStrike = 553,
    LegStrike = 554,
    Bite = 555,

    Axe = 556,      //hatchet, wood axe, 1hand axe, 2hand axe
    Blunt = 557,    //clubs, maces, hammers, flails, morningstart
    Pick = 558,     //picks
    ShortBlade = 559,    //knives, daggers, shortswords
    LongBlade = 560,    //longsword, falchion, greatsword, sabre
    Lash = 561,     //whip, scourge
    LightPoleArm = 562, //staff, shortspear, javelin
    HeavyPoleArm = 563, // spear > 2m, pike, glaive, bill, halberd

    CrossBow = 564,
    Bow = 565,
    Sling = 566,
    Atlatl = 567,
    Throwing = 568,

    //movement skills
    //Sneak = 590,
    //Acrobatics = 591,
    //Swim = 592,
    //Climbing = 593,

    //skills
    StoneWorking = 599,
    WoodWorking = 600,
    FireMaking = 601,
    Gathering = 602,
    Farming = 603,
    Mining = 604,
    Medicine = 605,
    AnimalTraining = 606,
    Tracking = 607,
    Pottery = 608,
    Fishing = 609,
    Butchery = 610,
    Cooking = 611,
    Tailoring = 612,
    WeatherSense = 613,


    //ITEMS
    ItemWeight = 1000,
    ItemVolume = 1001,

    SlashDamage = 1010,
    PierceDamage = 1011,
    BluntDamage = 1012,

    //block tools
    BaseDamage = 1020,
    DirtDamage = 1021,
    StoneDamage = 1022,

    //BLOCKS
    Durability = 2000,

    //Body Parts System

    //HeadHealth = 3000,
    //HeadSlashArmor = 3001,
    //HeadPierceArmor = 3002,
    //HeadBluntArmor = 3003,
    //HeadInsulation = 3004,
    //HeadWaterCover = 3005,
    //HeadWindCover = 3006,

    //NeckHealth = 3010,
    //NeckSlashArmor = 3011,
    //NeckPierceArmor = 3012,
    //NeckBluntArmor = 3013,
    //NeckInsulation = 3014,
    //NeckWaterCover = 3015,
    //NeckWindCover = 3016,

    //ChestHealth = 3020,
    //ChestSlashArmor = 3021,
    //ChestPierceArmor = 3022,
    //ChestBluntArmor = 3023,
    //ChestInsulation = 3024,
    //ChestWaterCover = 3025,
    //ChestWindCover = 3026,

    //StomachHealth = 3030,
    //StomachSlashArmor = 3031,
    //StomachPierceArmor = 3032,
    //StomachBluntArmor = 3033,
    //StomachInsulation = 3034,
    //StomachWaterCover = 3035,
    //StomachWindCover = 3036,

    //PelvisHealth = 3040,
    //PelvisSlashArmor = 3041,
    //PelvisPierceArmor = 3042,
    //PelvisBluntArmor = 3043,
    //PelvisInsulation = 3044,
    //PelvisWaterCover = 3045,
    //PelvisWindCover = 3046,

    //UpperBackHealth = 3050,
    //UpperBackSlashArmor = 3051,
    //UpperBackPierceArmor = 3052,
    //UpperBackBluntArmor = 3053,
    //UpperBackInsulation = 3054,
    //UpperBackWaterCover = 3055,
    //UpperBackWindCover = 3056,

    //LowerBackHealth = 3060,
    //LowerBackSlashArmor = 3061,
    //LowerBackPierceArmor = 3062,
    //LowerBackBluntArmor = 3063,
    //LowerBackInsulation = 3064,
    //LowerBackWaterCover = 3065,
    //LowerBackWindCover = 3066,

    //LeftThighHealth = 3070,
    //LeftThighSlashArmor = 3071,
    //LeftThighPierceArmor = 3072,
    //LeftThighBluntArmor = 3073,
    //LeftThighInsulation = 3074,
    //LeftThighWaterCover = 3075,
    //LeftThighWindCover = 3076,

    //RightThighHealth = 3080,
    //RightThighSlashArmor = 3081,
    //RightThighPierceArmor = 3082,
    //RightThighBluntArmor = 3083,
    //RightThighInsulation = 3084,
    //RightThighWaterCover = 3085,
    //RightThighWindCover = 3086,

    //LeftLowerLegHealth = 3090,
    //LeftLowerLegSlashArmor = 3091,
    //LeftLowerLegPierceArmor = 3092,
    //LeftLowerLegBluntArmor = 3093,
    //LeftLowerLegInsulation = 3094,
    //LeftLowerLegWaterCover = 3095,
    //LeftLowerLegWindCover = 3096,

    //RightLowerLegHealth = 3100,
    //RightLowerLegSlashArmor = 3101,
    //RightLowerLegPierceArmor = 3102,
    //RightLowerLegBluntArmor = 3103,
    //RightLowerLegInsulation = 3104,
    //RightLowerLegWaterCover = 3105,
    //RightLowerLegWindCover = 3106,

    //LeftFootHealth = 3110,
    //LeftFootSlashArmor = 3111,
    //LeftFootPierceArmor = 3112,
    //LeftFootBluntArmor = 3113,
    //LeftFootInsulation = 3114,
    //LeftFootWaterCover = 3115,
    //LeftFootWindCover = 3116,

    //RightFootHealth = 3120,
    //RightFootSlashArmor = 3121,
    //RightFootPierceArmor = 3122,
    //RightFootBluntArmor = 3123,
    //RightFootInsulation = 3124,
    //RightFootWaterCover = 3125,
    //RightFootWindCover = 3126,

    //LeftShoulderHealth = 3130,
    //LeftShoulderSlashArmor = 3131,
    //LeftShoulderPierceArmor = 3132,
    //LeftShoulderBluntArmor = 3133,
    //LeftShoulderInsulation = 3134,
    //LeftShoulderWaterCover = 3135,
    //LeftShoulderWindCover = 3136,

    //RightShoulderHealth = 3140,
    //RightShoulderSlashArmor = 3141,
    //RightShoulderPierceArmor = 3142,
    //RightShoulderBluntArmor = 3143,
    //RightShoulderInsulation = 3144,
    //RightShoulderWaterCover = 3145,
    //RightShoulderWindCover = 3146,

    //LeftUpperArmHealth = 3150,
    //LeftUpperArmSlashArmor = 3151,
    //LeftUpperArmPierceArmor = 3152,
    //LeftUpperArmBluntArmor = 3153,
    //LeftUpperArmInsulation = 3154,
    //LeftUpperArmWaterCover = 3155,
    //LeftUpperArmWindCover = 3156,

    //RightUpperArmHealth = 3160,
    //RightUpperArmSlashArmor = 3161,
    //RightUpperArmPierceArmor = 3162,
    //RightUpperArmBluntArmor = 3163,
    //RightUpperArmInsulation = 3164,
    //RightUpperArmWaterCover = 3165,
    //RightUpperArmWindCover = 3166,

    //LeftForearmHealth = 3170,
    //LeftForearmSlashArmor = 3171,
    //LeftForearmPierceArmor = 3172,
    //LeftForearmBluntArmor = 3173,
    //LeftForearmInsulation = 3174,
    //LeftForearmWaterCover = 3175,
    //LeftForearmWindCover = 3176,

    //RightForearmHealth = 3180,
    //RightForearmSlashArmor = 3181,
    //RightForearmPierceArmor = 3182,
    //RightForearmBluntArmor = 3183,
    //RightForearmInsulation = 3184,
    //RightForearmWaterCover = 3185,
    //RightForearmWindCover = 3186,

    //LeftHandHealth = 3190,
    //LeftHandSlashArmor = 3191,
    //LeftHandPierceArmor = 3192,
    //LeftHandBluntArmor = 3193,
    //LeftHandInsulation = 3194,
    //LeftHandWaterCover = 3195,
    //LeftHandWindCover = 3196,

    //RightHandHealth = 3200,
    //RightHandSlashArmor = 3201,
    //RightHandPierceArmor = 3202,
    //RightHandBluntArmor = 3203,
    //RightHandInsulation = 3204,
    //RightHandWaterCover = 3205,
    //RightHandWindCover = 3206,    


    //Body Parts system

    Head = 4000,
    Neck = 4001,
    Chest = 4002,
    Stomach = 4003,
    Pelvis = 4004,
    UpperBack = 4005,
    LowerBack = 4006,
    LeftShoulder=4007,
    RightShoulder = 4008,
    LeftUpperArm = 4009,
    RightUpperArm = 4010,
    LeftForearm = 4011,
    RightForearm = 4012,
    LeftHand = 4013,
    RightHand = 4014,
    LeftThigh = 4015,
    RightThigh = 4016,
    LeftCalf = 4017,
    RightCalf = 4018,
    LeftFoot = 4019,
    RightFoot = 4020,

    Body = 4050,
    Torso = 4051,
    Limb = 4052,
    Leg = 4053,
    Arm = 4054,
    Tail = 4055,
}
