using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SleepButton : MonoBehaviour, ISelectHandler {

    public ActionUIScript actionUIScript;
    public Button thisButton;

    void Awake()
    {
        thisButton = GetComponentInParent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        actionUIScript.Sleep();
        print("sleep button");
    }
}
