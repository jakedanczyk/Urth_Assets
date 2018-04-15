using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverNode : MonoBehaviour
{

    public River river;
    public MeshFilter roughFilter, fineFilter;
    public MeshCollider coll;
    public IEnumerator erode16;
    public int startIdx, endIdx;
    bool erosionCompleted = false;
    public BoxCollider erosionTrigger;
    public GameObject lod0, lod1;
    Transform playerTransform;

    private void Start()
    {
        erode16 = ErodeChunk16();
        playerTransform = BodyManager_Human_Player.playerObject.transform;
        StartCoroutine(CheckPlayerDistance());
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

    IEnumerator CheckPlayerDistance()
    {
        while (true)
        {
            float dist = Vector3.Distance(playerTransform.position, transform.position);

            if (dist > 600)
            {
                if (lod0.activeSelf)
                {
                    lod0.SetActive(false);
                    lod1.SetActive(true);
                }
            }
            else if(dist < 500)
            {
                if (!lod0.activeSelf)
                {
                    lod0.SetActive(true);
                    lod1.SetActive(false);
                }
            }
            yield return new WaitForSeconds(15);
        }
    }


    IEnumerator ErodeChunk16()
    {
        //find the block you are in, store waypoints and check if still in same block until you leave. Change blocks above to Air, turn lowest block to River16, passing waypoints stored since block began. 
        TerrainGen terrainGen = new TerrainGen();
        World16 world = World16.worldGameObject.GetComponent<World16>();
        HashSet<Vector3> clearedBlocks = new HashSet<Vector3>();
        Vector3 prev = Vector3.zero;

        Chunk16 nextLeftChunk, currentLeftChunk = world.GetChunk16((river.left[startIdx].x + river.source.x), (river.left[startIdx].y + river.source.y), (river.left[startIdx].z + river.source.z));
        Block16 prevLeftBlock = null, nextLeftBlock, currLeftBlock = world.GetBlock16((river.left[startIdx].x + river.source.x), (river.left[startIdx].y + river.source.y), (river.left[startIdx].z + river.source.z));
        List<Vector3> prevLeftPoints = new List<Vector3> { }, leftPoints = new List<Vector3> { river.left[startIdx] + river.source - currentLeftChunk.transform.position };
        Chunk16 nextRightChunk, currentRightChunk = world.GetChunk16((river.right[startIdx].x + river.source.x), (river.right[startIdx].y + river.source.y), (river.right[startIdx].z + river.source.z));
        Block16 prevRightBlock = null, nextRightBlock, currRightBlock = world.GetBlock16((river.right[startIdx].x + river.source.x), (river.right[startIdx].y + river.source.y), (river.right[startIdx].z + river.source.z));
        List<Vector3> prevRightPoints = new List<Vector3> { }, rightPoints = new List<Vector3> { river.right[startIdx] + river.source - currentRightChunk.transform.position };
        if (currentLeftChunk == null || currentRightChunk == null)
        {
            yield return null;
        }

        for (int i = startIdx + 1; i < endIdx; i++)
        {
            if (currentLeftChunk == null || currentRightChunk == null)
            {
                yield return null;
            }
            nextLeftBlock = world.GetBlock16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
            if (currLeftBlock == nextLeftBlock)
            {
                leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
            }
            else if (currLeftBlock == world.GetBlock16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y) + 4, (river.left[i].z + river.source.z)))
            { //next block is directly blow current block
                nextLeftChunk = world.GetChunk16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
                if (currentLeftChunk != nextLeftChunk)
                {  //if we've crossed chunks, vector3s need to be shifted to measure from origin of new chunk
                    currentLeftChunk.UpdateChunk16();
                    Serialization16.SaveChunk16(currentLeftChunk);
                    for (int j = 0; j < leftPoints.Count; j++)
                    {
                        leftPoints[j] = leftPoints[j] + currentLeftChunk.transform.position - nextLeftChunk.transform.position;
                    }
                    currentLeftChunk = nextLeftChunk;
                }
                leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
                currLeftBlock = nextLeftBlock;
            }
            else
            { //left side has cross block xz bounds, store leftPoints as prevLeftPoints
                leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
                if (currLeftBlock == currRightBlock)
                { //store points and wait until right side also exits
                    prevLeftPoints.Clear();
                    prevLeftPoints.AddRange(leftPoints);
                    leftPoints.Clear();
                    currentLeftChunk = world.GetChunk16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
                    leftPoints.Add(river.left[i - 1] + river.source - currentLeftChunk.transform.position);
                    leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
                    prevLeftBlock = currLeftBlock;
                    currLeftBlock = nextLeftBlock;
                }
                else if (currLeftBlock == prevRightBlock)
                { //if right side has exited and is waiting, block ready to be built. prepare leftPoints for next block
                    RiverValleyColumn(river.left[i - 1] + river.source, leftPoints.ToArray(), prevRightPoints.ToArray());
                    leftPoints.Clear();
                    currentLeftChunk.UpdateChunk16();
                    Serialization16.SaveChunk16(currentLeftChunk);
                    currentLeftChunk = world.GetChunk16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
                    leftPoints.Add(river.left[i - 1] + river.source - currentLeftChunk.transform.position);
                    leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
                    currLeftBlock = nextLeftBlock;
                }
                else
                { //else currentLeftBlock is left side only, build it
                    RiverValleyColumn(river.left[i - 1] + river.source, leftPoints.ToArray(), null);
                    leftPoints.Clear();
                    currentLeftChunk.UpdateChunk16();
                    Serialization16.SaveChunk16(currentLeftChunk);
                    currentLeftChunk = world.GetChunk16((river.left[i].x + river.source.x), (river.left[i].y + river.source.y), (river.left[i].z + river.source.z));
                    leftPoints.Add(river.left[i - 1] + river.source - currentLeftChunk.transform.position);
                    leftPoints.Add(river.left[i] + river.source - currentLeftChunk.transform.position);
                    currLeftBlock = nextLeftBlock;
                }
            }

            nextRightBlock = world.GetBlock16((river.right[i].x + river.source.x), (river.right[i].y + river.source.y), (river.right[i].z + river.source.z));
            if (nextRightBlock == currRightBlock)
            {
                rightPoints.Add(river.right[i] + river.source - currentRightChunk.transform.position);
            }
            else if (currRightBlock == world.GetBlock16((river.right[i].x + river.source.x), (river.right[i].y + river.source.y) + 4, (river.right[i].z + river.source.z)))
            { //next block is directly blow current block
                nextRightChunk = world.GetChunk16((river.right[i].x + river.source.x), (river.right[i].y + river.source.y), (river.right[i].z + river.source.z));
                if (currentRightChunk != nextRightChunk)
                {  //if we've crossed chunks, vector3s need to be shifted to measure from origin of new chunk
                    currentRightChunk.UpdateChunk16();
                    Serialization16.SaveChunk16(currentRightChunk);
                    for (int j = 0; j < rightPoints.Count; j++)
                    {
                        rightPoints[j] = rightPoints[j] + currentRightChunk.transform.position - nextRightChunk.transform.position;
                    }
                    currentRightChunk = nextRightChunk;
                }
                rightPoints.Add(river.right[i] + river.source - currentRightChunk.transform.position);
                currRightBlock = nextRightBlock;
            }
            else
            { //right side has cross block xz bounds, store rightPoints as prevRightPoints
                rightPoints.Add(river.right[i] + river.source - currentRightChunk.transform.position);

                if (currLeftBlock == currRightBlock)
                { //store points and wait until left side also exits
                    prevRightPoints.Clear();
                    prevRightPoints.AddRange(rightPoints);
                    rightPoints.Clear();
                    currentRightChunk = world.GetChunk16((river.right[i].x + river.source.x), (river.right[i].y + river.source.y), (river.right[i].z + river.source.z));
                    rightPoints.Add(river.right[i - 1] + river.source - currentRightChunk.transform.position);
                    rightPoints.Add(river.right[i] + river.source - currentRightChunk.transform.position);
                    prevRightBlock = currRightBlock;
                    currRightBlock = nextRightBlock;
                }
                else if (currRightBlock == prevLeftBlock)
                { //if left side has exited and is waiting, block ready to be built. prepare rightPoints for next block
                    RiverValleyColumn(river.right[i - 1] + river.source, prevLeftPoints.ToArray(), rightPoints.ToArray());
                    rightPoints.Clear();
                    currentRightChunk.UpdateChunk16();
                    Serialization16.SaveChunk16(currentRightChunk);
                    currentRightChunk = world.GetChunk16((river.right[i].x + river.source.x), (river.right[i].y + river.source.y), (river.right[i].z + river.source.z));
                    rightPoints.Add(river.right[i - 1] + river.source - currentRightChunk.transform.position);
                    rightPoints.Add(river.right[i] + river.source - currentRightChunk.transform.position);
                    currRightBlock = nextRightBlock;
                }
                else
                { //else currentRightBlock is right side only, build it
                    RiverValleyColumn(river.right[i - 1] + river.source, null, rightPoints.ToArray());
                    rightPoints.Clear();
                    currentRightChunk.UpdateChunk16();
                    Serialization16.SaveChunk16(currentRightChunk);
                    currentRightChunk = world.GetChunk16((river.right[i].x + river.source.x), (river.right[i].y + river.source.y), (river.right[i].z + river.source.z));
                    rightPoints.Add(river.right[i - 1] + river.source - currentRightChunk.transform.position);
                    rightPoints.Add(river.right[i] + river.source - currentRightChunk.transform.position);
                    currRightBlock = nextRightBlock;
                }
            }
        }
        erosionCompleted = true;
        yield return null;
    }


    void RiverValleyColumn(Vector3 blockPos, Vector3[] left, Vector3[] right)
    {
        World16 world = World16.worldGameObject.GetComponent<World16>();
        bool isTopReached = false;
        int up = 1;
        while (!isTopReached)
        {
            Block16 erosionBlock = world.GetBlock16(blockPos.x, up * 16 + blockPos.y, blockPos.z);
            if (erosionBlock is Block16Air)
            {
                isTopReached = true;
            }
            else
            {
                world.SetBlock16(blockPos.x, up * 16 + blockPos.y, blockPos.z, new Block16Air());
            }
            up++;
        }
        world.SetBlock16(blockPos.x, blockPos.y, blockPos.z, new Block16RiverGrass(left, right, up * 16));
    }
}


