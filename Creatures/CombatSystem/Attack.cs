using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack
{
    Animator anim;

    public Item_Weapon weapon; 
    public BodyManager attackBodyManager;

    public float reach;
    public float baseDamage;
    public Transform aimPoint;
    public Transform targetPoint;
    public GameObject struckObject;
    public DamageController targetDC;
    public string animTriggerKey;
    public List<Collider> collisionList;



    // Use this for initialization
    void Start()
    {

        //anim.SetTrigger(animTriggerKey);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        collisionList.Add(other.GetComponent<Collider>());
    }

    public void MakeAttack()
    {
        anim.SetTrigger(animTriggerKey);
        //attackBodyManager.mainWeapon.CollidersOn
    }

    public string AnimTrigger()
    { return animTriggerKey; }

    public Attack() { }

    public Attack(string newAnimTriggerKey)
    {
        this.animTriggerKey = newAnimTriggerKey;
    }

    //TODO full-bodied constructor for attack
}
