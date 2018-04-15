using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block1CopperOre : Block1
{


    public Block1CopperOre()
        : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 0;
        tile.y = 1;

        return tile;
    }
}