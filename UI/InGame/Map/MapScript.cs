using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {

    public GameObject mapObject;
    public GameObject camObject;
	// Update is called once per frame
	void Update () {
        if (camObject.activeSelf)
        {
            camObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapObject.activeSelf)
            {
                mapObject.SetActive(false);
            }
            else
            {
                camObject.SetActive(true);
                mapObject.SetActive(true);
            }
        }
    }
}
