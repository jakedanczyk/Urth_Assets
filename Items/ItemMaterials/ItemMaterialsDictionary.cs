using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaterialsDictionary : MonoBehaviour {

    public Dictionary<MaterialType, ItemMaterial> dictItemMaterials = new Dictionary<MaterialType, ItemMaterial>();

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
        dictItemMaterials.Add(MaterialType.Iron, new ItemMaterial(MaterialType.Iron, iron, 10, 10000, 10000));
        dictItemMaterials.Add(MaterialType.Steel, new ItemMaterial(MaterialType.Steel, steel, 10, 10000, 10000));
        dictItemMaterials.Add(MaterialType.Wood, new ItemMaterial(MaterialType.Wood, wood, .5f, 1000, 1000));
        dictItemMaterials.Add(MaterialType.Granite, new ItemMaterial(MaterialType.Granite, granite, 2.75f, 1000, 1000));
        dictItemMaterials.Add(MaterialType.Hemp, new ItemMaterial(MaterialType.Hemp, hemp, 1.5f, 690, 1));
    }

    /// Checks if there is a ItemMaterial with the given type and id
    /// </summary>
    public bool ContainMat(MaterialType matType)
    {
        return dictItemMaterials.ContainsKey(matType);
    }

    /// <summary>
	/// Gets the RPGStat with the given RPGStatTyp and ID
	/// </summary>
    public ItemMaterial GetMat(MaterialType matType)
    {
        if (ContainMat(matType))
        {
            return dictItemMaterials[matType];
        }
        return null;
    }
}
