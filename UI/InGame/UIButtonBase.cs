using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonBase : MonoBehaviour, IPointerDownHandler
{
    public RectTransform parentPanel;
    public void OnPointerDown(PointerEventData data)
    {
        parentPanel.SetAsLastSibling();
    }
}