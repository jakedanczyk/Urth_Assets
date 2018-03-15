using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErosionCollider : MonoBehaviour {

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == 13)
        {
            print("eroding");
            RiverNode parent = this.GetComponent<RiverNode>();
            parent.StartCoroutine(parent.erode16);
        }
    }
}
