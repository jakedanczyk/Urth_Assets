using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkColliderManager : MonoBehaviour {

    public GameObject activator, deactivator, destroyer, activator1, deactivator1, destroyer1, activator4, deactivator4, destroyer4, activator16, deactivator16, destroyer16, activator64, deactivator64, destroyer64, activator256, deactivator256, destroyer256;

    int t = 0;
    int z = 0;
    // Update is called once per frame
    void Update()
    {
        t += 1;
        if (t >= 240 && !destroyer16.activeSelf)
        {
            destroyer16.SetActive(true);
            z += 1;
            if (z > 4)
            {
                activator64.SetActive(true);
                deactivator64.SetActive(true);
                destroyer64.SetActive(true);
                activator256.SetActive(true);
                deactivator256.SetActive(true);
            }
        }
        else if (t > 250)
        {
            destroyer16.SetActive(false);
            t = 0;
            if (z > 4)
            {
                activator64.SetActive(false);
                deactivator64.SetActive(false);
                destroyer64.SetActive(false);
                activator256.SetActive(false);
                deactivator256.SetActive(false);
                z = 0;
            }
        }
    }
}
