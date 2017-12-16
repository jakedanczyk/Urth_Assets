using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChunk16s : MonoBehaviour
{
	static WorldPos[] chunk16Positions = {
        new WorldPos (0, 0, 0),

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

    public World16 world16;
    public Transform playerTransform;
    public Rigidbody playerRigidBody;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;


    List<WorldPos> updateList = new List<WorldPos>();
    List<WorldPos> buildList = new List<WorldPos>();

    int timer = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (DeleteChunk16s())
            return;

        FindChunk16sToLoad();
        LoadAndRenderChunk16s();
    }

    void FindChunk16sToLoad()
    {
        //Get the position of this gameobject to generate around

        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / (Chunk16.chunk16Size * 16)) * (Chunk16.chunk16Size * 16),
            Mathf.FloorToInt(transform.position.y / (Chunk16.chunk16Size * 16)) * (Chunk16.chunk16Size * 16),
            Mathf.FloorToInt(transform.position.z / (Chunk16.chunk16Size * 16)) * (Chunk16.chunk16Size * 16)
            );
		

        //If there aren't already chunk16s to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunk16Positions.Length; i++)
            {
                //translate the player position and array position into chunk16 position
                WorldPos newChunk16Pos = new WorldPos(
                    chunk16Positions[i].x * Chunk16.chunk16Size*16 + playerPos.x,
					chunk16Positions[i].y * Chunk16.chunk16Size*16 + playerPos.y,
                    chunk16Positions[i].z * Chunk16.chunk16Size*16 + playerPos.z
                    );

                //Get the chunk16 in the defined position
                Chunk16 newChunk16 = world16.GetChunk16(
                    newChunk16Pos.x, newChunk16Pos.y, newChunk16Pos.z);

                //If the chunk16 already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk16 != null
                    && (newChunk16.rendered || updateList.Contains(newChunk16Pos)))
                    continue;

                int player_y = (int)(Mathf.Floor(playerPos.y / 256));

                //load a column of chunk16s in this position

                for (int y = player_y - 6; y < player_y + 6; y++)
                {
                    for (int x = newChunk16Pos.x - 256; x <= newChunk16Pos.x + 256; x += 256)
                    {
                        for (int z = newChunk16Pos.z - 256; z <= newChunk16Pos.z + 256; z += 256)
                        {
                            buildList.Add(new WorldPos(
                                x, y * Chunk16.chunk16Size * 16, z));
                        }
                    }
                    updateList.Add(new WorldPos(
                                newChunk16Pos.x, y * Chunk16.chunk16Size * 16, newChunk16Pos.z));
                }
                return;
                //for (int y = player_y - 1; y < player_y+1; y++)
                //{

                //    for (int x = newChunk16Pos.x - Chunk16.chunk16Size; x <= newChunk16Pos.x + Chunk16.chunk16Size; x += Chunk16.chunk16Size)
                //    {
                //        for (int z = newChunk16Pos.z - Chunk16.chunk16Size; z <= newChunk16Pos.z + Chunk16.chunk16Size; z += Chunk16.chunk16Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk16.chunk16Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk16Pos.x, y * Chunk16.chunk16Size, newChunk16Pos.z));
                //}
                //for (int y = player_y - 1; y > player_y - 4; y--)
                //{

                //    for (int x = newChunk16Pos.x - Chunk16.chunk16Size; x <= newChunk16Pos.x + Chunk16.chunk16Size; x += Chunk16.chunk16Size)
                //    {
                //        for (int z = newChunk16Pos.z - Chunk16.chunk16Size; z <= newChunk16Pos.z + Chunk16.chunk16Size; z += Chunk16.chunk16Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk16.chunk16Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk16Pos.x, y * Chunk16.chunk16Size, newChunk16Pos.z));
                //}
                //for (int y = player_y +1; y < player_y + 4; y++)
                //{

                //    for (int x = newChunk16Pos.x - Chunk16.chunk16Size; x <= newChunk16Pos.x + Chunk16.chunk16Size; x += Chunk16.chunk16Size)
                //    {
                //        for (int z = newChunk16Pos.z - Chunk16.chunk16Size; z <= newChunk16Pos.z + Chunk16.chunk16Size; z += Chunk16.chunk16Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk16.chunk16Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk16Pos.x, y * Chunk16.chunk16Size, newChunk16Pos.z));
                //}
                //return;
            }
        }
    }

    void LoadAndRenderChunk16s()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 4; i++)
            {
                BuildChunk16(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunks were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            Chunk16 chunk16 = world16.GetChunk16(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk16 != null)
                chunk16.update = true;
            updateList.RemoveAt(0);
        }
    }

    void BuildChunk16(WorldPos pos)
    {
        if (world16.GetChunk16(pos.x, pos.y, pos.z) == null)
        {
            world16.CreateChunk16(pos.x, pos.y, pos.z);
        }
    }

    public int dist;
    bool DeleteChunk16s()
    {

        if (timer == 10)
        {
            float v = playerRigidBody.velocity.magnitude;
            var chunk16sToDelete = new List<WorldPos>();
            foreach (var chunk16 in world16.chunk16s)
            {
                float distance = Vector3.Distance(
					new Vector3(chunk16.Value.pos.x, 2*chunk16.Value.pos.y, chunk16.Value.pos.z),
					new Vector3(playerTransform.position.x, 2* playerTransform.position.y, playerTransform.position.z));

                if (distance > 4456 || (distance < 250 && v < 1 && playerControls.aboveDetailedChunk))
                {
                    chunk16sToDelete.Add(chunk16.Key);
                    print("deleting chunk16, " + distance);
                }
            }

            foreach (var chunk16 in chunk16sToDelete)
                world16.DestroyChunk16(chunk16.x, chunk16.y, chunk16.z);

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }
}