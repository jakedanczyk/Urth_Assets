using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World256 : MonoBehaviour {

    public Dictionary<WorldPos, Chunk256> chunk256s = new Dictionary<WorldPos, Chunk256>();
    public GameObject chunk256Prefab;


    public string worldName = "world256";

    public void CreateChunk256(int x, int y, int z)
    {
        WorldPos worldPos256 = new WorldPos(x, y, z);

        //Instantiate the chunk256 at the coordinates using the chunk256 prefab
        GameObject newChunk256Object = Instantiate(
                        chunk256Prefab, new Vector3(x, y, z),
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

        Chunk256 newChunk256 = newChunk256Object.GetComponent<Chunk256>();

        newChunk256.pos = worldPos256;
        newChunk256.world256 = this;

        //Add it to the chunk256s dictionary with the position as the key
        chunk256s.Add(worldPos256, newChunk256);

        var terrainGen = new TerrainGen();
        newChunk256 = terrainGen.Chunk256Gen(newChunk256);
        newChunk256.SetBlock256sUnmodified();
        if (newChunk256.airCount == 6144) { newChunk256.gameObject.SetActive(false); }

        //for(int i = 0; i < newChunk.treeList.Count; i++)
        //{
        //    GameObject newTree = Instantiate(treePrefab);
        //    newTree.transform.position = newChunk.treeList[i];
        //}
        //for (int i = 0; i < newChunk.bushList.Count; i++)
        //{World
        //    GameObject newBush = Instantiate(huckPrefab);
        //    newBush.transform.position = newChunk.bushList[i];
        //}
        //if (newChunk.boneSpawn)
        //{
        //    print("boneSpawn");
        //    GameObject newBone = Instantiate(bonePrefab);
        //    newBone.transform.position = newChunk.boneList[0];
        //}
        //if (newChunk.horseSpawn)
        //{
        //    print("boneSpawn");
        //    GameObject newHorse = Instantiate(horsePrefab);
        //    newHorse.transform.position = newChunk.horseList[0];
        //}

        Serialization256.Load256(newChunk256);
    }

    public void DestroyChunk256(int x, int y, int z)
    {
        Chunk256 chunk256 = null;
        if (chunk256s.TryGetValue(new WorldPos(x, y, z), out chunk256))
        {
            Serialization256.SaveChunk256(chunk256);
            Object.Destroy(chunk256.gameObject);
            chunk256s.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk256 GetChunk256(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        pos.x = Mathf.FloorToInt(x / 4096f) * 4096;
        pos.y = Mathf.FloorToInt(y / 4096f) * 4096;
        pos.z = Mathf.FloorToInt(z / 4096f) * 4096;

        Chunk256 containerChunk256 = null;

        chunk256s.TryGetValue(pos, out containerChunk256);

        return containerChunk256;
    }

    public Block256 GetBlock256(int x, int y, int z)
    {
        //x = Mathf.FloorToInt(x / 256) * 256;
        //y = Mathf.FloorToInt(y / 256) * 256;
        //z = Mathf.FloorToInt(z / 256) * 256;
        Chunk256 containerChunk256 = GetChunk256(x, y, z);

        if (containerChunk256 != null)
        {
            Block256 block256 = containerChunk256.GetBlock256(
                Mathf.FloorToInt((x - containerChunk256.pos.x)/256),
                Mathf.FloorToInt((y - containerChunk256.pos.y)/256),
                Mathf.FloorToInt((z - containerChunk256.pos.z)/256));

            return block256;
        }
        else
        {
            return new Block256Air();
        }

    }

    public void SetBlock256(int x, int y, int z, Block256 block256)
    {
        Chunk256 chunk256 = GetChunk256(x, y, z);

        if (chunk256 != null)
        {
            chunk256.SetBlock256(Mathf.FloorToInt((x - chunk256.pos.x) / 256),
                                Mathf.FloorToInt((y - chunk256.pos.y) / 256),
                                Mathf.FloorToInt((z - chunk256.pos.z) / 256),block256);
            chunk256.update = true;
            UpdateIfEqual(Mathf.FloorToInt((x - chunk256.pos.x) / 256) * 256, 0, new WorldPos(Mathf.FloorToInt(x / 256) * 256 - 256, Mathf.FloorToInt(y / 256) * 256, Mathf.FloorToInt(z / 256) * 256));
            UpdateIfEqual(Mathf.FloorToInt((x - chunk256.pos.x) / 256) * 256, 16 - 1, new WorldPos(Mathf.FloorToInt(x / 256) * 256 + 256, Mathf.FloorToInt(y / 256) * 256, Mathf.FloorToInt(z / 256) * 256));
            UpdateIfEqual(Mathf.FloorToInt((y - chunk256.pos.y) / 256) * 256, 0, new WorldPos(Mathf.FloorToInt(x / 256) * 256, Mathf.FloorToInt(y / 256) * 256 - 256, Mathf.FloorToInt(z / 256) * 256));
            UpdateIfEqual(Mathf.FloorToInt((y - chunk256.pos.y) / 256) * 256, 16 - 1, new WorldPos(Mathf.FloorToInt(x / 256) * 256, Mathf.FloorToInt(y / 256) * 256 + 256, Mathf.FloorToInt(z / 256) * 256));
            UpdateIfEqual(Mathf.FloorToInt((z - chunk256.pos.z) / 256) * 256, 0, new WorldPos(Mathf.FloorToInt(x / 256) * 256, Mathf.FloorToInt(y / 256) * 256, Mathf.FloorToInt(z / 256) * 256 - 256));
            UpdateIfEqual(Mathf.FloorToInt((z - chunk256.pos.z) / 256) * 256, 16 - 1, new WorldPos(Mathf.FloorToInt(x / 256) * 256, Mathf.FloorToInt(y / 256) * 256, Mathf.FloorToInt(z / 256) * 256 + 256));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk256 chunk256 = GetChunk256(pos.x, pos.y, pos.z);
            if (chunk256 != null)
                chunk256.update = true;
        }
    }
}
