using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block4Air : Block4
{
    public Block4Air()
        : base()
    {

	}

    public override MeshData Blockdata
	(Chunk4 chunk4, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }

    public override bool IsSolid(Block4.Direction direction)
    {
        return false;
    }
}