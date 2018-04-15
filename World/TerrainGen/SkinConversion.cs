using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinConversion : MonoBehaviour {
    public LayerMask mask;
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 16f, mask))
        {
            if (!hit.collider.GetComponent<Chunk16>().isSubChunked)
            {
                hit.collider.GetComponent<Chunk16>().SkinConvert();
            }
        }
    }
}
