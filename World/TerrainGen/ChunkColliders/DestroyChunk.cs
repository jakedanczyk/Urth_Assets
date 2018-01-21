using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChunk : MonoBehaviour {

    World world;

    private void Start()
    {
        world = World.worldGameObject.GetComponent<World>();
    }

    void OnTriggerExit(Collider col)
    {
        Chunk chunk = GetComponent<Chunk>();
        if(chunk != null)
            world.DestroyChunk(chunk.pos.x, chunk.pos.y, chunk.pos.z);
    }
}
