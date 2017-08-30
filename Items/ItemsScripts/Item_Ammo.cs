using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Ammo : Item_Stack {

    public int baseBlunt, baseCut, basePierce;
    public GameObject itemPrefab;
    public Collider attackCollider;
    public Projectile projectile;

}
