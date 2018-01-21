﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{
    public bool hasTree, hasBush, hasWater;
    public List<Vector3> treeList, boneList, bushList, horseList, waterList;
    public Block[, ,] blocks = new Block[chunkSize, chunkSize, chunkSize];

    public int airCount = 0;
    public bool boneSpawn, horseSpawn, spawned;
    public static int chunkSize = 16;
    public bool update = false;
    public bool rendered;

    public MeshFilter filter;
    public MeshCollider coll;

    public ChunkWater chunkWater;
    public GameObject water;

    public World world;
    public WorldPos pos;

    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }

    //Update is called once per frame
    //void Update()
    //{
    //    if (update)
    //    {
    //        update = false;
    //        UpdateChunk();
    //    }
    //}

	public Block GetBlock(int x, int y, int z)
	{
        if (InRange(x) && InRange(y) && InRange(z))
            return blocks[x, y, z];
        return world.GetBlock((float)pos.x + x*.25f, (float)pos.y + y * .25f, (float)pos.z + z * .25f);
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            blocks[x, y, z] = block;
        }
        else
        {
            world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }

    public void SetBlocksUnmodified()
    {
        foreach (Block block in blocks)
        {
            block.changed = false;
        }
    }

    // Updates the chunk based on its contents
    void UpdateChunk()
    {
        rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < chunkSize; x+=1)
        {
            for (int y = 0; y < chunkSize; y+=1)
            {
                for (int z = 0; z < chunkSize; z+=1)
                {
                    meshData = blocks[x, y, z].Blockdata(this, x, y, z, meshData);
                }
            }
        }

        RenderMesh(meshData);
    }

    // Sends the calculated mesh information
    // to the mesh and collision components
    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        if (hasWater)
        {
            chunkWater.fluidFilter.mesh.Clear();
            chunkWater.fluidFilter.mesh.vertices = meshData.fluidVertices.ToArray();
            chunkWater.fluidFilter.mesh.triangles = meshData.fluidTriangles.ToArray();

            chunkWater.fluidFilter.mesh.uv = meshData.fluiduv.ToArray();
            chunkWater.fluidFilter.mesh.RecalculateNormals();
            chunkWater.coll.sharedMesh = null;
            Mesh fluidMesh = new Mesh();
            fluidMesh.vertices = meshData.fluidVertices.ToArray();
            fluidMesh.triangles = meshData.fluidColTriangles.ToArray();
            fluidMesh.RecalculateNormals();
            chunkWater.coll.sharedMesh = fluidMesh;
        }
        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }

}