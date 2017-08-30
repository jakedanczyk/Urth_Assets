using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public GameObject prefab;
    GameObject prefabClone;

    void Start()
    {
        prefabClone = Instantiate(prefab);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
