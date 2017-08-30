using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaterialsDictionary : MonoBehaviour {

    public Dictionary<ItemMaterialType, ItemMaterial> dictItemMaterials = new Dictionary<ItemMaterialType, ItemMaterial>();

    //metals
    public Material iron;
    public Material steel;

    //rocks
    public Material wood;
    public Material granite;

    //woods

    //leathers
    public Material horseLeather;

    //fibers
    public Material hemp;

    void Awake()
    {
        dictItemMaterials.Add(ItemMaterialType.Iron, new ItemMaterial(ItemMaterialType.Iron, iron, 10, 10000, 10000));
        dictItemMaterials.Add(ItemMaterialType.Steel, new ItemMaterial(ItemMaterialType.Steel, steel, 10, 10000, 10000));
        dictItemMaterials.Add(ItemMaterialType.Wood, new ItemMaterial(ItemMaterialType.Wood, wood, .5f, 1000, 1000));
        dictItemMaterials.Add(ItemMaterialType.Granite, new ItemMaterial(ItemMaterialType.Granite, granite, 2.75f, 1000, 1000));
        dictItemMaterials.Add(ItemMaterialType.Hemp, new ItemMaterial(ItemMaterialType.Hemp, hemp, 1.5f, 690, 1));
    }

    /// Checks if there is a ItemMaterial with the given type and id
    /// </summary>
    public bool ContainMat(ItemMaterialType matType)
    {
        return dictItemMaterials.ContainsKey(matType);
    }

    /// <summary>
	/// Gets the RPGStat with the given RPGStatTyp and ID
	/// </summary>
    public ItemMaterial GetMat(ItemMaterialType matType)
    {
        if (ContainMat(matType))
        {
            return dictItemMaterials[matType];
        }
        return null;
    }
}
