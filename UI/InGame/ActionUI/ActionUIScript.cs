using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUIScript : MonoBehaviour {

    public BodyManager_Human_Player playerBodyManager;
    public TimeController timeController;
    
    public void Sleep()
    {
        playerBodyManager.Sleep();
    }

    public void StopSleep()
    {
        playerBodyManager.StopSleep();
    }
}
