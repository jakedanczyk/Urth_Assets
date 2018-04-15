using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour {

    public MessageLog messageLog;

    public static GameObject waterManagerObject;
    public GameObject riverPrefab,lakePrefab,caveRiverPrefab;
    Vector3 playerPos;
    public List<Chunk256> chunks;
    IEnumerator generate,erode256;
    public List<River> rivers;
    public HashSet<River> riversHash = new HashSet<River>();
    public List<Lake> lakes;
    public HashSet<Lake> lakesHash = new HashSet<Lake>();
    public List<AudioSource> riverAudioSources;

    private void Awake()
    {
        waterManagerObject = this.gameObject;
    }

    void Start()
    {
        messageLog = MessageLog.messageLogGameObject.GetComponent<MessageLog>();
        playerPos = BodyManager_Human_Player.playerObject.transform.position;
        generate = Generate();
        StartCoroutine(generate);
        erode256 = ErodeChunk256();
        messageLog.NewMessage("River and lake generation in progress");

    }

    IEnumerator Generate()
    {
        var terrainGen = new TerrainGen();
        while (rivers.Count < 100)
        {
            if (rivers.Count >= 1 && !rivers[rivers.Count - 1].DoneForNow) { yield return null; continue; }
            Vector3 newSpawn = new Vector3(playerPos.x + Random.Range(-16000, 16000), 0, playerPos.z + Random.Range(-16000, 16000));
            newSpawn.y = terrainGen.StoneHeight256(newSpawn.x, 0, newSpawn.z);
            if (newSpawn.y > 4880)
                continue;
            else if (newSpawn.y > 4200)
            {
                if (Random.Range(0, 100) < 75)
                    continue;
            }
            else if (newSpawn.y < 2100)
                continue;
            River newRiver = Instantiate(riverPrefab).GetComponent<River>();
            newRiver.source = newSpawn;
            newRiver.StartCoroutine(newRiver.initialGenerate);
            if(!newRiver.DoneForNow) { yield return null; continue; }
            yield return null;
        }
        messageLog.NewMessage("River generation complete");

        bool lakesSettled = false;
        while (!lakesSettled)
        {
            lakesSettled = true;
            foreach (Lake lake in lakesHash)
            {
                if (!lake.settled)
                    lakesSettled = false;
            }
            yield return null;
        }
        messageLog.NewMessage("Lake generation complete");
        //StartCoroutine(ErodeChunk256());
        StopCoroutine(generate);
        yield return null;
    }

    public River SpawnRiverFromLake(Vector3 origin,Lake lake)
    {
        if (rivers.Count > 150)
            return null;
        River newRiver = Instantiate(riverPrefab).GetComponent<River>();
        newRiver.source = origin;
        newRiver.sourceLake = lake;
        foreach (River river in lake.sources)
            newRiver.sourceFlow += river.sourceFlow;
        newRiver.StartCoroutine(newRiver.generateFromLake);
        return newRiver;
    }

    public void SpawnCaveRiver(Vector3 origin, River source)
    {
        CaveRiver newCaveRiver = Instantiate(caveRiverPrefab).GetComponent<CaveRiver>();
        newCaveRiver.source = source;
        newCaveRiver.gameObject.transform.position = origin;
    }

    IEnumerator ErodeChunk256()
    {
        TerrainGen terrainGen = new TerrainGen();
        World256 world = World256.worldGameObject.GetComponent<World256>();
        HashSet<Vector3> clearedBlocks = new HashSet<Vector3>();
        foreach(River river in rivers)
        {
            if (river != null)
            {
                foreach (Vector3 waypoint in river.waypoints)
                {
                    if (waypoint.y < terrainGen.shrubline)
                    {
                        world.SetBlock256((int)(river.source.x + waypoint.x), (int)(river.source.y + waypoint.y - 256), (int)(river.source.z + waypoint.z), new Block256GrassRiverValley());
                        world.SetBlock256((int)(river.source.x + waypoint.x), (int)(river.source.y + waypoint.y), (int)(river.source.z + waypoint.z), new Block256Air());
                        world.SetBlock256((int)(river.source.x + waypoint.x), (int)(river.source.y + waypoint.y + 256), (int)(river.source.z + waypoint.z), new Block256Air());
                        yield return null;
                    }
                }
            }
            yield return null;
        }
        foreach (Lake lake in lakesHash)
        {
            if (lake != null)
            {
                foreach (Vector2 cell in lake.gridList)
                {
                    world.SetBlock256((int)(lake.source.x + cell.x), (int)(lake.source.y + lake.depth), (int)(lake.source.z + cell.y), new Block256Air());
                    yield return null;
                }
            }
            yield return null;
        }
        yield return null;
    }
}
