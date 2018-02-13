using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChunks : MonoBehaviour
{
    public static GameObject terrainLoadManager;

    static WorldPos[] chunkPositions = {   new WorldPos (0, 0, 0),
		
		new WorldPos (1, 0, 0), new WorldPos (-1, 0, 0), new WorldPos (0, 0, -1), new WorldPos (0, 0, 1), 
		new WorldPos (-1, 0, -1), new WorldPos (-1, 0, 1), new WorldPos (1, 0, -1), new WorldPos (1, 0, 1), 
		new WorldPos (-2, 0, 0),

        new WorldPos (-2, 0, -2), new WorldPos (-2, 0, 2), new WorldPos (2, 0, -2), new WorldPos (2, 0, 2),

        new WorldPos (-2, 0, -1), new WorldPos (-2, 0, 1),
        new WorldPos (-1, 0, -2), new WorldPos (-1, 0, 2), new WorldPos (1, 0, -2), new WorldPos (1, 0, 2), new WorldPos (2, 0, -1),
        new WorldPos (2, 0, 1),

        new WorldPos (0, 0, -2), new WorldPos (0, 0, 2), new WorldPos (2, 0, 0),

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

    public World world;
    public GameObject treePrefab;
    public Transform playerTransform;

    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    Dictionary<WorldPos, Chunk1> parentList = new Dictionary<WorldPos, Chunk1>();
    Dictionary<WorldPos, Chunk1> replaceList = new Dictionary<WorldPos, Chunk1>();

    int timer = 0;

    void Start()
    {
        terrainLoadManager = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (DeleteChunks())
        //    return;

        //FindChunksToLoad();
        LoadAndRenderChunks();
    }

    void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around

        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / 4) * 4,
            Mathf.FloorToInt(transform.position.y / 4) * 4,
            Mathf.FloorToInt(transform.position.z / 4) * 4
            );
		

        //If there aren't already chunks to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                WorldPos newChunkPos = new WorldPos(
                    chunkPositions[i].x * 4 + playerPos.x,
					chunkPositions[i].y * 4 + playerPos.y,
                    chunkPositions[i].z * 4 + playerPos.z
                    );

                //Get the chunk in the defined position
                Chunk newChunk = world.GetChunk(
                    newChunkPos.x, newChunkPos.y, newChunkPos.z);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null
                    && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    continue;

				int player_y = (int)(Mathf.Floor (playerPos.y / 4));

                //load a column of chunks in this position
				for (int y = player_y - 4; y < player_y+4; y++)
                {
                    for (int x = newChunkPos.x - 4; x <= newChunkPos.x + 4; x += 4)
                    {
                        for (int z = newChunkPos.z - 4; z <= newChunkPos.z + 4; z += 4)
                        {
                            buildList.Add(new WorldPos(x, y * 4, z));
                        }
                    }
                    updateList.Add(new WorldPos(newChunkPos.x, y * 4, newChunkPos.z));
                }
                return;
            }
        }
    }

    void LoadAndRenderChunks()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 16; i++)
            {
                BuildChunk(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunks were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            for (int i = 0; i < updateList.Count && i < 4; i++)
            {
                Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
                if (chunk != null)
                {
                    chunk.update = true;
                    if (parentList.ContainsKey(updateList[0]))
                    {
                        parentList[updateList[0]].subChunkList.Add(chunk);
                        parentList.Remove(updateList[0]);
                    }
                }
                if (replaceList.ContainsKey(updateList[0]))
                {
                    replaceList[updateList[0]].gameObject.GetComponent<MeshRenderer>().enabled = false;
                    replaceList[updateList[0]].gameObject.layer = 16;
                    replaceList[updateList[0]].isSubChunked = true;
                    replaceList.Remove(updateList[0]);
                }
                updateList.RemoveAt(0);
            }
        }
    }

    void BuildChunk(WorldPos pos)
    {
        if (world.GetChunk(pos.x, pos.y, pos.z) == null)
        {
            world.CreateChunk(pos.x, pos.y, pos.z);
        }
    }

    public int dist;
    bool DeleteChunks()
    {
        if (timer == 60)
        {
            var chunksToDelete = new List<WorldPos>();
            foreach (var chunk in world.chunks)
            {
                float distance = Vector3.Distance(
					new Vector3(chunk.Value.pos.x, .5f * chunk.Value.pos.y, chunk.Value.pos.z),
					new Vector3(playerTransform.position.x, .5f * playerTransform.position.y, playerTransform.position.z));

                if (distance > 512)
                    chunksToDelete.Add(chunk.Key);
            }

            foreach (var chunk in chunksToDelete)
                world.DestroyChunk(chunk.x, chunk.y, chunk.z);

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }

    //Generate chunks to fill a Chunk1
    public void ReplaceChunk1(Chunk1 chunk1, WorldPos chunk1Pos)
    {
        for (int y = -1; y < 5; y++)
        {
            for (int x = -1; x < 5; x++)
            {
                for (int z = -1; z < 5; z++)
                {
                    if (z == 2 && x == 2 && y == 2)
                    {
                        continue;
                    }
                    WorldPos worldPos = new WorldPos(chunk1Pos.x + x * 4, chunk1Pos.y + y * 4, chunk1Pos.z + z * 4);
                    Chunk chunk = world.GetChunk(worldPos.x, worldPos.y, worldPos.z);
                    if (chunk == null)
                    {
                        buildList.Add(worldPos);
                        updateList.Add(worldPos);
                        parentList[worldPos] = chunk1;
                    }
                }
            }
        }
        WorldPos worldPosCenter = new WorldPos(chunk1Pos.x + 8, chunk1Pos.y + 8, chunk1Pos.z + 8);
        Chunk chunkCenter = world.GetChunk(worldPosCenter.x, worldPosCenter.y, worldPosCenter.z);
        if (chunkCenter == null)
        {
            buildList.Add(worldPosCenter);
            updateList.Add(worldPosCenter);
            parentList[worldPosCenter] = chunk1;
        }
        replaceList[worldPosCenter] = chunk1;
    }
}