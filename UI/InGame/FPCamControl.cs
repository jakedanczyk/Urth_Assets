using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamControl : MonoBehaviour {

    public GameObject player;
    
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }
}
