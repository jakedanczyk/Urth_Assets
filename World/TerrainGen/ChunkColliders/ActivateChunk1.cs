using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChunk1 : MonoBehaviour {

    void OnTriggerExit(Collider col)
    {
        col.gameObject.GetComponent<MeshRenderer>().enabled = true;
        col.gameObject.layer = 17;
        if (col.gameObject.GetComponent<Chunk1>().isSubChunked)
        {
            foreach (Chunk chunk in col.gameObject.GetComponent<Chunk1>().subChunkList)
            {
                chunk.gameObject.GetComponent<MeshRenderer>().enabled = false;
                chunk.gameObject.layer = 18;
            }
        }
        //col.gameObject.GetComponent<MeshCollider>().enabled = true;
    }
}
