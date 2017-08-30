﻿using UnityEngine;
using System.Collections;

public class RPGDefaultStats : RPGStatCollection {
    protected override void ConfigureStats() {
        var stamina = CreateOrGetStat<RPGAttribute>(RPGStatType.Stamina);
        stamina.StatName = "Stamina";
        stamina.StatBaseValue = 10;

        var wisdom = CreateOrGetStat<RPGAttribute>(RPGStatType.Wisdom);
        wisdom.StatName = "Wisdom";
        wisdom.StatBaseValue = 5;

        var health = CreateOrGetStat<RPGVital>(RPGStatType.Health);
        health.StatName = "Health";
        health.StatBaseValue = 100;
        health.AddLinker(new RPGStatLinkerBasic(CreateOrGetStat<RPGAttribute>(RPGStatType.Stamina), 10f));
        health.UpdateLinkers();
        health.SetCurrentValueToMax();

        var mana = CreateOrGetStat<RPGVital>(RPGStatType.Magicka);
        mana.StatName = "Magicka";
        mana.StatBaseValue = 2000;
        mana.AddLinker(new RPGStatLinkerBasic(CreateOrGetStat<RPGAttribute>(RPGStatType.Magicka), 50f));
        mana.UpdateLinkers();
        mana.SetCurrentValueToMax();
    }
}
