using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWater : MonoBehaviour {

    public MeshFilter fluidFilter;
    public MeshCollider coll;

    // Use this for initialization
    void Start () {
        fluidFilter = gameObject.GetComponent<MeshFilter>();
        coll = gameObject.GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
