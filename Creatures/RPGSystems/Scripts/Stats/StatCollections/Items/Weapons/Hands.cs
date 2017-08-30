using UnityEngine;
using System.Collections;

public class Hands : RPGStatCollection {
    protected override void ConfigureStats() {
        var dirtDamage = CreateOrGetStat<RPGAttribute>(RPGStatType.DirtDamage);
        dirtDamage.StatName = "Soil Removal";
        dirtDamage.StatBaseValue = 10;

        var stoneDamage = CreateOrGetStat<RPGAttribute>(RPGStatType.StoneDamage);
        stoneDamage.StatName = "Stone Breaking";
        stoneDamage.StatBaseValue = 10;
    }
}
