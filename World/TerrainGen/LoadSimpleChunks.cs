using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadSimpleChunks : MonoBehaviour
{
    static WorldPos[] chunkPositions = {   new WorldPos (0, 0, 0),

        new WorldPos (1, 0, 0), new WorldPos (-1, 0, 0), new WorldPos (0, 0, -1), new WorldPos (0, 0, 1),
        new WorldPos (-1, 0, -1), new WorldPos (-1, 0, 1), new WorldPos (1, 0, -1), new WorldPos (1, 0, 1),
        new WorldPos (-2, 0, 0),
        new WorldPos (-3, 0, -3), new WorldPos (-3, 0, 3), new WorldPos (3, 0, -3),
        new WorldPos (3, 0, 3),
        new WorldPos (-4, 0, -1),
        new WorldPos (-4, 0, 1), new WorldPos (-1, 0, -4), new WorldPos (-1, 0, 4), new WorldPos (1, 0, -4), new WorldPos (1, 0, 4),
        new WorldPos (4, 0, -1), new WorldPos (4, 0, 1),

        new WorldPos (-4, 0, 0), new WorldPos (0, 0, -4), new WorldPos (0, 0, 4), new WorldPos (4, 0, 0),

        new WorldPos (-3, 0, -2), new WorldPos (-3, 0, 2), new WorldPos (-2, 0, -3),
        new WorldPos (-2, 0, 3), new WorldPos (2, 0, -3), new WorldPos (2, 0, 3), new WorldPos (3, 0, -2), new WorldPos (3, 0, 2),

        new WorldPos (-3, 0, -1),
        new WorldPos (-3, 0, 1), new WorldPos (-1, 0, -3), new WorldPos (-1, 0, 3), new WorldPos (1, 0, -3), new WorldPos (1, 0, 3),
        new WorldPos (3, 0, -1), new WorldPos (3, 0, 1),

        new WorldPos (-3, 0, 0), new WorldPos (0, 0, -3), new WorldPos (0, 0, 3), new WorldPos (3, 0, 0),

        new WorldPos (-2, 0, -2), new WorldPos (-2, 0, 2), new WorldPos (2, 0, -2), new WorldPos (2, 0, 2),

        new WorldPos (-2, 0, -1), new WorldPos (-2, 0, 1),
        new WorldPos (-1, 0, -2), new WorldPos (-1, 0, 2), new WorldPos (1, 0, -2), new WorldPos (1, 0, 2), new WorldPos (2, 0, -1),
        new WorldPos (2, 0, 1),

        new WorldPos (0, 0, -2), new WorldPos (0, 0, 2), new WorldPos (2, 0, 0),

        new WorldPos (-2, 0, -2), new WorldPos (-2, 0, 2), new WorldPos (-2, 0, -2),
        new WorldPos (-2, 0, 2), new WorldPos (2, 0, -2), new WorldPos (2, 0, 2), new WorldPos (2, 0, -2), new WorldPos (2, 0, 2),


        // new WorldPos (-4, 0, -2), new WorldPos (-4, 0, 2), new WorldPos (-2, 0, -4), new WorldPos (-2, 0, 4),
        //new WorldPos (2, 0, -4), new WorldPos (2, 0, 4), new WorldPos (4, 0, -2), new WorldPos (4, 0, 2), new WorldPos (-5, 0, 0),
        //new WorldPos (-4, 0, -3), new WorldPos (-4, 0, 3), new WorldPos (-3, 0, -4), new WorldPos (-3, 0, 4), new WorldPos (0, 0, -5),
        //new WorldPos (0, 0, 5), new WorldPos (3, 0, -4), new WorldPos (3, 0, 4), new WorldPos (4, 0, -3), new WorldPos (4, 0, 3),
        //new WorldPos (5, 0, 0), new WorldPos (-5, 0, -1), new WorldPos (-5, 0, 1), new WorldPos (-1, 0, -5), new WorldPos (-1, 0, 5),
        //new WorldPos (1, 0, -5), new WorldPos (1, 0, 5), new WorldPos (5, 0, -1), new WorldPos (5, 0, 1), new WorldPos (-5, 0, -2),
        //new WorldPos (-5, 0, 2), new WorldPos (-2, 0, -5), new WorldPos (-2, 0, 5), new WorldPos (2, 0, -5), new WorldPos (2, 0, 5),
        //new WorldPos (5, 0, -2), new WorldPos (5, 0, 2), new WorldPos (-4, 0, -4), new WorldPos (-4, 0, 4), new WorldPos (4, 0, -4),
        //new WorldPos (4, 0, 4), new WorldPos (-5, 0, -3), new WorldPos (-5, 0, 3), new WorldPos (-3, 0, -5), new WorldPos (-3, 0, 5),
        //new WorldPos (3, 0, -5), new WorldPos (3, 0, 5), new WorldPos (5, 0, -3), new WorldPos (5, 0, 3), new WorldPos (-6, 0, 0),
        //new WorldPos (0, 0, -6), new WorldPos (0, 0, 6), new WorldPos (6, 0, 0), new WorldPos (-6, 0, -1), new WorldPos (-6, 0, 1),
        //new WorldPos (-1, 0, -6), new WorldPos (-1, 0, 6), new WorldPos (1, 0, -6), new WorldPos (1, 0, 6), new WorldPos (6, 0, -1),
        //new WorldPos (6, 0, 1), new WorldPos (-6, 0, -2), new WorldPos (-6, 0, 2), new WorldPos (-2, 0, -6), new WorldPos (-2, 0, 6),
        //new WorldPos (2, 0, -6), new WorldPos (2, 0, 6), new WorldPos (6, 0, -2), new WorldPos (6, 0, 2), new WorldPos (-5, 0, -4),
        //new WorldPos (-5, 0, 4), new WorldPos (-4, 0, -5), new WorldPos (-4, 0, 5), new WorldPos (4, 0, -5), new WorldPos (4, 0, 5),
        //new WorldPos (5, 0, -4), new WorldPos (5, 0, 4), new WorldPos (-6, 0, -3), new WorldPos (-6, 0, 3), new WorldPos (-3, 0, -6),
        //new WorldPos (-3, 0, 6), new WorldPos (3, 0, -6), new WorldPos (3, 0, 6), new WorldPos (6, 0, -3), new WorldPos (6, 0, 3),
        //new WorldPos (-7, 0, 0), new WorldPos (0, 0, -7), new WorldPos (0, 0, 7), new WorldPos (7, 0, 0), new WorldPos (-7, 0, -1),
        //new WorldPos (-7, 0, 1), new WorldPos (-5, 0, -5), new WorldPos (-5, 0, 5), new WorldPos (-1, 0, -7), new WorldPos (-1, 0, 7),
        //new WorldPos (1, 0, -7), new WorldPos (1, 0, 7), new WorldPos (5, 0, -5), new WorldPos (5, 0, 5), new WorldPos (7, 0, -1),
        //new WorldPos (7, 0, 1), new WorldPos (-6, 0, -4), new WorldPos (-6, 0, 4), new WorldPos (-4, 0, -6), new WorldPos (-4, 0, 6),
        //new WorldPos (4, 0, -6), new WorldPos (4, 0, 6), new WorldPos (6, 0, -4), new WorldPos (6, 0, 4), new WorldPos (-7, 0, -2),
        //new WorldPos (-7, 0, 2), new WorldPos (-2, 0, -7), new WorldPos (-2, 0, 7), new WorldPos (2, 0, -7), new WorldPos (2, 0, 7),
        //new WorldPos (7, 0, -2), new WorldPos (7, 0, 2)
    };

    public SimpleWorld simpleWorld;
    public Transform playerTransform;

    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    int timer = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DeleteSimpleChunks())
            return;

        FindSimpleChunksToLoad();
        LoadAndRenderSimpleChunks();
    }

    void FindSimpleChunksToLoad()
    {
        //Get the position of this gameobject to generate around

        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / SimpleChunk.chunkSize) * SimpleChunk.chunkSize,
            Mathf.FloorToInt(transform.position.y / SimpleChunk.chunkSize) * SimpleChunk.chunkSize,
            Mathf.FloorToInt(transform.position.z / SimpleChunk.chunkSize) * SimpleChunk.chunkSize
            );


        //If there aren't already chunks to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                WorldPos newSimpleChunkPos = new WorldPos(
                    chunkPositions[i].x * SimpleChunk.chunkSize + playerPos.x,
                    chunkPositions[i].y * SimpleChunk.chunkSize + playerPos.y,
                    chunkPositions[i].z * SimpleChunk.chunkSize + playerPos.z
                    );

                //Get the chunk in the defined position
                SimpleChunk newSimpleChunk = simpleWorld.GetSimpleChunk(
                    newSimpleChunkPos.x, newSimpleChunkPos.y, newSimpleChunkPos.z);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newSimpleChunk != null
                    && (newSimpleChunk.rendered || updateList.Contains(newSimpleChunkPos)))
                    continue;

                int player_y = (int)(Mathf.Floor(playerPos.y / SimpleChunk.chunkSize));

                //load a column of chunks in this position
                for (int y = player_y - 12; y < player_y + 6; y++)
                {

                    for (int x = newSimpleChunkPos.x - SimpleChunk.chunkSize; x <= newSimpleChunkPos.x + SimpleChunk.chunkSize; x += SimpleChunk.chunkSize)
                    {
                        for (int z = newSimpleChunkPos.z - SimpleChunk.chunkSize; z <= newSimpleChunkPos.z + SimpleChunk.chunkSize; z += SimpleChunk.chunkSize)
                        {
                            buildList.Add(new WorldPos(
                                x, y * SimpleChunk.chunkSize, z));
                        }
                    }
                    updateList.Add(new WorldPos(
                                newSimpleChunkPos.x, y * SimpleChunk.chunkSize, newSimpleChunkPos.z));
                }
                return;
            }
        }
    }

    void LoadAndRenderSimpleChunks()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 24; i++)
            {
                BuildSimpleChunk(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunks were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            SimpleChunk chunk = simpleWorld.GetSimpleChunk(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk != null)
                chunk.update = true;
            updateList.RemoveAt(0);
        }
    }

    void BuildSimpleChunk(WorldPos pos)
    {
        if (simpleWorld.GetSimpleChunk(pos.x, pos.y, pos.z) == null)
        {
            simpleWorld.CreateSimpleChunk(pos.x, pos.y, pos.z);
        }
    }

    bool DeleteSimpleChunks()
    {

        if (timer == 10)
        {
            var chunksToDelete = new List<WorldPos>();
            foreach (var chunk in simpleWorld.simpleChunks)
            {
                float distance = Vector3.Distance(
                    new Vector3(chunk.Value.pos.x, chunk.Value.pos.y, chunk.Value.pos.z),
                    new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z));

                if (distance > 64000 || distance < 256)
                chunksToDelete.Add(chunk.Key);

            }

            foreach (var chunk in chunksToDelete)
            {
                print("deleting");
                simpleWorld.DestroySimpleChunk(chunk.x, chunk.y, chunk.z);
            }

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }
}