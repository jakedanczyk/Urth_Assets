using UnityEngine;
using System.Collections;
using System;
using SimplexNoise;

public class TerrainGen
{
    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.005f;
    float stoneBaseNoiseHeight = 4;
    float stoneHeight = 0;

    float stoneMountainHeight = 8800;
    float stoneMountainFrequency = 0.000043f;
    float stoneMinHeight = 2000;

    float dirtBaseHeight = 1;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 1;
    float dirtHeight = 0;

    float snowBaseHeight = 1;
    float snowNoise = 0.04f;
    float snowNoiseHeight = 1;
    float snowHeight = 0;

    float treeline = 3700;
    float shrubline = 5050;
    float snowline = 5180;
    float caveFrequency = 0.0025f;
    int caveSize = 21;

    float goldFrequency = 0.025f;
    int goldSize = 15;

    float copperFrequency = 0.25f;
    int copperSize = 15;

    float treeFrequency = 0.04f;
    int treeDensity = 8;
    GameObject treeVar;

    float huckFrequency = 0.042f;
    int huckDensity = 8;
    GameObject huckVar;

    float boneFrequency = 10.05f;
    float boneDensity = 10.00001f;
    GameObject boneVar;

    float horseFrequency = 1.05f;
    float horseDensity = 1.0000001f;
    GameObject horseVar;

    GameObject waterVar;

    //float stoneHeight, dirtHeight, snowHeight;

    public void NoiseGen(float x, float y, float z)
    {
        float stoneHeight = stoneBaseHeight;
        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        if (y > (stoneHeight - 8))
        {
            if (y < shrubline)
            {
                dirtHeight = stoneHeight + dirtBaseHeight;
                dirtHeight += GetNoiseF(x, 100, z, dirtNoise, dirtNoiseHeight);
            }
            else if (y > snowline)
            {
                snowHeight = stoneHeight + snowBaseHeight;
                snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);
            }
        }
    }

    public Chunk ChunkGen(Chunk chunk)
    {
        for (float x = chunk.pos.x; x < chunk.pos.x + 4; x += .25f) //Change this line
        {
            for (float z = chunk.pos.z; z < chunk.pos.z + 4; z += .25f)//and this line
            {

                chunk = ChunkColumnGen(chunk, x, z);
            }
        }
        return chunk;
    }

    public Chunk ChunkColumnGen(Chunk chunk, float x, float z)
    {
        NoiseGen(x, chunk.pos.y, z);
        //float stoneHeight = stoneBaseHeight;
        //stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        //if (stoneHeight < stoneMinHeight)
        //    stoneHeight = stoneMinHeight;

        //stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        //if (chunk.pos.y > (stoneHeight - 8))
        //{
        //    if (chunk.pos.y < shrubline)
        //    {
        //        dirtHeight = stoneHeight + dirtBaseHeight;
        //        dirtHeight += GetNoiseF(x, 100, z, dirtNoise, dirtNoiseHeight);
        //    }
        //    else if (chunk.pos.y > snowline)
        //    {
        //        snowHeight = stoneHeight + snowBaseHeight;
        //        snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);
        //    }
        //}
        for (float y = chunk.pos.y - 2; y < chunk.pos.y + 4; y +=.25f )
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise((int)x, (int)y, (int)z, caveFrequency, 100);
            int goldChance = GetNoise((int)x, (int)y, (int)z, goldFrequency, 100);
            int copperChance = GetNoise((int)x, (int)y, (int)z, copperFrequency, 100);


            if (y <= stoneHeight && caveSize < caveChance)
            {
                //if(goldSize > goldChance)
                //{
                //    SetBlock(x, y, z, new BlockQuartzGold(), chunk);
                //}
                //if (copperSize > copperChance)
                //{
                //    SetBlock(x, y, z, new BlockCopperOre(), chunk);
                //}
                //else { SetBlock(x, y, z, new Block(), chunk); }
                SetBlock(x, y, z, new Block(), chunk);
                if (y == stoneHeight && GetNoise((int)x, 0, (int)z, boneFrequency, 11) < boneDensity)
                {
                    chunk.boneSpawn = true;
                    chunk.boneList.Add(new Vector3(x, y + 10, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }
            }
            else if (y < shrubline && y <= dirtHeight && caveSize < caveChance)
            {
                //System.Random rnd = new System.Random();
                //int month = rnd.Next(1, 13);
                //if (month < 11)
                //{
                SetBlock(x, y, z, new BlockGrass(), chunk);
                if (y <= treeline && !chunk.hasTree && GetNoise((int)x, 0, (int)z, treeFrequency, 100) < treeDensity)
                {
                    chunk.hasTree = true;
                        chunk.treeList.Add(new Vector3(x, y, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                }
                if (y == dirtHeight && y <= shrubline && GetNoise((int)x, 0, (int)z, huckFrequency, 100) < huckDensity)
                {
                    chunk.hasBush = true;
                    chunk.bushList.Add(new Vector3(x, y, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }
                //}
                //else
                //{
                //    SetBlock(x, y, z, new BlockWater(), chunk);
                //    chunk.hasWater = true;
                //}
            }
            else if (y >= snowline && y <= snowHeight)
            {
                SetBlock(x, y, z, new BlockSnow(), chunk);
                if (y == snowHeight && GetNoise((int)x, 0, (int)z, boneFrequency, 120) < boneDensity)
                {
                    chunk.boneSpawn = true;
                    chunk.boneList.Add(new Vector3(x, y + 10, z));
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

    public Chunk1 Chunk1Gen(Chunk1 chunk1)
    {
        for (int x = chunk1.pos.x; x < chunk1.pos.x + Chunk1.chunk1Size; x++) //Change this line
        {
            for (int z = chunk1.pos.z; z < chunk1.pos.z + Chunk1.chunk1Size; z++)//and this line
            {

                chunk1 = Chunk1ColumnGen (chunk1, x, z);
            }
        }
        return chunk1;
    }

    public Chunk1 Chunk1ColumnGen(Chunk1 chunk1, int x, int z)
    {
        NoiseGen(x,chunk1.pos.y,z);
        //int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        //stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        //if (stoneHeight < stoneMinHeight)
        //    stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        //stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        //int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        //dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk1.pos.y - 8; y < chunk1.pos.y + 16; y++)
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            int goldChance = GetNoise(x, y, z, goldFrequency, 100);
            int copperChance = GetNoise(x, y, z, copperFrequency, 100);


            if (y <= stoneHeight && caveSize < caveChance)
            {
                //if (goldSize > goldChance)
                //{
                //    SetBlock(x, y, z, new BlockQuartzGold(), chunk1);
                //}
                //if (copperSize > copperChance)
                //{
                //    SetBlock(x, y, z, new BlockCopperOre(), chunk1);
                //}
                 SetBlock1(x, y, z, new Block1(), chunk1);


            }
            else if (y < shrubline && y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock1(x, y, z, new Block1Grass(), chunk1);

                if (y == dirtHeight && y <= treeline && !chunk1.hasTree && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                {
                    chunk1.hasTree = true;
                    chunk1.treeList.Add(new Vector3(x, y, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }

                if (y == dirtHeight && GetNoise(x, 0, z, huckFrequency, 100) < huckDensity)
                {
                    chunk1.hasBush = true;
                    chunk1.bushList.Add(new Vector3(x, y, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }

                if (y == dirtHeight && y > treeline && GetNoise(x, 0, z, boneFrequency, 120) < boneDensity)
                {
                    chunk1.boneSpawn = true;
                    chunk1.boneList.Add(new Vector3(x, y + 10, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }

                if (y == dirtHeight && !chunk1.hasTree && GetNoise(x, 0, z, horseFrequency, 120) < horseDensity)
                {
                    chunk1.horseSpawn = true;
                    chunk1.horseList.Add(new Vector3(x, y + 10, z));
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                }
            }
            else if (y >= snowline && y <= snowHeight)
            {
                SetBlock1(x, y, z, new Block1Snow(), chunk1);
            }
            else
            {
                SetBlock1(x, y, z, new Block1Air(), chunk1);
                chunk1.airCount++;
            }

        }

        return chunk1;
    }

    public Chunk4 Chunk4Gen(Chunk4 chunk4)
    {
        for (int x = chunk4.pos.x; x < chunk4.pos.x + 64; x += 4) //Change this line
        {
            for (int z = chunk4.pos.z; z < chunk4.pos.z + 64; z += 4)//and this line
            {

                chunk4 = Chunk4ColumnGen(chunk4, x, z);
            }
        }
        return chunk4;
    }

    public Chunk4 Chunk4ColumnGen(Chunk4 chunk4, int x, int z)
    {
        NoiseGen(x, chunk4.pos.y, z);

        //int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        //stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        //if (stoneHeight < stoneMinHeight)
        //    stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        //stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        //int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        //dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for (int y = chunk4.pos.y - 32; y < chunk4.pos.y + 64; y += 4)
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            int goldChance = GetNoise(x, y, z, goldFrequency, 100);
            int copperChance = GetNoise(x, y, z, copperFrequency, 100);


            if (y <= stoneHeight && caveSize < caveChance)
            {
                //if(goldSize > goldChance)
                //{
                //    SetBlock(x, y, z, new BlockQuartzGold(), chunk);
                //}
                //if (copperSize > copperChance)
                //{
                //    SetBlock(x, y, z, new BlockCopperOre(), chunk);
                //}
                //else { SetBlock(x, y, z, new Block(), chunk); }
                SetBlock4(x, y, z, new Block4(), chunk4);
            }
            else if (y < shrubline && y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock4(x, y, z, new Block4Grass(), chunk4);

            }
            else if (y >= snowline && y <= snowHeight)
            {
                SetBlock4(x, y, z, new Block4Snow(), chunk4);
            }
            else
            {
                SetBlock4(x, y, z, new Block4Air(), chunk4);
                chunk4.airCount++;
            }

        }

        return chunk4;
    }

    public Chunk16 Chunk16Gen(Chunk16 chunk16)
    {
        for (int x = chunk16.pos.x; x < chunk16.pos.x + 256; x+=16) //Change this line
        {
            for (int z = chunk16.pos.z; z < chunk16.pos.z + 256; z+=16)//and this line
            {

					chunk16 = Chunk16ColumnGen (chunk16, x, z);
            }
        }
        return chunk16;
    }

	public Chunk16 Chunk16ColumnGen(Chunk16 chunk16, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));
        float snowHeight = stoneHeight + snowBaseHeight;
        snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);

        for (int y = chunk16.pos.y - 128; y < chunk16.pos.y + 256; y+=16)
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            int goldChance = GetNoise(x, y, z, goldFrequency, 100);
            int copperChance = GetNoise(x, y, z, copperFrequency, 100);


            if (y <= stoneHeight && caveSize < caveChance)
            {
                //if(goldSize > goldChance)
                //{
                //    SetBlock(x, y, z, new BlockQuartzGold(), chunk);
                //}
                //if (copperSize > copperChance)
                //{
                //    SetBlock(x, y, z, new BlockCopperOre(), chunk);
                //}
                //else { SetBlock(x, y, z, new Block(), chunk); }
                SetBlock16(x, y, z, new Block16(), chunk16);
            }
            else if (y < shrubline && y <= (dirtHeight-8) && caveSize < caveChance)
            {
                SetBlock16(x, y, z, new Block16Grass(), chunk16);

            }
            else if (y >= snowline && (y - 8) <= snowHeight)
            {
                SetBlock16(x, y, z, new Block16Snow(), chunk16);

            }
            else
            {
                SetBlock16(x, y, z, new Block16Air(), chunk16);
                chunk16.airCount++;
            }

        }

        return chunk16;
    }

    public Chunk64 Chunk64Gen(Chunk64 chunk64)
    {
        for (int x = chunk64.pos.x; x < chunk64.pos.x + 1024; x += 64) //Change this line
        {
            for (int z = chunk64.pos.z; z < chunk64.pos.z + 1024; z += 64)//and this line
            {

                chunk64 = Chunk64ColumnGen(chunk64, x, z);
            }
        }
        return chunk64;
    }

    public Chunk64 Chunk64ColumnGen(Chunk64 chunk64, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));
        float snowHeight = stoneHeight + snowBaseHeight;
        snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);

        for (int y = chunk64.pos.y - 512; y < chunk64.pos.y + 1024; y += 64)
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            int goldChance = GetNoise(x, y, z, goldFrequency, 100);
            int copperChance = GetNoise(x, y, z, copperFrequency, 100);


            if (y <= stoneHeight)
            {
                //if(goldSize > goldChance)
                //{
                //    SetBlock(x, y, z, new BlockQuartzGold(), chunk);
                //}
                //if (copperSize > copperChance)
                //{
                //    SetBlock(x, y, z, new BlockCopperOre(), chunk);
                //}
                //else { SetBlock(x, y, z, new Block(), chunk); }
                SetBlock64(x, y, z, new Block64(), chunk64);
            }
            else if (y < shrubline && (y-128) <= dirtHeight )
            {
                SetBlock64(x, y, z, new Block64Grass(), chunk64);

            }
            else if (y >= snowline && (y - 128) <= snowHeight)
            {
                SetBlock64(x, y, z, new Block64Snow(), chunk64);

            }
            else
            {
                SetBlock64(x, y, z, new Block64Air(), chunk64);
                chunk64.airCount++;
            }

        }

        return chunk64;
    }

    public Chunk256 Chunk256Gen(Chunk256 chunk256)
    {
        for (int x = chunk256.pos.x; x < chunk256.pos.x + 4096; x += 256) //Change this line
        {
            for (int z = chunk256.pos.z; z < chunk256.pos.z + 4096; z += 256)//and this line
            {

                chunk256 = Chunk256ColumnGen(chunk256, x, z);
            }
        }
        return chunk256;
    }

    public Chunk256 Chunk256ColumnGen(Chunk256 chunk256, int x, int z)
    {
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        if (stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));
        float snowHeight = stoneHeight + snowBaseHeight;
        snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);

        for (int y = chunk256.pos.y - 2048; y < chunk256.pos.y + 4096; y += 256)
        {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);
            int goldChance = GetNoise(x, y, z, goldFrequency, 100);
            int copperChance = GetNoise(x, y, z, copperFrequency, 100);


            if (y <= stoneHeight)
            {
                //if(goldSize > goldChance)
                //{
                //    SetBlock(x, y, z, new BlockQuartzGold(), chunk);
                //}
                //if (copperSize > copperChance)
                //{
                //    SetBlock(x, y, z, new BlockCopperOre(), chunk);
                //}
                //else { SetBlock(x, y, z, new Block(), chunk); }
                SetBlock256(x, y, z, new Block256(), chunk256);
            }
            else if (y <= shrubline && (y - 128) <= dirtHeight)
            {
                SetBlock256(x, y, z, new Block256Grass(), chunk256);

            }
            else if (y >= snowline && (y-128) <= snowHeight)
            {
                SetBlock256(x, y, z, new Block256Snow(), chunk256);

            }
            else
            {
                SetBlock256(x, y, z, new Block256Air(), chunk256);
                chunk256.airCount++;
            }

        }

        return chunk256;
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

    public static void SetBlock(float x, float y, float z, Block block, Chunk chunk, bool replaceBlocks = false)
    {
        x -= chunk.pos.x;
        x = (int)Mathf.Round(x*4);
        y -= chunk.pos.y;
        y = (int)Mathf.Round(y * 4);
        z -= chunk.pos.z;
        z = (int)Mathf.Round(z * 4);

        if (Chunk.InRange((int)x) && Chunk.InRange((int)y) && Chunk.InRange((int)z))
        {
            if (replaceBlocks || chunk.blocks[(int)x, (int)y, (int)z] == null)
                chunk.SetBlock((int)x, (int)y, (int)z, block);
        }
    }

    public static void SetBlock1(int x, int y, int z, Block1 block1, Chunk1 chunk1, bool replaceBlocks = false)
    {
        x -= chunk1.pos.x;
        y -= chunk1.pos.y;
        z -= chunk1.pos.z;

        if (Chunk1.InRange(x) && Chunk1.InRange(y) && Chunk1.InRange(z))
        {
            if (replaceBlocks || chunk1.block1s[x, y, z] == null)
                chunk1.SetBlock1(x, y, z, block1);
        }
    }

    public static void SetBlock4(int x, int y, int z, Block4 block4, Chunk4 chunk4, bool replaceBlocks = false)
    {
        x -= chunk4.pos.x;
        x /= 4;
        y -= chunk4.pos.y;
        y /= 4;
        z -= chunk4.pos.z;
        z /= 4;

        if (Chunk4.InRange(x) && Chunk4.InRange(y) && Chunk4.InRange(z))
        {
            if (replaceBlocks || chunk4.block4s[x, y, z] == null)
                chunk4.SetBlock4(x, y, z, block4);
        }
    }

    public static void SetBlock16(int x, int y, int z, Block16 block16, Chunk16 chunk16, bool replaceBlocks = false)
    {
        x -= chunk16.pos.x;
        x /= 16;
        y -= chunk16.pos.y;
        y /= 16;
        z -= chunk16.pos.z;
        z /= 16;

        if (Chunk16.InRange(x) && Chunk16.InRange(y) && Chunk16.InRange(z))
        {
            if (replaceBlocks || chunk16.block16s[x, y, z] == null)
                chunk16.SetBlock16(x, y, z, block16);
        }
    }

    public static void SetBlock64(int x, int y, int z, Block64 block64, Chunk64 chunk64, bool replaceBlocks = false)
    {
        x -= chunk64.pos.x;
        x /= 64;
        y -= chunk64.pos.y;
        y /= 64;
        z -= chunk64.pos.z;
        z /= 64;

        if (Chunk64.InRange(x) && Chunk64.InRange(y) && Chunk64.InRange(z))
        {
            if (replaceBlocks || chunk64.block64s[x, y, z] == null)
                chunk64.SetBlock64(x, y, z, block64);
        }
    }

    public static void SetBlock256(int x, int y, int z, Block256 block256, Chunk256 chunk256, bool replaceBlocks = false)
    {
        x -= chunk256.pos.x;
        x /= 256;
        y -= chunk256.pos.y;
        y /= 256;
        z -= chunk256.pos.z;
        z /= 256;

        if (Chunk256.InRange(x) && Chunk256.InRange(y) && Chunk256.InRange(z))
        {
            if (replaceBlocks || chunk256.block256s[x, y, z] == null)
                chunk256.SetBlock256(x, y, z, block256);
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
    public static float GetNoiseF(float x, float y, float z, float scale, float max)
    {
        return (Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f);
    }
}