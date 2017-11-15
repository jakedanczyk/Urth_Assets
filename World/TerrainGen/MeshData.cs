using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();

    public List<Vector3> fluidVertices = new List<Vector3>();
    public List<int> fluidTriangles = new List<int>();
    public List<Vector2> fluiduv = new List<Vector2>();

    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();

    public List<Vector3> fluidColVertices = new List<Vector3>();
    public List<int> fluidColTriangles = new List<int>();

    public bool useRenderDataForCol;

    public MeshData() { }

    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if (useRenderDataForCol)
        {
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 3);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 1);
        }
    }

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);

        if (useRenderDataForCol)
        {
            colVertices.Add(vertex);
        }

    }

    public void AddTriangle(int tri)
    {
        triangles.Add(tri);

        if (useRenderDataForCol)
        {
            colTriangles.Add(tri - (vertices.Count - colVertices.Count));
        }
    }

    public void AddQuadFluidTriangles()
    {
        fluidTriangles.Add(fluidVertices.Count - 4);
        fluidTriangles.Add(fluidVertices.Count - 3);
        fluidTriangles.Add(fluidVertices.Count - 2);

        fluidTriangles.Add(fluidVertices.Count - 4);
        fluidTriangles.Add(fluidVertices.Count - 2);
        fluidTriangles.Add(fluidVertices.Count - 1);
        if (useRenderDataForCol)
        {
            fluidColTriangles.Add(fluidColVertices.Count - 4);
            fluidColTriangles.Add(fluidColVertices.Count - 3);
            fluidColTriangles.Add(fluidColVertices.Count - 2);
            fluidColTriangles.Add(fluidColVertices.Count - 4);
            fluidColTriangles.Add(fluidColVertices.Count - 2);
            fluidColTriangles.Add(fluidColVertices.Count - 1);
        }
    }

    public void AddFluidVertex(Vector3 vertex)
    {
        fluidVertices.Add(vertex);
        if (useRenderDataForCol)
        {
            fluidColVertices.Add(vertex);
        }
    }

    public void AddFluidTriangle(int tri)
    {
        fluidTriangles.Add(tri);
        if (useRenderDataForCol)
        {
            fluidColTriangles.Add(tri - (fluidVertices.Count - fluidColVertices.Count));
        }
    }
}