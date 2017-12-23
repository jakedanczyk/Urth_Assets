using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Serialization;

[Serializable]
public class Save
{
    public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>();
    //List<WorldPosFloat> posList = new List<WorldPosFloat>();
    //List<Block> blockList = new List<Block>();
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

                    WorldPos pos = new WorldPos(x, y, z );
                    blocks.Add(pos, chunk.blocks[x, y, z]);
                }
            }
        }
    }

    //private void Serialize()
    //{
    //    UnitySerializer.CreateType += UnitySerializer_CreateType;
    //}

    //private void UnitySerializer_CreateType(object sender, UnitySerializer.ObjectMappingEventArgs e)
    //{
    //    posList.Clear();
    //    blockList.Clear();
    //    foreach (KeyValuePair<WorldPosFloat, Block> entry in blocks)
    //    {
    //        posList.Add(entry.Key);
    //        blockList.Add(entry.Value);
    //    }
    //}

    //void OnDeserialize()
    //{
    //    for (int i = 0; i < posList.Count; i++)
    //    {
    //        blocks.Add(posList[i], blockList[i]);
    //    }
    //}
}