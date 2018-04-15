using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Fire : MonoBehaviour {

    public Inventory fireContents;
    public LootInventory fireLoot;
    public float fuelRate = 1.26f;
    public float fuel = 3402f;
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
        prevFuelCheckTime = worldTime.totalGameSeconds;
        InvokeRepeating("BurnFuel", 30, 30);
    }
    
    public void AddFuel(Item newFuel)
    {
        fuel += newFuel.itemWeight;
        Destroy(newFuel.gameObject);
    }

    void BurnFuel()
    {
        fuel -= fuelRate * (worldTime.totalGameSeconds - prevFuelCheckTime);
        prevFuelCheckTime = worldTime.totalGameSeconds;
        if(fuel < 0)
        {
            Extinguish();
        }
    }

    public void Extinguish()
    {
        flameFX.SetActive(false);
        heat.SetActive(false);
        soundFX.enabled = false;
        isLit = false;
        StopAllCoroutines();
    }
}
