using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lake : MonoBehaviour
{
    public bool settled = false;
    MeshFilter filter;
    MeshCollider coll;
    public Vector3 source;
    public List<River> sources, outlets;
    public float depth, volume, size, maxSize;
    HashSet<Vector2> grid = new HashSet<Vector2>(); // hashset for O(1) existence checking
    public List<Vector2> gridList = new List<Vector2>(); // list for ordered insertion
    TerrainGen terrainGen;
    public int gridStep = 1;
    public int rewind = 0;
    public Vector2 lead = Vector2.zero;
    public Vector2 check;
    IEnumerator coroutineA,coroutineB;
    private void Awake()
    {
        terrainGen = new TerrainGen();
        coll = gameObject.GetComponent<MeshCollider>();
        filter = gameObject.GetComponent<MeshFilter>();
    }

    private void Start()
    {
        WaterManager.waterManagerObject.GetComponent<WaterManager>().lakes.Add(this);
    }

    public void Generate()
    {
        StopAllCoroutines();
        foreach(River outlet in outlets)
        {
            if(outlet.endLake != null)
            {
                outlet.endLake.sources.Remove(outlet);
                outlet.endLake.Generate();
            }
            if(outlet.parentStem != null)
            {
                outlet.parentStem.tributaries.Remove(outlet);
                outlet.parentStem.sourceFlow -= outlet.sourceFlow;
                outlet.parentStem.StartCoroutine(outlet.generate);
            }
            Destroy(outlet.gameObject);
        }
        source = this.transform.position;
        depth = 0;
        maxSize = 0;
        foreach (River river in sources)
        {
            maxSize += 0.005f * river.sourceFlow * 3600 * 24 * 30; //evaporation 5mm per month
        }
        if (maxSize == 0)
        {
            Destroy(this.gameObject);
            return;
        }
        if (maxSize < 5000)
            gridStep = 1;
        else if (maxSize < 10000)
            gridStep = 2; 
        else if (maxSize < 20000)
            gridStep = 4;
        else if (maxSize < 40000)
            gridStep = 8;
        else if (maxSize < 80000)
            gridStep = 16;
        else if (maxSize < 160000)
            gridStep = 32;
        else
            gridStep = 64;

        coroutineA = VolumeFill();
        coroutineB = FinalFill();
        StartCoroutine(coroutineA);
    }

    private void OnDestroy()
    {
        WaterManager.waterManagerObject.GetComponent<WaterManager>().lakes.Remove(this);
    }

    IEnumerator VolumeFill()
    {
        bool done = false;
        for (int i = 0; i < 100; i++)
        {
            if (!done)
            {
                grid.Clear();
                gridList.Clear();
                grid.Add(lead);
                gridList.Add(lead);
                size = 0;
                if (i == 0)
                    depth = .0625f;
                else
                    depth *= 2;
                if (depth >= 1)
                    depth = Mathf.RoundToInt(depth);
                Vector2 facing = new Vector2(0, 1);
                Vector2 left, right;
                while (size < maxSize)
                {
                    for (int j = 0; j < 500; j++)
                    {
                        left = new Vector2(-facing.y, facing.x);
                        check = lead + (gridStep * left);
                        if (!grid.Contains(lead + (gridStep * left)))
                        {
                            if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * left.x, 0, source.z + lead.y + gridStep * left.y) < source.y + depth)
                            {
                                size += gridStep * gridStep;
                                lead = lead + (left * gridStep);
                                facing = left;
                                rewind = 0;
                                grid.Add(lead);
                                gridList.Add(lead);
                                continue;
                            }
                            else if (!grid.Contains(lead + gridStep * facing))
                            {
                                if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * facing.x, 0, source.z + lead.y + gridStep * facing.y) < source.y + depth)
                                {
                                    size += gridStep * gridStep;
                                    lead = lead + gridStep * facing;
                                    rewind = 0;
                                    grid.Add(lead);
                                    gridList.Add(lead);
                                    continue;
                                }
                                else
                                {
                                    right = new Vector2(facing.y, -facing.x);
                                    if (!grid.Contains(lead + gridStep * right))
                                    {
                                        if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
                                        {
                                            size += gridStep * gridStep;
                                            lead = lead + gridStep * right;
                                            facing = right;
                                            rewind = 0;
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            continue;
                                        }
                                        else
                                        {
                                            rewind++;
                                            if (rewind >= grid.Count)
                                            {
                                                Render();
                                                break;
                                            }
                                            lead = gridList[gridList.Count - rewind];
                                        }
                                    }
                                    else
                                    {
                                        rewind++;
                                        if (rewind >= grid.Count)
                                        {
                                            Render();
                                            break;
                                        }
                                        lead = gridList[gridList.Count - rewind];
                                    }
                                }
                            }
                            else
                            {
                                right = new Vector2(facing.y, -facing.x);
                                if (!grid.Contains(lead + gridStep * right))
                                {
                                    if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
                                    {
                                        size += gridStep * gridStep;
                                        lead = lead + gridStep * right;
                                        facing = right;
                                        rewind = 0;
                                        grid.Add(lead);
                                        gridList.Add(lead);
                                        continue;
                                    }
                                    else
                                    {
                                        rewind++;
                                        if (rewind >= grid.Count)
                                        {
                                            Render();
                                            break;
                                        }
                                        lead = gridList[gridList.Count - rewind];
                                    }
                                }
                                else
                                {
                                    rewind++;
                                    if (rewind >= grid.Count)
                                    {
                                        Render();
                                        break;
                                    }
                                    lead = gridList[gridList.Count - rewind];
                                }
                            }
                        }
                        else if (!grid.Contains(lead + gridStep * facing))
                        {
                            if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * facing.x, 0, source.z + lead.y + gridStep * facing.y) < source.y + depth)
                            {
                                size += gridStep * gridStep;
                                lead = lead + gridStep * facing;
                                rewind = 0;
                                grid.Add(lead);
                                gridList.Add(lead);
                                continue;
                            }
                            else
                            {
                                right = new Vector2(facing.y, -facing.x);
                                if (!grid.Contains(lead + gridStep * right))
                                {
                                    if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
                                    {
                                        size += gridStep * gridStep;
                                        lead = lead + gridStep * right;
                                        facing = right;
                                        rewind = 0;
                                        grid.Add(lead);
                                        gridList.Add(lead);
                                        continue;
                                    }
                                    else
                                    {
                                        rewind++;
                                        if (rewind >= grid.Count)
                                        {
                                            Render();
                                            break;
                                        }
                                        lead = gridList[gridList.Count - rewind];
                                    }
                                }
                                else
                                {
                                    rewind++;
                                    if (rewind >= grid.Count)
                                    {
                                        Render();
                                        break;
                                    }
                                    lead = gridList[gridList.Count - rewind];
                                }
                            }
                        }
                        else
                        {
                            right = new Vector2(facing.y, -facing.x);
                            if (!grid.Contains(lead + gridStep * right))
                            {
                                if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
                                {
                                    size += gridStep * gridStep;
                                    lead = lead + gridStep * right;
                                    facing = right;
                                    rewind = 0;
                                    grid.Add(lead);
                                    gridList.Add(lead);
                                    continue;
                                }
                                else
                                {
                                    rewind++;
                                    if (rewind >= grid.Count)
                                    {
                                        Render();
                                        break;
                                    }
                                    lead = gridList[gridList.Count - rewind];
                                }
                            }
                            else
                            {
                                rewind++;
                                if (rewind >= grid.Count)
                                {
                                    Render();
                                    break;
                                }
                                lead = gridList[gridList.Count - rewind];
                            }
                        }
                    }
                    if (size > maxSize)
                    {
                        StopAllCoroutines();
                        done = true;
                        //coroutine = VolumeFill();
                        StartCoroutine("FinalFill");
                        yield return null;
                    }
                    else if (rewind >= grid.Count)
                    {
                        Render();
                        break;
                    }
                    yield return null;
                }
                if (size > maxSize)
                {
                    Render();
                    StopAllCoroutines();
                    done = true;

                    //coroutine = VolumeFill();
                    StartCoroutine("FinalFill");
                    yield return null;
                }
            }
        }
    }

    IEnumerator FinalFill()
    {
        StopCoroutine(coroutineA);


        //else if ((depth - 2) < 0.1)
        //{
        //    lead = Vector2.zero;
        //    depth = 1;
        //    grid.Clear();
        //    gridList.Clear();
        //    grid.Add(lead);
        //    gridList.Add(lead);
        //    size = 0;
        //    Vector2 facing = new Vector2(0, 1);
        //    Vector2 left, right;
        //    bool notDone = true;
        //    while (notDone)
        //    {
        //        for (int j = 0; j < 100; j++)
        //        {
        //            left = new Vector2(-facing.y, facing.x);
        //            check = lead + (gridStep * left);
        //            if (!grid.Contains(lead + (gridStep * left)))
        //            {
        //                if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * left.x, 0, source.z + lead.y + gridStep * left.y) < source.y + depth)
        //                {
        //                    size += gridStep * gridStep;
        //                    lead = lead + (left * gridStep);
        //                    facing = left;
        //                    rewind = 0;
        //                    grid.Add(lead);
        //                    gridList.Add(lead);
        //                    continue;
        //                }
        //                else if (!grid.Contains(lead + gridStep * facing))
        //                {
        //                    if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * facing.x, 0, source.z + lead.y + gridStep * facing.y) < source.y + depth)
        //                    {
        //                        size += gridStep * gridStep;
        //                        lead = lead + gridStep * facing;
        //                        rewind = 0;
        //                        grid.Add(lead);
        //                        gridList.Add(lead);
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        right = new Vector2(facing.y, -facing.x);
        //                        if (!grid.Contains(lead + gridStep * right))
        //                        {
        //                            if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
        //                            {
        //                                size += gridStep * gridStep;
        //                                lead = lead + gridStep * right;
        //                                facing = right;
        //                                rewind = 0;
        //                                grid.Add(lead);
        //                                gridList.Add(lead);
        //                                continue;
        //                            }
        //                            else
        //                            {
        //                                rewind++;
        //                                if (rewind >= grid.Count)
        //                                {
        //                                    notDone = false;
        //                                    Render();
        //                                    break;
        //                                }
        //                                lead = gridList[gridList.Count - rewind];
        //                            }
        //                        }
        //                        else
        //                        {
        //                            rewind++;
        //                            if (rewind >= grid.Count)
        //                            {
        //                                notDone = false;
        //                                Render();
        //                                break;
        //                            }
        //                            lead = gridList[gridList.Count - rewind];
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    right = new Vector2(facing.y, -facing.x);
        //                    if (!grid.Contains(lead + gridStep * right))
        //                    {
        //                        if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
        //                        {
        //                            size += gridStep * gridStep;
        //                            lead = lead + gridStep * right;
        //                            facing = right;
        //                            rewind = 0;
        //                            grid.Add(lead);
        //                            gridList.Add(lead);
        //                            continue;
        //                        }
        //                        else
        //                        {
        //                            rewind++;
        //                            if (rewind >= grid.Count)
        //                            {
        //                                notDone = false;
        //                                Render();
        //                                break;
        //                            }
        //                            lead = gridList[gridList.Count - rewind];
        //                        }
        //                    }
        //                    else
        //                    {
        //                        rewind++;
        //                        if (rewind >= grid.Count)
        //                        {
        //                            notDone = false;
        //                            Render();
        //                            break;
        //                        }
        //                        lead = gridList[gridList.Count - rewind];
        //                    }
        //                }
        //            }
        //            else if (!grid.Contains(lead + gridStep * facing))
        //            {
        //                if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * facing.x, 0, source.z + lead.y + gridStep * facing.y) < source.y + depth)
        //                {
        //                    size += gridStep * gridStep;
        //                    lead = lead + gridStep * facing;
        //                    rewind = 0;
        //                    grid.Add(lead);
        //                    gridList.Add(lead);
        //                    continue;
        //                }
        //                else
        //                {
        //                    right = new Vector2(facing.y, -facing.x);
        //                    if (!grid.Contains(lead + gridStep * right))
        //                    {
        //                        if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
        //                        {
        //                            size += gridStep * gridStep;
        //                            lead = lead + gridStep * right;
        //                            facing = right;
        //                            rewind = 0;
        //                            grid.Add(lead);
        //                            gridList.Add(lead);
        //                            continue;
        //                        }
        //                        else
        //                        {
        //                            rewind++;
        //                            if (rewind >= grid.Count)
        //                            {
        //                                notDone = false;
        //                                Render();
        //                                break;
        //                            }
        //                            lead = gridList[gridList.Count - rewind];
        //                        }
        //                    }
        //                    else
        //                    {
        //                        rewind++;
        //                        if (rewind >= grid.Count)
        //                        {
        //                            notDone = false;
        //                            Render();
        //                            break;
        //                        }
        //                        lead = gridList[gridList.Count - rewind];
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                right = new Vector2(facing.y, -facing.x);
        //                if (!grid.Contains(lead + gridStep * right))
        //                {
        //                    if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
        //                    {
        //                        size += gridStep * gridStep;
        //                        lead = lead + gridStep * right;
        //                        facing = right;
        //                        rewind = 0;
        //                        grid.Add(lead);
        //                        gridList.Add(lead);
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        rewind++;
        //                        if (rewind >= grid.Count)
        //                        {
        //                            notDone = false;
        //                            Render();
        //                            break;
        //                        }
        //                        lead = gridList[gridList.Count - rewind];
        //                    }
        //                }
        //                else
        //                {
        //                    rewind++;
        //                    if (rewind >= grid.Count)
        //                    {
        //                        notDone = false;
        //                        Render();
        //                        break;
        //                    }
        //                    lead = gridList[gridList.Count - rewind];
        //                }
        //            }
        //        }
        //        StopCoroutine(coroutineB);
        //        yield return null;
        //    }
        //}
        //else
        //{
        lead = Vector2.zero;
        grid.Clear();
        gridList.Clear();
        grid.Add(lead);
        gridList.Add(lead);
        size = 0;
        bool notDone = true;
        float lastDepth = depth;
        float highDepth = depth / 2;
        if (depth < .065f)
        {
            if (maxSize < 15000)
                gridStep = 1;
            else if (maxSize < 60000)
                gridStep = 2;
            else if (maxSize < 240000)
                gridStep = 4;
            else if (maxSize < 960000)
                gridStep = 8;
            else if (maxSize < 3840000)
                gridStep = 16;
            else
                gridStep = 64;
            highDepth = 0;
        }
        depth = 0.75f * depth;
        Vector2 facing = new Vector2(0, 1);
        Vector2 left, right;
        while (notDone)
    {
            if(depth < .005f)
            {
                foreach(River river in sources)
                {
                    river.Extend();
                }
                Destroy(this.gameObject);
            }
            for (int j = 0; j < 500; j++)
            {
                if (size > maxSize)
                {
                    if (depth > lastDepth)
                    {
                        float temp = depth;
                        depth = (depth + lastDepth) / 2f;
                        lastDepth = temp;
                    }
                    else if (depth < lastDepth)
                    {
                        float temp = depth;
                        depth = (highDepth + depth) / 2f;
                        lastDepth = temp;
                    }
                    lead = Vector2.zero;
                    grid.Clear();
                    gridList.Clear();
                    grid.Add(lead);
                    gridList.Add(lead);
                    size = 0;
                    Render();
                }
                else if (rewind >= grid.Count)
                {
                    RaycastHit hit;
                    LayerMask layerMask = 4;
                    if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                    {
                        var lake = hit.collider.gameObject.GetComponent<Lake>();
                        if (lake != null)
                        {
                            if (lake.size >= size)
                            {
                                foreach (River river in sources)
                                {
                                    if (!lake.sources.Contains(river))
                                        lake.sources.Add(river);
                                    river.endLake = lake;
                                }
                                lake.Generate();
                                Destroy(this.gameObject);
                            }
                            else
                            {
                                foreach (River river in lake.sources)
                                {
                                    if (!sources.Contains(river))
                                        sources.Add(river);
                                    river.endLake = this;
                                }
                                Destroy(lake.gameObject);
                                StopCoroutine(coroutineB);
                                Generate();
                            }
                        }
                    }
                    if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                    {
                        var lake = hit.collider.gameObject.GetComponent<Lake>();
                        if (lake != null)
                        {
                            if (lake.size > size)
                            {
                                foreach (River river in sources)
                                {
                                    if (!lake.sources.Contains(river))
                                        lake.sources.Add(river);
                                    river.endLake = lake;
                                }
                                lake.Generate();
                                Destroy(this.gameObject);
                            }
                            else
                            {
                                foreach (River river in lake.sources)
                                {
                                    if (!sources.Contains(river))
                                        sources.Add(river);
                                    river.endLake = this;
                                }
                                Destroy(lake.gameObject);
                                StopCoroutine(coroutineB);
                                Generate();
                            }
                        }
                    }
                    if (size < 0.25f * maxSize)
                    {
                        rewind = 0;
                        if (gridStep >= 2)
                        {
                            if (size < 5000)
                            {
                                gridStep = 1;
                                grid.Clear();
                                gridList.Clear();
                                grid.Add(lead);
                                gridList.Add(lead);
                                size = 0;
                                continue;
                            }
                            else if (size < 10000 && gridStep >= 4)
                            {
                                gridStep = 2;
                                grid.Clear();
                                gridList.Clear();
                                grid.Add(lead);
                                gridList.Add(lead);
                                size = 0;
                                continue;
                            }
                            else if (size < 20000 && gridStep >= 8)
                            {
                                gridStep = 4;
                                grid.Clear();
                                gridList.Clear();
                                grid.Add(lead);
                                gridList.Add(lead);
                                size = 0;
                                continue;
                            }
                            else if (size < 40000 && gridStep >= 16)
                            {
                                gridStep = 8;
                                grid.Clear();
                                gridList.Clear();
                                grid.Add(lead);
                                gridList.Add(lead);
                                size = 0;
                                continue;
                            }
                            else if (size < 80000 && gridStep >= 32)
                            {
                                gridStep = 16;
                                grid.Clear();
                                gridList.Clear();
                                grid.Add(lead);
                                gridList.Add(lead);
                                size = 0;
                                continue;
                            }
                            else if (size < 160000 && gridStep >= 64)
                            {
                                gridStep = 32;
                                grid.Clear();
                                gridList.Clear();
                                grid.Add(lead);
                                gridList.Add(lead);
                                size = 0;
                                continue;
                            }
                        }
                        SpawnOutlet();
                    }
                    settled = true;
                    notDone = false;
                    Render();
                    StopCoroutine(coroutineB);
                    yield return null;
                }
                left = new Vector2(-facing.y, facing.x);
                check = lead + (gridStep * left);
                if (!grid.Contains(lead + (gridStep * left)))
                {
                    if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * left.x, 0, source.z + lead.y + gridStep * left.y) < source.y + depth)
                    {
                        size += gridStep * gridStep;
                        lead = lead + (left * gridStep);
                        facing = left;
                        rewind = 0;
                        grid.Add(lead);
                        gridList.Add(lead);
                        continue;
                    }
                    else if (!grid.Contains(lead + gridStep * facing))
                    {
                        if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * facing.x, 0, source.z + lead.y + gridStep * facing.y) < source.y + depth)
                        {
                            size += gridStep * gridStep;
                            lead = lead + gridStep * facing;
                            rewind = 0;
                            grid.Add(lead);
                            gridList.Add(lead);
                            continue;
                        }
                        else
                        {
                            right = new Vector2(facing.y, -facing.x);
                            if (!grid.Contains(lead + gridStep * right))
                            {
                                if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
                                {
                                    size += gridStep * gridStep;
                                    lead = lead + gridStep * right;
                                    facing = right;
                                    rewind = 0;
                                    grid.Add(lead);
                                    gridList.Add(lead);
                                    continue;
                                }
                                else
                                {
                                    rewind++;
                                    if (rewind > grid.Count)
                                    {
                                        RaycastHit hit;
                                        LayerMask layerMask = 4;
                                        if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                                        {
                                            var lake = hit.collider.gameObject.GetComponent<Lake>();
                                            if (lake != null)
                                            {
                                                if (lake.size >= size)
                                                {
                                                    foreach (River river in sources)
                                                    {
                                                        if (!lake.sources.Contains(river))
                                                            lake.sources.Add(river);
                                                        river.endLake = lake;
                                                    }
                                                    lake.Generate();
                                                    Destroy(this.gameObject);
                                                }
                                                else
                                                {
                                                    foreach (River river in lake.sources)
                                                    {
                                                        if (!sources.Contains(river))
                                                            sources.Add(river);
                                                        river.endLake = this;
                                                    }
                                                    Destroy(lake.gameObject);
                                                    StopCoroutine(coroutineB);
                                                    Generate();
                                                }
                                            }
                                        }
                                        if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                                        {
                                            var lake = hit.collider.gameObject.GetComponent<Lake>();
                                            if (lake != null)
                                            {
                                                if (lake.size > size)
                                                {
                                                    foreach (River river in sources)
                                                    {
                                                        if (!lake.sources.Contains(river))
                                                            lake.sources.Add(river);
                                                        river.endLake = lake;
                                                    }
                                                    lake.Generate();
                                                    Destroy(this.gameObject);
                                                }
                                                else
                                                {
                                                    foreach (River river in lake.sources)
                                                    {
                                                        if (!sources.Contains(river))
                                                            sources.Add(river);
                                                        river.endLake = this;
                                                    }
                                                    Destroy(lake.gameObject);
                                                    StopCoroutine(coroutineB);
                                                    Generate();
                                                }
                                            }
                                        }
                                        if (size < 0.25f * maxSize)
                                        {
                                            rewind = 0;
                                            if (gridStep >= 2)
                                            {
                                                if (size < 5000)
                                                {
                                                    gridStep = 1;
                                                    grid.Clear();
                                                    gridList.Clear();
                                                    grid.Add(lead);
                                                    gridList.Add(lead);
                                                    size = 0;
                                                    continue;
                                                }
                                                else if (size < 10000 && gridStep >= 4)
                                                {
                                                    gridStep = 2;
                                                    grid.Clear();
                                                    gridList.Clear();
                                                    grid.Add(lead);
                                                    gridList.Add(lead);
                                                    size = 0;
                                                    continue;
                                                }
                                                else if (size < 20000 && gridStep >= 8)
                                                {
                                                    gridStep = 4;
                                                    grid.Clear();
                                                    gridList.Clear();
                                                    grid.Add(lead);
                                                    gridList.Add(lead);
                                                    size = 0;
                                                    continue;
                                                }
                                                else if (size < 40000 && gridStep >= 16)
                                                {
                                                    gridStep = 8;
                                                    grid.Clear();
                                                    gridList.Clear();
                                                    grid.Add(lead);
                                                    gridList.Add(lead);
                                                    size = 0;
                                                    continue;
                                                }
                                                else if (size < 80000 && gridStep >= 32)
                                                {
                                                    gridStep = 16;
                                                    grid.Clear();
                                                    gridList.Clear();
                                                    grid.Add(lead);
                                                    gridList.Add(lead);
                                                    size = 0;
                                                    continue;
                                                }
                                                else if (size < 160000 && gridStep >= 64)
                                                {
                                                    gridStep = 32;
                                                    grid.Clear();
                                                    gridList.Clear();
                                                    grid.Add(lead);
                                                    gridList.Add(lead);
                                                    size = 0;
                                                    continue;
                                                }
                                            }
                                            SpawnOutlet();
                                        }
                                        settled = true;
                                        StopCoroutine(coroutineB);
                                        notDone = false;
                                        Render();
                                        yield return null;
                                    }
                                    if(rewind > 0 && rewind < gridList.Count + 1)
                                        lead = gridList[gridList.Count - rewind];
                                }
                            }
                            else
                            {
                                rewind++;
                                if (rewind > grid.Count)
                                {
                                    RaycastHit hit;
                                    LayerMask layerMask = 4;
                                    if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                                    {
                                        var lake = hit.collider.gameObject.GetComponent<Lake>();
                                        if (lake != null)
                                        {
                                            if (lake.size >= size)
                                            {
                                                foreach (River river in sources)
                                                {
                                                    if (!lake.sources.Contains(river))
                                                        lake.sources.Add(river);
                                                    river.endLake = lake;
                                                }
                                                lake.Generate();
                                                Destroy(this.gameObject);
                                            }
                                            else
                                            {
                                                foreach (River river in lake.sources)
                                                {
                                                    if (!sources.Contains(river))
                                                        sources.Add(river);
                                                    river.endLake = this;
                                                }
                                                Destroy(lake.gameObject);
                                                StopCoroutine(coroutineB);
                                                Generate();
                                            }
                                        }
                                    }
                                    if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                                    {
                                        var lake = hit.collider.gameObject.GetComponent<Lake>();
                                        if (lake != null)
                                        {
                                            if (lake.size > size)
                                            {
                                                foreach (River river in sources)
                                                {
                                                    if (!lake.sources.Contains(river))
                                                        lake.sources.Add(river);
                                                    river.endLake = lake;
                                                }
                                                lake.Generate();
                                                Destroy(this.gameObject);
                                            }
                                            else
                                            {
                                                foreach (River river in lake.sources)
                                                {
                                                    if (!sources.Contains(river))
                                                        sources.Add(river);
                                                    river.endLake = this;
                                                }
                                                Destroy(lake.gameObject);
                                                StopCoroutine(coroutineB);
                                                Generate();
                                            }
                                        }
                                    }
                                    if (size < 0.25f * maxSize)
                                    {
                                        rewind = 0;
                                        if (gridStep >= 2)
                                        {
                                            if (size < 5000)
                                            {
                                                gridStep = 1;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 10000 && gridStep >= 4)
                                            {
                                                gridStep = 2;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 20000 && gridStep >= 8)
                                            {
                                                gridStep = 4;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 40000 && gridStep >= 16)
                                            {
                                                gridStep = 8;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 80000 && gridStep >= 32)
                                            {
                                                gridStep = 16;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 160000 && gridStep >= 64)
                                            {
                                                gridStep = 32;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                        }
                                        SpawnOutlet();
                                    }
                                    settled = true;
                                    StopCoroutine(coroutineB);
                                    notDone = false;
                                    Render();
                                    yield return null;
                                }
                                if (rewind > 0 && rewind < gridList.Count + 1)

                                    lead = gridList[gridList.Count - rewind];
                            }
                        }
                    }
                    else
                    {
                        right = new Vector2(facing.y, -facing.x);
                        if (!grid.Contains(lead + gridStep * right))
                        {
                            if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
                            {
                                size += gridStep * gridStep;
                                lead = lead + gridStep * right;
                                facing = right;
                                rewind = 0;
                                grid.Add(lead);
                                gridList.Add(lead);
                                continue;
                            }
                            else
                            {
                                rewind++;
                                if (rewind > grid.Count)
                                {
                                    RaycastHit hit;
                                    LayerMask layerMask = 4;
                                    if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                                    {
                                        var lake = hit.collider.gameObject.GetComponent<Lake>();
                                        if (lake != null)
                                        {
                                            if (lake.size >= size)
                                            {
                                                foreach (River river in sources)
                                                {
                                                    if (!lake.sources.Contains(river))
                                                        lake.sources.Add(river);
                                                    river.endLake = lake;
                                                }
                                                lake.Generate();
                                                Destroy(this.gameObject);
                                            }
                                            else
                                            {
                                                foreach (River river in lake.sources)
                                                {
                                                    if (!sources.Contains(river))
                                                        sources.Add(river);
                                                    river.endLake = this;
                                                }
                                                Destroy(lake.gameObject);
                                                StopCoroutine(coroutineB);
                                                Generate();
                                            }
                                        }
                                    }
                                    if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                                    {
                                        var lake = hit.collider.gameObject.GetComponent<Lake>();
                                        if (lake != null)
                                        {
                                            if (lake.size > size)
                                            {
                                                foreach (River river in sources)
                                                {
                                                    if (!lake.sources.Contains(river))
                                                        lake.sources.Add(river);
                                                    river.endLake = lake;
                                                }
                                                lake.Generate();
                                                Destroy(this.gameObject);
                                            }
                                            else
                                            {
                                                foreach (River river in lake.sources)
                                                {
                                                    if (!sources.Contains(river))
                                                        sources.Add(river);
                                                    river.endLake = this;
                                                }
                                                Destroy(lake.gameObject);
                                                StopCoroutine(coroutineB);
                                                Generate();
                                            }
                                        }
                                    }
                                    if (size < 0.25f * maxSize)
                                    {
                                        rewind = 0;
                                        if (gridStep >= 2)
                                        {
                                            if (size < 5000)
                                            {
                                                gridStep = 1;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 10000 && gridStep >= 4)
                                            {
                                                gridStep = 2;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 20000 && gridStep >= 8)
                                            {
                                                gridStep = 4;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 40000 && gridStep >= 16)
                                            {
                                                gridStep = 8;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 80000 && gridStep >= 32)
                                            {
                                                gridStep = 16;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 160000 && gridStep >= 64)
                                            {
                                                gridStep = 32;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                        }
                                        SpawnOutlet();
                                    }
                                    settled = true;
                                    StopCoroutine(coroutineB);
                                    notDone = false;
                                    Render();
                                    yield return null;
                                }
                                if (rewind > 0 && rewind < gridList.Count + 1)
                                    lead = gridList[gridList.Count - rewind];
                            }
                        }
                        else
                        {
                            rewind++;
                            if (rewind > grid.Count)
                            {
                                RaycastHit hit;
                                LayerMask layerMask = 4;
                                if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                                {
                                    var lake = hit.collider.gameObject.GetComponent<Lake>();
                                    if (lake != null)
                                    {
                                        if (lake.size >= size)
                                        {
                                            foreach (River river in sources)
                                            {
                                                if (!lake.sources.Contains(river))
                                                    lake.sources.Add(river);
                                                river.endLake = lake;
                                            }
                                            lake.Generate();
                                            Destroy(this.gameObject);
                                        }
                                        else
                                        {
                                            foreach (River river in lake.sources)
                                            {
                                                if (!sources.Contains(river))
                                                    sources.Add(river);
                                                river.endLake = this;
                                            }
                                            Destroy(lake.gameObject);
                                            StopCoroutine(coroutineB);
                                            Generate();
                                        }
                                    }
                                }
                                if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                                {
                                    var lake = hit.collider.gameObject.GetComponent<Lake>();
                                    if (lake != null)
                                    {
                                        if (lake.size > size)
                                        {
                                            foreach (River river in sources)
                                            {
                                                if (!lake.sources.Contains(river))
                                                    lake.sources.Add(river);
                                                river.endLake = lake;
                                            }
                                            lake.Generate();
                                            Destroy(this.gameObject);
                                        }
                                        else
                                        {
                                            foreach (River river in lake.sources)
                                            {
                                                if (!sources.Contains(river))
                                                    sources.Add(river);
                                                river.endLake = this;
                                            }
                                            Destroy(lake.gameObject);
                                            StopCoroutine(coroutineB);
                                            Generate();
                                        }
                                    }
                                }
                                if (size < 0.25f * maxSize)
                                {
                                    rewind = 0;
                                    if (gridStep >= 2)
                                    {
                                        if (size < 5000)
                                        {
                                            gridStep = 1;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 10000 && gridStep >= 4)
                                        {
                                            gridStep = 2;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 20000 && gridStep >= 8)
                                        {
                                            gridStep = 4;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 40000 && gridStep >= 16)
                                        {
                                            gridStep = 8;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 80000 && gridStep >= 32)
                                        {
                                            gridStep = 16;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 160000 && gridStep >= 64)
                                        {
                                            gridStep = 32;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                    }
                                    SpawnOutlet();
                                }
                                settled = true;
                                StopCoroutine(coroutineB);
                                notDone = false;
                                Render();
                                yield return null;
                            }
                            if (rewind > 0 && rewind < gridList.Count + 1)
                                lead = gridList[gridList.Count - rewind];
                        }
                    }
                }
                else if (!grid.Contains(lead + gridStep * facing))
                {
                    if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * facing.x, 0, source.z + lead.y + gridStep * facing.y) < source.y + depth)
                    {
                        size += gridStep * gridStep;
                        lead = lead + gridStep * facing;
                        rewind = 0;
                        grid.Add(lead);
                        gridList.Add(lead);
                        continue;
                    }
                    else
                    {
                        right = new Vector2(facing.y, -facing.x);
                        if (!grid.Contains(lead + gridStep * right))
                        {
                            if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
                            {
                                size += gridStep * gridStep;
                                lead = lead + gridStep * right;
                                facing = right;
                                rewind = 0;
                                grid.Add(lead);
                                gridList.Add(lead);
                                continue;
                            }
                            else
                            {
                                rewind++;
                                if (rewind > grid.Count)
                                {
                                    RaycastHit hit;
                                    LayerMask layerMask = 4;
                                    if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                                    {
                                        var lake = hit.collider.gameObject.GetComponent<Lake>();
                                        if (lake != null)
                                        {
                                            if (lake.size >= size)
                                            {
                                                foreach (River river in sources)
                                                {
                                                    if (!lake.sources.Contains(river))
                                                        lake.sources.Add(river);
                                                    river.endLake = lake;
                                                }
                                                lake.Generate();
                                                Destroy(this.gameObject);
                                            }
                                            else
                                            {
                                                foreach (River river in lake.sources)
                                                {
                                                    if (!sources.Contains(river))
                                                        sources.Add(river);
                                                    river.endLake = this;
                                                }
                                                Destroy(lake.gameObject);
                                                StopCoroutine(coroutineB);
                                                Generate();
                                            }
                                        }
                                    }
                                    if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                                    {
                                        var lake = hit.collider.gameObject.GetComponent<Lake>();
                                        if (lake != null)
                                        {
                                            if (lake.size > size)
                                            {
                                                foreach (River river in sources)
                                                {
                                                    if (!lake.sources.Contains(river))
                                                        lake.sources.Add(river);
                                                    river.endLake = lake;
                                                }
                                                lake.Generate();
                                                Destroy(this.gameObject);
                                            }
                                            else
                                            {
                                                foreach (River river in lake.sources)
                                                {
                                                    if (!sources.Contains(river))
                                                        sources.Add(river);
                                                    river.endLake = this;
                                                }
                                                Destroy(lake.gameObject);
                                                StopCoroutine(coroutineB);
                                                Generate();
                                            }
                                        }
                                    }
                                    if (size < 0.25f * maxSize)
                                    {
                                        rewind = 0;
                                        if (gridStep >= 2)
                                        {
                                            if (size < 5000)
                                            {
                                                gridStep = 1;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 10000 && gridStep >= 4)
                                            {
                                                gridStep = 2;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 20000 && gridStep >= 8)
                                            {
                                                gridStep = 4;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 40000 && gridStep >= 16)
                                            {
                                                gridStep = 8;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 80000 && gridStep >= 32)
                                            {
                                                gridStep = 16;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                            else if (size < 160000 && gridStep >= 64)
                                            {
                                                gridStep = 32;
                                                grid.Clear();
                                                gridList.Clear();
                                                grid.Add(lead);
                                                gridList.Add(lead);
                                                size = 0;
                                                continue;
                                            }
                                        }
                                        SpawnOutlet();
                                    }
                                    settled = true;
                                    StopCoroutine(coroutineB);
                                    notDone = false;
                                    Render();
                                    yield return null;
                                }
                                lead = gridList[gridList.Count - rewind];
                            }
                        }
                        else
                        {
                            rewind++;
                            if (rewind > grid.Count)
                            {
                                RaycastHit hit;
                                LayerMask layerMask = 4;
                                if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                                {
                                    var lake = hit.collider.gameObject.GetComponent<Lake>();
                                    if (lake != null)
                                    {
                                        if (lake.size >= size)
                                        {
                                            foreach (River river in sources)
                                            {
                                                if (!lake.sources.Contains(river))
                                                    lake.sources.Add(river);
                                                river.endLake = lake;
                                            }
                                            lake.Generate();
                                            Destroy(this.gameObject);
                                        }
                                        else
                                        {
                                            foreach (River river in lake.sources)
                                            {
                                                if (!sources.Contains(river))
                                                    sources.Add(river);
                                                river.endLake = this;
                                            }
                                            Destroy(lake.gameObject);
                                            StopCoroutine(coroutineB);
                                            Generate();
                                        }
                                    }
                                }
                                if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                                {
                                    var lake = hit.collider.gameObject.GetComponent<Lake>();
                                    if (lake != null)
                                    {
                                        if (lake.size > size)
                                        {
                                            foreach (River river in sources)
                                            {
                                                if (!lake.sources.Contains(river))
                                                    lake.sources.Add(river);
                                                river.endLake = lake;
                                            }
                                            lake.Generate();
                                            Destroy(this.gameObject);
                                        }
                                        else
                                        {
                                            foreach (River river in lake.sources)
                                            {
                                                if (!sources.Contains(river))
                                                    sources.Add(river);
                                                river.endLake = this;
                                            }
                                            Destroy(lake.gameObject);
                                            StopCoroutine(coroutineB);
                                            Generate();
                                        }
                                    }
                                }
                                if (size < 0.25f * maxSize)
                                {
                                    rewind = 0;
                                    if (gridStep >= 2)
                                    {
                                        if (size < 5000)
                                        {
                                            gridStep = 1;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 10000 && gridStep >= 4)
                                        {
                                            gridStep = 2;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 20000 && gridStep >= 8)
                                        {
                                            gridStep = 4;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 40000 && gridStep >= 16)
                                        {
                                            gridStep = 8;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 80000 && gridStep >= 32)
                                        {
                                            gridStep = 16;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 160000 && gridStep >= 64)
                                        {
                                            gridStep = 32;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                    }
                                    SpawnOutlet();
                                }
                                settled = true;
                                StopCoroutine(coroutineB);
                                notDone = false;
                                Render();
                                yield return null;
                            }
                            lead = gridList[gridList.Count - rewind];
                        }
                    }
                }
                else
                {
                    right = new Vector2(facing.y, -facing.x);
                    if (!grid.Contains(lead + gridStep * right))
                    {
                        if (terrainGen.StoneHeight256(source.x + lead.x + gridStep * right.x, 0, source.z + lead.y + gridStep * right.y) < source.y + depth)
                        {
                            size += gridStep * gridStep;
                            lead = lead + gridStep * right;
                            facing = right;
                            rewind = 0;
                            grid.Add(lead);
                            gridList.Add(lead);
                            continue;
                        }
                        else
                        {
                            rewind++;
                            if (rewind > grid.Count)
                            {
                                RaycastHit hit;
                                LayerMask layerMask = 4;
                                if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                                {
                                    var lake = hit.collider.gameObject.GetComponent<Lake>();
                                    if (lake != null)
                                    {
                                        if (lake.size >= size)
                                        {
                                            foreach (River river in sources)
                                            {
                                                if (!lake.sources.Contains(river))
                                                    lake.sources.Add(river);
                                                river.endLake = lake;
                                            }
                                            lake.Generate();
                                            Destroy(this.gameObject);
                                        }
                                        else
                                        {
                                            foreach (River river in lake.sources)
                                            {
                                                if (!sources.Contains(river))
                                                    sources.Add(river);
                                                river.endLake = this;
                                            }
                                            Destroy(lake.gameObject);
                                            StopCoroutine(coroutineB);
                                            Generate();
                                        }
                                    }
                                }
                                if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                                {
                                    var lake = hit.collider.gameObject.GetComponent<Lake>();
                                    if (lake != null)
                                    {
                                        if (lake.size > size)
                                        {
                                            foreach (River river in sources)
                                            {
                                                if (!lake.sources.Contains(river))
                                                    lake.sources.Add(river);
                                                river.endLake = lake;
                                            }
                                            lake.Generate();
                                            Destroy(this.gameObject);
                                        }
                                        else
                                        {
                                            foreach (River river in lake.sources)
                                            {
                                                if (!sources.Contains(river))
                                                    sources.Add(river);
                                                river.endLake = this;
                                            }
                                            Destroy(lake.gameObject);
                                            StopCoroutine(coroutineB);
                                            Generate();
                                        }
                                    }
                                }
                                if (size < 0.25f * maxSize)
                                {
                                    rewind = 0;
                                    if (gridStep >= 2)
                                    {
                                        if (size < 5000)
                                        {
                                            gridStep = 1;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 10000 && gridStep >= 4)
                                        {
                                            gridStep = 2;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 20000 && gridStep >= 8)
                                        {
                                            gridStep = 4;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 40000 && gridStep >= 16)
                                        {
                                            gridStep = 8;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 80000 && gridStep >= 32)
                                        {
                                            gridStep = 16;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                        else if (size < 160000 && gridStep >= 64)
                                        {
                                            gridStep = 32;
                                            grid.Clear();
                                            gridList.Clear();
                                            grid.Add(lead);
                                            gridList.Add(lead);
                                            size = 0;
                                            continue;
                                        }
                                    }
                                    SpawnOutlet();
                                }
                                settled = true;
                                StopCoroutine(coroutineB);
                                notDone = false;
                                Render();
                                yield return null;
                            }
                            if (rewind > 0 && rewind < gridList.Count + 1)
                                lead = gridList[gridList.Count - rewind];
                        }
                    }
                    else
                    {
                        rewind++;
                        if (rewind > grid.Count)
                        {
                            RaycastHit hit;
                            LayerMask layerMask = 4;
                            if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                            {
                                var lake = hit.collider.gameObject.GetComponent<Lake>();
                                if (lake != null)
                                {
                                    if (lake.size >= size)
                                    {
                                        foreach (River river in sources)
                                        {
                                            if (!lake.sources.Contains(river))
                                                lake.sources.Add(river);
                                            river.endLake = lake;
                                        }
                                        lake.Generate();
                                        Destroy(this.gameObject);
                                    }
                                    else
                                    {
                                        foreach (River river in lake.sources)
                                        {
                                            if (!sources.Contains(river))
                                                sources.Add(river);
                                            river.endLake = this;
                                        }
                                        Destroy(lake.gameObject);
                                        StopCoroutine(coroutineB);
                                        Generate();
                                    }
                                }
                            }
                            if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                            {
                                var lake = hit.collider.gameObject.GetComponent<Lake>();
                                if (lake != null)
                                {
                                    if (lake.size > size)
                                    {
                                        foreach (River river in sources)
                                        {
                                            if (!lake.sources.Contains(river))
                                                lake.sources.Add(river);
                                            river.endLake = lake;
                                        }
                                        lake.Generate();
                                        Destroy(this.gameObject);
                                    }
                                    else
                                    {
                                        foreach (River river in lake.sources)
                                        {
                                            if (!sources.Contains(river))
                                                sources.Add(river);
                                            river.endLake = this;
                                        }
                                        Destroy(lake.gameObject);
                                        StopCoroutine(coroutineB);
                                        Generate();
                                    }
                                }
                            }
                            if (size < 0.25f * maxSize)
                            {
                                rewind = 0;
                                if (gridStep >= 2)
                                {
                                    if (size < 5000)
                                    {
                                        gridStep = 1;
                                        grid.Clear();
                                        gridList.Clear();
                                        grid.Add(lead);
                                        gridList.Add(lead);
                                        size = 0;
                                        continue;
                                    }
                                    else if (size < 10000 && gridStep >= 4)
                                    {
                                        gridStep = 2;
                                        grid.Clear();
                                        gridList.Clear();
                                        grid.Add(lead);
                                        gridList.Add(lead);
                                        size = 0;
                                        continue;
                                    }
                                    else if (size < 20000 && gridStep >= 8)
                                    {
                                        gridStep = 4;
                                        grid.Clear();
                                        gridList.Clear();
                                        grid.Add(lead);
                                        gridList.Add(lead);
                                        size = 0;
                                        continue;
                                    }
                                    else if (size < 40000 && gridStep >= 16)
                                    {
                                        gridStep = 8;
                                        grid.Clear();
                                        gridList.Clear();
                                        grid.Add(lead);
                                        gridList.Add(lead);
                                        size = 0;
                                        continue;
                                    }
                                    else if (size < 80000 && gridStep >= 32)
                                    {
                                        gridStep = 16;
                                        grid.Clear();
                                        gridList.Clear();
                                        grid.Add(lead);
                                        gridList.Add(lead);
                                        size = 0;
                                        continue;
                                    }
                                    else if (size < 160000 && gridStep >= 64)
                                    {
                                        gridStep = 32;
                                        grid.Clear();
                                        gridList.Clear();
                                        grid.Add(lead);
                                        gridList.Add(lead);
                                        size = 0;
                                        continue;
                                    }
                                }
                                SpawnOutlet();
                            }
                            settled = true;
                            StopCoroutine(coroutineB);
                            notDone = false;
                            Render();
                            yield return null;
                        }
                        if (rewind > 0 && rewind < gridList.Count + 1)
                            lead = gridList[gridList.Count - rewind];
                    }
                }
            }
            if (size > maxSize)
            {
                if(depth > lastDepth)
                {
                    float temp = depth;
                    depth = (depth + lastDepth) / 2f;
                    lastDepth = temp;
                }
                else if(depth < lastDepth)
                {
                    float temp = depth;
                    depth = (highDepth + depth) / 2f;
                    lastDepth = temp;
                }
                lead = Vector2.zero;
                grid.Clear();
                gridList.Clear();
                grid.Add(lead);
                gridList.Add(lead);
                size = 0;
                Render();
            }
            else if (rewind >= grid.Count)
            {
                RaycastHit hit;
                LayerMask layerMask = 4;
                if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.up, out hit, 100, layerMask))
                {
                    var lake = hit.collider.gameObject.GetComponent<Lake>();
                    if (lake != null)
                    {
                        if (lake.size >= size)
                        {
                            foreach (River river in sources)
                            {
                                if (!lake.sources.Contains(river))
                                    lake.sources.Add(river);
                                river.endLake = lake;
                            }
                            lake.Generate();
                            Destroy(this.gameObject);
                        }
                        else
                        {
                            foreach (River river in lake.sources)
                            {
                                if (!sources.Contains(river))
                                    sources.Add(river);
                                river.endLake = this;
                            }
                            Destroy(lake.gameObject);
                            StopCoroutine(coroutineB);
                            Generate();
                        }
                    }
                }
                if (Physics.Raycast(new Vector3(source.x, source.y + depth, source.z), Vector3.down, out hit, 100, layerMask))
                {
                    var lake = hit.collider.gameObject.GetComponent<Lake>();
                    if (lake != null)
                    {
                        if (lake.size > size)
                        {
                            foreach (River river in sources)
                            {
                                if (!lake.sources.Contains(river))
                                    lake.sources.Add(river);
                                river.endLake = lake;
                            }
                            lake.Generate();
                            Destroy(this.gameObject);
                        }
                        else
                        {
                            foreach (River river in lake.sources)
                            {
                                if (!sources.Contains(river))
                                    sources.Add(river);
                                river.endLake = this;
                            }
                            Destroy(lake.gameObject);
                            StopCoroutine(coroutineB);
                            Generate();
                        }
                    }
                }
                if (size < 0.25f * maxSize)
                {
                    rewind = 0;
                    if (gridStep >= 2)
                    {
                        if (size < 5000)
                        {
                            gridStep = 1;
                            grid.Clear();
                            gridList.Clear();
                            grid.Add(lead);
                            gridList.Add(lead);
                            size = 0;
                            continue;
                        }
                        else if (size < 10000 && gridStep >= 4)
                        {
                            gridStep = 2;
                            grid.Clear();
                            gridList.Clear();
                            grid.Add(lead);
                            gridList.Add(lead);
                            size = 0;
                            continue;
                        }
                        else if (size < 20000 && gridStep >= 8)
                        {
                            gridStep = 4;
                            grid.Clear();
                            gridList.Clear();
                            grid.Add(lead);
                            gridList.Add(lead);
                            size = 0;
                            continue;
                        }
                        else if (size < 40000 && gridStep >= 16)
                        {
                            gridStep = 8;
                            grid.Clear();
                            gridList.Clear();
                            grid.Add(lead);
                            gridList.Add(lead);
                            size = 0;
                            continue;
                        }
                        else if (size < 80000 && gridStep >= 32)
                        {
                            gridStep = 16;
                            grid.Clear();
                            gridList.Clear();
                            grid.Add(lead);
                            gridList.Add(lead);
                            size = 0;
                            continue;
                        }
                        else if (size < 160000 && gridStep >= 64)
                        {
                            gridStep = 32;
                            grid.Clear();
                            gridList.Clear();
                            grid.Add(lead);
                            gridList.Add(lead);
                            size = 0;
                            continue;
                        }
                    }
                    SpawnOutlet();
                }
                settled = true;
                notDone = false;
                StopCoroutine(coroutineB);
                Render();
                yield return null;
            }
            yield return null;
        //}
        }
    }

    void Render()
    {
        MeshData meshData = new MeshData();
        meshData.useRenderDataForCol = true;
        for (int i = 0; i < grid.Count; i++)
        {
            //if (grid.Contains(new Vector2(gridList[i].x + gridStep, gridList[i].y)) && grid.Contains(new Vector2(gridList[i].x - gridStep, gridList[i].y)) && grid.Contains(new Vector2(gridList[i].x, gridList[i].y + gridStep)) && grid.Contains(new Vector2(gridList[i].x, gridList[i].y - gridStep)))
            //{
            //    meshData.AddVertex(new Vector3(gridList[i].x, depth, gridList[i].y));
            //}
            //else
            //{
                meshData.AddVertex(new Vector3(gridList[i].x, depth, gridList[i].y));
                meshData.AddVertex(new Vector3(gridList[i].x, depth, gridList[i].y + gridStep));
                meshData.AddVertex(new Vector3(gridList[i].x + gridStep, depth, gridList[i].y + gridStep));
                meshData.AddVertex(new Vector3(gridList[i].x + gridStep, depth, gridList[i].y));

                meshData.AddQuadTriangles();
    
        //}
        }
        RenderMesh(meshData);
    }

    void RenderMesh(MeshData meshData)
    {
        filter.mesh.Clear();
        filter.mesh.vertices = meshData.vertices.ToArray();
        filter.mesh.triangles = meshData.triangles.ToArray();

        filter.mesh.uv = meshData.uv.ToArray();
        filter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
    }


    void SpawnOutlet()
    {
        terrainGen = new TerrainGen();
        Vector3 bestSpawn = Vector3.zero;
        float bestSpawnScore = 9999999f, spawnScore, currLow, low, h;
        for (int i = 0; i < grid.Count; i++)
        {
            spawnScore = 0;
            if (!grid.Contains(new Vector2(gridList[i].x + gridStep, gridList[i].y)) && !grid.Contains(new Vector2(gridList[i].x + 4 * gridStep, gridList[i].y)))
            {
                currLow = 999999;
                for (int j = 1; j < Mathf.Max(16, gridStep); j++)
                {
                    h = terrainGen.StoneHeight256(source.x + gridList[i].x + 4 * j, 0, source.z + gridList[i].y);
                    if (h < currLow)
                        currLow = h;
                    spawnScore += h - source.y - depth;
                }
                if (spawnScore < bestSpawnScore)
                {
                    bestSpawnScore = spawnScore;
                    low = currLow;
                    bestSpawn = new Vector3(source.x + gridList[i].x + 64, low, source.z + gridList[i].y);
                }
            }
            if (!grid.Contains(new Vector2(gridList[i].x - gridStep, gridList[i].y)))
            {
                currLow = 999999;
                for (int j = 1; j < Mathf.Max(16, gridStep); j++)
                {
                    h = terrainGen.StoneHeight256(source.x + gridList[i].x - 4 * j, 0, source.z + gridList[i].y);
                    if (h < currLow)
                        currLow = h;
                    spawnScore += h - source.y - depth;
                }
                if (spawnScore < bestSpawnScore)
                {
                    bestSpawnScore = spawnScore;
                    low = currLow;
                    bestSpawn = new Vector3(source.x + gridList[i].x - 64, low, source.z + gridList[i].y);
                }
            }
            if (!grid.Contains(new Vector2(gridList[i].x, gridList[i].y + gridStep)))
            {
                currLow = 999999;
                for (int j = 1; j < Mathf.Max(16, gridStep); j++)
                {
                    h = terrainGen.StoneHeight256(source.x + gridList[i].x, 0, source.z + gridList[i].y + 4 * j);
                    if (h < currLow)
                        currLow = h;
                    spawnScore += h - source.y - depth;
                }
                if (spawnScore < bestSpawnScore)
                {
                    bestSpawnScore = spawnScore;
                    low = currLow;
                    bestSpawn = new Vector3(source.x + gridList[i].x, low, source.z + gridList[i].y + 64);
                }
            }
            if (!grid.Contains(new Vector2(gridList[i].x, gridList[i].y - gridStep)))
            {
                currLow = 999999;
                for (int j = 1; j < Mathf.Max(16, gridStep); j++)
                {
                    h = terrainGen.StoneHeight256(source.x + gridList[i].x, 0, source.z + gridList[i].y - 4 * j);
                    if (h < currLow)
                        currLow = h;
                    spawnScore += h - source.y - depth;
                }
                if (spawnScore < bestSpawnScore)
                {
                    bestSpawnScore = spawnScore;
                    low = currLow;
                    bestSpawn = new Vector3(source.x + gridList[i].x, low, source.z + gridList[i].y - 64);
                }
            }
        }
        if (bestSpawn != Vector3.zero)
        {
            outlets.Add(WaterManager.waterManagerObject.GetComponent<WaterManager>().SpawnRiverFromLake(bestSpawn, this));
        }
    }
}