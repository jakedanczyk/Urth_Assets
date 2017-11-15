using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftingSystem : MonoBehaviour
{
    public Inventory supplyInventory;
    public List<CraftingRecipe> knownRecipeList;
    public bool isCrafting;
    public CraftingRecipe selectedRecipe;
    public BodyManager_Human_Player player_bodyManager;

    void Update()
    {
        if (isCrafting)
        {
            if (Input.GetKeyDown(KeyCode.E) && !(selectedRecipe == null))
                CraftItem();
        }
    }

    public void CraftItem()
    {
        if (!selectedRecipe.RequirementsCheck())
        {
            print("Requirements Missing");
            return;
        }
        print("hey");
        for (int i = 0; i < selectedRecipe.inputs.Count; i++)
        {
            print("hey2");

            Item neededItem = selectedRecipe.inputs[i].GetComponent<Item>();
            var count = supplyInventory.inventoryContents.Where(item => item.itemID == neededItem.itemID).Count();
            print(count);
            if (count < selectedRecipe.inQuantities[i]) { print("cannot craft"); return; }
        }
        for (int i = 0; i < selectedRecipe.inputs.Count; i++)
        {
            for (int j = 0; j < selectedRecipe.inQuantities[i]; j++)
            {
                Item consumedItem = (supplyInventory.inventoryContents.Find(delegate (Item searchItem) { return searchItem.itemID == selectedRecipe.inputs[i].GetComponent<Item>().itemID; }));
                supplyInventory.RemoveItem(consumedItem);
                Destroy(consumedItem.gameObject);
            }
        }
        for (int i = 0; i < selectedRecipe.outputs.Count; i++)
        {
            for (int j = 0; j < selectedRecipe.outQuantities[i]; j++)
            {
                GameObject newItem = Instantiate(selectedRecipe.outputs[i]);
                supplyInventory.AddItem(newItem.GetComponent<Item>());
                print("item crafted");
            }
        }
    }
}
