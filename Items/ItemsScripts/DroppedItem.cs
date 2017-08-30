using UnityEngine;
using System.Collections;

public class DroppedItem : MonoBehaviour {

    public Item itemOnGround;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public DroppedItem(Item item)
    {
        itemOnGround = item;
    }
}
