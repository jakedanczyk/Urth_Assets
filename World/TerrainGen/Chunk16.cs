using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk16 : MonoBehaviour
{
    //public bool hasTree, hasBush;
    //public List<Vector3> treeList, boneList, bushList, horseList;
    public Block16[, ,] block16s = new Block16[16, 16, 16];

    public int airCount = 0;
    //public bool boneSpawn, horseSpawn, spawned;
    public static int chunk16Size = 16;
    public bool update = false;
    public bool rendered;

    MeshFilter filter;
    MeshCollider coll;

    public World16 world16;
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
            UpdateChunk16();
        }
    }

	public Block16 GetBlock16(int x, int y, int z)
	{
        if (InRange(x) && InRange(y) && InRange(z))
            return block16s[x, y, z];
        return world16.GetBlock16(pos.x + x, pos.y + y, pos.z + z);
    }

    public static bool InRange(int index)
    {
        if (index < 0 || index >= 16)
            return false;

        return true;
    }

    public void SetBlock16(int x, int y, int z, Block16 block16)
    {
        if (InRange(x) && InRange(y) && InRange(z))
        {
            block16s[x, y, z] = block16;
        }
        else
        {
            world16.SetBlock16(pos.x + x, pos.y + y, pos.z + z, block16);
        }
    }

    public void SetBlock16sUnmodified()
    {
        foreach (Block16 block16 in block16s)
        {
            block16.changed = false;
        }
    }

    // Updates the chunk based on its contents
    void UpdateChunk16()
    {
        rendered = true;
        MeshData meshData = new MeshData();

        for (int x = 0; x < chunk16Size; x+=1)
        {
            for (int y = 0; y < chunk16Size; y+=1)
            {
                for (int z = 0; z < chunk16Size; z+=1)
                {
                    meshData = block16s[x, y, z].Blockdata(this, x, y, z, meshData);
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