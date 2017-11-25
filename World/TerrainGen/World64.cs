using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World64 : MonoBehaviour {

    public Dictionary<WorldPos, Chunk64> chunk64s = new Dictionary<WorldPos, Chunk64>();
    public GameObject chunk64Prefab;


    public string worldName = "world64";

    public void CreateChunk64(int x, int y, int z)
    {
        WorldPos worldPos64 = new WorldPos(x, y, z);

        //Instantiate the chunk64 at the coordinates using the chunk64 prefab
        GameObject newChunk64Object = Instantiate(
                        chunk64Prefab, new Vector3(x, y, z),
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

        Chunk64 newChunk64 = newChunk64Object.GetComponent<Chunk64>();

        newChunk64.pos = worldPos64;
        newChunk64.world64 = this;

        //Add it to the chunk64s dictionary with the position as the key
        chunk64s.Add(worldPos64, newChunk64);

        var terrainGen = new TerrainGen();
        newChunk64 = terrainGen.Chunk64Gen(newChunk64);
        newChunk64.SetBlock64sUnmodified();
        if (newChunk64.airCount == 6144) { newChunk64.gameObject.SetActive(false); }

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

        Serialization64.Load64(newChunk64);
    }

    public void DestroyChunk64(int x, int y, int z)
    {
        Chunk64 chunk64 = null;
        if (chunk64s.TryGetValue(new WorldPos(x, y, z), out chunk64))
        {
            Serialization64.SaveChunk64(chunk64);
            Object.Destroy(chunk64.gameObject);
            chunk64s.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk64 GetChunk64(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        pos.x = Mathf.FloorToInt(x / 1024f) * 1024;
        pos.y = Mathf.FloorToInt(y / 1024f) * 1024;
        pos.z = Mathf.FloorToInt(z / 1024f) * 1024;

        Chunk64 containerChunk64 = null;

        chunk64s.TryGetValue(pos, out containerChunk64);

        return containerChunk64;
    }

    public Block64 GetBlock64(int x, int y, int z)
    {
        //x = Mathf.FloorToInt(x / 64) * 64;
        //y = Mathf.FloorToInt(y / 64) * 64;
        //z = Mathf.FloorToInt(z / 64) * 64;
        Chunk64 containerChunk64 = GetChunk64(x, y, z);

        if (containerChunk64 != null)
        {
            Block64 block64 = containerChunk64.GetBlock64(
                Mathf.FloorToInt((x - containerChunk64.pos.x)/64),
                Mathf.FloorToInt((y - containerChunk64.pos.y)/64),
                Mathf.FloorToInt((z - containerChunk64.pos.z)/64));

            return block64;
        }
        else
        {
            return new Block64Air();
        }

    }

    public void SetBlock64(int x, int y, int z, Block64 block64)
    {
        Chunk64 chunk64 = GetChunk64(x, y, z);

        if (chunk64 != null)
        {
            chunk64.SetBlock64(Mathf.FloorToInt((x - chunk64.pos.x) / 64),
                                Mathf.FloorToInt((y - chunk64.pos.y) / 64),
                                Mathf.FloorToInt((z - chunk64.pos.z) / 64),block64);
            chunk64.update = true;
            UpdateIfEqual(Mathf.FloorToInt((x - chunk64.pos.x) / 64) * 64, 0, new WorldPos(Mathf.FloorToInt(x / 64) * 64 - 64, Mathf.FloorToInt(y / 64) * 64, Mathf.FloorToInt(z / 64) * 64));
            UpdateIfEqual(Mathf.FloorToInt((x - chunk64.pos.x) / 64) * 64, 16 - 1, new WorldPos(Mathf.FloorToInt(x / 64) * 64 + 64, Mathf.FloorToInt(y / 64) * 64, Mathf.FloorToInt(z / 64) * 64));
            UpdateIfEqual(Mathf.FloorToInt((y - chunk64.pos.y) / 64) * 64, 0, new WorldPos(Mathf.FloorToInt(x / 64) * 64, Mathf.FloorToInt(y / 64) * 64 - 64, Mathf.FloorToInt(z / 64) * 64));
            UpdateIfEqual(Mathf.FloorToInt((y - chunk64.pos.y) / 64) * 64, 16 - 1, new WorldPos(Mathf.FloorToInt(x / 64) * 64, Mathf.FloorToInt(y / 64) * 64 + 64, Mathf.FloorToInt(z / 64) * 64));
            UpdateIfEqual(Mathf.FloorToInt((z - chunk64.pos.z) / 64) * 64, 0, new WorldPos(Mathf.FloorToInt(x / 64) * 64, Mathf.FloorToInt(y / 64) * 64, Mathf.FloorToInt(z / 64) * 64 - 64));
            UpdateIfEqual(Mathf.FloorToInt((z - chunk64.pos.z) / 64) * 64, 16 - 1, new WorldPos(Mathf.FloorToInt(x / 64) * 64, Mathf.FloorToInt(y / 64) * 64, Mathf.FloorToInt(z / 64) * 64 + 64));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk64 chunk64 = GetChunk64(pos.x, pos.y, pos.z);
            if (chunk64 != null)
                chunk64.update = true;
        }
    }
}
