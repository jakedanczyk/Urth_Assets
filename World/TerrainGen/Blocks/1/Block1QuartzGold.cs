using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block1QuartzGold : Block1
{


    public Block1QuartzGold()
        : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 2;
        tile.y = 1;

        return tile;
    }
}