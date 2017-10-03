﻿using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block4
{
    public enum Direction { north, east, south, west, up, down };

    public struct Tile { public int x; public int y;}
    const float tileSize = .25f;
    public bool changed = true;


    //Base block constructor
    public Block4()
    {

    }

    public virtual MeshData Blockdata
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

    protected virtual MeshData FaceDataUp
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y + 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y + 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y + 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y + 2f, 4*z - 2f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected virtual MeshData FaceDataDown
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y - 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y - 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y - 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y - 2f, 4*z + 2f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y - 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y + 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y + 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y - 2f, 4*z + 2f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y - 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y + 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y + 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y - 2f, 4*z + 2f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y - 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y + 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y + 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x + 2f, 4*y - 2f, 4*z - 2f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y - 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y + 2f, 4*z + 2f));
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y + 2f, 4*z - 2f));
        meshData.AddVertex(new Vector3(4f*x - 2f, 4*y - 2f, 4*z - 2f));

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