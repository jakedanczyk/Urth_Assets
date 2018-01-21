using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChunk256 : MonoBehaviour {
    
    void OnTriggerEnter(Collider col)
    {
        col.gameObject.GetComponent<MeshRenderer>().enabled = true;
        col.gameObject.layer = 9;
        if (col.gameObject.GetComponent<Chunk256>().isSubChunked)
        {
            foreach (Chunk64 chunk in col.gameObject.GetComponent<Chunk256>().subChunkList)
            {
                chunk.gameObject.GetComponent<MeshRenderer>().enabled = false;
                chunk.gameObject.layer = 10;
            }
        }
        //col.gameObject.GetComponent<MeshCollider>().enabled = true;
    }
}
