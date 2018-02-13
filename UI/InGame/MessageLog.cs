using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageLog : MonoBehaviour {

    public Text text;

    public void NewMessage(string msg)
    {
        text.text += "\n";
        text.text += msg;
    }

}
