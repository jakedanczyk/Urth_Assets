﻿using UnityEngine;
using System.Collections;
using System;
using SimplexNoise;

public class TerrainGen
{
    float stoneBaseHeight = -24;
    float stoneBaseNoise = 0.005f;
    float stoneBaseNoiseHeight = 4;
    float stoneHeight = 0;

    float stoneMinorHeight = 400;
    float stoneMinorNoise = 0.0005f;
    float stoneMountainHeight = 8800;
    float stoneMountainFrequency = 0.000043f;
    float stoneMinHeight = 2000;

    float cliffFrequency = 0.0006f;
    float cliffMax = 2;

    float dirtBaseHeight = 2;
    float dirtNoise = 0.1f;
    float dirtNoiseHeight = 1;
    float dirtHeight = 1;

    float snowBaseHeight = 1;
    float snowNoise = 0.04f;
    float snowNoiseHeight = 1;
    float snowHeight = 0;

    public float treeline = 3700;
    public float shrubline = 5050;
    float snowline = 5180;
    float caveFrequency = 0.0025f;
    int caveSize = 21;

    float goldFrequency = 0.025f;
    int goldSize = 15;

    float copperFrequency = 0.25f;
    int copperSize = 15;

    float treeFrequency = 0.009f;
    int treeDensity = 12;
    float huckFrequency = 0.0042f;
    int huckDensity = 8;

    float boneFrequency = 0.0005f;
    float boneDensity = 2.00001f;

    float horseFrequency = 0.005f;
    float horseDensity = 2.0000001f;

    float allosaurusFrequency = 0.0005f;
    float allosaurusDensity = 1.0000001f;

    float pondFrequency = .02f;
    float pondDensity = 50.0f;

    float forestFrequency = .00052f;
    float forestDensity = 50.0f;


    //float stoneHeight, dirtHeight, snowHeight;

    public void NoiseGen(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        float cliffHeight = Mathf.Min((y - 3000f)/300*Mathf.Pow(GetNoiseF(x, 0, z, cliffFrequency, cliffMax),3f),500f);

        if (cliffHeight > 20)
            stoneHeight += cliffHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        if (y > (stoneHeight - 65))
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

    public void NoiseGen16(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        float cliffHeight = Mathf.Min((y - 3000f) / 300 * Mathf.Pow(GetNoiseF(x, 0, z, cliffFrequency, cliffMax), 3f), 500f);

        if (cliffHeight > 20)
            stoneHeight += cliffHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        if (y > (stoneHeight - 274))
        {
            if (y < shrubline)
            {
                dirtHeight = stoneHeight + dirtBaseHeight;
                dirtHeight += GetNoiseF(x, 100, z, dirtNoise, dirtNoiseHeight);
            }
            else if (y > snowline-274)
            {
                snowHeight = stoneHeight + snowBaseHeight;
                snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);
            }
        }
    }
    public void NoiseGen64(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        float cliffHeight = Mathf.Min((y - 3000f) / 300 * Mathf.Pow(GetNoiseF(x, 0, z, cliffFrequency, cliffMax), 3f), 500f);

        if (cliffHeight > 20)
            stoneHeight += cliffHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        if (y > (stoneHeight - 1200))
        {
            if (y < shrubline)
            {
                dirtHeight = stoneHeight + dirtBaseHeight;
                dirtHeight += GetNoiseF(x, 100, z, dirtNoise, dirtNoiseHeight);
            }
            else if (y > snowline - 1200)
            {
                snowHeight = stoneHeight + snowBaseHeight;
                snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);
            }
        }
    }
    public void NoiseGen256(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        float cliffHeight = Mathf.Min((y - 3000f) / 300 * Mathf.Pow(GetNoiseF(x, 0, z, cliffFrequency, cliffMax), 3f), 500f);

        if (cliffHeight > 20)
            stoneHeight += cliffHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        if (y > (stoneHeight - 4200))
        {
            if (y < shrubline)
            {
                dirtHeight = stoneHeight + dirtBaseHeight;
                dirtHeight += GetNoiseF(x, 100, z, dirtNoise, dirtNoiseHeight);
            }
            else if (y > snowline-4200)
            {
                snowHeight = stoneHeight + snowBaseHeight;
                snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);
            }
        }
    }
    public float StoneHeight(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        float cliffHeight = Mathf.Min((y - 3000f) / 300 * Mathf.Pow(GetNoiseF(x, 0, z, cliffFrequency, cliffMax), 3f), 500f);

        if (cliffHeight > 20)
            stoneHeight += cliffHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);
        return stoneHeight;
    }

    public float DirtHeight(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        float cliffHeight = Mathf.Min((y - 3000f) / 300 * Mathf.Pow(GetNoiseF(x, 0, z, cliffFrequency, cliffMax), 3f), 500f);

        if (cliffHeight > 20)
            stoneHeight += cliffHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        dirtHeight = stoneHeight + dirtBaseHeight;
        dirtHeight += GetNoiseF(x, 100, z, dirtNoise, dirtNoiseHeight);
        return dirtHeight;
    }

    public float SnowHeight(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        float cliffHeight = Mathf.Min((y - 3000f) / 300 * Mathf.Pow(GetNoiseF(x, 0, z, cliffFrequency, cliffMax), 3f), 500f);

        if (cliffHeight > 20)
            stoneHeight += cliffHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);


        snowHeight = stoneHeight + snowBaseHeight;
        snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);

        return snowHeight;
    }
    public float StoneHeight256(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);
        return stoneHeight;
    }

    public float DirtHeight256(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        dirtHeight = stoneHeight + dirtBaseHeight;
        dirtHeight += GetNoiseF(x, 100, z, dirtNoise, dirtNoiseHeight);
        return dirtHeight;
    }

    public float SnowHeight256(float x, float y, float z)
    {
        stoneHeight = stoneBaseHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMountainFrequency, stoneMountainHeight);

        if (stoneHeight < stoneMinHeight)
            stoneHeight = stoneMinHeight;

        stoneHeight += GetNoiseF(x, 0, z, stoneMinorNoise, stoneMinorHeight);
        stoneHeight += GetNoiseF(x, 0, z, stoneBaseNoise, stoneBaseNoiseHeight);

        snowHeight = stoneHeight + snowBaseHeight;
        snowHeight += GetNoiseF(x, 100, z, snowNoise, snowNoiseHeight);

        return snowHeight;
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
        stoneHeight = stoneBaseHeight;

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
                SetBlock(x, y, z, new Block(), chunk);

                //if(goldSize > goldChance)
                //{
                //    SetBlock(x, y, z, new BlockQuartzGold(), chunk);
                //}
                //if (copperSize > copperChance)
                //{
                //    SetBlock(x, y, z, new BlockCopperOre(), chunk);
                //}
                //else { SetBlock(x, y, z, new Block(), chunk); }
            }
            else if (y < shrubline && y <= dirtHeight && caveSize < caveChance)
            {
                SetBlock(x, y, z, new BlockGrass(), chunk);
                //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                //treeVar.transform.position = new Vector3(x, y, z);
                //CreateTree(x, y + 1, z, chunk);
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

                chunk1 = Chunk1ColumnGen(chunk1, x, z);
            }
        }
        return chunk1;
    }

    public Chunk1 Chunk1ColumnGen(Chunk1 chunk1, int x, int z)
    {
        stoneHeight = stoneBaseHeight;

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
        stoneHeight = stoneBaseHeight;
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
                SetBlock4(x, y, z, new Block4(), chunk4);
                //if(goldSize > goldChance)
                //{
                //    SetBlock(x, y, z, new BlockQuartzGold(), chunk);
                //}
                //if (copperSize > copperChance)
                //{
                //    SetBlock(x, y, z, new BlockCopperOre(), chunk);
                //}
                //else { SetBlock(x, y, z, new Block(), chunk); }
            }
            else if (y < shrubline && (y - 2) <= dirtHeight && caveSize < caveChance)
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
        stoneHeight = stoneBaseHeight;
        NoiseGen16(x, chunk16.pos.y, z);

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
            else if (y < shrubline && y - 15 <= (dirtHeight) && caveSize < caveChance)
            {
                SetBlock16(x, y, z, new Block16Grass(), chunk16);
                //if (Mathf.Abs(y - dirtHeight) < 8 && y <= treeline && !chunk16.hasTree && GetNoise(x, 0, z, treeFrequency, 20) < treeDensity)
                //{
                if (y <= treeline && GetNoise(x, 0, z, forestFrequency, 400) < forestDensity)
                {
                    var terrainGen = new TerrainGen();
                    chunk16.hasTree = true;
                    for (int i = 0; i < 1; i++)
                    {
                        int tx = UnityEngine.Random.Range(-8, 8);
                        int tz = UnityEngine.Random.Range(-8, 8);
                        float ty = terrainGen.DirtHeight(x + tx, dirtHeight, z + tz);
                        chunk16.treeList.Add(new Vector3(x + tx, dirtHeight, z + tz));
                    }
                }
                else
                { 
                    if (GetNoise(x, 0, z, huckFrequency, 100) < huckDensity)
                    {
                        chunk16.hasBush = true;
                        chunk16.bushList.Add(new Vector3(x, y, z));
                    }
                    if (GetNoise(x, 0, z, boneFrequency, 100) < boneDensity)
                    {
                        chunk16.boneSpawn = true;
                        chunk16.boneList.Add(new Vector3(x, y + 10, z));
                    }
                    if (GetNoise(x, 0, z, horseFrequency, 100) < horseDensity)
                    {
                        chunk16.horseSpawn = true;
                        chunk16.horseList.Add(new Vector3(x, y + 10, z));
                    }
                    if (GetNoise(x, 0, z, allosaurusFrequency, 100) < allosaurusDensity)
                    {
                        chunk16.allosaurusSpawn = true;
                        chunk16.allosaurusList.Add(new Vector3(x, y + 10, z));
                    }
                }
            }
            else if (y - 16 >= snowline && (y - 16) <= snowHeight)
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
        stoneHeight = stoneBaseHeight;
        NoiseGen64(x, chunk64.pos.y, z);

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
            else if (y < shrubline && (y - 63) <= dirtHeight )
            {
                SetBlock64(x, y, z, new Block64Grass(), chunk64);
                //if (Mathf.Abs(y - dirtHeight) < 31 && y <= treeline && !chunk64.hasTree && GetNoise(x, 0, z, treeFrequency, 40) < treeDensity)
                //{
                //    chunk64.hasTree = true;
                //    for(int i = 0; i < 32; i++)
                //    {
                //        int tx = UnityEngine.Random.Range(0, 63);
                //        int tz = UnityEngine.Random.Range(0, 63);
                //        chunk64.treeList.Add(new Vector3(x+tx, dirtHeight, z+tz));
                //    }
                    //treeVar = (GameObject)Resources.Load("Tree1", typeof(GameObject));
                    //treeVar.transform.position = new Vector3(x, y, z);
                    //CreateTree(x, y + 1, z, chunk);
                //}
            }
            else if (y >= snowline && (y - 63) <= snowHeight)
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
        stoneHeight = stoneBaseHeight;
        NoiseGen256(x, chunk256.pos.y, z);

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
            else if (y <= shrubline && (y - 255) <= dirtHeight)
            {
                SetBlock256(x, y, z, new Block256Grass(), chunk256);

            }
            else if (y >= snowline && (y - 255) <= snowHeight)
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

    void CreatePond(int x, int y, int z, Chunk chunk)
    {
        //create trunk
        for (int yt = -8; yt < 1; yt++)
        {
            for (int xt = 8 + yt; xt > -8 - yt; xt--)
                for (int zt = 8 + yt; zt > -8 - yt; zt--)
                {                    
                    SetBlock(x + (xt / 4f), y + (yt / 4f), z + (zt / 4f), new BlockWater(), chunk, true);
                }
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