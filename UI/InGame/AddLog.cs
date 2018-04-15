using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using System.Linq;

public class AddLog : MonoBehaviour, ISelectHandler
{
    public UIManager ui;
    public Button thisButton;

    void Awake()
    {
        thisButton = GetComponentInParent<Button>();
    }


    public void OnSelect(BaseEventData eventData)
    {
        Item newLog = ui.inventory.inventoryContents.FirstOrDefault(item => (item.itemName.Contains("Log") || item.itemName.Contains("log")));
        if (newLog)
        {
            ui.CurrFire.AddFuel(newLog);
        }
    }
}
