using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk64 : MonoBehaviour
{
    public bool hasTree, hasWater;
    public List<Vector3> treeList;
    public Block64[, ,] block64s = new Block64[16, 16, 16];

    public int airCount = 0;
    //public bool boneSpawn, horseSpawn, spawned;
    public static int chunk64Size = 64;
    public bool update = false;
    public bool rendered;
    public bool isSubChunked = false;
    public List<Chunk16> subChunkList = new List<Chunk16>();

    MeshFilter filter;
    MeshCollider coll;

    public World64 world64;
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
    //        UpdateChunk64();
    //    }
    //}

	public Block64 GetBlock64(int x, int y, int z)
	{
        if (InRange(x) && InRange(y) && InRange(z))
            return block64s[x, y, z];
        return world64.GetBlock64(pos.x + (64 * x), pos.y + (64 * y), pos.z + (64 * z));
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= 16)
            return false;

        return true;
    }

    public void SetBlock64(int x, int y, int z, Block64 block64)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            block64s[x, y, z] = block64;
        }
        else
        {
            world64.SetBlock64(pos.x + x, pos.y + y, pos.z + z, block64);
        }
    }

    public void SetBlock64sUnmodified()
    {
        foreach (Block64 block64 in block64s)
        {
            block64.changed = false;
        }
    }

    // Updates the chunk based on its contents
    public void UpdateChunk64()
    {
        rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < 16; x+=1)
        {
            for (int y = 0; y < 16; y+=1)
            {
                for (int z = 0; z < 16; z+=1)
                {
                    meshData = block64s[x, y, z].Blockdata(this, x, y, z, meshData);
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
            foreach (Chunk16 chunk16 in subChunkList)
            {
                chunk16.gameObject.GetComponent<MeshRenderer>().enabled = true;
                chunk16.gameObject.layer = 13;
            }
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.layer = 10;
        }
        else
        {
            LoadChunk16s.terrainLoadManager.GetComponent<LoadChunk16s>().ReplaceChunk64(this, pos);
        }
    }
}