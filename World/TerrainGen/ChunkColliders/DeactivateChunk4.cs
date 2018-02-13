using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateChunk4 : MonoBehaviour {

    public World4 world;
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 15)
        {
            Chunk4 chunk = col.gameObject.GetComponent<Chunk4>();
            chunk.ConvertDown();
            //col.gameObject.GetComponent<MeshCollider>().enabled = false;
            //world.RemoveChunk4(chunk.pos);
            //Chunk4 neighborChunk = world.GetChunk4(chunk.pos.x + 1024, chunk.pos.y, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk4();
            //neighborChunk = world.GetChunk4(chunk.pos.x - 1024, chunk.pos.y, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk4();
            //neighborChunk = world.GetChunk4(chunk.pos.x, chunk.pos.y + 1024, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk4();
            //neighborChunk = world.GetChunk4(chunk.pos.x, chunk.pos.y - 1024, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk4();
            //neighborChunk = world.GetChunk4(chunk.pos.x, chunk.pos.y, chunk.pos.z + 1024);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk4();
            //neighborChunk = world.GetChunk4(chunk.pos.x, chunk.pos.y, chunk.pos.z - 1024);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk4();
        }
    }
}
