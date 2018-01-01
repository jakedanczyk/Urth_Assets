using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartColliderScript : MonoBehaviour {

    public Collider bodyPartCollider;
    public RPGStatType bodyPartType;
    public BodyManager parentBody;

	// Use this for initialization
	void Start () {
        if (LevelSerializer.IsDeserializing) return;
        parentBody = GetComponentInParent<BodyManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
