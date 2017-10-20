using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tree : MonoBehaviour {

    public float health;
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
        health = 100 * size * size;
	}

    // Update is called once per frame
    void Update()
    {
        if (standing && health <= 0)
        { Fall(); }
        if (!standing && downedHealth < 0)
        { TurnToWoodPile(); }
    }

    public void Fall()
    {
        standing = false;
        GetComponent<Rigidbody>().mass = size;
        GetComponent<Rigidbody>().isKinematic = false;
        this.tag = "FelledTree";
        treeModel.tag = "FelledTree";
        WoodContent();
    }

    public int kindling, sticks, greenBoughs, logs;

    void WoodContent()
    {
        logs = (int)(size * size * .1);
        greenBoughs = (int)(size * size);
        sticks = (int)(size * size * 10);
        kindling = (int)(size * size);
        downedHealth = size * size * 100;
    }

    public void TakeDamage(float dam)
    {
        health -= dam;
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
        var newPineBoughPile = Instantiate(pineBoughPilePrefab);
        newPineBoughPile.transform.position = this.transform.position;
        newPineBoughPile.GetComponent<PineBoughPile>().stackCount = greenBoughs;
        var newKindlingPile = Instantiate(kindlingPilePrefab);
        newKindlingPile.transform.position = this.transform.position + new Vector3(0,0,1);
        newKindlingPile.GetComponent<KindlingPile>().stackCount = kindling;

        Destroy(this.gameObject);
    }

}
