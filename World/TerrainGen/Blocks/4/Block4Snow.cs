using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block4Snow : Block4
{


    public Block4Snow()
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