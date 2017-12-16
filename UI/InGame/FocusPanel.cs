using UnityEngine;
using UnityEngine.EventSystems;

public class FocusPanel : MonoBehaviour, IPointerDownHandler
{

    public RectTransform parentPanel;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;


    public void OnPointerDown(PointerEventData data)
    {
        parentPanel.SetAsLastSibling();
    }

}