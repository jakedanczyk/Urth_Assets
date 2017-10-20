using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block16Snow : Block16
{


    public Block16Snow()
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