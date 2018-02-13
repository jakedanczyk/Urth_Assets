using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World4 : MonoBehaviour {

    public static GameObject worldGameObject;

    public Dictionary<WorldPos, Chunk4> chunk4s = new Dictionary<WorldPos, Chunk4>();
    public GameObject chunk4Prefab;
    //public GameObject treePrefab;
    //public GameObject bonePrefab;
    //public GameObject horsePrefab;
    //public GameObject huckPrefab;

    public string worldName = "world4";

    public void Awake()
    {
        worldGameObject = this.gameObject;
    }

    public void CreateChunk4(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunk4Object = Instantiate(
                        chunk4Prefab, new Vector3(x, y, z),
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

        Chunk4 newChunk4 = newChunk4Object.GetComponent<Chunk4>();

        newChunk4.pos = worldPos;
        newChunk4.world4 = this;

        //Add it to the chunk4s dictionary with the position as the key
        chunk4s.Add(worldPos, newChunk4);

        var terrainGen = new TerrainGen();
        newChunk4 = terrainGen.Chunk4Gen(newChunk4);
        //if (newChunk4.airCount == 6144) { newChunk4.gameObject.SetActive(false); }
        newChunk4.SetBlock4sUnmodified();
        //for(int i = 0; i < newChunk4.treeList.Count; i++)
        //{
        //    GameObject newTree = Instantiate(treePrefab);
        //    newTree.transform.position = newChunk4.treeList[i];
        //}
        //for (int i = 0; i < newChunk4.bushList.Count; i++)
        //{
        //    GameObject newBush = Instantiate(huckPrefab);
        //    newBush.transform.position = newChunk4.bushList[i];
        //}
        //if (newChunk4.boneSpawn)
        //{
        //    print("boneSpawn");
        //    GameObject newBone = Instantiate(bonePrefab);
        //    newBone.transform.position = newChunk4.boneList[0];
        //}
        //if (newChunk4.horseSpawn)
        //{
        //    print("boneSpawn");
        //    GameObject newHorse = Instantiate(horsePrefab);
        //    newHorse.transform.position = newChunk4.horseList[0];
        //}
        Serialization4.Load(newChunk4);
    }

    public void DestroyChunk4(int x, int y, int z)
    {
        Chunk4 chunk4 = null;
        if (chunk4s.TryGetValue(new WorldPos(x, y, z), out chunk4))
        {
            Serialization4.SaveChunk4(chunk4);
            Object.Destroy(chunk4.gameObject);
            chunk4s.Remove(new WorldPos(x, y, z));
        }
    }

    public void RemoveChunk4(WorldPos worldPos)
    {
        chunk4s.Remove(worldPos);
    }
    public void AddChunk4(WorldPos worldPos, Chunk4 chunk)
    {
        chunk4s[worldPos] = chunk;
    }

    public Chunk4 GetChunk4(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        pos.x = Mathf.FloorToInt(x / 64f) * 64;
        pos.y = Mathf.FloorToInt(y / 64f) * 64;
        pos.z = Mathf.FloorToInt(z / 64f) * 64;

        Chunk4 containerChunk4 = null;

        chunk4s.TryGetValue(pos, out containerChunk4);

        return containerChunk4;
    }

    public Block4 GetBlock4(int x, int y, int z)
    {
        //x = Mathf.FloorToInt(x / 4) * 4;
        //y = Mathf.FloorToInt(y / 4) * 4;
        //z = Mathf.FloorToInt(z / 4) * 4;
        Chunk4 containerChunk4 = GetChunk4(x, y, z);

        if (containerChunk4 != null)
        {
            Block4 block = containerChunk4.GetBlock4(
                Mathf.FloorToInt((x - containerChunk4.pos.x) / 4),
                Mathf.FloorToInt((y - containerChunk4.pos.y) / 4),
                Mathf.FloorToInt((z - containerChunk4.pos.z) / 4));

            return block;
        }
        else
        {
            return new Block4Air();
        }

    }

    public void SetBlock4(int x, int y, int z, Block4 block4)
    {
        Chunk4 chunk4 = GetChunk4(x, y, z);

        if (chunk4 != null)
        {
            chunk4.SetBlock4(Mathf.FloorToInt((x - chunk4.pos.x) / 4),
                    Mathf.FloorToInt((y - chunk4.pos.y) / 4),
                    Mathf.FloorToInt((z - chunk4.pos.z) / 4), block4);

            chunk4.SetBlock4(x - chunk4.pos.x, y - chunk4.pos.y, z - chunk4.pos.z, block4);
            chunk4.update = true;
            chunk4.UpdateChunk4();
            UpdateIfEqual(x - chunk4.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk4.pos.x, Chunk4.chunk4Size - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk4.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk4.pos.y, Chunk4.chunk4Size - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk4.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk4.pos.z, Chunk4.chunk4Size - 1, new WorldPos(x, y, z + 1));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk4 chunk4 = GetChunk4(pos.x, pos.y, pos.z);
            if (chunk4 != null)
            {
                chunk4.update = true;
                chunk4.UpdateChunk4();
            }            
        }
    }

    //void CreateTree(int x, int y, int z)
    //{
    //    GameObject newTree = Instantiate(treePrefab);
    //    newTree.transform.position = new Vector3(x, y, z);
    //}
}
