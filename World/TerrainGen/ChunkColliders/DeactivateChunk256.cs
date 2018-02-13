using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateChunk256 : MonoBehaviour {

    public World256 world;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 9)
        {
            //col.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //col.gameObject.layer = 16;
            Chunk256 chunk = col.gameObject.GetComponent<Chunk256>();
            chunk.ConvertDown();
            //for(int i = 0)
            //world.RemoveChunk256(chunk.pos);
            //Chunk256 neighborChunk = world.GetChunk256(chunk.pos.x + 4096, chunk.pos.y, chunk.pos.z);
            //if(neighborChunk != null)
            //    neighborChunk.UpdateChunk256();
            //neighborChunk = world.GetChunk256(chunk.pos.x - 4096, chunk.pos.y, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk256();
            //neighborChunk = world.GetChunk256(chunk.pos.x, chunk.pos.y + 4096, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk256();
            //neighborChunk = world.GetChunk256(chunk.pos.x, chunk.pos.y - 4096, chunk.pos.z);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk256();
            //neighborChunk = world.GetChunk256(chunk.pos.x, chunk.pos.y, chunk.pos.z + 4096);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk256();
            //neighborChunk = world.GetChunk256(chunk.pos.x, chunk.pos.y, chunk.pos.z - 4096);
            //if (neighborChunk != null)
            //    neighborChunk.UpdateChunk256();
            //col.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
}
    