using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleChunkTextureDictionary : MonoBehaviour {

    public Dictionary<SimpleChunkType, Texture> textDict;

    public Texture stoneTexture, grassTexture, airTexture;

    void Awake()
    {
        textDict = new Dictionary<SimpleChunkType, Texture>();
        textDict.Add(SimpleChunkType.Stone, stoneTexture);
        textDict.Add(SimpleChunkType.Grass, grassTexture);
        textDict.Add(SimpleChunkType.Air, null);
    }
}