using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingObject : MonoBehaviour {


    public int health;
    public int maxHealth;
    public int loadLimit;
    public int load;
    public int weight;
    public int totalWeight;
    public List<BuildingObject> supportedBuildingObjects;
    public List<Item> mats;
    public PreviewObject preview;

    private void Awake()
    {
        mats = preview.supplyListItems;
    }

    void WeightUpdate()
    {
        load = supportedBuildingObjects.Sum(wgt => wgt.totalWeight);
        totalWeight = weight + load;
        if(load > loadLimit)
        {
            Collapse();
        }
    }

    void Collapse()
    {
        for(int i = 0; i < mats.Count; i++)
        {
            if(UnityEngine.Random.Range(0,1) > 0)
            {
                mats[i].transform.position = this.transform.position;
                mats[i].gameObject.SetActive(true);
            }
        }
    }
}
