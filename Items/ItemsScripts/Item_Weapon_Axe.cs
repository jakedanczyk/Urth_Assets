using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon_Axe : Item_Weapon {

    public Collider edgeCollider, buttCollider, defenseCollider;
    public int woodChopBonus, woodSplitBonus;

    public override float TreeChopRateFactor()
    {
        return base.TreeChopRateFactor() + woodChopBonus;
    }
}
