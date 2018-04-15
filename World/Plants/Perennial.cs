using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perennial : MonoBehaviour {

    public string plantName;
    public int numFruits;
    public int difficulty; //gather xp per item
    public int gatherRate;
    public int greenFruitDate;
    public int ripeFruitDate;
    public int shrivelFruitDate;

    public Item fruit;
    public GameObject fruitPrefab;
    public GameObject ripeModel, harvestedModel;

    public void SetHarvested()
    {
        ripeModel.SetActive(false);
        harvestedModel.SetActive(true);
    }
}
