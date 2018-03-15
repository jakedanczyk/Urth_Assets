using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialType {

    //Metals
    Copper = 1,
    Tin = 2,
    Bronze = 3,

    Iron = 10,
    CastIron = 11,
    Steel = 12,

    Gold = 20,
    Silver = 21,

    Mithril = 30, //half steel density, 25% harder and tougher. Silver-white
    Adamant = 31, //Double density, hardness, toughness of steel. Olive green
    Wulfram = 32, //Triple density, hardness, same toughness of steel. 


    //Ores


    //Rocks
    GenericRock = 99,
    Basalt = 100,
    Granite = 101,
    Sandstone = 102,
    Limestone = 103,
    Shale = 104,
    Schist = 105,
    Gneiss = 106,
    Quartzite = 107,

    //Woods
    Wood = 500,
    SoftWood = 501,
    HardWood = 502,

    //Leathers
    Leather = 1000,
    HorseLeather = 1001,

    //Fibers
    Hemp = 1500,
    Linen = 1501,
    Cotton = 1502,
    Jute = 1503,
    Silk = 1520,
    Wool = 1521,

}
