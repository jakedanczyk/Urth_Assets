using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour
{
    public GameObject player;
    public float cameraHeight = 10000.0f;

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, cameraHeight,player.transform.position.z);
    }
}