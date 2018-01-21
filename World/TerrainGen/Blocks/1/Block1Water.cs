using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block1Water : Block1
{

    public int depth = 8;

    public Block1Water()
        : base()
    {

	}

    public override MeshData Blockdata
    (Chunk1 chunk, int x, int y, int z, MeshData meshData)
    {

        meshData.useRenderDataForCol = true;

        if (chunk.GetBlock1(x, y + 1, z) is Block1Water)
        {
            Block1Water bw = (Block1Water)chunk.GetBlock1(x, y + 1, z);
            depth = bw.depth + 1;
        }
        else
        {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }
        //if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.down))
        //{
        //}

        //if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.up))
        //{
        //    meshData = FaceDataDown(chunk, x, y, z, meshData);
        //}

        //if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.south))
        //{
        //    meshData = FaceDataNorth(chunk, x, y, z, meshData);
        //}

        //if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.north))
        //{
        //    meshData = FaceDataSouth(chunk, x, y, z, meshData);
        //}

        //if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.west))
        //{
        //    meshData = FaceDataEast(chunk, x, y, z, meshData);
        //}

        //if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.east))
        //{
        //    meshData = FaceDataWest(chunk, x, y, z, meshData);
        //}

        return meshData;

    }

    protected override MeshData FaceDataUp
        (Chunk1 chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddFluidVertex(new Vector3(x - .5f, y + (.5f * depth / 8), z + .5f));
        meshData.AddFluidVertex(new Vector3(x + .5f, y + (.5f * depth / 8), z + .5f));
        meshData.AddFluidVertex(new Vector3(x + .5f, y + (.5f * depth / 8), z - .5f));
        meshData.AddFluidVertex(new Vector3(x - .5f, y + (.5f * depth / 8), z - .5f));

        meshData.AddQuadFluidTriangles();
        meshData.fluiduv.AddRange(FaceUVs(Direction.up));
        return meshData;
    }

    //protected override MeshData FaceDataDown
    //    (Chunk chunk, int x, int y, int z, MeshData meshData)
    //{
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y - .5f, z - .5f));
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y - .5f, z - .5f));
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y - .5f, z + .5f));
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y - .5f, z + .5f));

    //    meshData.AddQuadFluidTriangles();
    //    meshData.fluiduv.AddRange(FaceUVs(Direction.down));
    //    return meshData;
    //}

    //protected override MeshData FaceDataNorth
    //    (Chunk chunk, int x, int y, int z, MeshData meshData)
    //{
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y - .5f, z + .5f));
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y + .5f, z + .5f));
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y + .5f, z + .5f));
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y - .5f, z + .5f));

    //    meshData.AddQuadFluidTriangles();
    //    meshData.fluiduv.AddRange(FaceUVs(Direction.north));
    //    return meshData;
    //}

    //protected override MeshData FaceDataEast
    //    (Chunk chunk, int x, int y, int z, MeshData meshData)
    //{
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y - .5f, z - .5f));
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y + .5f, z - .5f));
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y + .5f, z + .5f));
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y - .5f, z + .5f));

    //    meshData.AddQuadFluidTriangles();
    //    meshData.fluiduv.AddRange(FaceUVs(Direction.east));
    //    return meshData;
    //}

    //protected override MeshData FaceDataSouth
    //    (Chunk chunk, int x, int y, int z, MeshData meshData)
    //{
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y - .5f, z - .5f));
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y + .5f, z - .5f));
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y + .5f, z - .5f));
    //    meshData.AddFluidVertex(new Vector3(x + .5f, y - .5f, z - .5f));

    //    meshData.AddQuadFluidTriangles();
    //    meshData.fluiduv.AddRange(FaceUVs(Direction.south));
    //    return meshData;
    //}

    //protected override MeshData FaceDataWest
    //    (Chunk chunk, int x, int y, int z, MeshData meshData)
    //{
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y - .5f, z + .5f));
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y + .5f, z + .5f));
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y + .5f, z - .5f));
    //    meshData.AddFluidVertex(new Vector3(x - .5f, y - .5f, z - .5f));

    //    meshData.AddQuadFluidTriangles();
    //    meshData.fluiduv.AddRange(FaceUVs(Direction.west));
    //    return meshData;
    //}


    public override bool IsSolid(Direction direction)
    {
        //switch (direction)
        //{
        //    //case Direction.north:
        //    //    return true;
        //    //case Direction.east:
        //    //    return true;
        //    //case Direction.south:
        //    //    return true;
        //    //case Direction.west:
        //    //    return true;
        //    //case Direction.up:
        //    //    return true;
        //        //case Direction.down:
        //        //    return true;
        //}
        return false;
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 3;
        tile.y = 2;

        return tile;
    }
}