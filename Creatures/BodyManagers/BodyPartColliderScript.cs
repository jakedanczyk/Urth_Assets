using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartColliderScript : MonoBehaviour {

    public Collider bodyPartCollider;
    public RPGStatType bodyPartType;
    public BodyManager parentBody;

	// Use this for initialization
	void Start () {
        parentBody = transform.root.GetComponent<BodyManager>();
	}
}
