using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save16
{
    public Dictionary<WorldPos, Block16> blocks = new Dictionary<WorldPos, Block16>();

    public Save16(Chunk16 chunk16)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (!chunk16.block16s[x, y, z].changed)
                        continue;

                    WorldPos pos = new WorldPos(16*x, 16*y, 16*z);
                    blocks.Add(pos, chunk16.block16s[x, y, z]);
                }
            }
        }
    }
}