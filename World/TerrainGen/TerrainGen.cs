using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGen
{
    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.05f;
    float stoneBaseNoiseHeight = 4;

    float stoneMountainHeight = 32000;
    float stoneMountainFrequency = 0.0000013f;
    float stoneMinHeight = 8000;

    float dirtBaseHeight = 16;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 1;

    float treeline = 23600;
    float shrubline = 24600;

    float caveFrequency = 0.0025f;
    int caveSize = 25;

    float goldFrequency = 0.025f;
    int goldSize = 15;

    float copperFrequency = 0.25f;
    int copperSize = 15;

    float treeFrequency = 0.2f;
    int treeDensity = 2;
    GameObject treeVar;

    float huckFrequency = 0.2f;
    int huckDensity = 2;
    GameObject huckVar;

    float boneFrequency = 1.05f;
    float boneDensity = 1.00001f;
    GameObject boneVar;

    float horseFrequency = 1.05f;
    float horseDensity = 1.0000001f;
    GameObject horseVar;

    public Chunk ChunkGen(Chunk chunk)
    {
        for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.chunkSize; x++) //Change this line
        {
            for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.chunkSize; z++)//and this line
            {

					chunk = ChunkColumnGen (chunk, x, z);
            }
        }
        return chunk;
    }

	public Chunk ChunkColumnGen(Chunk chunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++)
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            int goldChance = GetNoise(x, y, z, goldFrequency, 100);
            int copperChance = GetNoise(x, y, z, copperFrequency, 100);


            if (y <= stoneHeight && caveSize < caveChance)
            {
                if(goldSize > goldChance)
                {
                    SetBlock(x, y, z, new BlockQuartzGold(), chunk);
                }
                if (copperSize > copperChance)
                {
                    SetBlock(x, y, z, new BlockCopperOre(), chunk);
                }
                else { SetBlock(x, y, z, new Block(), chunk); }
            }
            else if (y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new BlockGrass(), chunk);

                if (y == dirtHeight && y <= treeline && !chunk.hasTree && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                {
                    chunk.hasTree = true;
                    chunk.treeList.Add(new Vector3(x,y,z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }

                if (y == dirtHeight && y <= shrubline && GetNoise(x, 0, z, huckFrequency, 100) < huckDensity)
                {
                    chunk.hasBush = true;
                    chunk.bushList.Add(new Vector3(x, y, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }

                if (y == dirtHeight && y >treeline && GetNoise(x, 0, z, boneFrequency, 120) < boneDensity)
                {
                    chunk.boneSpawn = true;
                    chunk.boneList.Add(new Vector3(x, y + 10, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }

                if (y == dirtHeight && !chunk.hasTree && GetNoise(x, 0, z, horseFrequency, 120) < horseDensity)
                {
                    chunk.horseSpawn = true;
                    chunk.horseList.Add(new Vector3(x, y + 10, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }
            }
            else
            {
                SetBlock(x, y, z, new BlockAir(), chunk);
                chunk.airCount++;
            }

        }

        return chunk;
    }

    public SimpleChunk SimpleChunkGen(SimpleChunk simpleChunk)
    {
        for (int x = simpleChunk.pos.x; x < simpleChunk.pos.x + Chunk.chunkSize; x++) //Change this line
        {
            for (int z = simpleChunk.pos.z; z < simpleChunk.pos.z + Chunk.chunkSize; z++)//and this line
            {

                simpleChunk = SimpleChunkColumnGen(simpleChunk, x, z);
            }
        }
        return simpleChunk;
    }

    public SimpleChunk SimpleChunkColumnGen(SimpleChunk simpleChunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        if (simpleChunk.pos.y <= stoneHeight)
        {
            simpleChunk.simpleChunkType = SimpleChunkType.Stone;
        }

        else if (simpleChunk.pos.y <= dirtHeight && simpleChunk.pos.y <= treeline)
        {
            simpleChunk.simpleChunkType = SimpleChunkType.Grass;
        }

        else
        {
            simpleChunk.simpleChunkType = SimpleChunkType.Air;
            simpleChunk.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }


        //for (int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++)
        //{
        //    //Get a value to base cave generation on
        //    int caveChance = GetNoise(x, y, z, caveFrequency, 100);

        //    if (y <= stoneHeight && caveSize < caveChance)
        //    {
        //        SetBlock(x, y, z, new Block(), chunk);
        //    }
        //    else if (y <= dirtHeight && y <= treeline && caveSize < caveChance)
        //    {
        //        SetBlock(x, y, z, new BlockGrass(), chunk);

        //        if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
        //        {
        //            chunk.treeList.Add(new Vector3(x, y, z));
        //            //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
        //            //treeVar.transform.position = new Vector3(x, y, z);
        //            //CreateTree(x, y + 1, z, chunk);
        //        }
        //    }
        //    else
        //    {
        //        SetBlock(x, y, z, new BlockAir(), chunk);
        //    }

        //}

        return simpleChunk;
    }


    public SimpleChunk2 SimpleChunkGen2(SimpleChunk2 simpleChunk)
    {
        for (int x = simpleChunk.pos.x; x < simpleChunk.pos.x + Chunk.chunkSize; x++) //Change this line
        {
            for (int z = simpleChunk.pos.z; z < simpleChunk.pos.z + Chunk.chunkSize; z++)//and this line
            {

                simpleChunk = SimpleChunkColumnGen2(simpleChunk, x, z);
            }
        }
        return simpleChunk;
    }

    public SimpleChunk2 SimpleChunkColumnGen2(SimpleChunk2 simpleChunk, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        if (simpleChunk.pos.y <= stoneHeight)
        {
            simpleChunk.simpleChunkType = SimpleChunkType.Stone;
        }

        else if (simpleChunk.pos.y <= dirtHeight)
        {
            simpleChunk.simpleChunkType = SimpleChunkType.Grass;
        }

        else
        {
            simpleChunk.simpleChunkType = SimpleChunkType.Air;
            simpleChunk.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }


        //for (int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++)
        //{
        //    //Get a value to base cave generation on
        //    int caveChance = GetNoise(x, y, z, caveFrequency, 100);

        //    if (y <= stoneHeight && caveSize < caveChance)
        //    {
        //        SetBlock(x, y, z, new Block(), chunk);
        //    }
        //    else if (y <= dirtHeight && y <= treeline && caveSize < caveChance)
        //    {
        //        SetBlock(x, y, z, new BlockGrass(), chunk);

        //        if (y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
        //        {
        //            chunk.treeList.Add(new Vector3(x, y, z));
        //            //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
        //            //treeVar.transform.position = new Vector3(x, y, z);
        //            //CreateTree(x, y + 1, z, chunk);
        //        }
        //    }
        //    else
        //    {
        //        SetBlock(x, y, z, new BlockAir(), chunk);
        //    }

        //}

        return simpleChunk;
    }

    void CreateTree(int x, int y, int z, Chunk chunk)
    {
        //create leaves
        for (int xi = -2; xi <= 2; xi++)
        {
            for (int yi = 4; yi <= 8; yi++)
            {
                for (int zi = -2; zi <= 2; zi++)
                {
                    SetBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
                }
            }
        }

        //create trunk
        for (int yt = 0; yt < 6; yt++)
        {
            SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
        }
    }

    public static void SetBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.pos.x;
        y -= chunk.pos.y;
        z -= chunk.pos.z;

        if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
        {
            if (replaceBlocks || chunk.blocks[x, y, z] == null)
                chunk.SetBlock(x, y, z, block);
        }
    }

    public static void SetSimpleChunk(SimpleChunk simpleChunk, SimpleChunk simpleChunkTemplate)
    {
        simpleChunk.SetSimpleChunk(simpleChunkTemplate);
    }

    public static int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}