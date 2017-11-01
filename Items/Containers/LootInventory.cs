﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInventory : Inventory {

	public new void RebuildUIPanel()
    {
        int childs = inventoryUIPanel.childCount;
        List<Item> temp = new List<Item>();
        temp.AddRange(inventoryContents);
        List<ItemUI_ButtonScript> buttons = new List<ItemUI_ButtonScript>();
        for (int i = childs - 1; i >= 0; i--)
        {
            buttons.Add(inventoryUIPanel.transform.GetChild(i).gameObject.GetComponent<ItemUI_ButtonScript>());
        }
        foreach (ItemUI_ButtonScript button in buttons)
        {
            button.gameObject.transform.parent = button.parentItem.transform;
        }
        foreach (Item item in temp)
        {
            Item aItem = item;
            print(item.itemName);
            DropItem(aItem);
            AddItem(aItem);
            //item.itemUIElementScript.parentInventory = this;

            //Instantiate(item.itemUIelement, inventoryUIPanel.transform);
        }
    }

    public new void AddItem(Item newItem)
    {
        newItem.itemUIelement.transform.SetParent(inventoryUIPanel);
        newItem.itemUIelement.GetComponent<RectTransform>().localPosition = Vector3.zero;
        newItem.itemUIelement.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        newItem.itemUIelement.GetComponent<RectTransform>().localScale = Vector3.one;
        newItem.itemUIElementScript.parentInventory = this;
        newItem.loose = false;
        newItem.gameObject.SetActive(false);
        inventoryContents.Add(newItem);
        contentsWeight += newItem.itemWeight;
    }

}
