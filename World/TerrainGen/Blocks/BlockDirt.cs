using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockDirt : Block
{
    public int durability = 1000;

    //Base block constructor
    public BlockDirt() : base()
    {
    }

    public virtual Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 1;
                tile.y = 0;
                return tile;
            case Direction.down:
                tile.x = 1;
                tile.y = 0;
                return tile;
        }

        tile.x = 1;
        tile.y = 0;

        return tile;
    }

    public void DamageDurability(int damage)
    {
        durability -= damage;
    }

    public int GetDurability()
    {
        return durability;
    }
}