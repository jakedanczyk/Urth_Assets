using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World1 : MonoBehaviour {

    public Dictionary<WorldPos, Chunk1> chunk1s = new Dictionary<WorldPos, Chunk1>();
    public GameObject chunk1Prefab;
    //public GameObject treePrefab;
    //public GameObject bonePrefab;
    //public GameObject horsePrefab;
    //public GameObject huckPrefab;

    public string worldName = "world1";

    public void CreateChunk1(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunk1Object = Instantiate(
                        chunk1Prefab, new Vector3(x, y, z),
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

        Chunk1 newChunk1 = newChunk1Object.GetComponent<Chunk1>();

        newChunk1.pos = worldPos;
        newChunk1.world1 = this;

        //Add it to the chunk1s dictionary with the position as the key
        chunk1s.Add(worldPos, newChunk1);

        var terrainGen = new TerrainGen();
        newChunk1 = terrainGen.Chunk1Gen(newChunk1);
        //if (newChunk1.airCount == 6144) { newChunk1.gameObject.SetActive(false); }
        newChunk1.SetBlock1sUnmodified();
        //for(int i = 0; i < newChunk1.treeList.Count; i++)
        //{
        //    GameObject newTree = Instantiate(treePrefab);
        //    newTree.transform.position = newChunk1.treeList[i];
        //}
        //for (int i = 0; i < newChunk1.bushList.Count; i++)
        //{
        //    GameObject newBush = Instantiate(huckPrefab);
        //    newBush.transform.position = newChunk1.bushList[i];
        //}
        //if (newChunk1.boneSpawn)
        //{
        //    print("boneSpawn");
        //    GameObject newBone = Instantiate(bonePrefab);
        //    newBone.transform.position = newChunk1.boneList[0];
        //}
        //if (newChunk1.horseSpawn)
        //{
        //    print("boneSpawn");
        //    GameObject newHorse = Instantiate(horsePrefab);
        //    newHorse.transform.position = newChunk1.horseList[0];
        //}
        Serialization1.Load(newChunk1);
    }

    public void DestroyChunk1(int x, int y, int z)
    {
        Chunk1 chunk1 = null;
        if (chunk1s.TryGetValue(new WorldPos(x, y, z), out chunk1))
        {
            Serialization1.SaveChunk1(chunk1);
            Object.Destroy(chunk1.gameObject);
            chunk1s.Remove(new WorldPos(x, y, z));
        }
    }

    public Chunk1 GetChunk1(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk1.chunk1Size;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk1.chunk1Size;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk1.chunk1Size;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk1.chunk1Size;

        Chunk1 containerChunk1 = null;

        chunk1s.TryGetValue(pos, out containerChunk1);

        return containerChunk1;
    }

    public Block1 GetBlock(int x, int y, int z)
    {
        Chunk1 containerChunk1 = GetChunk1(x, y, z);

        if (containerChunk1 != null)
        {
            Block1 block = containerChunk1.GetBlock1(
                x - containerChunk1.pos.x,
                y - containerChunk1.pos.y,
                z - containerChunk1.pos.z);

            return block;
        }
        else
        {
            return new Block1Air();
        }

    }

    public void SetBlock1(int x, int y, int z, Block1 block1)
    {
        Chunk1 chunk1 = GetChunk1(x, y, z);

        if (chunk1 != null)
        {
            chunk1.SetBlock1(x - chunk1.pos.x, y - chunk1.pos.y, z - chunk1.pos.z, block1);
            chunk1.update = true;
            UpdateIfEqual(x - chunk1.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk1.pos.x, Chunk1.chunk1Size - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk1.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk1.pos.y, Chunk1.chunk1Size - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk1.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk1.pos.z, Chunk1.chunk1Size - 1, new WorldPos(x, y, z + 1));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk1 chunk1 = GetChunk1(pos.x, pos.y, pos.z);
            if (chunk1 != null)
                chunk1.update = true;
        }
    }

    //void CreateTree(int x, int y, int z)
    //{
    //    GameObject newTree = Instantiate(treePrefab);
    //    newTree.transform.position = new Vector3(x, y, z);
    //}
}
