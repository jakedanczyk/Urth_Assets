using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item_Weapon : Item {

    public int baseBlunt, baseCut, basePierce;

    public int butcheringAid;
    public bool wielded;
    public WeaponType weaponType;

    public Vector3 gripAdjust;
    public Vector3 gripOrientation;

    public List<Collider> collList;

    public void Wield()
    {
        BodyManager_Human body = GetComponent<BodyManager_Human>();
        body.DrawWeapon(this);
    }

}
