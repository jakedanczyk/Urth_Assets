using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using System;

public class ItemUI_ButtonScript : MonoBehaviour, ISelectHandler, IPointerDownHandler, IPointerUpHandler // required interface when using the OnSelect method. 
{

    public Item parentItem;
    public Inventory parentInventory;
    public Button thisButton;
    public RectTransform panel;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;
    public Text nameText, weightText, volumeText, infoText;
    //void Awake()
    //{
    //parentItem = GetComponentInParent<Item>();
    //thisButton = GetComponentInParent<Button>();
    // GetComponentInChildren<Text>().text = parentItem.itemName;
    //}

    // Use this for initialization
    void Start ()
    {
        playerControls = UnityStandardAssets.Characters.FirstPerson.PlayerControls.playerControlsGameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.PlayerControls>();
        if (parentItem == null)
            parentItem = this.GetComponentInParent<Item>();
        nameText.text = parentItem.itemName;
        weightText.text = parentItem.itemWeight + "g";
        volumeText.text = parentItem.itemVolume + "mL";
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
        parentInventory.inventoryUIPanel.gameObject.SetActive(true);
        parentInventory.SelectItem(parentItem);
    }

    public void OnSelect(BaseEventData eventData)
    {
        parentInventory.SelectItem(parentItem);
    }

    public void OnPointerDown(PointerEventData data)
    {
        parentInventory.SelectItem(parentItem);
        parentInventory.inventoryUIPanel.SetAsLastSibling();
        if (parentInventory is PlayerInventory)
            playerControls.SetInventoryActive();
        else
            playerControls.SetLootActive();
    }
    public void OnPointerUp(PointerEventData data)
    {
        parentInventory.SelectItem(parentItem);
        parentInventory.inventoryUIPanel.SetAsLastSibling();
        playerControls.SetLootActive();
        if (parentInventory is PlayerInventory)
            playerControls.SetInventoryActive();
        else
            playerControls.SetLootActive();
    }
}
