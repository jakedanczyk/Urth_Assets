using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChunk256 : MonoBehaviour {

    World world;

    private void Start()
    {
        world = World.worldGameObject.GetComponent<World>();
    }

    void OnTriggerExit(Collider col)
    {
        Chunk256 chunk = GetComponent<Chunk256>();
        world.DestroyChunk(chunk.pos.x, chunk.pos.y, chunk.pos.z);
    }
}
