using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverNode : MonoBehaviour {

    public River river;
    public MeshFilter roughFilter, fineFilter;
    public MeshCollider coll;
    public IEnumerator erode16;
    public int startIdx, endIdx;
    public BoxCollider erosionTrigger;
    public GameObject lod0;

    private void Start()
    {
        erode16 = ErodeChunk16();
    }

    public void RenderMeshFine(MeshData meshData)
    {
        fineFilter.mesh.Clear();
        fineFilter.mesh.vertices = meshData.vertices.ToArray();
        fineFilter.mesh.triangles = meshData.triangles.ToArray();

        fineFilter.mesh.uv = meshData.uv.ToArray();
        fineFilter.mesh.RecalculateNormals();

        coll.sharedMesh = null;
        Mesh mesh = new Mesh();
        mesh.vertices = meshData.colVertices.ToArray();
        mesh.triangles = meshData.colTriangles.ToArray();
        mesh.RecalculateNormals();
            
        coll.sharedMesh = mesh;
        lod0.AddComponent<BoxCollider>().isTrigger = true;
    }

    public void RenderMeshRough(MeshData meshData)
    {
        roughFilter.mesh.Clear();
        roughFilter.mesh.vertices = meshData.vertices.ToArray();
        roughFilter.mesh.triangles = meshData.triangles.ToArray();

        roughFilter.mesh.uv = meshData.uv.ToArray();
        roughFilter.mesh.RecalculateNormals();
    }

    IEnumerator ErodeChunk16()
    {
        //find the block you are in, store waypoints and check if still in same block until you leave. Change blocks above to Air, turn lowest block to River16, passing waypoints stored since block began. 
        TerrainGen terrainGen = new TerrainGen();
        World16 world = World16.worldGameObject.GetComponent<World16>();
        HashSet<Vector3> clearedBlocks = new HashSet<Vector3>();
        Vector3 prev = Vector3.zero;
        bool inSameBlock = false;

        Chunk16 nextLeftChunk,currentLeftChunk = world.GetChunk16((river.left[startIdx].x + river.source.x), (river.left[startIdx].y + river.source.y), (river.left[startIdx].z + river.source.z));
        Block16 currentLeftBlock = world.GetBlock16((river.left[startIdx].x + river.source.x),(river.left[startIdx].y + river.source.y), (river.left[startIdx].z + river.source.z));
        List<Vector3> leftPoints = new List<Vector3> { river.left[startIdx] + river.source - currentLeftChunk.transform.position };
        Chunk16 nextRightChunk,currentRightChunk = world.GetChunk16((river.right[startIdx].x + river.source.x), (river.right[startIdx].y + river.source.y), (river.right[startIdx].z + river.source.z));
        Block16 currentRightBlock = world.GetBlock16((river.right[startIdx].x + river.source.x), (river.right[startIdx].y + river.source.y), (river.right[startIdx].z + river.source.z));
        List<Vector3> rightPoints = new List<Vector3> { river.right[startIdx] + river.source - currentRightChunk.transform.position };
        if (currentRightBlock == currentLeftBlock)
            inSameBlock = true;
        //if(startIdx == 0)
        //{
        //    if(currentRightBlock == currentLeftBlock)
        //    {
        //        int i = 2;
        //        bool spawning = true;
        //        Block16 nextLeftBlock = world.GetBlock16((river.left[1].x + river.source.x), (river.left[1].y + river.source.y), (river.left[1].z + river.source.z));
        //        Block16 nextRightBlock = world.GetBlock16((river.right[1].x + river.source.x), (river.right[1].y + river.source.y), (river.left[1].z + river.source.z));
        //        while (spawning)
        //        {
        //            if (nextLeftBlock == currentLeftBlock)
        //            {
        //                if (nextRightBlock == currentRightBlock)
        //                {
        //                    leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
        //                    rightPoints.Add(river.right[i] + river.source - currentRightChunk.transform.position);
        //                    i++;
        //                    nextLeftBlock = world.GetBlock16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
        //                    nextRightBlock = world.GetBlock16((river.right[i].x + river.source.x), (river.right[i].y + river.source.y), (river.left[i].z + river.source.z));
        //                }
        //                else
        //                {

        //                }
        //            }
        //            else if (nextRightBlock == currentRightBlock)
        //            {

        //            }
        //            else
        //            {
        //                spawning = false;
        //                bool isTopReached = false;
        //                int up = 1;
        //                while (!isTopReached)
        //                {
        //                    Block16 erosionBlock = world.GetBlock16((river.left[0].x + river.source.x), (river.left[0].y + river.source.y + up * 16), (river.left[0].z + river.source.z));
        //                    if (erosionBlock is Block16Air)
        //                    {
        //                        isTopReached = true;
        //                    }
        //                    else
        //                    {
        //                        world.SetBlock16((river.waypoints[startIdx + i - 1].x + river.source.x), (river.waypoints[startIdx + i - 1].y + river.source.y + up * 16), (river.waypoints[startIdx + i - 1].z + river.source.z), new Block16Air());
        //                    }
        //                    up++;
        //                }
        //                currBlockLeft.Add(river.left[startIdx - 1 + i] + river.source - currentLeftChunk.transform.position);
        //                currBlockRight.Add(river.right[startIdx - 1 + i] + river.source - currentLeftChunk.transform.position);
        //                world.SetBlock16((river.waypoints[startIdx + i - 1].x + river.source.x), (river.waypoints[startIdx + i - 1].y + river.source.y), (river.waypoints[startIdx + i - 1].z + river.source.z), new Block16RiverGrass(currBlockLeft.ToArray(), currBlockRight.ToArray(), up * 16));
        //            }
        //        }
        //    }
        //}
        for (int i = startIdx; i < endIdx; i++)
        {
            if (inSameBlock)
            {

            }
            Block16 nextLeftBlock = world.GetBlock16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
            Block16 nextRightBlock = world.GetBlock16((river.right[i].x + river.source.x), (river.right[i].y + river.source.y), (river.left[1].z + river.source.z));
            if (currentLeftBlock == nextLeftBlock)
            {
                leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
            }
            else if (currentLeftBlock == world.GetBlock16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y) + 4, (river.left[i].z + river.source.z)))
            { //next block is directly blow current block
                nextLeftChunk = world.GetChunk16((river.left[i].x + river.source.x),(river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
                if (currentLeftChunk != nextLeftChunk)
                {  //if we've crossed chunks, vector3s need to be shifted to measure from origin of new chunk
                    for (int j = 0; j < leftPoints.Count; j++)
                    {
                        leftPoints[j] = leftPoints[j] + currentLeftChunk.transform.position - nextLeftChunk.transform.position;
                    }
                    currentLeftChunk = nextLeftChunk;
                }
                leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
                currentLeftBlock = nextLeftBlock;
            }
            else
            { //have left xz bounds of current block, set blocks above to air, this block to RiverGrass
                bool isTopReached = false;
                int up = 1;
                while (!isTopReached)
                {
                    Block16 erosionBlock = world.GetBlock16((river.waypoints[startIdx + i - 1].x + river.source.x), (river.waypoints[startIdx + i - 1].y + river.source.y + up * 16), (river.waypoints[startIdx + i - 1].z + river.source.z));
                    if (erosionBlock is Block16Air)
                    {
                        isTopReached = true;
                    }
                    else
                    {
                        world.SetBlock16((river.waypoints[startIdx + i - 1].x + river.source.x), (river.waypoints[startIdx + i - 1].y + river.source.y + up * 16), (river.waypoints[startIdx + i - 1].z + river.source.z), new Block16Air());
                    }
                    up++;
                }
                currBlockLeft.Add(river.left[startIdx - 1 + i] + river.source - currentLeftChunk.transform.position);
                currBlockRight.Add(river.right[startIdx - 1 + i] + river.source - currentLeftChunk.transform.position);
                world.SetBlock16((river.waypoints[startIdx + i - 1].x + river.source.x), (river.waypoints[startIdx + i - 1].y + river.source.y), (river.waypoints[startIdx + i - 1].z + river.source.z), new Block16RiverGrass(currBlockLeft.ToArray(), currBlockRight.ToArray(),up * 16));
                currentBlock = nextBlock;
                currentLeftChunk = world.GetChunk16((river.waypoints[startIdx + i].x + river.source.x), (river.waypoints[startIdx + i].y + river.source.y), (river.waypoints[startIdx + i].z + river.source.z));
                currBlockLeft.Clear();
                currBlockRight.Clear();
                //currBlockLeft.Add(river.left[startIdx + i - 3] + river.source - currentChunk.transform.position);
                //currBlockRight.Add(river.right[startIdx + i - 3] + river.source - currentChunk.transform.position);
                currBlockLeft.Add(river.left[startIdx + i - 2] + river.source - currentLeftChunk.transform.position);
                currBlockRight.Add(river.right[startIdx + i - 2] + river.source - currentLeftChunk.transform.position);
                currBlockLeft.Add(river.left[startIdx + i - 1] + river.source - currentLeftChunk.transform.position);
                currBlockRight.Add(river.right[startIdx + i - 1] + river.source - currentLeftChunk.transform.position);
            }

            if(nextRightBlock == currentRightBlock)
            {
                rightPoints.Add(river.right[i] + river.source - currentRightChunk.transform.position);
            }
            else if (currentLeftBlock == world.GetBlock16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y) + 4, (river.left[i].z + river.source.z)))
            { //next block is directly blow current block
                nextLeftChunk = world.GetChunk16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
                if (currentLeftChunk != nextLeftChunk)
                {  //if we've crossed chunks, vector3s need to be shifted to measure from origin of new chunk
                    for (int j = 0; j < leftPoints.Count; j++)
                    {
                        leftPoints[j] = leftPoints[j] + currentLeftChunk.transform.position - nextLeftChunk.transform.position;
                        currBlockRight[j] = currBlockRight[j] + currentLeftChunk.transform.position - nextChunk.transform.position;
                    }
                    currentLeftChunk = nextLeftChunk;
                }
                currBlockLeft.Add(river.left[startIdx - 1 + i] + river.source - currentLeftChunk.transform.position);
                currBlockRight.Add(river.right[startIdx - 1 + i] + river.source - currentLeftChunk.transform.position);
                currentBlock = nextBlock;
            }
            yield return null;
        }

        //        int prevBlockCount = clearedBlocks.Count;
        //    //Vector3 next = new Vector3(Mathf.FloorToInt(river.waypoints[startIdx + i].x / 16) * 16, Mathf.FloorToInt(river.waypoints[startIdx + i].y / 16) * 16, Mathf.FloorToInt(river.waypoints[startIdx + i].z / 16) * 16) + river.source;
        //    Vector3 next = new Vector3(river.waypoints[startIdx + i].x, river.waypoints[startIdx + i].y, river.waypoints[startIdx + i].z) + river.source;
        //    clearedBlocks.Add(next);
        //    if (clearedBlocks.Count > prevBlockCount)
        //    {
        //        bool isTopReached = false;
        //        int up = 1;
        //        while (!isTopReached)
        //        {
        //            if (world.GetBlock16((int)next.x, (int)next.y + up*16, (int)next.z) is Block16Air)
        //            {
        //                isTopReached = true;
        //            }
        //            else
        //            {
        //                world.SetBlock16((int)next.x, (int)next.y + up*16, (int)next.z, new Block16Air());
        //            }
        //            up++;
        //        }
        //        if (clearedBlocks.Count >= 1 && chunk != null)
        //        {
        //            Vector3[] left = new Vector3[i - currEntry], right = new Vector3[i - currEntry];
        //            for (int j = 0; j < i - currEntry; j++)
        //            {
        //                left[j] = river.left[currEntry + startIdx + j] + river.source - chunk.gameObject.transform.position;
        //                right[j] = river.right[currEntry + startIdx + j] + river.source;
        //            }
        //            world.SetBlock16((int)prev.x, (int)prev.y, (int)prev.z, new Block16RiverGrass(left,right));
        //            currEntry = i - 1;
        //        }
        //    }
        //    prev = next;
        //    //chunk = world.GetChunk16((int)(next.x + river.source.x), (int)(next.y + river.source.y), (int)(next.z + +river.source.z));
        //    chunk = world.GetChunk16((int)(next.x), (int)(next.y), (int)(next.z));
        //    yield return null;
        //}
    }
}
