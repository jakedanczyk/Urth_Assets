using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenLeaderScript : MonoBehaviour {

    public Transform thisTransform;
    public Rigidbody playerRigidBody;

    Vector3 newPoint;
    Vector3 oldPoint;
    Vector3 vector;
    float hVelocity;

    void FixedUpdate()
    {
        oldPoint = newPoint;
        newPoint = playerRigidBody.position;
        vector = newPoint - oldPoint;
        hVelocity = Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z);
        thisTransform.localPosition = new Vector3(0, vector.y*25, 25*hVelocity);
    }
}
