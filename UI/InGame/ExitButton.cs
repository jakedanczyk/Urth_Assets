using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour, IPointerClickHandler {

    public GameObject parentPanel;

    public void ExitPanel() {
        print("exit");
        parentPanel.SetActive(false); }

    public void OnPointerClick(PointerEventData data)
    {
        parentPanel.SetActive(false);
    }
}
