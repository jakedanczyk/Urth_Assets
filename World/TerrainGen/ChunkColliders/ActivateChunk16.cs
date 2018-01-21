using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChunk16 : MonoBehaviour {

    void OnTriggerExit(Collider col)
    {
            col.gameObject.GetComponent<MeshRenderer>().enabled = true;
            col.gameObject.layer = 13;
            if (col.gameObject.GetComponent<Chunk16>().isSubChunked)
            {
                foreach (Chunk4 chunk in col.gameObject.GetComponent<Chunk16>().subChunkList)
                {
                    chunk.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    chunk.gameObject.layer = 14;
                }
            }
            //col.gameObject.GetComponent<MeshCollider>().enabled = true;
    }
}
