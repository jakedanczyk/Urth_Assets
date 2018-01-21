using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChunk16 : MonoBehaviour {

    World16 world;

    private void Start()
    {
        world = World16.worldGameObject.GetComponent<World16>();
    }

    void OnTriggerEnter(Collider col)
    {
        Chunk16 chunk = GetComponent<Chunk16>();
        if (chunk != null)
            world.DestroyChunk16(chunk.pos.x, chunk.pos.y, chunk.pos.z);
    }
}
