using UnityEngine;
using System.Collections;

public class ItemMaterial {

    public ItemMaterialType itemMaterialID;
    public Material unityMaterial;
    public float density;
    public int tensileYield;
    public int compressiveYield;

    public ItemMaterial(ItemMaterialType thisMaterialID, Material thisMaterial, float newDensity, int newTY, int newCY) {
        itemMaterialID = thisMaterialID;
        unityMaterial = thisMaterial;
        density = newDensity; //g per cubic cm
        tensileYield = newTY; //mPa
        compressiveYield = newCY; //mPa
    }
}