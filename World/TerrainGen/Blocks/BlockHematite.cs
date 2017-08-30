using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockHematite : Block
{
    public int durability = 100;


    public BlockHematite()
        : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 0;
                tile.y = 2;
                return tile;
            case Direction.down:
                tile.x = 0;
                tile.y = 2;
                return tile;
        }

        tile.x = 0;
        tile.y = 2;

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