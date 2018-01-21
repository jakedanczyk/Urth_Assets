using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChunk64 : MonoBehaviour {

    World64 world;

    private void Start()
    {
        world = World64.worldGameObject.GetComponent<World64>();
    }

    void OnTriggerEnter(Collider col)
    {
        Chunk64 chunk = GetComponent<Chunk64>();
        if (chunk != null)
            world.DestroyChunk64(chunk.pos.x, chunk.pos.y, chunk.pos.z);
    }
}
