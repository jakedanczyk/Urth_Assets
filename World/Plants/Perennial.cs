using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perennial : MonoBehaviour {

    public int numFruits;
    public int difficulty; //gather xp per item
    public int gatherRate;
    public int greenFruitDate;
    public int ripeFruitDate;
    public int shrivelFruitDate;

    public Item fruit;
    public GameObject fruitPrefab;
    public GameObject ripeModel, harvestedModel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetHarvested()
    {
        ripeModel.SetActive(false);
        harvestedModel.SetActive(true);
    }
}
