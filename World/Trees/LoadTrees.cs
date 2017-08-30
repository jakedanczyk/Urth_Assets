using UnityEngine;
using System.Collections;
using SimplexNoise;

public class LoadTrees : MonoBehaviour {
    public GameObject prefab;
    public float gridX = 5f;
    public float gridY = 5f;
    public float spacing = 2f;

    float treeFrequency = 0.2f;
    int treeDensity = 0;

    // Use this for initialization
    void Start () {

        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                Vector3 pos = new Vector3(x, 0, y) * spacing;
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
