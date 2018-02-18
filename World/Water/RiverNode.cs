using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverNode : MonoBehaviour {

    public MeshFilter roughFilter, fineFilter;
    public MeshCollider coll;
    public IEnumerator erode16;

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
    }

    public void RenderMeshRough(MeshData meshData)
    {
        roughFilter.mesh.Clear();
        roughFilter.mesh.vertices = meshData.vertices.ToArray();
        roughFilter.mesh.triangles = meshData.triangles.ToArray();

        roughFilter.mesh.uv = meshData.uv.ToArray();
        roughFilter.mesh.RecalculateNormals();
    }

    public List<Vector3> waypoints, left, right;
    public List<float> widths;

    IEnumerator ErodeChunk16()
    {
        TerrainGen terrainGen = new TerrainGen();
        World256 world = World256.worldGameObject.GetComponent<World256>();
        HashSet<Vector3> clearedBlocks = new HashSet<Vector3>();
        foreach (Vector3 waypoint in waypoints)
        {
            if (waypoint.y < terrainGen.shrubline)
            {
                //world.SetBlock256((int)(river.source.x + waypoint.x), (int)(river.source.y + waypoint.y - 256), (int)(river.source.z + waypoint.z), new Block256GrassRiverValley());
                //world.SetBlock256((int)(river.source.x + waypoint.x), (int)(river.source.y + waypoint.y), (int)(river.source.z + waypoint.z), new Block256Air());
                //world.SetBlock256((int)(river.source.x + waypoint.x), (int)(river.source.y + waypoint.y + 256), (int)(river.source.z + waypoint.z), new Block256Air());
                yield return null;
            }
        }
    }
}
