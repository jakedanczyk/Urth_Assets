using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CraftingRecipe : MonoBehaviour {

    Inventory supplyInventory;

    public List<GameObject> inputs;
    public List<int> inQuantities;

    public List<GameObject> outputs;
    public List<int> outQuantities;

    public abstract bool RequirementsCheck();

    //public void CraftItem(CraftingRecipe recipe)
    //{

    //    for (int i = 0; i < recipe.inputs.Count; i++)
    //    {
    //        var count = supplyInventory.inventoryContents.Where(item => item.itemID == recipe.inputs[i].itemID).Count();
    //        print(count);
    //        if (count < recipe.inQuantities[i]) { print("insufficient supplies"); return; }
    //    }
    //    for (int i = 0; i < recipe.inputs.Count; i++)
    //    {
    //        for (int j = 0; j < recipe.inQuantities[i]; j++)
    //        {
    //            Item consumedItem = (supplyInventory.inventoryContents.Find(delegate (Item searchItem) { return searchItem.itemID == recipe.inputs[i].itemID; }));
    //            supplyInventory.RemoveItem(consumedItem);
    //            Destroy(consumedItem.gameObject);
    //        }
    //    }
    //    for (int i = 0; i < recipe.outputs.Count; i++)
    //    {
    //        for (int j = 0; j < recipe.outQuantities[i]; j++)
    //        {
    //            Item newItem = Instantiate(recipe.outputs[1]);
    //            supplyInventory.AddItem(newItem);
    //            print("item crafted");
    //        }
    //    }
    //}
}
