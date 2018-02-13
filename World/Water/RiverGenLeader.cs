using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverGenLeader : MonoBehaviour
{

    public bool isTouchingRiver, isTouchingLake;
    public River touchedRiver;
    public Lake touchedLake;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 4 && other.gameObject != this.transform.parent.gameObject)
        {
            if (other.gameObject.name == "River(Clone)")
            {
                isTouchingRiver = true;
                touchedRiver = other.GetComponent<River>();
            }
            else if (other.gameObject.name == "Lake(Clone)")
            {
                if (transform.parent.GetComponent<River>().endLake != null && other.gameObject != this.transform.parent.GetComponentInParent<River>().endLake.gameObject) { }
                else
                    return;
                isTouchingLake = true;
                touchedLake = other.GetComponent<Lake>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4 && other.gameObject != this.transform.parent.gameObject)
        {
            if (other.gameObject.name == "River(Clone)")
            {
                isTouchingRiver = false;
            }
            else if (other.gameObject.name == "Lake(Clone)" && this.transform.parent.GetComponentInParent<River>().endLake != null && other.gameObject != this.transform.parent.GetComponentInParent<River>().endLake.gameObject)
            {
                isTouchingLake = false;
            }
        }
    }
}
