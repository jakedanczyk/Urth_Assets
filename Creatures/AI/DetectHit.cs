using UnityEngine;
using System.Collections;

public class DetectHit : MonoBehaviour {

    [SerializeField]
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
}
