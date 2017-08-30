using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitUIScript : MonoBehaviour
{
    int printPos = 375;

    BodyManager body;

    private void Awake()
    {
        body = GetComponentInParent<OutfittingUIScript>().body;
    }
    void OnGUI()
    {

    }
}