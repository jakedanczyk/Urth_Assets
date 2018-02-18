using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCollider_Default : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        col.gameObject.SetActive(false);
    }
}
