using UnityEngine;
using System.Collections;

public class ItemTester : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Item item = ItemDatabase.GetItem(0);
        if (item != null)
        {
            Debug.Log(string.Format("Item ID: {0}, name: {1}, desc: {2}",
                item.itemID, item.itemName, item.itemDesc));
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
