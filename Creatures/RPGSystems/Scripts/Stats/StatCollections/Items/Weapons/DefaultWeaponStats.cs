using UnityEngine;
using System.Collections;

public class DefaultWeaponStats : RPGStatCollection {

    protected override void ConfigureStats()
    {
        //physical
        var baseDamage = CreateOrGetStat<RPGAttribute>(RPGStatType.BaseDamage);
        baseDamage.StatName = "Base Damage";
        baseDamage.StatBaseValue = 200;
    }
}
