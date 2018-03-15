using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErosionTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            print("eroding");
            RiverNode parent = this.GetComponentInParent<RiverNode>();
            parent.StartCoroutine(parent.erode16);
        }
    }
}
