using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockSnow : Block
{


    public BlockSnow()
        : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 2;
        tile.y = 2;

        return tile;
    }
}