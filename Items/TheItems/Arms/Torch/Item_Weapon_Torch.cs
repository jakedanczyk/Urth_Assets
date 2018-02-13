using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon_Torch : Item_Weapon
{
    public float fuelRate = 666f;
    public float fuel = 10000f;
    public bool isLit;

    float maxTemp = 500f;
    float temp = 500f;
    public GameObject heat;
    public GameObject flameFX;
    public AudioSource soundFX;

    public WorldTime worldTime;

    public void LightTorch()
    {
        flameFX.SetActive(true);
        heat.SetActive(true);
        soundFX.enabled = true;
        isLit = true;
        InvokeRepeating("BurnFuel", 30, 30);
    }

    void BurnFuel()
    {
        fuel -= fuelRate;
        if (fuel < 0)
        {
            heat.gameObject.SetActive(false);
            flameFX.SetActive(false);
            isLit = false;
        }
    }
}
