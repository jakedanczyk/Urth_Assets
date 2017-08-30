using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackContainer : MonoBehaviour {

    public int stackCount;
    public GameObject itemPrefab;

    public void PullItem(Inventory pullingInventory)
    {
        var instance = Instantiate(itemPrefab);
        pullingInventory.AddItem(instance.GetComponent<Item>());
        instance.SetActive(false);
        stackCount--;
        if (stackCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

}
