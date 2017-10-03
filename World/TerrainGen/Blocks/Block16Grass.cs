using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block16Grass : Block16
{


    public Block16Grass()
        : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 2;
                tile.y = 0;
                return tile;
            case Direction.down:
                tile.x = 2;
                tile.y = 0;
                return tile;
        }

        tile.x = 2;
        tile.y = 0;

        return tile;
    }
}