using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk1 : MonoBehaviour
{
    public bool hasTree, hasBush, hasWater;
    public List<Vector3> treeList, boneList, bushList, horseList, waterList;
    public Block1[, ,] block1s = new Block1[chunk1Size, chunk1Size, chunk1Size];

    public int airCount = 0;
    public bool boneSpawn, horseSpawn, spawned;
    public static int chunk1Size = 16;
    public bool update = false;
    public bool rendered;
    public bool isSubChunked = false;
    public List<Chunk> subChunkList = new List<Chunk>();

    MeshFilter filter;
    MeshCollider coll;

    public ChunkWater chunkWater;
    public GameObject water;

    public World1 world1;
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
    //        UpdateChunk1();
    //    }
    //}

	public Block1 GetBlock1(int x, int y, int z)
	{
        if (InRange(x) && InRange(y) && InRange(z))
            return block1s[x, y, z];
        return world1.GetBlock1(pos.x + x, pos.y + y, pos.z + z);
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunk1Size)
            return false;

        return true;
    }

    public void SetBlock1(int x, int y, int z, Block1 block1)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            block1s[x, y, z] = block1;
        }
        else
        {
            world1.SetBlock1(pos.x + x, pos.y + y, pos.z + z, block1);
        }
    }

    public void SetBlock1sUnmodified()
    {
        foreach (Block1 block1 in block1s)
        {
            block1.changed = false;
        }
    }

    // Updates the chunk1 based on its contents
    public void UpdateChunk1()
    {
        rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < chunk1Size; x+=1)
        {
            for (int y = 0; y < chunk1Size; y+=1)
            {
                for (int z = 0; z < chunk1Size; z+=1)
                {
                    meshData = block1s[x, y, z].Blockdata(this, x, y, z, meshData);
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
            if(chunkWater.coll.sharedMesh != null)
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


    public void ConvertDown()
    {
        if (isSubChunked)
        {
            foreach (Chunk chunk in subChunkList)
            {
                chunk.gameObject.GetComponent<MeshRenderer>().enabled = true;
                chunk.gameObject.layer = 19;
            }
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.layer = 16;
        }
        else
            LoadChunks.terrainLoadManager.GetComponent<LoadChunks>().ReplaceChunk1(this, pos);
    }
}