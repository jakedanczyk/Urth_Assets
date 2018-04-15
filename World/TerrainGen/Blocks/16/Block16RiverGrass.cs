using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Block16RiverGrass : Block16
{
    public enum Corner { ne, se, sw, nw, inside };

    public Corner entryLeft, entryRight, exitLeft, exitRight;
    public Vector3[] leftPoints,rightPoints;
    float canyonDepth;
    
    public Block16RiverGrass(Vector3[] left, Vector3[] right, float depth)
        : base()
    {
        canyonDepth = depth;
        leftPoints = left;
        rightPoints = right;
    }

    public override MeshData Blockdata
 (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {

        meshData.useRenderDataForCol = true;
        if (!chunk16.isWalkable)
        {
            if (!chunk16.GetBlock16(x, y + 1, z).IsSolid(Direction.down))
            {
                chunk16.isWalkable = true;
                meshData = FaceDataUp(chunk16, x, y, z, meshData);
            }

            //if (!chunk16.GetBlock16(x, y - 1, z).IsSolid(Direction.up))
            //{
            //    meshData = FaceDataDown(chunk16, x, y, z, meshData);
            //}

            //if (!chunk16.GetBlock16(x, y, z + 1).IsSolid(Direction.south))
            //{
            //    chunk16.isWalkable = true;
            //    meshData = FaceDataNorth(chunk16, x, y, z, meshData);
            //}

            //if (!chunk16.GetBlock16(x, y, z - 1).IsSolid(Direction.north))
            //{
            //    chunk16.isWalkable = true;
            //    meshData = FaceDataSouth(chunk16, x, y, z, meshData);
            //}

            //if (!chunk16.GetBlock16(x + 1, y, z).IsSolid(Direction.west))
            //{
            //    chunk16.isWalkable = true;
            //    meshData = FaceDataEast(chunk16, x, y, z, meshData);
            //}

            //if (!chunk16.GetBlock16(x - 1, y, z).IsSolid(Direction.east))
            //{
            //    chunk16.isWalkable = true;
            //    meshData = FaceDataWest(chunk16, x, y, z, meshData);
            //}
        }
        else
        {
            if (!chunk16.GetBlock16(x, y + 1, z).IsSolid(Direction.down))
            {
                meshData = FaceDataUp(chunk16, x, y, z, meshData);
            }

            //if (!chunk16.GetBlock16(x, y - 1, z).IsSolid(Direction.up))
            //{
            //    meshData = FaceDataDown(chunk16, x, y, z, meshData);
            //}

            //if (!chunk16.GetBlock16(x, y, z + 1).IsSolid(Direction.south))
            //{
            //    meshData = FaceDataNorth(chunk16, x, y, z, meshData);
            //}

            //if (!chunk16.GetBlock16(x, y, z - 1).IsSolid(Direction.north))
            //{
            //    meshData = FaceDataSouth(chunk16, x, y, z, meshData);
            //}

            //if (!chunk16.GetBlock16(x + 1, y, z).IsSolid(Direction.west))
            //{
            //    meshData = FaceDataEast(chunk16, x, y, z, meshData);
            //}

            //if (!chunk16.GetBlock16(x - 1, y, z).IsSolid(Direction.east))
            //{
            //    meshData = FaceDataWest(chunk16, x, y, z, meshData);
            //}
        }
        return meshData;

    }

    protected override MeshData FaceDataUp
    (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        int leftCount = 0, rightCount = 0;
        if (leftPoints != null)
            leftCount = leftPoints.Length;
        if (rightPoints != null)
            rightCount = rightPoints.Length;
        var terrainGen = new TerrainGen();
        float nw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float sw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        if (leftCount > 2)
        {
            if (leftPoints[0].x < 16 * x - 8f)
            { //start is west of block
                if (IsLeft(leftPoints[0], leftPoints[1], Vector3.right * (16 * x - 8) + Vector3.forward * (16 * z + 8)))
                { //nw corner is to left, passes south of nw corner
                    if (IsLeft(leftPoints[0], leftPoints[1], Vector3.right * (16 * x - 8) + Vector3.forward * (16 * z - 8)))
                    { //sw corner is to left, sw corner is first left side vertex
                        entryLeft = Corner.sw;
                    }
                    else
                    { //nw corner is first left side vertex
                        entryLeft = Corner.nw;
                    }
                }
                else
                { // passes north of nw corner, ne corner is first left side vertex
                    entryLeft = Corner.ne;
                }
            }
            else if (leftPoints[0].x > 16 * x + 8f)
            { //start is east of block
                if (!IsLeft(leftPoints[0], leftPoints[1], Vector3.right * (16 * x + 8) + Vector3.forward * (16 * z + 8)))
                { //ne corner is to right, passes south of ne corner
                    if (IsLeft(leftPoints[0], leftPoints[1], Vector3.right * (16 * x + 8) + Vector3.forward * (16 * z - 8)))
                    { //se corner is to left, se corner is first left side vertex
                        entryLeft = Corner.se;
                    }
                    else
                    { //sw corner is first left side vertex
                        entryLeft = Corner.sw;
                    }
                }
                else
                { // passes north of ne corner into north side, ne corner is first left side vertex
                    entryLeft = Corner.ne;
                }
            }
            else if (leftPoints[0].z > 16 * z + 8f)
            { //start is north of block, between west and east edges. ne is first left side vertex
                entryLeft = Corner.ne;
            }
            else if (leftPoints[0].z < 16 * z - 8f)
            {  //start is south of block, between west and east edges. sw is first left side vertex;
                entryLeft = Corner.sw;
            }
            else
            {
            }

            if (leftPoints[leftCount - 1].x < 16 * x - 8f)
            { //exit is west of block
                if (!IsLeft(leftPoints[leftCount - 2], leftPoints[leftCount - 1], Vector3.right * (16 * x - 8) + Vector3.forward * (16 * z + 8)))
                { //nw corner is to right, exits south of nw corner
                    if (IsLeft(leftPoints[leftCount - 2], leftPoints[leftCount - 1], Vector3.right * (16 * x - 8) + Vector3.forward * (16 * z - 8)))
                    { //sw corner is to left, sw corner left side exit
                        exitLeft = Corner.sw;
                    }
                    else
                    { //se corner is left side exit
                        exitLeft = Corner.se;
                    }
                }
                else
                { // exits north of nw corner, nw corner is left side exit
                    exitLeft = Corner.nw;
                }
            }
            else if (leftPoints[leftCount - 1].x > 16 * x + 8f)
            { //end is east of block
                if (IsLeft(leftPoints[leftCount - 2], leftPoints[leftCount - 1], Vector3.right * (16 * x + 8) + Vector3.forward * (16 * z + 8)))
                { //ne corner is to left, exits south of ne corner
                    if (IsLeft(leftPoints[leftCount - 2], leftPoints[leftCount - 1], Vector3.right * (16 * x + 8) + Vector3.forward * (16 * z - 8)))
                    { //se corner is to left, exists south of se corner, se corner is left side exit corner
                        exitLeft = Corner.se;
                    }
                    else
                    { //ne corner is left side exit corner
                        exitLeft = Corner.ne;
                    }
                }
                else
                { // exits north of ne corner into north side, nw corner is left side exit corner
                    exitLeft = Corner.nw;
                }
            }
            else if (leftPoints[leftCount - 1].z > 16 * z + 8f)
            { //exit is north of block, between west and east edges. nw is left side exit corner
                exitLeft = Corner.nw;
            }
            else if (leftPoints[leftCount - 1].z < 16 * z - 8f)
            {  //exit is south of block, between west and east edges. se is left side exit corner
                exitLeft = Corner.se;
            }
            meshData = LeftSide(chunk16, x, y, z, meshData);
        }
        else
        {
            //something not right
        }
        if (rightCount > 2)
        {
            if (rightPoints[0].x < 16 * x - 8f)
            { //start is west of block
                if (IsLeft(rightPoints[0], rightPoints[1], Vector3.right * (16 * x - 8) + Vector3.forward * (16 * z + 8)))
                { //nw corner is to the left
                    if (IsLeft(rightPoints[0], rightPoints[1], Vector3.right * (16 * x - 8) + Vector3.forward * (16 * z - 8)))
                    { //sw corner is to the left, se corner is right side entry corner
                        entryRight = Corner.se;
                    }
                    else
                    { //sw corner is to the right, sw corner is right side entry corner
                        entryRight = Corner.sw;
                    }
                }
                else
                { // nw corner is to the right, nw corner is  right side entry corner
                    entryRight = Corner.nw;
                }
            }
            else if (rightPoints[0].x > 16 * x + 8f)
            { //start is east of block
                if (!IsLeft(rightPoints[0], rightPoints[1], Vector3.right * (16 * x + 8) + Vector3.forward * (16 * z + 8)))
                { //ne corner is to the right
                    if (IsLeft(rightPoints[0], rightPoints[1], Vector3.right * (16 * x + 8) + Vector3.forward * (16 * z - 8)))
                    { //se corner is to the left, ne corner is first right side vertex
                        entryRight = Corner.ne;
                    }
                    else
                    { //se corner is to right, se corner is first right side vertex
                        entryRight = Corner.se;
                    }
                }
                else
                { // passes north of ne corner into north side, nw corner is first right side vertex
                    entryRight = Corner.nw;
                }
            }
            else if (rightPoints[0].z > 16 * z + 8f)
            { //start is north of block, between west and east edges. nw is first right side vertex
                entryRight = Corner.nw;
            }
            else if (rightPoints[0].z < 16 * z - 8f)
            {  //start is south of block, between west and east edges. se is first right side vertex;
                entryRight = Corner.se;
            }

            if (rightPoints[rightCount - 1].x < 16 * x - 8f)
            { //exit is west of block
                if (!IsLeft(rightPoints[rightCount - 2], rightPoints[rightCount - 1], Vector3.right * (16 * x - 8) + Vector3.forward * (16 * z + 8)))
                { //nw corner is to right
                    if (IsLeft(rightPoints[rightCount - 2], rightPoints[rightCount - 1], Vector3.right * (16 * x - 8) + Vector3.forward * (16 * z - 8)))
                    { //sw corner is to left, nw corner is right side exit
                        exitRight = Corner.nw;
                    }
                    else
                    { //sw corner is to right, sw corner is right side exit
                        exitRight = Corner.sw;
                    }
                }
                else
                { // exits north of nw corner, nw corner is right side exit
                    exitRight = Corner.ne;
                }
            }
            else if (rightPoints[rightCount - 1].x > 16 * x + 8f)
            { //end is east of block
                if (IsLeft(rightPoints[rightCount - 2], rightPoints[rightCount - 1], Vector3.right * (16 * x + 8) + Vector3.forward * (16 * z + 8)))
                { //ne corner is to left
                    if (IsLeft(rightPoints[rightCount - 2], rightPoints[rightCount - 1], Vector3.right * (16 * x + 8) + Vector3.forward * (16 * z - 8)))
                    { // se corner is to left, sw corner is right side exit corner
                        exitRight = Corner.sw;
                    }
                    else
                    { //se corner is right side exit corner
                        exitRight = Corner.se;
                    }
                }
                else
                { // exits north of ne corner into north side, ne corner is right side exit corner
                    exitRight = Corner.ne;
                }
            }
            else if (rightPoints[rightCount - 1].z > 16 * z + 8f)
            { //exit is north of block, between west and east edges. ne is right side exit corner
                exitRight = Corner.ne;
            }
            else if (rightPoints[rightCount - 1].z < 16 * z - 8f)
            {  //exit is south of block, between west and east edges. sw is right side exit corner
                exitRight = Corner.sw;
            }
            meshData = RightSide(chunk16, x, y, z, meshData);
        }
        else
        {
            //something not right
        }
        return meshData;
    }

    MeshData LeftSide(Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float sw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float nw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        int leftCount = leftPoints.Length;
        if(entryLeft == Corner.sw)
        {
            for (int i = 0; i < leftCount - 2; i += 2)
            {
                meshData.AddVertex(leftPoints[i + 1]);
                meshData.AddVertex(leftPoints[i]);
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(leftPoints[i + 2]);
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            if (leftCount > 2)
            {
                if (exitLeft == Corner.nw)
                {
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitLeft == Corner.ne)
                {
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(leftPoints[leftCount - 3]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitLeft == Corner.se)
                {
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(leftPoints[leftCount - 3]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[2]);
                    meshData.AddVertex(leftPoints[1]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
            }
        }
        else if(entryLeft == Corner.nw)
        { 
            for (int i = 0; i < leftPoints.Length - 2; i += 2)
            {
                meshData.AddVertex(leftPoints[i + 1]);
                meshData.AddVertex(leftPoints[i]);
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(leftPoints[i + 2]);
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            if (leftCount > 2)
            {
                if (exitLeft == Corner.ne)
                {
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                }
                else if (exitLeft == Corner.se)
                {
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(leftPoints[leftCount - 3]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitLeft == Corner.sw)
                {
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(leftPoints[leftCount - 3]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[2]);
                    meshData.AddVertex(leftPoints[1]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
            }
        }
        else if(entryLeft == Corner.ne)
        {
            for (int i = 0; i < leftPoints.Length - 2; i += 2)
            {
                meshData.AddVertex(leftPoints[i + 1]);
                meshData.AddVertex(leftPoints[i]);
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(leftPoints[i + 2]);
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            if (leftCount > 2)
            {
                if (exitLeft == Corner.se)
                {
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitLeft == Corner.sw)
                {
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(leftPoints[leftCount - 3]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitLeft == Corner.nw)
                {
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(leftPoints[leftCount - 3]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[2]);
                    meshData.AddVertex(leftPoints[1]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
            }
        }
        else if(entryLeft == Corner.se)
        {
            for (int i = 0; i < leftPoints.Length - 2; i += 2)
            {
                meshData.AddVertex(leftPoints[i + 1]);
                meshData.AddVertex(leftPoints[i]);
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddVertex(leftPoints[i + 2]);
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            if (leftCount > 2)
            {
                if (exitLeft == Corner.sw)
                {
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitLeft == Corner.nw)
                {
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(leftPoints[leftCount - 3]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitLeft == Corner.ne)
                {
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 1]);
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddVertex(leftPoints[leftCount - 2]);
                    meshData.AddVertex(leftPoints[leftCount - 3]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(leftPoints[2]);
                    meshData.AddVertex(leftPoints[1]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
            }
        }

        return meshData;
    }

    MeshData RightSide(Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float sw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float nw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        int rightCount = rightPoints.Length;

        if (entryRight == Corner.sw)
        {
            for (int i = 0; i < rightCount - 2; i += 2)
            {
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(rightPoints[i]);
                meshData.AddVertex(rightPoints[i + 1]);
                meshData.AddVertex(rightPoints[i + 2]);
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            if (exitRight == Corner.se)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            else if (exitRight == Corner.ne)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[rightCount - 3]);
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            else if (exitRight == Corner.nw)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[rightCount - 3]);
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[1]);
                meshData.AddVertex(rightPoints[2]);
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
        }
        else if (entryRight == Corner.nw)
        {
            for (int i = 0; i < rightPoints.Length - 2; i += 2)
            {
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(rightPoints[i]);
                meshData.AddVertex(rightPoints[i + 1]);
                meshData.AddVertex(rightPoints[i + 2]);
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            if (rightCount > 2)
            {
                if (exitRight == Corner.sw)
                {
                    meshData.AddVertex(rightPoints[rightCount - 2]);
                    meshData.AddVertex(rightPoints[rightCount - 1]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitRight == Corner.se)
                {
                    meshData.AddVertex(rightPoints[rightCount - 2]);
                    meshData.AddVertex(rightPoints[rightCount - 1]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(rightPoints[rightCount - 3]);
                    meshData.AddVertex(rightPoints[rightCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
                else if (exitRight == Corner.ne)
                {
                    meshData.AddVertex(rightPoints[rightCount - 2]);
                    meshData.AddVertex(rightPoints[rightCount - 1]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(rightPoints[rightCount - 3]);
                    meshData.AddVertex(rightPoints[rightCount - 2]);
                    meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                    meshData.AddVertex(rightPoints[1]);
                    meshData.AddVertex(rightPoints[2]);
                    meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                    meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                    meshData.AddQuadTriangles();
                    meshData.uv.AddRange(FaceUVs(Direction.up));
                }
            }
        }
        else if (entryRight == Corner.ne)
        {
            for (int i = 0; i < rightPoints.Length - 2; i += 2)
            {
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(rightPoints[i]);
                meshData.AddVertex(rightPoints[i + 1]);
                meshData.AddVertex(rightPoints[i + 2]);
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            if (exitRight == Corner.nw)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            else if (exitRight == Corner.sw)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[rightCount - 3]);
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            else if (exitRight == Corner.se)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[rightCount - 3]);
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[1]);
                meshData.AddVertex(rightPoints[2]);
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
        }
        else if (entryRight == Corner.se)
        {
            for (int i = 0; i < rightPoints.Length - 2; i += 2)
            {
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddVertex(rightPoints[i]);
                meshData.AddVertex(rightPoints[i + 1]);
                meshData.AddVertex(rightPoints[i + 2]);
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            if (exitRight == Corner.ne)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            else if (exitRight == Corner.nw)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[rightCount - 3]);
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
            else if (exitRight == Corner.sw)
            {
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(rightPoints[rightCount - 1]);
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[rightCount - 3]);
                meshData.AddVertex(rightPoints[rightCount - 2]);
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
                meshData.AddVertex(rightPoints[1]);
                meshData.AddVertex(rightPoints[2]);
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.up));
            }
        }

        return meshData;
    }

    protected override MeshData FaceDataDown
    (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float sw = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;

        meshData.AddVertex(new Vector3(16f * x - 8f, sw - .4f, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, se - .4f, 16 * z - 8f));
        meshData.AddVertex(new Vector3(16f * x + 8f, ne - .4f, 16 * z + 8f));
        meshData.AddVertex(new Vector3(16f * x - 8f, nw - .4f, 16 * z + 8f));

        meshData.AddQuadTriangles();
        meshData.uv.AddRange(FaceUVs(Direction.down));
        return meshData;
    }

    protected override MeshData FaceDataNorth
     (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        float bottom = 16 * y - 8f;
        var terrainGen = new TerrainGen();
        float nw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float nwLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float ne = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float neLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;

        if (bottom > ne)
        {
            meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
            if (bottom > nw)
            {
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.north));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.north));
                return meshData;
            }
        }
        else if (bottom > nw)
        {
            meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.north));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.north));
            return meshData;
        }
    }

    protected override MeshData FaceDataEast
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float ne = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float neLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float se = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float seLow = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float bottom = 16 * y - 8f;

        if (bottom > se)
        {
            meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
            if (bottom > ne)
            {
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.east));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.east));
                return meshData;
            }
        }
        else if (bottom > ne)
        {
            meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.east));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, ne, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, neLow, 16 * z + 8f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.east));
            return meshData;
        }
    }

    protected override MeshData FaceDataSouth
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float se = terrainGen.DirtHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float sw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float seLow = terrainGen.StoneHeight(16 * x + 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float swLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float bottom = 16 * y - 8f;

        if (bottom > se)
        {
            if (bottom > sw)
            {
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.south));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.south));
                return meshData;
            }
        }
        else if (bottom > sw)
        {
            meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.south));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, se, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x + 8f, seLow, 16 * z - 8f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.south));
            return meshData;
        }
    }

    protected override MeshData FaceDataWest
        (Chunk16 chunk16, int x, int y, int z, MeshData meshData)
    {
        var terrainGen = new TerrainGen();
        float nw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float nwLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z + 8f + chunk16.pos.z) - chunk16.pos.y;
        float sw = terrainGen.DirtHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float swLow = terrainGen.StoneHeight(16 * x - 8f + chunk16.pos.x, 16 * y + chunk16.pos.y, 16 * z - 8f + chunk16.pos.z) - chunk16.pos.y;
        float bottom = 16 * y - 8f;

        if (bottom > nw)
        {
            meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
            if (bottom > sw)
            {
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.west));
                return meshData;
            }
            else
            {
                meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
                meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
                meshData.AddQuadTriangles();
                meshData.uv.AddRange(FaceUVs(Direction.west));
                return meshData;
            }
        }
        else if (bottom > sw)
        {
            meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.west));
            return meshData;
        }
        else
        {
            meshData.AddVertex(new Vector3(16f * x - 8f, nwLow, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, nw, 16 * z + 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, sw, 16 * z - 8f));
            meshData.AddVertex(new Vector3(16f * x - 8f, swLow, 16 * z - 8f));
            meshData.AddQuadTriangles();
            meshData.uv.AddRange(FaceUVs(Direction.west));
            return meshData;
        }
    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 2;
                tile.y = 0;
                return tile;
            case Direction.down:
                tile.x = 2;
                tile.y = 0;
                return tile;
        }

        tile.x = 2;
        tile.y = 0;

        return tile;
    }

    public bool IsLeft(Vector3 a, Vector3 b, Vector3 c)
    {
        return ((b.x - a.x) * (c.z - a.z) - (b.z - a.z) * (c.x - a.x)) > 0;
    }
}