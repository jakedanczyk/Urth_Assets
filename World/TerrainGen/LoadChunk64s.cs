using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChunk64s : MonoBehaviour
{
    public static GameObject terrainLoadManager;

	static WorldPos[] chunk64Positions = {
        new WorldPos (0, 0, 0),

        new WorldPos (1, 0, 0), new WorldPos (-1, 0, 0), new WorldPos (0, 0, -1), new WorldPos (0, 0, 1),
        new WorldPos (-1, 0, -1), new WorldPos (-1, 0, 1), new WorldPos (1, 0, -1), new WorldPos (1, 0, 1),
        new WorldPos (-2, 0, 0), new WorldPos (0, 0, -2), new WorldPos (0, 0, 2), new WorldPos (2, 0, 0),

        new WorldPos (-2, 0, -1), new WorldPos (-2, 0, 1), new WorldPos (-1, 0, -2), new WorldPos (-1, 0, 2),
        new WorldPos (1, 0, -2), new WorldPos (1, 0, 2), new WorldPos (2, 0, -1), new WorldPos (2, 0, 1),

        new WorldPos (-2, 0, -2), new WorldPos (-2, 0, 2), new WorldPos (2, 0, -2), new WorldPos (2, 0, 2),

        new WorldPos (-3, 0, -3), new WorldPos (-3, 0, 3), new WorldPos (3, 0, -3), new WorldPos (3, 0, 3),

        new WorldPos (-3, 0, -2), new WorldPos (-3, 0, 2), new WorldPos (-2, 0, -3),
        new WorldPos (-2, 0, 3), new WorldPos (2, 0, -3), new WorldPos (2, 0, 3), new WorldPos (3, 0, -2), new WorldPos (3, 0, 2),

        new WorldPos (-3, 0, -1),
        new WorldPos (-3, 0, 1), new WorldPos (-1, 0, -3), new WorldPos (-1, 0, 3), new WorldPos (1, 0, -3), new WorldPos (1, 0, 3),
        new WorldPos (3, 0, -1), new WorldPos (3, 0, 1),

        new WorldPos (-3, 0, 0), new WorldPos (0, 0, -3), new WorldPos (0, 0, 3), new WorldPos (3, 0, 0),

        //new WorldPos (-4, 0, -1), new WorldPos (-4, 0, 1), new WorldPos (-1, 0, -4), new WorldPos (-1, 0, 4),
        //new WorldPos (1, 0, -4), new WorldPos (1, 0, 4), new WorldPos (4, 0, -1), new WorldPos (4, 0, 1),

        //new WorldPos (-4, 0, 0), new WorldPos (0, 0, -4), new WorldPos (0, 0, 4), new WorldPos (4, 0, 0),

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

    public World64 world64;
    public Transform playerTransform;
    public Rigidbody playerRigidBody;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;

    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    Dictionary<WorldPos, Chunk256> parentList = new Dictionary<WorldPos, Chunk256>();
    Dictionary<WorldPos, Chunk256> replaceList = new Dictionary<WorldPos, Chunk256>();

    int timer = 0;

    void Start()
    {
        terrainLoadManager = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (DeleteChunk64s())
        //    return;

        //FindChunk64sToLoad();
        LoadAndRenderChunk64s();
    }

    void FindChunk64sToLoad()
    {
        //Get the position of this gameobject to generate around

        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / (1024)) * (1024),
            Mathf.FloorToInt(transform.position.y / (1024)) * (1024),
            Mathf.FloorToInt(transform.position.z / (1024)) * (1024)
            );
		

        //If there aren't already chunk64s to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunk64Positions.Length; i++)
            {
                //translate the player position and array position into chunk64 position
                WorldPos newChunk64Pos = new WorldPos(
                    chunk64Positions[i].x * 1024 + playerPos.x,
					chunk64Positions[i].y * 1024 + playerPos.y,
                    chunk64Positions[i].z * 1024 + playerPos.z
                    );

                //Get the chunk64 in the defined position
                Chunk64 newChunk64 = world64.GetChunk64(
                    newChunk64Pos.x, newChunk64Pos.y, newChunk64Pos.z);

                //If the chunk64 already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk64 != null
                    && (newChunk64.rendered || updateList.Contains(newChunk64Pos)))
                    continue;

                int player_y = (int)(Mathf.Floor(playerPos.y / 1024));

                //load a column of chunk64s in this position
                for (int y = player_y - 3; y < player_y + 4; y++)
                {
                    for (int x = newChunk64Pos.x - 1024; x <= newChunk64Pos.x + 1024; x += 1024)
                    {
                        for (int z = newChunk64Pos.z - 1024; z <= newChunk64Pos.z + 1024; z += 1024)
                        {
                            buildList.Add(new WorldPos(
                                x, y * 1024, z));
                        }
                    }
                    updateList.Add(new WorldPos(
                                newChunk64Pos.x, y * 1024, newChunk64Pos.z));
                }
                return;
                //for (int y = player_y - 1; y < player_y+1; y++)
                //{

                //    for (int x = newChunk64Pos.x - Chunk64.chunk64Size; x <= newChunk64Pos.x + Chunk64.chunk64Size; x += Chunk64.chunk64Size)
                //    {
                //        for (int z = newChunk64Pos.z - Chunk64.chunk64Size; z <= newChunk64Pos.z + Chunk64.chunk64Size; z += Chunk64.chunk64Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk64.chunk64Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk64Pos.x, y * Chunk64.chunk64Size, newChunk64Pos.z));
                //}
                //for (int y = player_y - 1; y > player_y - 4; y--)
                //{

                //    for (int x = newChunk64Pos.x - Chunk64.chunk64Size; x <= newChunk64Pos.x + Chunk64.chunk64Size; x += Chunk64.chunk64Size)
                //    {
                //        for (int z = newChunk64Pos.z - Chunk64.chunk64Size; z <= newChunk64Pos.z + Chunk64.chunk64Size; z += Chunk64.chunk64Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk64.chunk64Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk64Pos.x, y * Chunk64.chunk64Size, newChunk64Pos.z));
                //}
                //for (int y = player_y +1; y < player_y + 4; y++)
                //{

                //    for (int x = newChunk64Pos.x - Chunk64.chunk64Size; x <= newChunk64Pos.x + Chunk64.chunk64Size; x += Chunk64.chunk64Size)
                //    {
                //        for (int z = newChunk64Pos.z - Chunk64.chunk64Size; z <= newChunk64Pos.z + Chunk64.chunk64Size; z += Chunk64.chunk64Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk64.chunk64Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk64Pos.x, y * Chunk64.chunk64Size, newChunk64Pos.z));
                //}
                //return;
            }
        }
    }

    void LoadAndRenderChunk64s()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 4; i++)
            {
                BuildChunk64(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunks were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            for (int i = 0; i < updateList.Count && i < 4; i++)
            {
                Chunk64 chunk64 = world64.GetChunk64(updateList[0].x, updateList[0].y, updateList[0].z);
                if (chunk64 != null)
                {
                    chunk64.update = true;
                    chunk64.UpdateChunk64();
                    if (parentList.ContainsKey(updateList[0]))
                    {
                        parentList[updateList[0]].subChunkList.Add(chunk64);
                        parentList.Remove(updateList[0]);
                    }
                }
                if (replaceList.ContainsKey(updateList[0]))
                {
                    replaceList[updateList[0]].gameObject.GetComponent<MeshRenderer>().enabled = false;
                    replaceList[updateList[0]].gameObject.layer = 8;
                    replaceList[updateList[0]].isSubChunked = true;
                    replaceList.Remove(updateList[0]);
                }
                updateList.RemoveAt(0);
            }
        }
    }

    void BuildChunk64(WorldPos pos)
    {
        if (world64.GetChunk64(pos.x, pos.y, pos.z) == null)
        {
            world64.CreateChunk64(pos.x, pos.y, pos.z);
        }
    }

    public int dist;
    bool DeleteChunk64s()
    {
        if (timer == 10)
        {
            float v = playerRigidBody.velocity.magnitude;

            var chunk64sToDelete = new List<WorldPos>();
            foreach (var chunk64 in world64.chunk64s)
            {
                float distance = Vector3.Distance(
					new Vector3(chunk64.Value.pos.x, chunk64.Value.pos.y, chunk64.Value.pos.z),
					new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z));

                if (distance > 8000 || (distance < 1446 && v < 1 && playerControls.aboveDetailedChunk))
                {
                    chunk64sToDelete.Add(chunk64.Key);
                    //print("deleting chunk64, " + distance);
                }
            }

            foreach (var chunk64 in chunk64sToDelete)
                world64.DestroyChunk64(chunk64.x, chunk64.y, chunk64.z);

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }

    //Generate chunk64s to fill a Chunk256 then disable that chunk
    public void ReplaceChunk256(Chunk256 chunk256,WorldPos chunk256Pos)
    {
        for (int y = -1; y < 5; y++)
        {
            for (int x = -1; x < 5; x++)
            {
                for (int z = -1; z < 5; z++)
                {
                    WorldPos worldPos = new WorldPos(chunk256Pos.x + x * 1024, chunk256Pos.y + y * 1024, chunk256Pos.z + z * 1024);
                    Chunk64 chunk64 = world64.GetChunk64(worldPos.x, worldPos.y, worldPos.z);
                    if (chunk64 == null)
                    {
                        buildList.Add(worldPos);
                        updateList.Add(worldPos);
                        parentList[worldPos] = chunk256;
                    }
                }
            }
        }
        replaceList[new WorldPos(chunk256Pos.x + 2048, chunk256Pos.y + 2048, chunk256Pos.z + 2048)] = chunk256;
    }
}