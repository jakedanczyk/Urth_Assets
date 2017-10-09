using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWater : MonoBehaviour {

    public MeshFilter fluidFilter;

    // Use this for initialization
    void Start () {
        fluidFilter = gameObject.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
