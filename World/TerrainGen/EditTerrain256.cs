using UnityEngine;
using System;

public static class EditTerrain256
{
    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            (int)Math.Floor(pos.x / 256f) * 256,
            (int)Math.Floor(pos.y / 256f) * 256,
            (int)Math.Floor(pos.z / 256f) * 256
            );

        return blockPos;
    }

    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
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
        if ((pos - (256 * (pos / 256)) == 128 || (pos - (256 * (pos / 256)) == -128)))
        {
            if (adjacent)
            {
                pos += (norm * 256);
            }
            else
            {
                pos -= (norm * 256);
            }
        }

        return (float)pos;
    }

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }

    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }

    public static bool HitBlock(RaycastHit hit, int damage, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;
        
        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        block.DamageDurability(damage);
        if (block.GetDurability() < 0)
        {
            GameObject drop = MonoBehaviour.Instantiate( Resources.Load("Item_TerrainBlock_Granite") as GameObject);
            drop.transform.position = hit.point;
            EditTerrain.SetBlock(hit, new BlockAir());
            return true;
        }
        return true;
    }
}