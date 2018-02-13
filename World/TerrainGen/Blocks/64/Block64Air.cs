using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block64Air : Block64
{
    public Block64Air()
        : base()
    {

	}

    public override MeshData Blockdata
	(Chunk64 chunk64, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }

    public override bool IsSolid(Block64.Direction direction)
    {
        return false;
    }
}