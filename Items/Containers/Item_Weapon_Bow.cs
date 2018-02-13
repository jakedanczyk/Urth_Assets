using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon_Bow : Item_Weapon {

    public float drawTime = 1;

    public Transform nockPoint;

    public Animator anim;

    public void Draw()
    {
        anim.SetBool("isDrawing", true);
    }

    public void Fire()
    {
        anim.SetTrigger("Fire");
        anim.SetBool("isDrawing", false);
    }

    public void Release()
    {
        anim.SetBool("isDrawing", false);
    }

}
