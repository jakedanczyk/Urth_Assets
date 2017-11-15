using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using System;

public class StartFireButtonScript : MonoBehaviour, ISelectHandler // required interface when using the OnSelect method. 
{

    public Fire currentFire;
    public Inventory currentFireInventory;
    public Button thisButton;
    public BodyManager_Human_Player player_bodyManager;

    void Awake()
    {
        thisButton = GetComponentInParent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        player_bodyManager.StartFire(currentFire, 1);
    }
}
