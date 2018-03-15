using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all weapons. For now body part weapons will need an Item_Weapon
public abstract class Item_Weapon : Item {

    public int baseBlunt, baseCut, basePierce;

    public int butcheringAid;
    public bool isWielded,isTwoHand;
    public WeaponType weaponType;

    public Collider primaryCollider;

    public Vector3 gripAdjust;
    public Vector3 gripOrientation;

    public BodyManager wielderBodyManager;

    public List<Collider> collList;
    public List<WeaponColliderScript> weaponCollList;

    public void Wield()
    {
        BodyManager_Human body = GetComponent<BodyManager_Human>();
        body.DrawWeapon(this);
    }

    public virtual float TreeChopRateFactor()
    {
        return (baseBlunt * baseCut);
    }

}
