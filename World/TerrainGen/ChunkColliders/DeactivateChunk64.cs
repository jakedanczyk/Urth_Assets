using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateChunk64 : MonoBehaviour {

    public World64 world;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 11)
        {
            //col.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //col.gameObject.layer = 16;
            Chunk64 chunk = col.gameObject.GetComponent<Chunk64>();
            chunk.ConvertDown();
            //col.gameObject.GetComponent<MeshCollider>().enabled = false;
            //world.RemoveChunk64(chunk.pos);
            //Chunk64 neighborChunk = world.GetChunk64(chunk.pos.x + 1024, chunk.pos.y, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk64();
            //neighborChunk = world.GetChunk64(chunk.pos.x - 1024, chunk.pos.y, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk64();
            //neighborChunk = world.GetChunk64(chunk.pos.x, chunk.pos.y + 1024, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk64();
            //neighborChunk = world.GetChunk64(chunk.pos.x, chunk.pos.y - 1024, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk64();
            //neighborChunk = world.GetChunk64(chunk.pos.x, chunk.pos.y, chunk.pos.z + 1024);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk64();
            //neighborChunk = world.GetChunk64(chunk.pos.x, chunk.pos.y, chunk.pos.z - 1024);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk64();
        }
    }
}
