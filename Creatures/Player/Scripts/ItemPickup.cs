using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

    public Transform camTrans;
    public Inventory inventory;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {

            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 20f))
            {
                string s = hit.collider.gameObject.tag;
                Debug.Log(s);

                if (hit.collider.gameObject.tag == "Item")
                {
                    DroppedItem pickedUpItem = hit.collider.gameObject.GetComponent<DroppedItem>();
                    inventory.AddItem(pickedUpItem.itemOnGround);
                    Destroy(hit.collider.gameObject);
                }


            }
        }
    }
}
