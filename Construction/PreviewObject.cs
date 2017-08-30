using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PreviewObject : MonoBehaviour {

    public List<Collider> col = new List<Collider>();
    public objectsorts sort;
    public Material green;
    public Material red;
    public bool isBuildable;

    public List<Item> supplyListItems;
    public List<int> supplyListNumbers;

    public bool second;

    public PreviewObject childCol;

    public Transform graphics;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 21)
            col.Add(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 21)
            col.Remove (other);
    }

    void Update()
    {
        if(!second)
        changecolor();
    }
    public void changecolor()
    {
        if (sort == objectsorts.foundation)
        {
            if (col.Count == 0)
                isBuildable = true;
            else
                isBuildable = false;
        }
        else
        {
            if (col.Count == 0 && childCol.col.Count > 0)
                isBuildable = true;
            else
                isBuildable = false;
        }

        if (isBuildable)
        {
            foreach(Transform child in graphics)
            {
                child.GetComponent<Renderer>().material = green;
            }
        }
        else
        {
            foreach (Transform child in graphics)
            {
                child.GetComponent<Renderer>().material = red;
            }
        }
    }
}

public class BuildingMats
{
    public Item item;
    public int numItems;
}

public enum objectsorts
{
    normal,
    foundation,
    floor,
    wall

}

