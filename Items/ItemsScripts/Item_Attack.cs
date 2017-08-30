using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Attack : MonoBehaviour {

    public string attackName;
    public int bluntDamage, pierceDamage, slashDamage;

    public Item_Attack(string s, int x, int y, int z)
    {
        attackName = s;
        bluntDamage = x;
        pierceDamage = y;
        slashDamage = z;
    }

}
