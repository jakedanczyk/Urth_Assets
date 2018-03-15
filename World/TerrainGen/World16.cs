using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World16 : MonoBehaviour {

    public static GameObject worldGameObject;
    public Dictionary<WorldPos, Chunk16> chunk16s = new Dictionary<WorldPos, Chunk16>();
    public GameObject chunk16Prefab;
    public GameObject treePrefab;
    public GameObject bonePrefab;
    public GameObject horsePrefab;
    public GameObject huckPrefab;
    public GameObject allosaurPrefab;
    public ItemDictionary itemDictionary;

    public string worldName = "world16";

    public void Awake()
    {
        worldGameObject = this.gameObject;
    }

    public void CreateChunk16(int x, int y, int z)
    {
        WorldPos worldPos16 = new WorldPos(x, y, z);

        //Instantiate the chunk16 at the coordinates using the chunk16 prefab
        GameObject newChunk16Object = Instantiate(
                        chunk16Prefab, new Vector3(x, y, z),
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

        Chunk16 newChunk16 = newChunk16Object.GetComponent<Chunk16>();

        newChunk16.pos = worldPos16;
        newChunk16.world16 = this;

        //Add it to the chunk16s dictionary with the position as the key
        chunk16s.Add(worldPos16, newChunk16);

        var terrainGen = new TerrainGen();
        newChunk16 = terrainGen.Chunk16Gen(newChunk16);
        newChunk16.SetBlock16sUnmodified();
        //if (newChunk16.airCount == 6144) { newChunk16.gameObject.SetActive(false); }

        for (int i = 0; i < newChunk16.treeList.Count; i++)
        {
            GameObject newTree = Instantiate(treePrefab);
            newTree.transform.position = newChunk16.treeList[i];
        }
        for (int i = 0; i < newChunk16.bushList.Count; i++)
        {
            GameObject newBush = Instantiate(huckPrefab);
            newBush.transform.position = newChunk16.bushList[i];
        }
        if (newChunk16.boneSpawn)
        {
            print("boneSpawn");
            GameObject newBone = Instantiate(bonePrefab);
            newBone.transform.position = newChunk16.boneList[0];
            int r = Random.Range(0, 9);
            GameObject randomItem = Instantiate(itemDictionary.items[r]);
            newBone.GetComponent<LootInventory>().AddItem(randomItem.GetComponent<Item>());
        }
        if (newChunk16.horseSpawn)
        {
            print("horseSpawn");
            GameObject newHorse = Instantiate(horsePrefab);
            newHorse.transform.position = newChunk16.horseList[0];
        }
        if (newChunk16.allosaurusSpawn)
        {
            print("allosaurusSpawn");
            GameObject newAllosaurus = Instantiate(allosaurPrefab);
            newAllosaurus.transform.position = newChunk16.allosaurusList[0];
        }
        Serialization16.Load16(newChunk16);
    }

    public void DestroyChunk16(int x, int y, int z)
    {
        Chunk16 chunk16 = null;
        if (chunk16s.TryGetValue(new WorldPos(x, y, z), out chunk16))
        {
            Serialization16.SaveChunk16(chunk16);
            Object.Destroy(chunk16.gameObject);
            chunk16s.Remove(new WorldPos(x, y, z));
        }
    }

    public void RemoveChunk16(WorldPos worldPos)
    {
        chunk16s.Remove(worldPos);
    }
    public void AddChunk16(WorldPos worldPos, Chunk16 chunk)
    {
        chunk16s[worldPos] = chunk;
    }

    public Chunk16 GetChunk16(float x, float y, float z)
    {
        WorldPos pos = new WorldPos();
        pos.x = Mathf.FloorToInt((x + 8) / 256f) * 256;
        pos.y = Mathf.FloorToInt((y + 8) / 256f) * 256;
        pos.z = Mathf.FloorToInt((z + 8) / 256f) * 256;

        Chunk16 containerChunk16 = null;

        chunk16s.TryGetValue(pos, out containerChunk16);

        return containerChunk16;
    }

    public Block16 GetBlock16(Vector3 position)
    {
        return GetBlock16(position.x, position.y, position.z);
    }

    public Block16 GetBlock16(float xf, float yf, float zf)
    {
        int xi = Mathf.RoundToInt(xf / 16) * 16;
        int yi = Mathf.RoundToInt(yf / 16) * 16;
        int zi = Mathf.RoundToInt(zf / 16) * 16;
        Chunk16 containerChunk16 = GetChunk16(xi, yi, zi);

        if (containerChunk16 != null)
        {
            Block16 block16 = containerChunk16.GetBlock16(
                Mathf.RoundToInt((xi - containerChunk16.pos.x)/16),
                Mathf.RoundToInt((yi - containerChunk16.pos.y)/16),
                Mathf.RoundToInt((zi - containerChunk16.pos.z)/16));

            return block16;
        }
        else
        {
            return new Block16Air();
        }

    }

    public void SetBlock16(float x, float y, float z, Block16 block16)
    {
        SetBlock16(Mathf.RoundToInt(x / 16) * 16, Mathf.RoundToInt(y / 16) * 16, Mathf.RoundToInt(z / 16) * 16, block16);
    }

        public void SetBlock16(int x, int y, int z, Block16 block16)
    {
        Debug.Log("SetBlockinWorld");

        Chunk16 chunk16 = GetChunk16(x, y, z);

        if (chunk16 != null)
        {
            chunk16.SetBlock16(Mathf.RoundToInt((x - chunk16.pos.x) / 16),
                                Mathf.RoundToInt((y - chunk16.pos.y) / 16),
                                Mathf.RoundToInt((z - chunk16.pos.z) / 16),block16);
            chunk16.update = true;
            chunk16.UpdateChunk16();
            UpdateIfEqual(Mathf.RoundToInt((x - chunk16.pos.x)/16)*16, 0, new WorldPos(Mathf.RoundToInt(x / 16) * 16 - 16, Mathf.RoundToInt(y / 16) * 16, Mathf.RoundToInt(z / 16) * 16));
            UpdateIfEqual(Mathf.RoundToInt((x - chunk16.pos.x) / 16) * 16, Chunk16.chunk16Size - 1, new WorldPos(Mathf.RoundToInt(x / 16) * 16 + 16, Mathf.RoundToInt(y / 16) * 16, Mathf.RoundToInt(z / 16) * 16));
            UpdateIfEqual(Mathf.RoundToInt((y - chunk16.pos.y) / 16) * 16, 0, new WorldPos(Mathf.RoundToInt(x / 16) * 16, Mathf.RoundToInt(y / 16) * 16 - 16, Mathf.RoundToInt(z / 16) * 16));
            UpdateIfEqual(Mathf.RoundToInt((y - chunk16.pos.y) / 16) * 16, Chunk16.chunk16Size - 1, new WorldPos(Mathf.RoundToInt(x / 16) * 16, Mathf.RoundToInt(y / 16) * 16 + 16, Mathf.RoundToInt(z / 16) * 16));
            UpdateIfEqual(Mathf.RoundToInt((z - chunk16.pos.z) / 16) * 16, 0, new WorldPos(Mathf.RoundToInt(x / 16) * 16, Mathf.RoundToInt(y / 16) * 16, Mathf.RoundToInt(z / 16) * 16 - 16));
            UpdateIfEqual(Mathf.RoundToInt((z - chunk16.pos.z) / 16) * 16, Chunk16.chunk16Size - 1, new WorldPos(Mathf.RoundToInt(x / 16) * 16, Mathf.RoundToInt(y / 16) * 16, Mathf.RoundToInt(z / 16) * 16 + 16));
        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        Debug.Log("UpdateIfEqual");

        if (value1 == value2)
        {
            Chunk16 chunk16 = GetChunk16(pos.x, pos.y, pos.z);
            if (chunk16 != null)
            {
                chunk16.update = true;
                chunk16.UpdateChunk16();
            }
        }
    }
}
