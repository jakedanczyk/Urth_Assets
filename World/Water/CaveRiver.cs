using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveRiver : MonoBehaviour {


    public River source;
    public List<Block16> block16s = new List<Block16>();
    public List<Block4> block4s = new List<Block4>();
    public List<Block1> block1s = new List<Block1>();
    public List<Block> blocks = new List<Block>();
    public List<CaveLake> caveLakes = new List<CaveLake>();
    public List<Vector3> cells = new List<Vector3>();
    public float flow;
    IEnumerator generate16, generate4, generate1, generate;

    void Awake()
    {
        generate = Generate();
    }

    private void Start()
    {
        flow = source.sourceFlow;
        cells.Add(this.transform.position);
    }

    IEnumerator Generate16()
    {
        yield return null;
    }

    IEnumerator Generate()
    {
        var terrainGen = new TerrainGen();
        bool isDone = false;
        while (!isDone)
        {
            int maxDrop = 0;
            List<Tuple<int, int>> grids = new List<Tuple<int, int>>();

            for (int x = -1; x < 2; x++)
            {
                for (int z = -1; z < 2; z++)
                {
                    for (int d = 0; d < 10000; d++)
                    {
                        if (!terrainGen.CaveCheck(cells[cells.Count - 1].x + x, cells[cells.Count - 1].y - d, cells[cells.Count - 1].z + z))
                        {
                            if(d == maxDrop)
                            {
                                grids.Add(new Tuple<int,int>(x, z));
                            }
                            else if(d > maxDrop)
                            {
                                maxDrop = d;
                                grids.Clear();
                                grids.Add(new Tuple<int, int>(x, z));
                            }
                        }
                    }
                }
            }
            if(grids.Count >= 1)
            {
                int r = (int)Random.Range(0, grids.Count);
                cells.Add(cells[cells.Count - 1] + new Vector3(grids[r].first,-maxDrop, grids[r].second));
            }
            else
            {
                isDone = true;
                //form cave lake
            }
            yield return null;
        }
    }
}
