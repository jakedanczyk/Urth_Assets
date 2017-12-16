using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using System;

public class ItemUI_ButtonScript : MonoBehaviour, ISelectHandler // required interface when using the OnSelect method. 
{

public    Item parentItem;
public    Inventory parentInventory;
public    Button thisButton;

    //void Awake()
    //{
        //parentItem = GetComponentInParent<Item>();
        //thisButton = GetComponentInParent<Button>();
       // GetComponentInChildren<Text>().text = parentItem.itemName;
    //}

    // Use this for initialization
    void Start ()
    {
        if (parentItem == null)
            parentItem = this.GetComponentInParent<Item>();
        thisButton.GetComponentInChildren<Text>().text = parentItem.itemName;
    }

    // Update is called once per frame
    //void Update () {
    //}
    
    public void AttachToInventory()
    {
        parentInventory = GetComponentInParent<Inventory>();
    }

    void SelectOnClick()
    {
        parentInventory.SelectItem(parentItem);
    }

    public void OnSelect(BaseEventData eventData)
    {
        parentInventory.SelectItem(parentItem);
    }
}
