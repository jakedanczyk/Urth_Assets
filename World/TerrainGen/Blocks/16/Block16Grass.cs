using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block16Grass : Block16
{


    public Block16Grass()
        : base()
    {
    }

    public override MeshData Blockdata (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;
        if (!chunk16.isWalkable)
        {
            if (!chunk16.GetBlock16(x, y + 1, z).IsSolid(Direction.down))
            {
                chunk16.isWalkable = true;
                meshData = FaceDataUp(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y - 1, z).IsSolid(Direction.up))
            {
                meshData = FaceDataDown(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y, z + 1).IsSolid(Direction.south))
            {
                chunk16.isWalkable = true;
                meshData = FaceDataNorth(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y, z - 1).IsSolid(Direction.north))
            {
                chunk16.isWalkable = true;
                meshData = FaceDataSouth(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x + 1, y, z).IsSolid(Direction.west))
            {
                chunk16.isWalkable = true;
                meshData = FaceDataEast(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x - 1, y, z).IsSolid(Direction.east))
            {
                chunk16.isWalkable = true;
                meshData = FaceDataWest(chunk16, x, y, z, meshData);
            }
        }
        else
        {
            if (!chunk16.GetBlock16(x, y + 1, z).IsSolid(Direction.down))
            {
                meshData = FaceDataUp(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y - 1, z).IsSolid(Direction.up))
            {
                meshData = FaceDataDown(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y, z + 1).IsSolid(Direction.south))
            {
                meshData = FaceDataNorth(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y, z - 1).IsSolid(Direction.north))
            {
                meshData = FaceDataSouth(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x + 1, y, z).IsSolid(Direction.west))
            {
                meshData = FaceDataEast(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x - 1, y, z).IsSolid(Direction.east))
            {
                meshData = FaceDataWest(chunk16, x, y, z, meshData);
            }
        }
        return meshData;

    }

    protected override MeshData FaceDataUp
    (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float sw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;

        meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected override MeshData FaceDataDown
    (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float sw = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;

        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    protected override MeshData FaceDataNorth
     (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float neLow = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float nw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float nwLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;

        meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;

        //if (bottom > ne)
        //{
        //    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        //    if (bottom > nw)
        //    {
        //        meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //        meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
        //        meshData.AddQuadTriangles();
        //        meshData.uv.AddRange(FaceUVs(Direction.north));
        //        return meshData;
        //    }
        //    else
        //    {
        //        meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //        meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
        //        meshData.AddQuadTriangles();
        //        meshData.uv.AddRange(FaceUVs(Direction.north));
        //        return meshData;
        //    }
        //}
        //else if (bottom > nw)
        //{
        //    meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //    meshData.AddQuadTriangles();
        //    meshData.uv.AddRange(FaceUVs(Direction.north));
        //    return meshData;
        //}
        //else
        //{
        //    meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
        //    meshData.AddQuadTriangles();
        //    meshData.uv.AddRange(FaceUVs(Direction.north));
        //    return meshData;
        //}
    }

    protected override MeshData FaceDataEast
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float seLow = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float neLow = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;

        meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;

        //if (bottom > se)
        //{
        //    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //    if (bottom > ne)
        //    {
        //        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //        meshData.AddQuadTriangles();
        //        meshData.uv.AddRange(FaceUVs(Direction.east));
        //        return meshData;
        //    }
        //    else
        //    {
        //        meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        //        meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
        //        meshData.AddQuadTriangles();
        //        meshData.uv.AddRange(FaceUVs(Direction.east));
        //        return meshData;
        //    }
        //}
        //else if (bottom > ne)
        //{
        //    meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        //    meshData.AddQuadTriangles();
        //    meshData.uv.AddRange(FaceUVs(Direction.east));
        //    return meshData;
        //}
        //else
        //{
        //    meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
        //    meshData.AddQuadTriangles();
        //    meshData.uv.AddRange(FaceUVs(Direction.east));
        //    return meshData;
        //}
    }

    protected override MeshData FaceDataSouth
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float swLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float sw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float seLow = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;

        meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
        //if (bottom > se)
        //{
        //    if (bottom > sw)
        //    {
        //        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //        meshData.AddQuadTriangles();
        //        meshData.uv.AddRange(FaceUVs(Direction.south));
        //        return meshData;
        //    }
        //    else
        //    {
        //        meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //        meshData.AddQuadTriangles();
        //        meshData.uv.AddRange(FaceUVs(Direction.south));
        //        return meshData;
        //    }
        //}
        //else if (bottom > sw)
        //{
        //    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
        //    meshData.AddQuadTriangles();
        //    meshData.uv.AddRange(FaceUVs(Direction.south));
        //    return meshData;
        //}
        //else
        //{
        //    meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
        //    meshData.AddQuadTriangles();
        //    meshData.uv.AddRange(FaceUVs(Direction.south));
        //    return meshData;
        //}
    }

    protected override MeshData FaceDataWest
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nwLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float nw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float sw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float swLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float bottom = 16 * y - 8f;

        meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.west));
        return meshData;

        //if (bottom > nw)
        //{
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //    if (bottom > sw)
        //    {
        //        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //        meshData.AddQuadTriangles();
        //        meshData.uv.AddRange(FaceUVs(Direction.west));
        //        return meshData;
        //    }
        //    else
        //    {
        //        meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //        meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
        //        meshData.AddQuadTriangles();
        //        meshData.uv.AddRange(FaceUVs(Direction.west));
        //        return meshData;
        //    }
        //}
        //else if (bottom > sw)
        //{
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //    meshData.AddQuadTriangles();
        //    meshData.uv.AddRange(FaceUVs(Direction.west));
        //    return meshData;
        //}
        //else
        //{
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
        //    meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
        //    meshData.AddQuadTriangles();
        //    meshData.uv.AddRange(FaceUVs(Direction.west));
        //    return meshData;
        //}
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