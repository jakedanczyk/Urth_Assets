using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootInventoryFocusPanel : MonoBehaviour, IPointerDownHandler
{

    private RectTransform panel;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;
    public LootInventory attachedInventory;
    void Awake()
    {
        panel = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        print("clicked");
        panel.SetAsLastSibling();
        playerControls.lootActive = true;
        playerControls.inventoryActive = false;
    }

}