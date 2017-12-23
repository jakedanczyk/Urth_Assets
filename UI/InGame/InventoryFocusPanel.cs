using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryFocusPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public RectTransform panel;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;
    public PlayerInventory attachedInventory;

    public void OnPointerDown(PointerEventData data)
    {
        panel.SetAsLastSibling();
        playerControls.SetInventoryActive();
    }
    public void OnPointerUp(PointerEventData data)
    {
        panel.SetAsLastSibling();
        playerControls.SetInventoryActive();
    }
}