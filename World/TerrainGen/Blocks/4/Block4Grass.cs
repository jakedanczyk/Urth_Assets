using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block4Grass : Block4
{
    public Block4Grass()
        : base()
    {
    }

    public override MeshData Blockdata
(Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {

        meshData.useRenderDataForCol = true;

        if (!chunk4.GetBlock4(x, y + 1, z).IsSolid(Direction.down))
        {
            meshData = FaceDataUp(chunk4, x, y, z, meshData);
        }

        if (!chunk4.GetBlock4(x, y - 1, z).IsSolid(Direction.up))
        {
            meshData = FaceDataDown(chunk4, x, y, z, meshData);
        }

        if (!chunk4.GetBlock4(x, y, z + 1).IsSolid(Direction.south))
        {
            meshData = FaceDataNorth(chunk4, x, y, z, meshData);
        }

        if (!chunk4.GetBlock4(x, y, z - 1).IsSolid(Direction.north))
        {
            meshData = FaceDataSouth(chunk4, x, y, z, meshData);
        }

        if (!chunk4.GetBlock4(x + 1, y, z).IsSolid(Direction.west))
        {
            meshData = FaceDataEast(chunk4, x, y, z, meshData);
        }

        if (!chunk4.GetBlock4(x - 1, y, z).IsSolid(Direction.east))
        {
            meshData = FaceDataWest(chunk4, x, y, z, meshData);
        }

        return meshData;

    }

    protected override MeshData FaceDataUp
    (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.DirtHeight(4 * x - 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z + 2f + chunk4.pos.z) - chunk4.pos.y;
        float ne = terrainGen.DirtHeight(4 * x + 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z + 2f + chunk4.pos.z) - chunk4.pos.y;
        float se = terrainGen.DirtHeight(4 * x + 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z - 2f + chunk4.pos.z) - chunk4.pos.y;
        float sw = terrainGen.DirtHeight(4 * x - 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z - 2f + chunk4.pos.z) - chunk4.pos.y;

        meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
        meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
        meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
        meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected override MeshData FaceDataDown
    (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        float bottom = 4 * y - 2f;
        var terrainGen = new TerrainGen();
        float nw = terrainGen.StoneHeight(4 * x - 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z + 2f + chunk4.pos.z) - chunk4.pos.y;
        float ne = terrainGen.StoneHeight(4 * x + 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z + 2f + chunk4.pos.z) - chunk4.pos.y;
        float se = terrainGen.StoneHeight(4 * x + 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z - 2f + chunk4.pos.z) - chunk4.pos.y;
        float sw = terrainGen.StoneHeight(4 * x - 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z - 2f + chunk4.pos.z) - chunk4.pos.y;

        meshData.AddVertex(new Vector3(4f * x - 2f, sw - .1f, 4 * z - 2f));
        meshData.AddVertex(new Vector3(4f * x + 2f, se - .1f, 4 * z - 2f));
        meshData.AddVertex(new Vector3(4f * x + 2f, ne - .1f, 4 * z + 2f));
        meshData.AddVertex(new Vector3(4f * x - 2f, nw - .1f, 4 * z + 2f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }


    protected override MeshData FaceDataNorth
       (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        float bottom = 4 * y - 2f;
        var terrainGen = new TerrainGen();
        float nw = terrainGen.DirtHeight(4 * x - 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z + 2f + chunk4.pos.z) - chunk4.pos.y;
        float ne = terrainGen.DirtHeight(4 * x + 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z + 2f + chunk4.pos.z) - chunk4.pos.y;

        if (bottom > ne)
        {
            meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
            if (bottom > nw)
            {
                meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
                meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.north));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
                meshData.AddVertex(new Vector3(4f * x - 2f, bottom, 4 * z + 2f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.north));
                return meshData;
            }
        }
        else if (bottom > nw)
        {
            meshData.AddVertex(new Vector3(4f * x + 2f, bottom, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.north));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(4f * x + 2f, bottom, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, bottom, 4 * z + 2f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.north));
            return meshData;
        }
    }

    protected override MeshData FaceDataEast
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float ne = terrainGen.DirtHeight(4 * x + 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z + 2f + chunk4.pos.z) - chunk4.pos.y;
        float se = terrainGen.DirtHeight(4 * x + 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z - 2f + chunk4.pos.z) - chunk4.pos.y;
        float bottom = 4 * y - 2f;

        if (bottom > se)
        {
            meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
            if (bottom > ne)
            {
                meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
                meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.east));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
                meshData.AddVertex(new Vector3(4f * x + 2f, bottom, 4 * z + 2f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.east));
                return meshData;
            }
        }
        else if (bottom > ne)
        {
            meshData.AddVertex(new Vector3(4f * x + 2f, bottom, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.east));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(4f * x + 2f, bottom, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, ne, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, bottom, 4 * z + 2f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.east));
            return meshData;
        }
    }

    protected override MeshData FaceDataSouth
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float se = terrainGen.DirtHeight(4 * x + 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z - 2f + chunk4.pos.z) - chunk4.pos.y;
        float sw = terrainGen.DirtHeight(4 * x - 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z - 2f + chunk4.pos.z) - chunk4.pos.y;
        float bottom = 4 * y - 2f;

        if (bottom > se)
        {
            if (bottom > sw)
            {
                meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
                meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
                meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
                meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.south));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(4f * x - 2f, bottom, 4 * z - 2f));
                meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
                meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
                meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.south));
                return meshData;
            }
        }
        else if (bottom > sw)
        {
            meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, bottom, 4 * z - 2f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.south));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(4f * x - 2f, bottom, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, se, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x + 2f, bottom, 4 * z - 2f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.south));
            return meshData;
        }
    }

    protected override MeshData FaceDataWest
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.DirtHeight(4 * x - 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z + 2f + chunk4.pos.z) - chunk4.pos.y;
        float sw = terrainGen.DirtHeight(4 * x - 2f + chunk4.pos.x, 4 * y + chunk4.pos.y, 4 * z - 2f + chunk4.pos.z) - chunk4.pos.y;
        float bottom = 4 * y - 2f;

        if (bottom > nw)
        {
            meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
            if (bottom > sw)
            {
                meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
                meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.west));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
                meshData.AddVertex(new Vector3(4f * x - 2f, bottom, 4 * z - 2f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.west));
                return meshData;
            }
        }
        else if (bottom > sw)
        {
            meshData.AddVertex(new Vector3(4f * x - 2f, bottom, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.west));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(4f * x - 2f, bottom, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, nw, 4 * z + 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, sw, 4 * z - 2f));
            meshData.AddVertex(new Vector3(4f * x - 2f, bottom, 4 * z - 2f));
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
                tile.x = 2;
                tile.y = 0;
                return tile;
            case Direction.down:
                tile.x = 2;
                tile.y = 0;
                return tile;
        }

        tile.x = 2;
        tile.y = 0;

        return tile;
    }
}