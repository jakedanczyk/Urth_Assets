using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChunk256s : MonoBehaviour
{
	static WorldPos[] chunk256Positions = {
        new WorldPos (0, 0, 0),

        new WorldPos (1, 0, 0), new WorldPos (-1, 0, 0), new WorldPos (0, 0, -1), new WorldPos (0, 0, 1),
        new WorldPos (-1, 0, -1), new WorldPos (-1, 0, 1), new WorldPos (1, 0, -1), new WorldPos (1, 0, 1),
        new WorldPos (-2, 0, 0),
		//new WorldPos (-3, 0, -3), new WorldPos (-3, 0, 3), new WorldPos (3, 0, -3),
		//new WorldPos (3, 0, 3),
		//new WorldPos (-4, 0, -1),
		//new WorldPos (-4, 0, 1), new WorldPos (-1, 0, -4), new WorldPos (-1, 0, 4), new WorldPos (1, 0, -4), new WorldPos (1, 0, 4),
		//new WorldPos (4, 0, -1), new WorldPos (4, 0, 1),

		//new WorldPos (-4, 0, 0), new WorldPos (0, 0, -4), new WorldPos (0, 0, 4), new WorldPos (4, 0, 0),

		//new WorldPos (-3, 0, -2), new WorldPos (-3, 0, 2), new WorldPos (-2, 0, -3),
		//new WorldPos (-2, 0, 3), new WorldPos (2, 0, -3), new WorldPos (2, 0, 3), new WorldPos (3, 0, -2), new WorldPos (3, 0, 2),

		//new WorldPos (-3, 0, -1),
		//new WorldPos (-3, 0, 1), new WorldPos (-1, 0, -3), new WorldPos (-1, 0, 3), new WorldPos (1, 0, -3), new WorldPos (1, 0, 3),
		//new WorldPos (3, 0, -1), new WorldPos (3, 0, 1),

		//new WorldPos (-3, 0, 0), new WorldPos (0, 0, -3), new WorldPos (0, 0, 3), new WorldPos (3, 0, 0),

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

    public World256 world256;
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
        if (DeleteChunk256s())
            return;

        FindChunk256sToLoad();
        LoadAndRenderChunk256s();
    }

    void FindChunk256sToLoad()
    {
        //Get the position of this gameobject to generate around

        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / (4096)) * (4096),
            Mathf.FloorToInt(transform.position.y / (4096)) * (4096),
            Mathf.FloorToInt(transform.position.z / (4096)) * (4096)
            );
		

        //If there aren't already chunk256s to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunk256Positions.Length; i++)
            {
                //translate the player position and array position into chunk256 position
                WorldPos newChunk256Pos = new WorldPos(
                    chunk256Positions[i].x * 4096 + playerPos.x,
					chunk256Positions[i].y * 4096 + playerPos.y,
                    chunk256Positions[i].z * 4096 + playerPos.z
                    );

                //Get the chunk256 in the defined position
                Chunk256 newChunk256 = world256.GetChunk256(
                    newChunk256Pos.x, newChunk256Pos.y, newChunk256Pos.z);

                //If the chunk256 already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk256 != null
                    && (newChunk256.rendered || updateList.Contains(newChunk256Pos)))
                    continue;

                int player_y = (int)(Mathf.Floor(playerPos.y / 4096));

                //load a column of chunk256s in this position

                for (int y = player_y - 2; y < player_y + 2; y++)
                {
                    for (int x = newChunk256Pos.x - 4096; x <= newChunk256Pos.x + 4096; x += 4096)
                    {
                        for (int z = newChunk256Pos.z - 4096; z <= newChunk256Pos.z + 4096; z += 4096)
                        {
                            buildList.Add(new WorldPos(
                                x, y * 4096, z));
                        }
                    }
                    updateList.Add(new WorldPos(
                                newChunk256Pos.x, y * 4096, newChunk256Pos.z));
                }
                return;
                //for (int y = player_y - 1; y < player_y+1; y++)
                //{

                //    for (int x = newChunk256Pos.x - Chunk256.chunk256Size; x <= newChunk256Pos.x + Chunk256.chunk256Size; x += Chunk256.chunk256Size)
                //    {
                //        for (int z = newChunk256Pos.z - Chunk256.chunk256Size; z <= newChunk256Pos.z + Chunk256.chunk256Size; z += Chunk256.chunk256Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk256.chunk256Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk256Pos.x, y * Chunk256.chunk256Size, newChunk256Pos.z));
                //}
                //for (int y = player_y - 1; y > player_y - 4; y--)
                //{

                //    for (int x = newChunk256Pos.x - Chunk256.chunk256Size; x <= newChunk256Pos.x + Chunk256.chunk256Size; x += Chunk256.chunk256Size)
                //    {
                //        for (int z = newChunk256Pos.z - Chunk256.chunk256Size; z <= newChunk256Pos.z + Chunk256.chunk256Size; z += Chunk256.chunk256Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk256.chunk256Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk256Pos.x, y * Chunk256.chunk256Size, newChunk256Pos.z));
                //}
                //for (int y = player_y +1; y < player_y + 4; y++)
                //{

                //    for (int x = newChunk256Pos.x - Chunk256.chunk256Size; x <= newChunk256Pos.x + Chunk256.chunk256Size; x += Chunk256.chunk256Size)
                //    {
                //        for (int z = newChunk256Pos.z - Chunk256.chunk256Size; z <= newChunk256Pos.z + Chunk256.chunk256Size; z += Chunk256.chunk256Size)
                //        {
                //            buildList.Add(new WorldPos(
                //                x, y * Chunk256.chunk256Size, z));
                //        }
                //    }
                //    updateList.Add(new WorldPos(
                //                newChunk256Pos.x, y * Chunk256.chunk256Size, newChunk256Pos.z));
                //}
                //return;
            }
        }
    }

    void LoadAndRenderChunk256s()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 4; i++)
            {
                BuildChunk256(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunks were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            Chunk256 chunk256 = world256.GetChunk256(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk256 != null)
                chunk256.update = true;
            updateList.RemoveAt(0);
        }
    }

    void BuildChunk256(WorldPos pos)
    {
        if (world256.GetChunk256(pos.x, pos.y, pos.z) == null)
        {
            world256.CreateChunk256(pos.x, pos.y, pos.z);
        }
    }

    public int dist;
    bool DeleteChunk256s()
    {

        if (timer == 10)
        {
            float v = playerRigidBody.velocity.magnitude;

            var chunk256sToDelete = new List<WorldPos>();
            foreach (var chunk256 in world256.chunk256s)
            {
                float distance = Vector3.Distance(
					new Vector3(chunk256.Value.pos.x, chunk256.Value.pos.y, chunk256.Value.pos.z),
					new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z));

                if (distance > 500000 || (distance < 4096 && v < 1 && playerControls.aboveDetailedChunk))
                    chunk256sToDelete.Add(chunk256.Key);
            }

            foreach (var chunk256 in chunk256sToDelete)
                world256.DestroyChunk256(chunk256.x, chunk256.y, chunk256.z);

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }
}