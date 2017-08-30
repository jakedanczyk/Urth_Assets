using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockGold : Block
{
    public int durability = 100;


    public BlockGold()
        : base()
    {
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 1;
                tile.y = 1;
                return tile;
            case Direction.down:
                tile.x = 1;
                tile.y = 1;
                return tile;
        }

        tile.x = 1;
        tile.y = 1;

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