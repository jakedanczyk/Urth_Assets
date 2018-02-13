using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateChunk16 : MonoBehaviour {

    public World16 world;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 13)
        {
            Chunk16 chunk = col.gameObject.GetComponent<Chunk16>();
            chunk.ConvertDown();
            //col.gameObject.GetComponent<MeshCollider>().enabled = false;
            //world.RemoveChunk16(chunk.pos);
            //Chunk16 neighborChunk = world.GetChunk16(chunk.pos.x + 256, chunk.pos.y, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk16();
            //neighborChunk = world.GetChunk16(chunk.pos.x - 256, chunk.pos.y, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk16();
            //neighborChunk = world.GetChunk16(chunk.pos.x, chunk.pos.y + 256, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk16();
            //neighborChunk = world.GetChunk16(chunk.pos.x, chunk.pos.y - 256, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk16();
            //neighborChunk = world.GetChunk16(chunk.pos.x, chunk.pos.y, chunk.pos.z + 256);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk16();
            //neighborChunk = world.GetChunk16(chunk.pos.x, chunk.pos.y, chunk.pos.z - 256);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk16();
        }
    }
}
