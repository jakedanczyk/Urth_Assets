using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk4 : MonoBehaviour
{
    public bool hasTree, hasBush;
    public List<Vector3> treeList, boneList, bushList, horseList;
    public Block4[, ,] block4s = new Block4[chunk4Size, chunk4Size, chunk4Size];

    public int airCount = 0;
    public bool boneSpawn, horseSpawn, spawned;
    public static int chunk4Size = 16;
    public bool update = false;
    public bool rendered;

    MeshFilter filter;
    MeshCollider coll;

    public World4 world4;
    public WorldPos pos;

    void Start()
    {
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }

    //Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk4();
        }
    }

	public Block4 GetBlock4(int x, int y, int z)
	{
        if (InRange(x) && InRange(y) && InRange(z))
            return block4s[x, y, z];
        return world4.GetBlock4(pos.x + x, pos.y + y, pos.z + z);
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= chunk4Size)
            return false;

        return true;
    }

    public void SetBlock4(int x, int y, int z, Block4 block4)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            block4s[x, y, z] = block4;
        }
        else
        {
            world4.SetBlock4(pos.x + x, pos.y + y, pos.z + z, block4);
        }
    }

    public void SetBlock4sUnmodified()
    {
        foreach (Block4 block4 in block4s)
        {
            block4.changed = false;
        }
    }

    // Updates the chunk4 based on its contents
    void UpdateChunk4()
    {
        rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < chunk4Size; x+=1)
        {
            for (int y = 0; y < chunk4Size; y+=1)
            {
                for (int z = 0; z < chunk4Size; z+=1)
                {
                    meshData = block4s[x, y, z].Blockdata(this, x, y, z, meshData);
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

}