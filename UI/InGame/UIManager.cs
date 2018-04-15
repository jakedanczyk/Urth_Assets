using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class UIManager : MonoBehaviour
{
    public bool WindowsOpen { get; set; }
    public BodyManager_Human_Player player_bodyManager;
    public PlayerControls playerControls;
    public Inventory inventory;

    public Fire CurrFire { get; set; }
    public GameObject craftingUI, fireUIPanel;
    public StartFireButtonScript fireButton;

    public void OpenFireMenu(Fire fire)
    {
        WindowsOpen = true;
        CurrFire = fire;
        fireUIPanel.SetActive(true);
        fireButton.currentFire = fire;
        fireButton.currentFireInventory = fire.fireLoot;
    }

}
