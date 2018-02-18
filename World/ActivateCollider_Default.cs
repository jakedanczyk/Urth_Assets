using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCollider_Default : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        col.gameObject.SetActive(true);
    }
}
