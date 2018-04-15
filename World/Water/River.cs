using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : MonoBehaviour
{
    public List<Vector3> waypoints, left, right; //these points are relative to river origin
    public List<float> speeds = new List<float>();
    public List<float> widths = new List<float>();
    public Vector3 source;
    public float sourceFlow;
    public List<float> turns;
    float radius = 3f;

    int attempts = 0;
    
    public GameObject lakePrefab;
    public IEnumerator initialGenerate, generate, findJunction, joinLake, generateFromLake;
    public RiverGenLeader riverGenLeader;
    public List<River> tributaries;
    public River parentStem;
    public Lake endLake,sourceLake;
    public List<RiverNode> nodes = new List<RiverNode>();

    public GameObject nodePrefab;
    GameObject waterManagerObject;
    WaterManager waterManager;

    private void Awake()
    {
        initialGenerate = InitialGenerate();
        generate = Generate();
        generateFromLake = GenerateFromLake();
    }

    private void Start()
    {
        waterManagerObject = WaterManager.waterManagerObject;
        waterManager = waterManagerObject.GetComponent<WaterManager>();
        if (LevelSerializer.IsDeserializing) return;
        waterManager.riversHash.Add(this);
        waterManager.rivers.Add(this);
    }

    IEnumerator InitialGenerate()
    {
        attempts += 1;
        if(attempts > 10)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
        var terrainGen = new TerrainGen();
        this.gameObject.transform.position = source;
        waypoints.Add(Vector3.zero);
        sourceFlow = Random.Range(.03f, 3f);
        widths.Add(sourceFlow);
        float theta = 45f * Mathf.PI / 180f;
        float low;
        float height;
        float turn = 0;
        float moveX = 0;
        float moveZ = 0;
        float t, x, z;
        for (int c = 0; c < 10000; c++) // (true)
        {
            for (int i = 0; i < 10; i++)
            {
                low = 999999f;
                for (int j = 0; j < 8; j++)
                {
                    t = theta * j;
                    x = (15 * Mathf.Cos(t));
                    z = (15 * Mathf.Sin(t));
                    height = terrainGen.StoneHeight256(source.x + waypoints[waypoints.Count - 1].x + x, 0, source.z + waypoints[waypoints.Count - 1].z + z);
                    if (height < low)
                    {
                        turn = t;
                        low = height;
                        moveX = radius * Mathf.Cos(turn);
                        moveZ = radius * Mathf.Sin(turn);
                    }
                }
                if (turns.Count > 1 && Mathf.Abs(Mathf.Abs(turn - turns[turns.Count - 1]) - Mathf.PI) < 0.1)
                {// if we've doubled back, form a lake
                    DoneForNow = true;
                    Render();
                    GameObject lake = Instantiate(lakePrefab);
                    lake.transform.position = waypoints[waypoints.Count - 1] + source;
                    endLake = lake.GetComponent<Lake>();
                    if (!endLake.sources.Contains(this))
                        endLake.sources.Add(this);
                    endLake.Generate();
                    riverGenLeader.transform.position = waypoints[waypoints.Count - 1] + source;
                    findJunction = FindJunction(riverGenLeader.touchedRiver);
                    StartCoroutine(findJunction);
                    JoinLake(riverGenLeader.touchedLake);
                    StopCoroutine(initialGenerate);
                    yield return null;
                }
                float drop = terrainGen.StoneHeight256(source.x + waypoints[waypoints.Count - 1].x + moveX, 0, source.z + waypoints[waypoints.Count - 1].z + moveZ) - source.y;
                turns.Add(turn);
                waypoints.Add(new Vector3(waypoints[waypoints.Count - 1].x + moveX, drop, waypoints[waypoints.Count - 1].z + moveZ));
                speeds.Add(10 * (waypoints[waypoints.Count - 2].y - drop) / 16);
                widths.Add(Mathf.Min(0.5f * sourceFlow / speeds[speeds.Count - 1], 15.5f));
                left.Add(new Vector3(waypoints[waypoints.Count - 1].x - (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z + (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                right.Add(new Vector3(waypoints[waypoints.Count - 1].x + (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z - (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                if (terrainGen.CaveCheck(source.x + waypoints[waypoints.Count - 1].x + moveX, source.y + drop, source.z + waypoints[waypoints.Count - 1].z + moveZ))
                {   //over a cave, fall in
                    for(int d = 0; d < 10000; d++)
                    {
                        if (!terrainGen.CaveCheck(source.x + waypoints[waypoints.Count - 1].x + moveX, source.y + drop - d, source.z + waypoints[waypoints.Count - 1].z + moveZ))
                        {// found cave floor, add waypoints at waterfall bottom, then back to top for reverse face
                            turns.Add(turn);
                            waypoints.Add(new Vector3(waypoints[waypoints.Count - 1].x, drop-d, waypoints[waypoints.Count - 1].z));
                            speeds.Add(10 * (waypoints[waypoints.Count - 2].y - drop) / 16);
                            widths.Add(Mathf.Min(0.5f * sourceFlow / speeds[speeds.Count - 1], 15.5f));
                            left.Add(new Vector3(waypoints[waypoints.Count - 1].x - (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop - d, waypoints[waypoints.Count - 1].z + (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                            right.Add(new Vector3(waypoints[waypoints.Count - 1].x + (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop - d, waypoints[waypoints.Count - 1].z - (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                            turns.Add(turn);
                            waypoints.Add(new Vector3(waypoints[waypoints.Count - 1].x, drop, waypoints[waypoints.Count - 1].z));
                            speeds.Add(10 * (waypoints[waypoints.Count - 2].y - drop) / 16);
                            widths.Add(Mathf.Min(0.5f * sourceFlow / speeds[speeds.Count - 1], 15.5f));
                            left.Add(new Vector3(waypoints[waypoints.Count - 1].x - (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z + (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                            right.Add(new Vector3(waypoints[waypoints.Count - 1].x + (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z - (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                            turns.Add(turn);
                            waypoints.Add(new Vector3(waypoints[waypoints.Count - 1].x, drop - d, waypoints[waypoints.Count - 1].z));
                            speeds.Add(10 * (waypoints[waypoints.Count - 2].y - drop) / 16);
                            widths.Add(Mathf.Min(0.5f * sourceFlow / speeds[speeds.Count - 1], 15.5f));
                            left.Add(new Vector3(waypoints[waypoints.Count - 1].x - (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop - d, waypoints[waypoints.Count - 1].z + (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                            right.Add(new Vector3(waypoints[waypoints.Count - 1].x + (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop - d, waypoints[waypoints.Count - 1].z - (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                            StopCoroutine(initialGenerate);
                            WaterManager.waterManagerObject.GetComponent<WaterManager>().SpawnCaveRiver(new Vector3(waypoints[waypoints.Count - 2].x + source.x, source.y + waypoints[waypoints.Count - 2].y, waypoints[waypoints.Count - 2].z + source.z),this);
                            DoneForNow = true;
                            Render();
                            yield return null;
                        }
                    }
                }
                else if (drop - waypoints[waypoints.Count - 1].y > 0)
                { //in a depression, form a pond/lake
                    DoneForNow = true;
                    Render();
                    GameObject lake = Instantiate(lakePrefab);
                    lake.transform.position = waypoints[waypoints.Count - 1] + source;
                    endLake = lake.GetComponent<Lake>();
                    if (!endLake.sources.Contains(this))
                        endLake.sources.Add(this);
                    endLake.Generate();
                    riverGenLeader.transform.position = waypoints[waypoints.Count - 1] + source;
                    findJunction = FindJunction(riverGenLeader.touchedRiver);
                    StartCoroutine(findJunction);
                    JoinLake(riverGenLeader.touchedLake);
                    StopCoroutine(initialGenerate);
                    yield return null;
                }
                else
                { //normal situation, proceed
                }
            }
            riverGenLeader.transform.position = waypoints[waypoints.Count - 1] + source;
            yield return null;
        }
        findJunction = FindJunction(riverGenLeader.touchedRiver);
        StartCoroutine(findJunction);
        JoinLake(riverGenLeader.touchedLake);
        StopCoroutine(initialGenerate);
        Render();
        yield return null;
    }

    IEnumerator GenerateFromLake()
    {
        attempts += 1;
        if (attempts > 10)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
        var terrainGen = new TerrainGen();
        this.gameObject.transform.position = source;
        waypoints.Add(Vector3.zero);
        widths.Add(sourceFlow);
        float theta = 45f * Mathf.PI / 180f;
        float low;
        float height;
        float turn = 0;
        float moveX = 0;
        float moveZ = 0;
        float t, x, z;
        for (int c = 0; c < 10000; c++) // (true)
        {
            for (int i = 0; i < 10; i++)
            {
                low = 999999f;
                for (int j = 0; j < 8; j++)
                {
                    t = theta * j;
                    x = (radius * Mathf.Cos(t));
                    z = (radius * Mathf.Sin(t));
                    height = terrainGen.StoneHeight256(source.x + waypoints[waypoints.Count - 1].x + x, 0, source.z + waypoints[waypoints.Count - 1].z + z);
                    if (height < low)
                    {
                        turn = t;
                        low = height;
                        moveX = 16f * Mathf.Cos(turn);
                        moveZ = 16f * Mathf.Sin(turn);
                    }
                }
                turns.Add(turn);
                float drop = low - source.y; // - waypoints[waypoints.Count - 1].y + terrainGen.StoneHeight(source.x + waypoints[waypoints.Count - 1].x + moveX, 0, source.z + waypoints[waypoints.Count - 1].z + moveZ);
                if (drop - waypoints[waypoints.Count - 1].y > 0)
                {
                    DoneForNow = true;
                    //in a depression, form a pond/lake
                    GameObject lake = Instantiate(lakePrefab);
                    lake.transform.position = waypoints[waypoints.Count - 1] + source;
                    endLake = lake.GetComponent<Lake>();
                    if (!endLake.sources.Contains(this))
                        endLake.sources.Add(this);
                    endLake.Generate();
                    riverGenLeader.transform.position = waypoints[waypoints.Count - 1] + source;
                    findJunction = FindJunction(riverGenLeader.touchedRiver);
                    StartCoroutine(findJunction);
                    JoinLake(riverGenLeader.touchedLake);
                    StopCoroutine(generateFromLake);
                    Render();
                    yield return null;
                }

                waypoints.Add(new Vector3(waypoints[waypoints.Count - 1].x + moveX, drop, waypoints[waypoints.Count - 1].z + moveZ));
                speeds.Add(10 * (waypoints[waypoints.Count - 2].y - drop) / 16);
                widths.Add(Mathf.Min(0.5f * sourceFlow / speeds[speeds.Count - 1], 15.5f));
                left.Add(new Vector3(waypoints[waypoints.Count - 1].x + (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z + (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                right.Add(new Vector3(waypoints[waypoints.Count - 1].x - (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z - (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
            }
            riverGenLeader.transform.position = waypoints[waypoints.Count - 1] + source;
            yield return null;
        }
        findJunction = FindJunction(riverGenLeader.touchedRiver);
        StartCoroutine(findJunction);
        JoinLake(riverGenLeader.touchedLake);
        StopCoroutine(generateFromLake);
        Render();
        yield return null;
    }

    IEnumerator Generate()
    {
        attempts += 1;
        if (attempts > 10)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
        var terrainGen = new TerrainGen();
        float theta = 45f * Mathf.PI / 180f;
        float low;
        float height;
        float turn = 0;
        float moveX = 0;
        float moveZ = 0;
        float t, x, z;
        for (int c = 0; c < 10000; c++) // (true)
        {
            for (int i = 0; i < 10; i++)
            {
                low = 999999f;
                for (int j = 0; j < 8; j++)
                {
                    t = theta * j;
                    x = (radius * Mathf.Cos(t));
                    z = (radius * Mathf.Sin(t));
                    height = terrainGen.StoneHeight256(source.x + waypoints[waypoints.Count - 1].x + x, 0, source.z + waypoints[waypoints.Count - 1].z + z);
                    if (height < low)
                    {
                        turn = t;
                        low = height;
                        moveX = 16f * Mathf.Cos(turn);
                        moveZ = 16f * Mathf.Sin(turn);
                    }
                }
                turns.Add(turn);
                float drop = low - source.y; // - waypoints[waypoints.Count - 1].y + terrainGen.StoneHeight(source.x + waypoints[waypoints.Count - 1].x + moveX, 0, source.z + waypoints[waypoints.Count - 1].z + moveZ);
                if (drop - waypoints[waypoints.Count - 1].y > 0)
                {
                    //in a depression, form a pond/lake
                    DoneForNow = true;
                    GameObject lake = Instantiate(lakePrefab);
                    lake.transform.position = waypoints[waypoints.Count - 1] + source;
                    endLake = lake.GetComponent<Lake>();
                    if (!endLake.sources.Contains(this))
                        endLake.sources.Add(this);
                    endLake.Generate();
                    StopCoroutine(generate);
                    riverGenLeader.transform.position = waypoints[waypoints.Count - 1] + source;
                    findJunction = FindJunction(riverGenLeader.touchedRiver);
                    StartCoroutine(findJunction);
                    JoinLake(riverGenLeader.touchedLake);
                    StopCoroutine(initialGenerate);
                    Render();
                    yield return null;
                }

                waypoints.Add(new Vector3(waypoints[waypoints.Count - 1].x + moveX, drop, waypoints[waypoints.Count - 1].z + moveZ));
                speeds.Add(10 * (waypoints[waypoints.Count - 2].y - drop) / 16);
                widths.Add(Mathf.Min(0.5f * sourceFlow / speeds[speeds.Count - 1], 15.5f));
                left.Add(new Vector3(waypoints[waypoints.Count - 1].x + (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z + (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                right.Add(new Vector3(waypoints[waypoints.Count - 1].x - (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z - (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
            }
            riverGenLeader.transform.position = waypoints[waypoints.Count - 1];
            findJunction = FindJunction(riverGenLeader.touchedRiver);
            StartCoroutine(findJunction);
            JoinLake(riverGenLeader.touchedLake);
            StopCoroutine(initialGenerate);
            Render();
            yield return null;
        }
        Render();
        yield return null;
    }

    public void Extend()
    {
        StopCoroutine(initialGenerate);
        StopCoroutine(generate);
        var terrainGen = new TerrainGen();

        float extend = radius;
        float theta = 45f * Mathf.PI / 360f;
        float low;
        float height;
        float turn = 0;
        float moveX = 0;
        float moveZ = 0;
        float t, x, z;
        for (int c = 0; c < 10000; c++) // (true)
        {
            extend = extend * 2;
            low = 999999f;
            for (int j = 0; j < Mathf.RoundToInt(8 * extend / radius); j++)
            {
                t = theta * j;
                x = (extend * Mathf.Cos(t));
                z = (extend * Mathf.Sin(t));
                height = terrainGen.StoneHeight256(source.x + waypoints[waypoints.Count - 1].x + x, 0, source.z + waypoints[waypoints.Count - 1].z + z);
                if (height < low)
                {
                    turn = t;
                    low = height;
                    moveX = x;
                    moveZ = z;
                }
            }
            float drop = low - source.y; // - waypoints[waypoints.Count - 1].y + terrainGen.StoneHeight(source.x + waypoints[waypoints.Count - 1].x + moveX, 0, source.z + waypoints[waypoints.Count - 1].z + moveZ);
            if (drop - waypoints[waypoints.Count - 1].y > 0)
            {
                // no exits found, double search radius and halve search angle and try again;
            }
            else
            {
                turns.Add(turn);
                waypoints.Add(new Vector3(waypoints[waypoints.Count - 1].x + moveX, drop, waypoints[waypoints.Count - 1].z + moveZ));
                speeds.Add(10 * (waypoints[waypoints.Count - 2].y - drop) / extend);
                widths.Add(Mathf.Min(0.5f * sourceFlow / speeds[speeds.Count - 1], 15.5f));
                left.Add(new Vector3(waypoints[waypoints.Count - 1].x + (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z + (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                right.Add(new Vector3(waypoints[waypoints.Count - 1].x - (widths[widths.Count - 1] / 2) * Mathf.Sin(turn), drop, waypoints[waypoints.Count - 1].z - (widths[widths.Count - 1] / 2) * Mathf.Cos(turn)));
                StartCoroutine(generate);
                return;
            }
        }
        Render();
    }

    void Render()
    {
        foreach(RiverNode oldNode in nodes)
        {
            if(oldNode != null)
                Destroy(oldNode.gameObject);
        }
        nodes.Clear();
        MeshData fineMeshData, roughMeshData;
        GameObject newNodeObject;
        RiverNode newNode;
        for (int i = 0; i < left.Count - 1; i+= 100)
        {
            fineMeshData = new MeshData();
            roughMeshData = new MeshData();
            newNodeObject = Instantiate<GameObject>(nodePrefab, this.transform);
            newNode = newNodeObject.GetComponent<RiverNode>();
            newNode.startIdx = i;
            newNode.endIdx = i + Mathf.Min(100, left.Count - i);
            newNode.river = this;
            nodes.Add(newNode);
            if (i + 50 < waypoints.Count)
                newNodeObject.transform.localPosition = waypoints[i + 50];
            else
                newNodeObject.transform.localPosition = waypoints[i];
            for (int j = 0; j < 100 && (j + i < left.Count - 1); j++)
            {
                fineMeshData.AddVertex(right[i + j] - newNodeObject.transform.localPosition);
                fineMeshData.AddVertex(left[i + j] - newNodeObject.transform.localPosition);
                fineMeshData.AddVertex(left[i + j + 1] - newNodeObject.transform.localPosition);
                fineMeshData.AddVertex(right[i + j + 1] - newNodeObject.transform.localPosition);

                fineMeshData.AddQuadTriangles();
                fineMeshData.useRenderDataForCol = true;

                if (j % 10 == 0 && (i + j + 10) < left.Count)
                {
                    roughMeshData.AddVertex(right[i + j] - newNodeObject.transform.localPosition);
                    roughMeshData.AddVertex(left[i + j] - newNodeObject.transform.localPosition);
                    roughMeshData.AddVertex(left[i + j + 10] - newNodeObject.transform.localPosition);
                    roughMeshData.AddVertex(right[i + j + 10] - newNodeObject.transform.localPosition);

                    roughMeshData.AddQuadTriangles();
                }
            }
            newNode.RenderMeshFine(fineMeshData);
            newNode.RenderMeshRough(roughMeshData);
        }
    }

    IEnumerator FindJunction(River river)
    {
        for (int j = 0; j < 100; j++)
        {
            if (riverGenLeader.isTouchingRiver)
            {
                bool found = false;
                int i = 0;
                while (!found)
                {
                    if (riverGenLeader.isTouchingRiver == false)
                    {
                        found = true;
                        waypoints.RemoveRange(waypoints.Count - i, i);
                        right.RemoveRange(right.Count - i, i);
                        left.RemoveRange(left.Count - i, i);
                        widths.RemoveRange(widths.Count - i, i);
                        speeds.RemoveRange(speeds.Count - i, i);
                        turns.RemoveRange(turns.Count - i, i);
                        if (sourceFlow >= riverGenLeader.touchedRiver.sourceFlow)
                        {
                            riverGenLeader.touchedRiver.parentStem = this;
                            tributaries.Add(riverGenLeader.touchedRiver);
                            sourceFlow += riverGenLeader.touchedRiver.sourceFlow;
                            if (endLake != null)
                            {
                                endLake.Generate();
                            }
                            if (riverGenLeader.touchedRiver.endLake != null)
                            {
                                Destroy(riverGenLeader.touchedRiver.endLake.gameObject);
                            }
                            Render();
                            StopCoroutine(findJunction);
                            StartCoroutine(generate);
                            yield return null;
                        }
                        else
                        {
                            Render();
                            parentStem = riverGenLeader.touchedRiver;
                            riverGenLeader.touchedRiver.tributaries.Add(this);
                            riverGenLeader.touchedRiver.sourceFlow += sourceFlow;
                            if (riverGenLeader.touchedRiver.endLake != null)
                            {
                                riverGenLeader.touchedRiver.endLake.Generate();
                            }
                            if (endLake != null)
                            {
                                Destroy(endLake.gameObject);
                            }
                            StopCoroutine(findJunction);
                            yield return null;
                        }
                        break;
                    }
                    i++;
                    riverGenLeader.transform.position = source + waypoints[waypoints.Count - i];
                    yield return null;
                }
                StopCoroutine(findJunction);
                yield return null;
            }
        }
        yield return null;
    }

    void JoinLake(Lake lake)
    {
        if (riverGenLeader.isTouchingLake && riverGenLeader.touchedLake != endLake)
        {
            Destroy(endLake.gameObject);
            endLake = riverGenLeader.touchedLake;
            if (!endLake.sources.Contains(this))
                endLake.sources.Add(this);
            endLake.StopAllCoroutines();
            endLake.Generate();
            Destroy(riverGenLeader.gameObject);
        }
    }

    private bool doneForNow = false;
    public bool DoneForNow { get; set; }

    private void OnDestroy()
    {
        waterManager.rivers.Remove(this);
        waterManager.riversHash.Remove(this);
    }
}