using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChunk4s : MonoBehaviour
{
    public static GameObject terrainLoadManager;

    static WorldPos[] chunk4Positions = {   new WorldPos (0, 0, 0),
		
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

    public World4 world4;
    //public GameObject treePrefab;
    public Transform playerTransform;
    public Rigidbody playerRigidBody;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;

    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    Dictionary<WorldPos, Chunk16> parentList = new Dictionary<WorldPos, Chunk16>();
    Dictionary<WorldPos, Chunk16> replaceList = new Dictionary<WorldPos, Chunk16>();


    int timer = 0;

    void Start()
    {
        terrainLoadManager = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (DeleteChunk4s())
        //    return;

        //FindChunk4sToLoad();
        LoadAndRenderChunk4s();
    }

    void FindChunk4sToLoad()
    {
        //Get the position of this gameobject to generate around

        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / 64) * 64,
            Mathf.FloorToInt(transform.position.y / 64) * 64,
            Mathf.FloorToInt(transform.position.z / 64) * 64
            );
		

        //If there aren't already chunk4s to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunk4Positions.Length; i++)
            {
                //translate the player position and array position into chunk4 position
                WorldPos newChunk4Pos = new WorldPos(
                    chunk4Positions[i].x * 64 + playerPos.x,
					chunk4Positions[i].y * 64 + playerPos.y,
                    chunk4Positions[i].z * 64 + playerPos.z
                    );

                //Get the chunk4 in the defined position
                Chunk4 newChunk4 = world4.GetChunk4(
                    newChunk4Pos.x, newChunk4Pos.y, newChunk4Pos.z);

                //If the chunk4 already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk4 != null
                    && (newChunk4.rendered || updateList.Contains(newChunk4Pos)))
                    continue;

				int player_y = (int)(Mathf.Floor (playerPos.y / 64));

                //load a column of chunk4s in this position
				for (int y = player_y - 6; y < player_y + 6; y++)
                {

                    for (int x = newChunk4Pos.x - 64; x <= newChunk4Pos.x + 64; x += 64)
                    {
                        for (int z = newChunk4Pos.z - 64; z <= newChunk4Pos.z + 64; z += 64)
                        {
                            buildList.Add(new WorldPos(x, y * 64, z));
                        }
                    }
                    updateList.Add(new WorldPos(newChunk4Pos.x, y * 64, newChunk4Pos.z));
                }
                return;
            }
        }
    }

    void LoadAndRenderChunk4s()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 4; i++)
            {
                BuildChunk4(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunk4s were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            for (int i = 0; i < updateList.Count && i < 4; i++)
            {
                Chunk4 chunk4 = world4.GetChunk4(updateList[0].x, updateList[0].y, updateList[0].z);
                if (chunk4 != null)
                {
                    chunk4.update = true;
                    chunk4.UpdateChunk4();
                    if (parentList.ContainsKey(updateList[0]))
                    {
                        parentList[updateList[0]].subChunkList.Add(chunk4);
                        parentList.Remove(updateList[0]);
                    }
                }
                if (replaceList.ContainsKey(updateList[0]))
                {
                    replaceList[updateList[0]].gameObject.GetComponent<MeshRenderer>().enabled = false;
                    replaceList[updateList[0]].gameObject.layer = 12;
                    replaceList[updateList[0]].isSubChunked = true;
                    replaceList.Remove(updateList[0]);
                }
                updateList.RemoveAt(0);
            }
        }
    }

    void BuildChunk4(WorldPos pos)
    {
        if (world4.GetChunk4(pos.x, pos.y, pos.z) == null)
        {
            world4.CreateChunk4(pos.x, pos.y, pos.z);
        }
    }

    public int dist;
    bool DeleteChunk4s()
    {

        if (timer == 60)
        {
            float v = playerRigidBody.velocity.magnitude;
            var chunk4sToDelete = new List<WorldPos>();
            foreach (var chunk4 in world4.chunk4s)
            {
                float distance = Vector3.Distance(
                    new Vector3(chunk4.Value.pos.x, .5f * chunk4.Value.pos.y, chunk4.Value.pos.z),
                    new Vector3(playerTransform.position.x, .5f * playerTransform.position.y, playerTransform.position.z));

                if (distance > 512 || (distance < 64 && v < 1 && playerControls.aboveDetailedChunk))
                {
                    chunk4sToDelete.Add(chunk4.Key);
                    //print("deleting chunk4, " + distance);
                }
            }

            foreach (var chunk4 in chunk4sToDelete)
                world4.DestroyChunk4(chunk4.x, chunk4.y, chunk4.z);

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }

    //Generate chunk4s to fill a Chunk16
    public void ReplaceChunk16(Chunk16 chunk16, WorldPos chunk16Pos)
    {
        for (int y = -1; y < 5; y++)
        {
            for (int x = -1; x < 5; x++)
            {
                for (int z = -1; z < 5; z++)
                {
                    WorldPos worldPos = new WorldPos(chunk16Pos.x + x * 64, chunk16Pos.y + y * 64, chunk16Pos.z + z * 64);
                    Chunk4 chunk4 = world4.GetChunk4(worldPos.x, worldPos.y, worldPos.z);
                    if (chunk4 == null)
                    {
                        buildList.Add(worldPos);
                        updateList.Add(worldPos);
                        parentList[worldPos] = chunk16;
                    }
                }
            }
        }
        replaceList[new WorldPos(chunk16Pos.x + 128, chunk16Pos.y + 128, chunk16Pos.z + 128)] = chunk16;
    }
}