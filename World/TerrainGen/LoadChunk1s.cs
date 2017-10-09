using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChunk1s : MonoBehaviour
{
	static WorldPos[] chunk1Positions = {   new WorldPos (0, 0, 0),
		
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

    public World1 world1;
    //public GameObject treePrefab;
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
        if (DeleteChunk1s())
            return;

        FindChunk1sToLoad();
        LoadAndRenderChunk1s();
    }

    void FindChunk1sToLoad()
    {
        //Get the position of this gameobject to generate around

        WorldPos playerPos = new WorldPos(
            Mathf.FloorToInt(transform.position.x / Chunk1.chunk1Size) * Chunk1.chunk1Size,
            Mathf.FloorToInt(transform.position.y / Chunk1.chunk1Size) * Chunk1.chunk1Size,
            Mathf.FloorToInt(transform.position.z / Chunk1.chunk1Size) * Chunk1.chunk1Size
            );
		

        //If there aren't already chunk1s to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunk1Positions.Length; i++)
            {
                //translate the player position and array position into chunk1 position
                WorldPos newChunk1Pos = new WorldPos(
                    chunk1Positions[i].x * Chunk1.chunk1Size + playerPos.x,
					chunk1Positions[i].y * Chunk1.chunk1Size + playerPos.y,
                    chunk1Positions[i].z * Chunk1.chunk1Size + playerPos.z
                    );

                //Get the chunk1 in the defined position
                Chunk1 newChunk1 = world1.GetChunk1(
                    newChunk1Pos.x, newChunk1Pos.y, newChunk1Pos.z);

                //If the chunk1 already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk1 != null
                    && (newChunk1.rendered || updateList.Contains(newChunk1Pos)))
                    continue;

				int player_y = (int)(Mathf.Floor (playerPos.y / Chunk1.chunk1Size));

                //load a column of chunk1s in this position
				for (int y = player_y - 4; y < player_y+4; y++)
                {

                    for (int x = newChunk1Pos.x - Chunk1.chunk1Size; x <= newChunk1Pos.x + Chunk1.chunk1Size; x += Chunk1.chunk1Size)
                    {
                        for (int z = newChunk1Pos.z - Chunk1.chunk1Size; z <= newChunk1Pos.z + Chunk1.chunk1Size; z += Chunk1.chunk1Size)
                        {
                            buildList.Add(new WorldPos(x, y * Chunk1.chunk1Size, z));
                        }
                    }
                    updateList.Add(new WorldPos(newChunk1Pos.x, y * Chunk1.chunk1Size, newChunk1Pos.z));
                }
                return;
            }
        }
    }

    void LoadAndRenderChunk1s()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 4; i++)
            {
                BuildChunk1(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunk1s were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            Chunk1 chunk1 = world1.GetChunk1(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk1 != null)
                chunk1.update = true;
            updateList.RemoveAt(0);
        }
    }

    void BuildChunk1(WorldPos pos)
    {
        if (world1.GetChunk1(pos.x, pos.y, pos.z) == null)
        {
            world1.CreateChunk1(pos.x, pos.y, pos.z);
        }
    }

    public int dist;
    bool DeleteChunk1s()
    {

        if (timer == 10)
        {
            float v = playerRigidBody.velocity.magnitude;
            var chunk1sToDelete = new List<WorldPos>();
            foreach (var chunk1 in world1.chunk1s)
            {
                float distance = Vector3.Distance(
					new Vector3(chunk1.Value.pos.x, .5f * chunk1.Value.pos.y, chunk1.Value.pos.z),
					new Vector3(playerTransform.position.x, .5f * playerTransform.position.y, playerTransform.position.z));

                if (distance > 96 || (distance < 14 && v < 1 && playerControls.aboveDetailedChunk))
                    chunk1sToDelete.Add(chunk1.Key);
            }

            foreach (var chunk1 in chunk1sToDelete)
                world1.DestroyChunk1(chunk1.x, chunk1.y, chunk1.z);

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }
}