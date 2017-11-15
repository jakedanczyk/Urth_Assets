using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Item : MonoBehaviour {
    public int itemID;
    public string itemName;
    public string itemDesc;
    public int itemWeight; //g
    public int itemVolume; //mL
    public int baseQuality;
    public int currQuality;
    public bool loose;

    public ItemMaterialType primaryMaterialType;
    public GameObject itemModel;
    public List<ItemAttribute> itemAttributes;
    public List<Item> componentItems;
    public GameObject itemUIelement;
    public ItemUI_ButtonScript itemUIElementScript;

    public ItemMaterialsDictionary matDict;

    public void Awake()
    { 
    }

    public void Start()
    {
        if (itemUIelement == null)
        {
            itemUIelement = Instantiate(Resources.Load("ItemUI_Element") as GameObject);
            itemUIElementScript = itemUIelement.GetComponent<ItemUI_ButtonScript>();
            itemUIElementScript.parentItem = this;
            itemUIElementScript.thisButton.GetComponentInChildren<Text>().text = itemUIElementScript.parentItem.itemName;
        }
        else
        {
            itemUIElementScript = itemUIelement.GetComponent<ItemUI_ButtonScript>();
        }
    }

    public bool HasAttributeType(ItemAttribute attribute)
    {
        if (itemAttributes.Contains(attribute))
        {
            return true;
        }
        else return false;
    }

    protected void MassCalc()
    {
        if (itemAttributes.OfType<ItemAssembly>().Any()) { }
        else itemWeight = (int)(itemVolume * matDict.dictItemMaterials[primaryMaterialType].density);
    }

    public void AttachUIScriptToInventory()
    {
        itemUIElementScript.AttachToInventory();
    }

    public void DropItem()
    {
        GameObject droppedItem = Instantiate(itemModel);
        this.transform.SetParent(droppedItem.transform);
        //droppedItem.AddComponent<DroppedItem>
    }
}
