using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageLog : MonoBehaviour {

    public static GameObject messageLogGameObject;

    private void Awake()
    {
        messageLogGameObject = gameObject;
    }

    public Text text;

    public void NewMessage(string msg)
    {
        text.text = msg + "\n" + text.text;
    }
}
