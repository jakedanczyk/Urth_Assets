using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block256Air : Block256
{
    public Block256Air()
        : base()
    {

	}

    public override MeshData Blockdata
	(Chunk256 chunk256, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }

    public override bool IsSolid(Block256.Direction direction)
    {
        return false;
    }
}