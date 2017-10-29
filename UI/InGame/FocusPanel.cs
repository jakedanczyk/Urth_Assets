using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FocusPanel : MonoBehaviour, IPointerDownHandler
{

    private RectTransform panel;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;
    void Awake()
    {
        panel = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        panel.SetAsLastSibling();
    }

}