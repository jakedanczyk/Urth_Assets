using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class BaseInventory : MonoBehaviour
{

    GameObject inventoryOverlay;
    public GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    public List<Item> contents = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    int numSlots = 60;
    bool open;
    [SerializeField]
    public GameObject rHand;
    public GameObject tool;

    void Start()
    {
        open = true;
        inventoryOverlay = GameObject.Find("Base Inventory Panel");
        slotPanel = inventoryOverlay.transform.Find("Slot Panel").gameObject;
        for(int i = 0; i < 60; i++)
        {
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(slotPanel.transform);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        { open = !open; }

        if (open)
        {
            inventoryOverlay.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!open)
        {
            inventoryOverlay.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Equip()
    {
        tool.transform.parent = rHand.transform;
        tool.transform.position = rHand.transform.position;
    }

}
