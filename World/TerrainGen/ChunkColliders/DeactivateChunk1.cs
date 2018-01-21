using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateChunk1 : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 17)
        {
            //col.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //col.gameObject.layer = 16;
            col.gameObject.GetComponent<Chunk1>().ConvertDown();
            //col.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
}
