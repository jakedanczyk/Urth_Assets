using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block1
{
    public enum Direction { north, east, south, west, up, down };

    public struct Tile { public int x; public int y;}
    const float tileSize = .25f;
    public bool changed = true;

    public int con;
    public int durability = 1000;

    //Base block constructor
    public Block1()
    {
    }

    public virtual MeshData Blockdata
     (Chunk1 chunk1, int x, int y, int z, MeshData meshData)
    {

        meshData.useRenderDataForCol = true;

        if (!chunk1.GetBlock1(x, y + 1, z).IsSolid(Direction.down))
        {
            meshData = FaceDataUp(chunk1, x, y, z, meshData);
        }

        if (!chunk1.GetBlock1(x, y - 1, z).IsSolid(Direction.up))
        {
            meshData = FaceDataDown(chunk1, x, y, z, meshData);
        }

        if (!chunk1.GetBlock1(x, y, z + 1).IsSolid(Direction.south))
        {
            meshData = FaceDataNorth(chunk1, x, y, z, meshData);
        }

        if (!chunk1.GetBlock1(x, y, z - 1).IsSolid(Direction.north))
        {
            meshData = FaceDataSouth(chunk1, x, y, z, meshData);
        }

        if (!chunk1.GetBlock1(x + 1, y, z).IsSolid(Direction.west))
        {
            meshData = FaceDataEast(chunk1, x, y, z, meshData);
        }

        if (!chunk1.GetBlock1(x - 1, y, z).IsSolid(Direction.east))
        {
            meshData = FaceDataWest(chunk1, x, y, z, meshData);
        }

        return meshData;

    }

    protected virtual MeshData FaceDataUp
        (Chunk1 chunk1, int x, int y, int z, MeshData meshData)
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
        (Chunk1 chunk1, int x, int y, int z, MeshData meshData)
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
        (Chunk1 chunk1, int x, int y, int z, MeshData meshData)
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
        (Chunk1 chunk1, int x, int y, int z, MeshData meshData)
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
        (Chunk1 chunk1, int x, int y, int z, MeshData meshData)
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
        (Chunk1 chunk1, int x, int y, int z, MeshData meshData)
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

	public int Condition (int con, int dam)
	{
		if (dam == 0)
			return con;
		else {
			con = con - dam;
			return con;
		}
	}

    public void DamageDurability(int damage)
    {
        durability -= damage;
    }

    public int GetDurability()
    {
        return durability;
    }
}