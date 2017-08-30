using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterCheck : MonoBehaviour
{

    public LayerMask mask = 256 | 1024;

    public bool roof, walls, wall1, wall2, wall3, wall4;

    private void Awake()
    {
        InvokeRepeating("RoofCheck", 1, 1);
        InvokeRepeating("WallsCheck", 1.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RoofCheck()
    {
        RaycastHit hit;
        if (!(Physics.Raycast(this.gameObject.transform.position, Vector3.up, out hit, 44f, mask)))
        {
            roof = false;
        }
        else roof = true; 
    }

    void WallsCheck()
    {
        RaycastHit hit1, hit2, hit3, hit4;
        if (!(Physics.Raycast(this.gameObject.transform.position, Vector3.forward, out hit1, 44f, mask)))
        {
            wall1 = false;
        }
        else wall1 = true;

        if (!(Physics.Raycast(this.gameObject.transform.position, Vector3.back, out hit2, 44f, mask)))
        {
            wall2 = false;
        }
        else wall2 = true;

        if (!(Physics.Raycast(this.gameObject.transform.position, Vector3.left, out hit3, 44f, mask)))
        {
            wall3 = false;
        }
        else wall3 = true;

        if (!(Physics.Raycast(this.gameObject.transform.position, Vector3.right, out hit4, 44f, mask)))
        {
            wall4 = false;
        }
        else wall4 = true;

        if (wall1 && wall2 && wall3 && wall4)
        {
            walls = true;
        }
        else walls = false;
    }
}
