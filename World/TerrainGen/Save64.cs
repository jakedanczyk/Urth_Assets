using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save64
{
    public Dictionary<WorldPos, Block64> blocks = new Dictionary<WorldPos, Block64>();

    public Save64(Chunk64 chunk64)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (!chunk64.block64s[x, y, z].changed)
                        continue;

                    WorldPos pos = new WorldPos(64*x, 64*y, 64*z);
                    blocks.Add(pos, chunk64.block64s[x, y, z]);
                }
            }
        }
    }
}