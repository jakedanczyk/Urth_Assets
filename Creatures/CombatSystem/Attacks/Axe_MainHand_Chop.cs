using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_MainHand_Chop : Attack {

    //public Attack(string animTriggerKey) : base(animTriggerKey) { }  // constructor 
    Weapon axe;

    public void MakeAttack()
    {
        attackBodyManager.anim.SetTrigger("MainHand_Chop");
       // axe.GetComponent<Collider>().isTrigger = true;
       // axe.axeCollider.isTrigger = true;
    }

}
