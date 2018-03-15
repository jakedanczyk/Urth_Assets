using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tree : MonoBehaviour {

    public float health,startinghealth;
    public float size; 
    public bool standing;
    public GameObject treeModel;
    public GameObject woodPileModel;
    public CapsuleCollider treeCol;
    public Collider pileCol;
    public GameObject logPilePrefab;
    public GameObject pineBoughPilePrefab;
    public GameObject kindlingPilePrefab;
    public Rigidbody treeRigidBody;


    private void Awake()
    {
        treeModel.SetActive(true);
        woodPileModel.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        size = UnityEngine.Random.Range(.2f, 5f);
        transform.localScale = new Vector3(size, size, size);
        standing = true;
        health = 1000 * size * size;
        startinghealth = health;
        treeRigidBody.mass = (10000 * size) * (size) * (size);
        CalculateWoodContent();
    }

    public void Fall()
    {
        standing = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public int kindling, sticks, greenBoughs, logs;

    void CalculateWoodContent()
    {
        logs = (int)(size * size * .1);
        greenBoughs = (int)(size * size);
        sticks = (int)(size * size * 10);
        kindling = (int)(size * size);
        downedHealth = size * size * 100;
    }

    public void TakeDamage(float dam)
    {
        if (health > 0)
        {
            health -= dam;
            if (health < 0)
            {
                Fall();
                health = -1;
            }
        }
        else
        {
            health -= dam;
            if (health < -startinghealth)
                TurnToWoodPile();
        }
    }

    public float downedHealth;

    public void TakeDamage2(float dam)
    {
        downedHealth -= dam;
    }

    public void TurnToWoodPile()
    {
        //woodPileModel.SetActive(true);
        //treeModel.SetActive(false);
        //treeCol.enabled = false;
        //pileCol.enabled = true;
        var newLogPile = Instantiate(logPilePrefab);
        newLogPile.transform.position = this.transform.position;
        newLogPile.GetComponent<LogPile>().stackCount = logs;
        newLogPile.GetComponent<Rigidbody>().mass = treeRigidBody.mass * 0.5f;
        var newPineBoughPile = Instantiate(pineBoughPilePrefab);
        newPineBoughPile.transform.position = this.transform.position;
        newPineBoughPile.GetComponent<PineBoughPile>().stackCount = greenBoughs;
        newPineBoughPile.GetComponent<Rigidbody>().mass = treeRigidBody.mass * 0.25f;
        var newKindlingPile = Instantiate(kindlingPilePrefab);
        newKindlingPile.transform.position = this.transform.position + new Vector3(0,0,1);
        newKindlingPile.GetComponent<KindlingPile>().stackCount = kindling;
        newKindlingPile.GetComponent<Rigidbody>().mass = treeRigidBody.mass * 0.25f;

        Destroy(this.gameObject);
    }

}
