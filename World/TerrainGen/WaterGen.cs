using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGen : MonoBehaviour {

    public GameObject waterPrefab;
    public static GameObject waterGenObject;

    public AudioClip smallCreekTrickle;
    public World1 world1;
    public Dictionary<WorldPos,bool> waterSpawnPosDict = new Dictionary<WorldPos, bool>();
    public List<WorldPos> waterActiveSpawnPosList = new List<WorldPos>();
    public List<WorldPos> waterActivePosList = new List<WorldPos>();
    public List<WorldPos> riverSpawnList = new List<WorldPos>();
    TerrainGen terrainGen = new TerrainGen();

    public List<WorldPos> riverLeadList = new List<WorldPos>();
    public List<WorldPos> riverTargetList = new List<WorldPos>();


    private void Awake()
    {
        waterGenObject = this.gameObject;
    }

    private void Start()
    {
        int height = (int)terrainGen.StoneHeight(-4797, 0, -5319);
        //riverSpawnList.Add(new WorldPos(-4797, height, -5319));
        waterSpawnPosDict[new WorldPos(-4797, height, -5319)] = true;
        waterActiveSpawnPosList.Add(new WorldPos(-4797, height, -5319));
        riverLeadList.Add(new WorldPos(-4797, height, -5319));
        world1 = World1.worldGameObject.GetComponent<World1>();
    }

    int radius = 8;

    void Update()
    {
        for (int i = 0; i < 100; i++)
        {
            Chunk1 chunk = world1.GetChunk1(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z);                
            if (chunk != null)
            {
                Block1 block = world1.GetBlock1(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z);
                if (!(block is Block1Air))
                {
                    if (!(world1.GetBlock1(riverLeadList[0].x - 1, riverLeadList[0].y + 1, riverLeadList[0].z) is Block1Air)
                    && !(world1.GetBlock1(riverLeadList[0].x + 1, riverLeadList[0].y + 1, riverLeadList[0].z) is Block1Air)
                    && !(world1.GetBlock1(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z - 1) is Block1Air)
                    && !(world1.GetBlock1(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z + 1) is Block1Air))
                    {
                        world1.SetBlock1(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z, new Block1Water());
                        riverLeadList[0] = new WorldPos(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z);
                        world1.GetChunk1(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z).update = true;
                        if (chunk.hasWater == false)
                        {
                            GameObject newWater = Instantiate(waterPrefab);
                            newWater.transform.position = chunk.transform.position;
                            chunk.water = newWater;
                            chunk.chunkWater = newWater.GetComponent<ChunkWater>();
                            AudioSource audio = newWater.AddComponent<AudioSource>();
                            audio.loop = true;
                            audio.clip = smallCreekTrickle;
                            audio.spatialBlend = 1.0f;
                            audio.Play();
                            chunk.hasWater = true;
                        }
                    }
                    else
                    {
                        world1.SetBlock1(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z, new Block1Air());
                        riverLeadList[0] = new WorldPos(riverLeadList[0].x, riverLeadList[0].y + 1, riverLeadList[0].z);
                    }
                }
                else
                {
                    float theta = 45f * Mathf.PI / 180f;
                    int low = 999999;
                    int height;
                    int moveX = 0;
                    int moveZ = 0;
                    for (int j = 1; j < 9; j++)
                    {
                        int x = (int)(radius * Mathf.Cos(theta * j));
                        int z = (int)(radius * Mathf.Sin(theta * j));
                        height = (int)terrainGen.StoneHeight(riverLeadList[0].x + x, 0, riverLeadList[0].z + z);
                        if (height < low)
                        {   
                            low = height;
                            moveX = Mathf.RoundToInt(Mathf.Cos(theta * j));
                            moveZ = Mathf.RoundToInt(Mathf.Sin(theta * j));
                        }
                    }
                    riverLeadList[0] = new WorldPos(riverLeadList[0].x + moveX, low, riverLeadList[0].z + moveZ);
                }
            }
        }
    }
        //for (int i = 0; i < waterActiveSpawnPosList.Count && i < 10; i++)
        //{
        //    WorldPos pos = waterActiveSpawnPosList[i];

        //    if (world1.GetBlock(pos.x - 1, pos.y, pos.z) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x - 1, pos.y, pos.z, new Block1Water());
        //        world1.GetChunk1(pos.x - 1, pos.y, pos.z).update = true;
        //        world1.GetChunk1(pos.x - 1, pos.y, pos.z).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x - 1, pos.y, pos.z));
        //    }
        //    if (world1.GetBlock(pos.x + 1, pos.y, pos.z) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x + 1, pos.y, pos.z, new Block1Water());
        //        world1.GetChunk1(pos.x + 1, pos.y, pos.z).update = true;
        //        world1.GetChunk1(pos.x + 1, pos.y, pos.z).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x + 1, pos.y, pos.z));
        //    }
        //    if (world1.GetBlock(pos.x, pos.y, pos.z - 1) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z - 1, new Block1Water());
        //        world1.GetChunk1(pos.x, pos.y, pos.z - 1).update = true;
        //        world1.GetChunk1(pos.x, pos.y, pos.z - 1).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x, pos.y, pos.z - 1));
        //    }
        //    if (world1.GetBlock(pos.x, pos.y, pos.z + 1) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z + 1, new Block1Water());
        //        world1.GetChunk1(pos.x, pos.y, pos.z + 1).update = true;
        //        world1.GetChunk1(pos.x, pos.y, pos.z + 1).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x, pos.y, pos.z + 1));
        //    }
        //    waterActiveSpawnPosList.RemoveAt(0);
        //    if (i == 10)
        //        return;
        //}
        
        //for (int i = 0; i < waterActivePosList.Count && i < 100; i++)
        //{
        //    WorldPos pos = waterActivePosList[i];
        //    if (waterSpawnPosDict.ContainsKey(new WorldPos(pos.x, pos.y + 1, pos.z)))
        //        waterActiveSpawnPosList.Add(new WorldPos(pos.x, pos.y + 1, pos.z));
        //    if (waterSpawnPosDict.ContainsKey(new WorldPos(pos.x - 1, pos.y, pos.z)))
        //        waterActiveSpawnPosList.Add(new WorldPos(pos.x - 1, pos.y, pos.z));
        //    if (waterSpawnPosDict.ContainsKey(new WorldPos(pos.x + 1, pos.y, pos.z)))
        //        waterActiveSpawnPosList.Add(new WorldPos(pos.x + 1, pos.y, pos.z));
        //    if (waterSpawnPosDict.ContainsKey(new WorldPos(pos.x, pos.y, pos.z + 1)))
        //        waterActiveSpawnPosList.Add(new WorldPos(pos.x, pos.y, pos.z + 1));
        //    if (waterSpawnPosDict.ContainsKey(new WorldPos(pos.x, pos.y, pos.z - 1)))
        //        waterActiveSpawnPosList.Add(new WorldPos(pos.x, pos.y, pos.z - 1));

        //    if (world1.GetBlock(pos.x, pos.y-1, pos.z) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x, pos.y - 1, pos.z, new Block1Water());
        //        world1.GetChunk1(pos.x, pos.y - 1, pos.z).update = true;
        //        world1.GetChunk1(pos.x, pos.y - 1, pos.z).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x, pos.y - 1, pos.z));
        //    }
        //    else if (world1.GetBlock(pos.x - 1, pos.y - 1, pos.z) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x - 1, pos.y - 1, pos.z, new Block1Water());
        //        world1.GetChunk1(pos.x - 1, pos.y - 1, pos.z).update = true;
        //        world1.GetChunk1(pos.x - 1, pos.y - 1, pos.z).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x - 1, pos.y - 1, pos.z));
        //    }
        //    else if (world1.GetBlock(pos.x + 1, pos.y - 1, pos.z) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x + 1, pos.y - 1, pos.z, new Block1Water());
        //        world1.GetChunk1(pos.x + 1, pos.y - 1, pos.z).update = true;
        //        world1.GetChunk1(pos.x + 1, pos.y - 1, pos.z).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x + 1, pos.y - 1, pos.z));
        //    }
        //    else if (world1.GetBlock(pos.x, pos.y - 1, pos.z - 1) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x, pos.y - 1, pos.z - 1, new Block1Water());
        //        world1.GetChunk1(pos.x, pos.y - 1, pos.z - 1).update = true;
        //        world1.GetChunk1(pos.x, pos.y - 1, pos.z - 1).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x, pos.y - 1, pos.z - 1));
        //    }
        //    else if (world1.GetBlock(pos.x, pos.y - 1, pos.z + 1) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x, pos.y - 1, pos.z + 1, new Block1Water());
        //        world1.GetChunk1(pos.x, pos.y - 1, pos.z + 1).update = true;
        //        world1.GetChunk1(pos.x, pos.y - 1, pos.z + 1).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x, pos.y - 1, pos.z + 1));
        //    }
        //    else if (world1.GetBlock(pos.x - 1, pos.y, pos.z) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x - 1, pos.y, pos.z, new Block1Water());
        //        world1.GetChunk1(pos.x - 1, pos.y, pos.z).update = true;
        //        world1.GetChunk1(pos.x - 1, pos.y, pos.z).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x + 1, pos.y, pos.z));
        //    }
        //    else if (world1.GetBlock(pos.x + 1, pos.y, pos.z) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x + 1, pos.y, pos.z, new Block1Water());
        //        world1.GetChunk1(pos.x + 1, pos.y, pos.z).update = true;
        //        world1.GetChunk1(pos.x + 1, pos.y, pos.z).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x + 1, pos.y, pos.z));
        //    }
        //    else if (world1.GetBlock(pos.x, pos.y, pos.z - 1) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x, pos.y, pos.z - 1, new Block1Water());
        //        world1.GetChunk1(pos.x, pos.y, pos.z - 1).update = true;
        //        world1.GetChunk1(pos.x, pos.y, pos.z - 1).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x, pos.y, pos.z - 1));
        //    }
        //    else if (world1.GetBlock(pos.x, pos.y, pos.z + 1) is Block1Air)
        //    {
        //        world1.SetBlock1(pos.x, pos.y, pos.z, new Block1Air());
        //        world1.SetBlock1(pos.x, pos.y, pos.z + 1, new Block1Water());
        //        world1.GetChunk1(pos.x, pos.y, pos.z + 1).update = true;
        //        world1.GetChunk1(pos.x, pos.y, pos.z + 1).hasWater = true;
        //        waterActivePosList.Add(new WorldPos(pos.x, pos.y, pos.z + 1));
        //    }
        //    //waterActivePosList.Add(waterActivePosList[0]);
        //    waterActivePosList.RemoveAt(0);
        //    if (i == 100)
        //        return;
        //}

}
