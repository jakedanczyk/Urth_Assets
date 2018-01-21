using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChunk4 : MonoBehaviour {

    World4 world;

    private void Start()
    {
        world = World4.worldGameObject.GetComponent<World4>();
    }

    void OnTriggerExit(Collider col)
    {
        Chunk4 chunk = GetComponent<Chunk4>();
        if(chunk != null)
            world.DestroyChunk4(chunk.pos.x, chunk.pos.y, chunk.pos.z);
    }
}
