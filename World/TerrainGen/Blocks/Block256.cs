using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block256
{
    public enum Direction { north, east, south, west, up, down };

    public struct Tile { public int x; public int y;}
    const float tileSize = .25f;
    public bool changed = true;


    //Base block constructor
    public Block256()
    {

    }

    public virtual MeshData Blockdata
     (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {

        meshData.useRenderDataForCol = true;

        if (!chunk256.GetBlock256(x, y + 1, z).IsSolid(Direction.down))
        {
            meshData = FaceDataUp(chunk256, x, y, z, meshData);
        }

        if (!chunk256.GetBlock256(x, y - 1, z).IsSolid(Direction.up))
        {
            meshData = FaceDataDown(chunk256, x, y, z, meshData);
        }

        if (!chunk256.GetBlock256(x, y, z + 1).IsSolid(Direction.south))
        {
            meshData = FaceDataNorth(chunk256, x, y, z, meshData);
        }

        if (!chunk256.GetBlock256(x, y, z - 1).IsSolid(Direction.north))
        {
            meshData = FaceDataSouth(chunk256, x, y, z, meshData);
        }

        if (!chunk256.GetBlock256(x + 1, y, z).IsSolid(Direction.west))
        {
            meshData = FaceDataEast(chunk256, x, y, z, meshData);
        }

        if (!chunk256.GetBlock256(x - 1, y, z).IsSolid(Direction.east))
        {
            meshData = FaceDataWest(chunk256, x, y, z, meshData);
        }

        return meshData;

    }

    protected virtual MeshData FaceDataUp
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y + 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y + 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y + 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y + 128f, 256*z - 128f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected virtual MeshData FaceDataDown
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y - 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y - 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y - 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y - 128f, 256*z + 128f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y - 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y + 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y + 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y - 128f, 256*z + 128f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y - 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y + 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y + 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y - 128f, 256*z + 128f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y - 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y + 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y + 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x + 128f, 256*y - 128f, 256*z - 128f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y - 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y + 128f, 256*z + 128f));
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y + 128f, 256*z - 128f));
        meshData.AddVertex(new Vector3(256f*x - 128f, 256*y - 128f, 256*z - 128f));

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

	public int Condition (int con, int dam)
	{
		if (dam == 0)
			return con;
		else {
			con = con - dam;
			return con;
		}
	}
}