using UnityEngine;
using UnityEngine.EventSystems;

public class FocusPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public RectTransform parentPanel;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;


    public void OnPointerDown(PointerEventData data)
    {
        parentPanel.SetAsLastSibling();
    }
    public void OnPointerUp(PointerEventData data)
    {
        parentPanel.SetAsLastSibling();
    }
}