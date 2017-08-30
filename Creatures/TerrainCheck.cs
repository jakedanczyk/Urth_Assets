using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class TerrainCheck : MonoBehaviour
{

    public GameObject parentObject;
    public Rigidbody rigid;
    public PlayerControls controls;
    public LayerMask mask = 256 | 1024;

    public bool falling = true;


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(parentObject.transform.position, Vector3.down, out hit, 128f, mask))
        {
            Debug.DrawLine(parentObject.transform.position, hit.point, Color.blue);
            if (!falling)
            {
                falling = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                controls.m_GravityMultiplier = 3;
            }
            return;
        }
        else
        {
            falling = false;
            rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            controls.m_GravityMultiplier = 0;
        }
    }
}
