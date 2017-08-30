using UnityEngine;
using System.Collections;

public class DetectHit : MonoBehaviour {

    Animator anim;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit");
        //anim.SetBool("isDead", true);
    }

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
