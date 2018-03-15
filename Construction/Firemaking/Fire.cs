using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fire : MonoBehaviour {

    public Inventory fireContents;
    public LootInventory fireLoot;
    public float fuelRate = 22f;
    public float fuel = 20000f;
    public bool isLit;

    float maxTemp = 500f;
    float temp = 500f;
    public GameObject heat;
    public GameObject flameFX;
    public AudioSource soundFX;

    public WorldTime worldTime;
    float prevFuelCheckTime;

    private void Start()
    {
        worldTime = WorldTime.thisObject.GetComponent<WorldTime>();
    }

    public void LightFire()
    {
        flameFX.SetActive(true);
        heat.SetActive(true);
        soundFX.enabled = true;
        isLit = true;
        gameObject.tag = "LitFire";
        prevFuelCheckTime = worldTime.totalGameSeconds;
        InvokeRepeating("BurnFuel", 30, 30);
    }

    void BurnFuel()
    {
        fuel -= fuelRate * (worldTime.totalGameSeconds - prevFuelCheckTime);
        prevFuelCheckTime = worldTime.totalGameSeconds;
        if (fireContents.inventoryContents.Any())
        {
            Item newFuel = fireContents.inventoryContents.FirstOrDefault(item => item.primaryMaterialType == MaterialType.Wood);
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
