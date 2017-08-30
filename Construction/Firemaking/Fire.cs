using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fire : MonoBehaviour {

    public Inventory fireContents;
    public float fuelRate = 666f;
    public float fuel = 10000f;
    public bool isLit;

    float maxTemp = 500f;
    float temp = 500f;
    public GameObject heat;
    public GameObject flameFX;

    public WorldTime worldTime;

    public void LightFire()
    {
        flameFX.SetActive(true);
        heat.SetActive(true);
        isLit = true;
        gameObject.tag = "LitFire";
        InvokeRepeating("BurnFuel", 30, 30);
    }

    void BurnFuel()
    {
        fuel -= fuelRate;
        if (fireContents.inventoryContents.Any())
        {
            Item newFuel = fireContents.inventoryContents.FirstOrDefault(item => item.primaryMaterialType == ItemMaterialType.Wood);
            if (!(newFuel == null))
            {
                fuel += newFuel.itemWeight * 10;
                Destroy(newFuel.gameObject);
            }
        }
        if(fuel < 0)
        {
            heat.gameObject.SetActive(false);
            flameFX.SetActive(false);
            isLit = false;
        }
    }
}
