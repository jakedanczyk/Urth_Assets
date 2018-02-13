using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block256Snow : Block256
{


    public Block256Snow()
        : base()
    {
    }

    protected override MeshData FaceDataUp
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.SnowHeight256(256 * x - 128f + chunk256.pos.x, 256 * y + chunk256.pos.y, 256 * z + 128f + chunk256.pos.z) - chunk256.pos.y;
        float ne = terrainGen.SnowHeight256(256 * x + 128f + chunk256.pos.x, 256 * y + chunk256.pos.y, 256 * z + 128f + chunk256.pos.z) - chunk256.pos.y;
        float se = terrainGen.SnowHeight256(256 * x + 128f + chunk256.pos.x, 256 * y + chunk256.pos.y, 256 * z - 128f + chunk256.pos.z) - chunk256.pos.y;
        float sw = terrainGen.SnowHeight256(256 * x - 128f + chunk256.pos.x, 256 * y + chunk256.pos.y, 256 * z - 128f + chunk256.pos.z) - chunk256.pos.y;

        meshData.AddVertex(new Vector3(256f * x - 128f, nw, 256 * z + 128f));
        meshData.AddVertex(new Vector3(256f * x + 128f, ne, 256 * z + 128f));
        meshData.AddVertex(new Vector3(256f * x + 128f, se, 256 * z - 128f));
        meshData.AddVertex(new Vector3(256f * x - 128f, sw, 256 * z - 128f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 1;
        tile.y = 1;

        return tile;
    }
}