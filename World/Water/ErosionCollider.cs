using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErosionCollider : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 13)
        {
            RiverNode parent = this.GetComponent<RiverNode>();
            parent.StartCoroutine(parent.erode16);
        }
    }
}
