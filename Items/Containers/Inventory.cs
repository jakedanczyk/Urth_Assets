using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    public List<Item> inventoryContents;
    public RectTransform inventoryUIPanel;
    public Button selectedButton;
    public Item selectedItem;
    public float contentsWeight;


	// Use this for initialization
	void Start () {
        SumWeight();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RebuildUIPanel()
    {
        int childs = inventoryUIPanel.childCount;
        List<Item> temp = new List<Item>();
        temp.AddRange(inventoryContents);
        //for(int i = childs-1; i>=0; i--) { GameObject.Destroy(inventoryUIPanel.transform.GetChild(i).gameObject); }
        foreach(Item item in temp)
        {
            Item aItem = item;
            print(item.itemName);
            DropItem(aItem);
            AddItem(aItem);
            //item.itemUIElementScript.parentInventory = this;

            //Instantiate(item.itemUIelement, inventoryUIPanel.transform);
        }
    }

    public void AddItem(Item newItem)
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

    public void RemoveItem(Item newItem)
    {
        if (newItem == null)
            return;
        inventoryContents.Remove(newItem);
        newItem.itemUIelement.transform.SetParent(newItem.transform);
        newItem.itemUIElementScript.parentInventory = null;
        contentsWeight -= newItem.itemWeight;
    }

    public void DropItem(Item anItem)
    {
        if (anItem == null)
            return;
        anItem.loose = true;
        anItem.gameObject.SetActive(true);
        anItem.transform.position = this.transform.position;
        anItem.transform.parent = null;
        anItem.GetComponent<Rigidbody>().isKinematic = false;
        inventoryContents.Remove(anItem);
        anItem.itemUIelement.transform.SetParent(anItem.transform);
        anItem.itemUIElementScript.parentInventory = null;
        contentsWeight -= anItem.itemWeight;
    }

    public void TransferItem(Item newItem, Inventory newInventory)
    {
        inventoryContents.Remove(newItem);
        newItem.itemUIelement.transform.SetParent(newInventory.inventoryUIPanel);
        contentsWeight -= newItem.itemWeight;
        newInventory.inventoryContents.Add(newItem);
        newItem.itemUIElementScript.parentInventory = newInventory;
        newInventory.contentsWeight = +newItem.itemWeight;
    }

    public float SumWeight()
    {
        return contentsWeight = inventoryContents.Select(c => c.itemWeight).ToList().Sum();
    }

}
