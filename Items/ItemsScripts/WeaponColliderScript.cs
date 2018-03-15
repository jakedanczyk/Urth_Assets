using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColliderScript : MonoBehaviour {

    public Item_Weapon parentWeapon;
    public bool isActive;

    void OnCollisionEnter(Collision other)
    {
        if (isActive && other.transform.root != parentWeapon.wielderBodyManager.transform)
        {
            parentWeapon.wielderBodyManager.audioSource.PlayOneShot(parentWeapon.wielderBodyManager.audioManager.ImpactSound(parentWeapon,other));
            parentWeapon.wielderBodyManager.primaryAttackCollisionList.Add(other);
        }
    }
}