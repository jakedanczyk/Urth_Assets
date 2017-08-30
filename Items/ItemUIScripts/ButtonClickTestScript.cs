using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.

public class ButtonClickTestScript : MonoBehaviour, ISelectHandler {


    public  void OnSelect(BaseEventData eventData)
    {
        print("wtf");
    }
}
