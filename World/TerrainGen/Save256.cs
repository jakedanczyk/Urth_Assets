using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save256
{
    public Dictionary<WorldPos, Block256> blocks = new Dictionary<WorldPos, Block256>();

    public Save256(Chunk256 chunk256)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (!chunk256.block256s[x, y, z].changed)
                        continue;

                    WorldPos pos = new WorldPos(256 * x, 256 * y, 256 * z);
                    blocks.Add(pos, chunk256.block256s[x, y, z]);
                }
            }
        }
    }
}