using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save1
{
    public Dictionary<WorldPos, Block1> block1s = new Dictionary<WorldPos, Block1>();

    public Save1(Chunk1 chunk1)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (!chunk1.block1s[x, y, z].changed)
                        continue;

                    WorldPos pos = new WorldPos(x, y, z);
                    block1s.Add(pos, chunk1.block1s[x, y, z]);
                }
            }
        }
    }
}