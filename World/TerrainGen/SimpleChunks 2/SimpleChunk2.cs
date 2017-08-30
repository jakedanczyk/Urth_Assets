using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

[Serializable]
public class SimpleChunk2 : MonoBehaviour
{
    public SimpleChunkType simpleChunkType;
    public bool isEmpty;
    public SimpleChunkTextureDictionary textDict;

    public static int chunkSize = 16;
    public bool update = false;
    public bool rendered;

    MeshFilter filter;
    MeshCollider coll;

    public SimpleWorld2 simpleWorld;
    public WorldPos pos;


    //Base constructor
    public SimpleChunk2(SimpleChunkType newSimpleChunkType)
    {
        simpleChunkType = newSimpleChunkType;
    }

    private void Awake()
    {
        if (simpleChunkType == SimpleChunkType.Air)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            this.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }

    void Start()
    {
        if(simpleChunkType == SimpleChunkType.Air)
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            this.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
        filter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }

    //Update is called once per frame
    void Update()
    {
        if (update)
        {
            update = false;
            UpdateChunk();
        }
    }

    public void SetTexture()
    {
        //this.gameObject.GetComponent<Renderer>().material.mainTexture = textureDict[simpleChunkType];
    }

     static bool InRange(int index)
    {
        if (index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    //public void SetBlock(int x, int y, int z, Block block)
    //{
    //    if (InRange(x) && InRange(y) && InRange(z))
    //    {
    //    }
    //    else
    //    {
    //        world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
    //    }
    //}

    // Updates the chunk based on its contents
    void UpdateChunk()
    {
        rendered = true;
        MeshData meshData = new MeshData();

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


    public void SetSimpleChunk(SimpleChunk2 simpleChunk)
    {
        print("1");
        simpleWorld.SetSimpleChunk(pos.x, pos.y, pos.z, simpleChunk);
    }


    // Updates the chunk based on its contents

    public enum Direction { north, east, south, west, up, down };

    public struct Tile { public int x; public int y; }
    const float tileSize = .25f;




    //public virtual MeshData SimpleChunkData
    // (SimpleChunk simpleChunk, int x, int y, int z, MeshData meshData)
    //{

    //    meshData.useRenderDataForCol = true;

    //    if (!simpleChunk.GetSimpleChunk(x, y+1, z).IsSolid(Direction.down))
    //    {
    //        meshData = FaceDataUp(simpleChunk, x, y, z, meshData);
    //    }

    //    if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up))
    //    {
    //        meshData = FaceDataDown(chunk, x, y, z, meshData);
    //    }

    //    if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south))
    //    {
    //        meshData = FaceDataNorth(chunk, x, y, z, meshData);
    //    }

    //    if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north))
    //    {
    //        meshData = FaceDataSouth(chunk, x, y, z, meshData);
    //    }

    //    if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west))
    //    {
    //        meshData = FaceDataEast(chunk, x, y, z, meshData);
    //    }

    //    if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east))
    //    {
    //        meshData = FaceDataWest(chunk, x, y, z, meshData);
    //    }

    //    return meshData;

    //}

    protected virtual MeshData FaceDataUp
        (SimpleChunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - .5f, y + .5f, z + .5f));
        meshData.AddVertex(new Vector3(x + .5f, y + .5f, z + .5f));
        meshData.AddVertex(new Vector3(x + .5f, y + .5f, z - .5f));
        meshData.AddVertex(new Vector3(x - .5f, y + .5f, z - .5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected virtual MeshData FaceDataDown
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - .5f, y - .5f, z - .5f));
        meshData.AddVertex(new Vector3(x + .5f, y - .5f, z - .5f));
        meshData.AddVertex(new Vector3(x + .5f, y - .5f, z + .5f));
        meshData.AddVertex(new Vector3(x - .5f, y - .5f, z + .5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + .5f, y - .5f, z + .5f));
        meshData.AddVertex(new Vector3(x + .5f, y + .5f, z + .5f));
        meshData.AddVertex(new Vector3(x - .5f, y + .5f, z + .5f));
        meshData.AddVertex(new Vector3(x - .5f, y - .5f, z + .5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + .5f, y - .5f, z - .5f));
        meshData.AddVertex(new Vector3(x + .5f, y + .5f, z - .5f));
        meshData.AddVertex(new Vector3(x + .5f, y + .5f, z + .5f));
        meshData.AddVertex(new Vector3(x + .5f, y - .5f, z + .5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - .5f, y - .5f, z - .5f));
        meshData.AddVertex(new Vector3(x - .5f, y + .5f, z - .5f));
        meshData.AddVertex(new Vector3(x + .5f, y + .5f, z - .5f));
        meshData.AddVertex(new Vector3(x + .5f, y - .5f, z - .5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - .5f, y - .5f, z + .5f));
        meshData.AddVertex(new Vector3(x - .5f, y + .5f, z + .5f));
        meshData.AddVertex(new Vector3(x - .5f, y + .5f, z - .5f));
        meshData.AddVertex(new Vector3(x - .5f, y - .5f, z - .5f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.west));
        return meshData;
    }

    public virtual Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();
        tile.x = 0;
        tile.y = 0;

        return tile;
    }

    public virtual Vector2[] FaceUVs(Direction direction)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);

        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y);

        return UVs;
    }

    public virtual bool IsSolid(Direction direction)
    {
        switch (direction)
        {
            case Direction.north:
                return true;
            case Direction.east:
                return true;
            case Direction.south:
                return true;
            case Direction.west:
                return true;
            case Direction.up:
                return true;
            case Direction.down:
                return true;
        }

        return false;
    }

}