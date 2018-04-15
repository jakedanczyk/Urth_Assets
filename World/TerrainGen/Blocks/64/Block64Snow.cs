using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block64Snow : Block64
{


    public Block64Snow()
        : base()
    {
    }

    protected override MeshData FaceDataUp
    (Chunk64 chunk64, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        terrainGen.NoiseGen(64 * x - 32f + chunk64.pos.x, 64 * y + chunk64.pos.y, 64 * z + 32f + chunk64.pos.z, 64);
        float nw = terrainGen.SnowHeight - chunk64.pos.y;
        terrainGen.NoiseGen(64 * x + 32f + chunk64.pos.x, 64 * y + chunk64.pos.y, 64 * z + 32f + chunk64.pos.z, 64);
        float ne = terrainGen.SnowHeight - chunk64.pos.y;
        terrainGen.NoiseGen(64 * x + 32f + chunk64.pos.x, 64 * y + chunk64.pos.y, 64 * z - 32f + chunk64.pos.z, 64);
        float se = terrainGen.SnowHeight - chunk64.pos.y;
        terrainGen.NoiseGen(64 * x - 32f + chunk64.pos.x, 64 * y + chunk64.pos.y, 64 * z - 32f + chunk64.pos.z, 64);
        float sw = terrainGen.SnowHeight - chunk64.pos.y;

        meshData.AddVertex(new Vector3(64f * x - 32f, nw, 64 * z + 32f));
        meshData.AddVertex(new Vector3(64f * x + 32f, ne, 64 * z + 32f));
        meshData.AddVertex(new Vector3(64f * x + 32f, se, 64 * z - 32f));
        meshData.AddVertex(new Vector3(64f * x - 32f, sw, 64 * z - 32f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 2;
        tile.y = 2;

        return tile;
    }
}