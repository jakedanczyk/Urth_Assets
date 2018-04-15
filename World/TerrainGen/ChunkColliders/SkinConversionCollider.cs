using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkinConversionCollider : MonoBehaviour {

    public World16 world;
    public List<Chunk16> skinnedChunksList = new List<Chunk16>();
    public HashSet<Chunk16> skinnedChunksHash = new HashSet<Chunk16>();
    public int skinnedCount;

    public ChunkColliderManager manager;

    IEnumerator DistanceCheck()
    {
        for(int i = 0; i < skinnedCount; i++)
        {
            if (!skinnedChunksList[i].gameObject.activeSelf)
            {
                float dist = Vector3.Distance(transform.position, skinnedChunksList[i].transform.position);
                if (dist > 365)
                {
                    foreach(Chunk1 chunk1 in skinnedChunksList[i].subChunk1List)
                    {
                        chunk1.gameObject.SetActive(false);
                    }
                    skinnedChunksList[i].gameObject.SetActive(true);
                }
            }
        }
        yield return new WaitForSeconds(2);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 13)
        {
            Chunk16 chunk = col.gameObject.GetComponent<Chunk16>();
            chunk.SkinConvert();
            if (skinnedChunksHash.Add(chunk))
            {
                skinnedCount++;
                skinnedChunksList.Add(chunk);
            }
        }
    }
}