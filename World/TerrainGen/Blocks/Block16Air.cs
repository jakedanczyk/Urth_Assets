using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block16Air : Block16
{
    public Block16Air()
        : base()
    {

	}

    public override MeshData Blockdata
	(Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }

    public override bool IsSolid(Block16.Direction direction)
    {
        return false;
    }
}