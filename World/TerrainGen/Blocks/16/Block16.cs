using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block16
{
    public enum Direction { north, east, south, west, up, down };

    public struct Tile { public int x; public int y;}
    const float tileSize = .25f;
    public bool changed = true, exposed = false, u = false, d = false, n = false, s = false, e = false, w = false;

    //Base block constructor
    public Block16()
    {

    }

    public virtual MeshData Blockdata(Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;
        if (!chunk16.isWalkable)
        {
            if (!chunk16.GetBlock16(x, y + 1, z).IsSolid(Direction.down))
            {
                exposed = true;
                u = true;
                chunk16.isWalkable = true;
                meshData = FaceDataUp(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y - 1, z).IsSolid(Direction.up))
            {
                exposed = true;
                d = true;
                meshData = FaceDataDown(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y, z + 1).IsSolid(Direction.south))
            {
                exposed = true;
                n = true;
                chunk16.isWalkable = true;
                meshData = FaceDataNorth(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y, z - 1).IsSolid(Direction.north))
            {
                s = true;
                chunk16.isWalkable = true;
                meshData = FaceDataSouth(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x + 1, y, z).IsSolid(Direction.west))
            {
                e = true;
                chunk16.isWalkable = true;
                meshData = FaceDataEast(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x - 1, y, z).IsSolid(Direction.east))
            {
                exposed = true;
                w = true;
                chunk16.isWalkable = true;
                meshData = FaceDataWest(chunk16, x, y, z, meshData);
            }
        }
        else
        {
            meshData.useRenderDataForCol = true;

            if (!chunk16.GetBlock16(x, y + 1, z).IsSolid(Direction.down))
            {
                exposed = true;
                u = true;
                meshData = FaceDataUp(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y - 1, z).IsSolid(Direction.up))
            {
                exposed = true;
                d = true;
                meshData = FaceDataDown(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y, z + 1).IsSolid(Direction.south))
            {
                exposed = true;
                n = true;
                meshData = FaceDataNorth(chunk16, x, y, z, meshData);
            }

            if (!chunk16.GetBlock16(x, y, z - 1).IsSolid(Direction.north))
            {
                exposed = true;
                s = true;
                meshData = FaceDataSouth(chunk16, x, y, z, meshData);
            }

            try
            {
                if (!chunk16.GetBlock16(x + 1, y, z).IsSolid(Direction.west))
                {
                    exposed = true;
                    e = true;
                    meshData = FaceDataEast(chunk16, x, y, z, meshData);
                }
            }
            catch
            {
                Console.WriteLine("weirdness");
            }

            if (!chunk16.GetBlock16(x - 1, y, z).IsSolid(Direction.east))
            {
                exposed = true;
                w = true;
                meshData = FaceDataWest(chunk16, x, y, z, meshData);
            }
        }
        return meshData;
    }

    protected virtual MeshData FaceDataUp
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y + 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y + 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y + 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y + 8f, 16*z - 8f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    protected virtual MeshData FaceDataDown
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y - 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y - 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y - 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y - 8f, 16*z + 8f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    protected virtual MeshData FaceDataNorth
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y - 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y + 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y + 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y - 8f, 16*z + 8f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.north));
        return meshData;
    }

    protected virtual MeshData FaceDataEast
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y - 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y + 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y + 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y - 8f, 16*z + 8f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.east));
        return meshData;
    }

    protected virtual MeshData FaceDataSouth
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y - 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y + 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y + 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x + 8f, 16*y - 8f, 16*z - 8f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.south));
        return meshData;
    }

    protected virtual MeshData FaceDataWest
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y - 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y + 8f, 16*z + 8f));
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y + 8f, 16*z - 8f));
        meshData.AddVertex(new Vector3(16f*x - 8f, 16*y - 8f, 16*z - 8f));

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