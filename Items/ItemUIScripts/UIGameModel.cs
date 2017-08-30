using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIGameModel : MonoBehaviour
{
    public bool overGui;

    private Vector3 rotVector;

    void Awake()
    {
        rotVector = Random.insideUnitSphere * 90f;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (overGui)
            meshRenderer.material.color = Color.red;
        else
            meshRenderer.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    void Update()
    {
        transform.Rotate(Time.deltaTime * rotVector);
    }
}