using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootInventoryFocusPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public RectTransform panel;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;
    public LootInventory attachedInventory;

    public void OnPointerDown(PointerEventData data)
    {
        panel.SetAsLastSibling();
        playerControls.SetLootActive();
    }
    public void OnPointerUp(PointerEventData data)
    {
        panel.SetAsLastSibling();
        playerControls.SetLootActive();
    }
}