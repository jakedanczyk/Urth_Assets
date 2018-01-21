using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateChunk64 : MonoBehaviour {
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 11)
        {
            //col.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //col.gameObject.layer = 16;
            col.gameObject.GetComponent<Chunk64>().ConvertDown();
            //col.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
}
