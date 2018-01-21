using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChunk64 : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
            col.gameObject.GetComponent<MeshRenderer>().enabled = true;
            col.gameObject.layer = 11;
            if (col.gameObject.GetComponent<Chunk64>().isSubChunked)
            {
                foreach (Chunk16 chunk in col.gameObject.GetComponent<Chunk64>().subChunkList)
                {
                    chunk.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    chunk.gameObject.layer = 12;
                }
            }
            //col.gameObject.GetComponent<MeshCollider>().enabled = true;
    }
}
