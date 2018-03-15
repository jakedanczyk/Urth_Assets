using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamControl : MonoBehaviour {

    public Transform head;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = head.position;
        transform.localPosition += offset;
    }
}
