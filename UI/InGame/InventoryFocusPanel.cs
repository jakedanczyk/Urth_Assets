using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryFocusPanel : MonoBehaviour, IPointerDownHandler
{

    private RectTransform panel;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;
    public PlayerInventory attachedInventory;
    void Awake()
    {
        panel = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        print("clicked");
        panel.SetAsLastSibling();
        playerControls.lootActive = false;
        playerControls.inventoryActive = true;
    }

}