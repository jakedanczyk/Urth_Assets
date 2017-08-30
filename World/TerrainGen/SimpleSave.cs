using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SimpleSave
{
    public Dictionary<WorldPos, SimpleChunk> simpleChunks = new Dictionary<WorldPos, SimpleChunk>();

    public SimpleSave(SimpleChunk simpleChunk)
    {
        WorldPos pos = new WorldPos(0, 0, 0);
        simpleChunks.Add(pos, simpleChunk);      
    }
}