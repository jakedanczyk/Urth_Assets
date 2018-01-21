using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChunk4 : MonoBehaviour {

    void OnTriggerExit(Collider col)
    {
        col.gameObject.GetComponent<MeshRenderer>().enabled = true;
        col.gameObject.layer = 15;
        Chunk4 parentChunk = col.gameObject.GetComponent<Chunk4>();
        if (parentChunk != null)
        {
            if (parentChunk.isSubChunked)
            {
                foreach (Chunk1 chunk in parentChunk.subChunkList)
                {
                    chunk.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    chunk.gameObject.layer = 16;
                }
            }
        }
    }
}
