using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block256GrassRiverValley : Block256
{


    public Block256GrassRiverValley()
        : base()
    {
    }

    protected override MeshData FaceDataUp
    (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.StoneHeight256(256 * x - 128f + chunk256.pos.x, 256 * y + chunk256.pos.y, 256 * z + 128f + chunk256.pos.z) - chunk256.pos.y;
        float ne = terrainGen.StoneHeight256(256 * x + 128f + chunk256.pos.x, 256 * y + chunk256.pos.y, 256 * z + 128f + chunk256.pos.z) - chunk256.pos.y;
        float se = terrainGen.StoneHeight256(256 * x + 128f + chunk256.pos.x, 256 * y + chunk256.pos.y, 256 * z - 128f + chunk256.pos.z) - chunk256.pos.y;
        float sw = terrainGen.StoneHeight256(256 * x - 128f + chunk256.pos.x, 256 * y + chunk256.pos.y, 256 * z - 128f + chunk256.pos.z) - chunk256.pos.y;

        meshData.AddVertex(new Vector3(256f * x - 128f, nw - 16, 256 * z + 128f));
        meshData.AddVertex(new Vector3(256f * x + 128f, ne - 16, 256 * z + 128f));
        meshData.AddVertex(new Vector3(256f * x + 128f, se - 16, 256 * z - 128f));
        meshData.AddVertex(new Vector3(256f * x - 128f, sw - 16, 256 * z - 128f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected override MeshData FaceDataNorth
       (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        float bottom = 256* y - 128f;
        var terrainGen = new TerrainGen();
        float nw = terrainGen.StoneHeight256(256* x - 128f + chunk256.pos.x, 256* y + chunk256.pos.y, 256* z + 128f + chunk256.pos.z) - chunk256.pos.y;
        float ne = terrainGen.StoneHeight256(256* x + 128f + chunk256.pos.x, 256* y + chunk256.pos.y, 256* z + 128f + chunk256.pos.z) - chunk256.pos.y;

        if (bottom > ne)
        {
            meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
            if (bottom > nw)
            {
                meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
                meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.north));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
                meshData.AddVertex(new Vector3(256f * x - 128f, bottom, 256* z + 128f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.north));
                return meshData;
            }
        }
        else if (bottom > nw)
        {
            meshData.AddVertex(new Vector3(256f * x + 128f, bottom, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.north));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(256f * x + 128f, bottom, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, bottom, 256* z + 128f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.north));
            return meshData;
        }
    }

    protected override MeshData FaceDataEast
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float ne = terrainGen.StoneHeight256(256* x + 128f + chunk256.pos.x, 256* y + chunk256.pos.y, 256* z + 128f + chunk256.pos.z) - chunk256.pos.y;
        float se = terrainGen.StoneHeight256(256* x + 128f + chunk256.pos.x, 256* y + chunk256.pos.y, 256* z - 128f + chunk256.pos.z) - chunk256.pos.y;
        float bottom = 256* y - 128f;

        if (bottom > se)
        {
            meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
            if (bottom > ne)
            {
                meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
                meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.east));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
                meshData.AddVertex(new Vector3(256f * x + 128f, bottom, 256* z + 128f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.east));
                return meshData;
            }
        }
        else if (bottom > ne)
        {
            meshData.AddVertex(new Vector3(256f * x + 128f, bottom, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.east));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(256f * x + 128f, bottom, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, bottom, 256* z + 128f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.east));
            return meshData;
        }
    }

    protected override MeshData FaceDataSouth
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float se = terrainGen.StoneHeight256(256* x + 128f + chunk256.pos.x, 256* y + chunk256.pos.y, 256* z - 128f + chunk256.pos.z) - chunk256.pos.y;
        float sw = terrainGen.StoneHeight256(256* x - 128f + chunk256.pos.x, 256* y + chunk256.pos.y, 256* z - 128f + chunk256.pos.z) - chunk256.pos.y;
        float bottom = 256* y - 128f;

        if (bottom > se)
        {
            if (bottom > sw)
            {
                meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
                meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
                meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
                meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.south));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(256f * x - 128f, bottom, 256* z - 128f));
                meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
                meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
                meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.south));
                return meshData;
            }
        }
        else if (bottom > sw)
        {
            meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, bottom, 256* z - 128f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.south));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(256f * x - 128f, bottom, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, se, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x + 128f, bottom, 256* z - 128f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.south));
            return meshData;
        }
    }

    protected override MeshData FaceDataWest
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.StoneHeight256(256* x - 128f + chunk256.pos.x, 256* y + chunk256.pos.y, 256* z + 128f + chunk256.pos.z) - chunk256.pos.y;
        float sw = terrainGen.StoneHeight256(256* x - 128f + chunk256.pos.x, 256* y + chunk256.pos.y, 256* z - 128f + chunk256.pos.z) - chunk256.pos.y;
        float bottom = 256* y - 128f;

        if (bottom > nw)
        {
            meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
            if (bottom > sw)
            {
                meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
                meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.west));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
                meshData.AddVertex(new Vector3(256f * x - 128f, bottom, 256* z - 128f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.west));
                return meshData;
            }
        }
        else if (bottom > sw)
        {
            meshData.AddVertex(new Vector3(256f * x - 128f, bottom, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.west));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(256f * x - 128f, bottom, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256* z + 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256* z - 128f));
            meshData.AddVertex(new Vector3(256f * x - 128f, bottom, 256* z - 128f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.west));
            return meshData;
        }
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 1;
                tile.y = 0;
                return tile;
            case Direction.down:
                tile.x = 1;
                tile.y = 0;
                return tile;
        }

        tile.x = 1;
        tile.y = 0;

        return tile;
    }
}