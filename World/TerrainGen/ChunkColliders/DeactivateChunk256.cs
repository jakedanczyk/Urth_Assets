using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateChunk256 : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 9)
        {
            //col.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //col.gameObject.layer = 16;
            print("chunk 256 entered");
            col.gameObject.GetComponent<Chunk256>().ConvertDown();
            //col.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
}
    