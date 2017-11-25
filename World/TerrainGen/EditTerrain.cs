using UnityEngine;
using System.Collections;
using System;

public static class EditTerrain
{
    public static WorldPosFloat GetBlockPos(Vector3 pos)
    {
        WorldPosFloat blockPos = new WorldPosFloat(
            (float)Math.Round(Math.Round(pos.x * 4) / 4f, 3),
            (float)Math.Round(Math.Round(pos.y * 4) / 4f, 3),
            (float)Math.Round(Math.Round(pos.z * 4) / 4f, 3)
            );

        return blockPos;
    }

    public static WorldPosFloat GetBlockPos(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return GetBlockPos(pos);
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - Math.Round(Math.Round(pos*4)/4f,3) == 0.125f || pos - Math.Round(Math.Round(pos * 4) / 4f, 3) == -0.125f)
        {
            if (adjacent)
            {
                pos += (norm / 8);
            }
            else
            {
                pos -= (norm / 8);
            }
        }

        return (float)pos;
    }

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPosFloat pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlockEdit(pos.x, pos.y, pos.z, block);

        return true;
    }

    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPosFloat pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }

    public static bool HitBlock(RaycastHit hit, int damage, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;
        
        WorldPosFloat pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        block.DamageDurability(damage);
        if (block.GetDurability() < 0)
        {
            if (block is BlockSnow)
            {
                GameObject drop = MonoBehaviour.Instantiate(Resources.Load("Item_TerrainBlock_Snow") as GameObject);
                drop.transform.position = hit.point;
            }
            else if (block is BlockGrass)
            {
                GameObject drop = MonoBehaviour.Instantiate(Resources.Load("Item_TerrainBlock_Grass") as GameObject);
                drop.transform.position = hit.point;
            }
            else if (block is BlockQuartzGold)
            {
                GameObject drop = MonoBehaviour.Instantiate(Resources.Load("Item_TerrainBlock_QuartzGold") as GameObject);
                drop.transform.position = hit.point;
            }
            else if (block is BlockDirt)
            {
                GameObject drop = MonoBehaviour.Instantiate(Resources.Load("Item_TerrainBlock_Dirt") as GameObject);
                drop.transform.position = hit.point;
            }
            else
            {
                GameObject drop = MonoBehaviour.Instantiate(Resources.Load("Item_TerrainBlock_Granite") as GameObject);
                drop.transform.position = hit.point;
            }
            EditTerrain.SetBlock(hit, new BlockAir());
            return true;
        }
        return true;
    }
}