using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save
{
    public Dictionary<WorldPosFloat, Block> blocks = new Dictionary<WorldPosFloat, Block>();

    public Save(Chunk chunk)
    {
        for (int x = 0; x < Chunk.chunkSize; x++)
        {
            for (int y = 0; y < Chunk.chunkSize; y++)
            {
                for (int z = 0; z < Chunk.chunkSize; z++)
                {
                    if (!chunk.blocks[x, y, z].changed)
                        continue;

                    WorldPosFloat pos = new WorldPosFloat(.25f*x, .25f * y, z * .25f);
                    blocks.Add(pos, chunk.blocks[x, y, z]);
                }
            }
        }
    }
}