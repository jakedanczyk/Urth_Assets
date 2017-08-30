using UnityEngine;
using System.Collections;

public class RPGGrassStats : RPGStatCollection
{
    protected override void ConfigureStats()
    {
        var durability = CreateOrGetStat<RPGVital>(RPGStatType.Durability);
        durability.StatName = "Durability";
        durability.StatBaseValue = 100;
    }
}
