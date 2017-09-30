using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Save4
{
    public Dictionary<WorldPos, Block4> block4s = new Dictionary<WorldPos, Block4>();

    public Save4(Chunk4 chunk4)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (!chunk4.block4s[x, y, z].changed)
                        continue;

                    WorldPos pos = new WorldPos(x, y, z);
                    block4s.Add(pos, chunk4.block4s[x, y, z]);
                }
            }
        }
    }
}