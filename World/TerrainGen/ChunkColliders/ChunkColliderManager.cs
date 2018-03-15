using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChunkColliderManager : MonoBehaviour
{

    public MessageLog messageLog;

    public GameObject activator, deactivator, destroyer, activator1, deactivator1, destroyer1, activator4, deactivator4, destroyer4, activator16, deactivator16, destroyer16, activator64, deactivator64, destroyer64, activator256, deactivator256, destroyer256;

    int t = 0;
    int z = 0;
    int detailLevel = -1;    // -1 = 16m blocks, 0 = 4m blocks, 1 = 1m blocks, 2 = .25m blocks

    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                detailLevel = Math.Min(detailLevel + 1, 2);
                if (detailLevel == 0)
                {
                    deactivator16.SetActive(true);
                    destroyer4.SetActive(true);
                    messageLog.NewMessage("Terrain resolution: 4 meter");
                }
                else if (detailLevel == 1)
                {
                    deactivator4.SetActive(true);
                    destroyer1.SetActive(true);
                    messageLog.NewMessage("Terrain resolution: 1 meter");
                }
                else if (detailLevel == 2)
                {
                    deactivator1.SetActive(true);
                    destroyer.SetActive(true);
                    messageLog.NewMessage("Terrain resolution: 0.25 meter");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                detailLevel = Math.Max(detailLevel - 1, 0);
                if (detailLevel == 1)
                {
                    deactivator1.SetActive(false);
                    messageLog.NewMessage("Terrain resolution: 1 meter");
                }
                else if (detailLevel == 0)
                {
                    deactivator4.SetActive(false);
                    messageLog.NewMessage("Terrain resolution: 4 meter");
                }
                else if (detailLevel == -1)
                {
                    deactivator16.SetActive(false);
                    messageLog.NewMessage("Terrain resolution: 16 meter");
                }
            }
        }

        t += 1;
        if (t >= 240)
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