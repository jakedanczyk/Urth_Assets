using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager_Horse : BodyManager_Ungulate {

    public override void RemoveGarment(Item_Garment garment)
    {

    }

    public override void ProcessThisBody()
    {
    }

    public override void SheatheWeapon(Item_Weapon aWeapon)
    {
        if (!aWeapon.wielded)
            return;
        Debug.LogWarning("Sheate weapon: " + aWeapon.itemName);
        aWeapon.GetComponent<Rigidbody>().isKinematic = false;
        aWeapon.GetComponent<Rigidbody>().useGravity = true;
        aWeapon.gameObject.SetActive(false);
    }

}
