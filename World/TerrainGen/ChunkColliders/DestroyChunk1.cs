using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChunk1 : MonoBehaviour {

    World1 world;

    private void Start()
    {
        world = World1.worldGameObject.GetComponent<World1>();
    }

    void OnTriggerExit(Collider col)
    {
        Chunk1 chunk = GetComponent<Chunk1>();
        if(chunk != null)
            world.DestroyChunk1(chunk.pos.x, chunk.pos.y, chunk.pos.z);
    }
}
