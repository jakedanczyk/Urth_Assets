using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk256 : MonoBehaviour
{
    //public bool hasTree, hasBush;
    //public List<Vector3> treeList, boneList, bushList, horseList;
    public Block256[, ,] block256s = new Block256[16, 16, 16];

    public int airCount = 0;
    //public bool boneSpawn, horseSpawn, spawned;
    public static int chunk256Size = 256;
    public bool update = false;
    public bool rendered;
    public bool isSubChunked = false;
    public List<Chunk64> subChunkList = new List<Chunk64>();

    MeshFilter filter;
    MeshCollider coll;

    public World256 world256;
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
    //        UpdateChunk256();
    //    }
    //}

	public Block256 GetBlock256(int x, int y, int z)
	{
        if (InRange(x) && InRange(y) && InRange(z))
            return block256s[x, y, z];
        return world256.GetBlock256(pos.x + (256 * x), pos.y + (256 * y), pos.z + (256 * z));
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= 16)
            return false;

        return true;
    }

    public void SetBlock256(int x, int y, int z, Block256 block256)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            block256s[x, y, z] = block256;
        }
        else
        {
            world256.SetBlock256(pos.x + x, pos.y + y, pos.z + z, block256);
        }
    }

    public void SetBlock256sUnmodified()
    {
        foreach (Block256 block256 in block256s)
        {
            block256.changed = false;
        }
    }

    // Updates the chunk based on its contents
    public void UpdateChunk256()
    {
        rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < 16; x+=1)
        {
            for (int y = 0; y < 16; y+=1)
            {
                for (int z = 0; z < 16; z+=1)
                {
                    meshData = block256s[x, y, z].Blockdata(this, x, y, z, meshData);
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
            foreach (Chunk64 chunk64 in subChunkList)
            {
                chunk64.gameObject.GetComponent<MeshRenderer>().enabled = true;
                chunk64.gameObject.layer = 11;
            }
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.layer = 8;
        }
        else
        {
            LoadChunk64s.terrainLoadManager.GetComponent<LoadChunk64s>().ReplaceChunk256(this, pos);
        }
    }
}