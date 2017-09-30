using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block1Air : Block1
{
    public Block1Air()
        : base()
    {

	}

    public override MeshData Blockdata
	(Chunk1 chunk1, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }

    public override bool IsSolid(Block1.Direction direction)
    {
        return false;
    }
}