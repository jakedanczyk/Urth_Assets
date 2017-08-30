using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class BlockBuildingSystem : MonoBehaviour {
    public Dictionary<BlockType, Type> blocks = new Dictionary<BlockType, Type>
    {
        { BlockType.Granite, typeof(Block) }, { BlockType.Grass, typeof(BlockGrass) } };

    public Inventory supplyInventory;

    public buildObjects currentObject;
    private Vector3 currentPos;
    private Vector3 currentRot;
    public Transform currentPreview;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layer;

    public float offset = 1.0f;
    public float gridSize = 1.0f;

    public bool isBuilding;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public int i = 0;
    void Update()
    {
        if (isBuilding)
        {
            startPreview();
            if (Input.GetButtonDown("Fire1"))
                Build();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            if (isBuilding)
            {
                Destroy(currentPreview.gameObject);
                isBuilding = false;
            }
            else
            {
                ChangeCurrentBlock();
                isBuilding = true;
            }
        }
    }

    public void ChangeCurrentBlock()
    {

    }

    public void startPreview()
    {
        if(Physics.Raycast (cam.position, cam.forward, out hit, 40, layer))
        {
            Debug.DrawLine(cam.position, hit.point, Color.green);
            if (hit.transform != this.transform)
                showPreview(hit);
        }
    }

    public void showPreview (RaycastHit hit2)
    {
        currentPos = hit2.point;
        currentPos -= Vector3.one * offset;
        currentPos /= gridSize;
        currentPos = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));
        currentPos *= gridSize;
        currentPos += Vector3.one * offset;
        currentPreview.position = currentPos;
        if (Input.GetButtonDown("Fire2"))
            currentRot += new Vector3(0, 90, 0);
        currentPreview.localEulerAngles = currentRot;
    }

    public void Build()
    {
        PreviewBlock PO = currentPreview.GetComponent<PreviewBlock>();
        if(PO.isBuildable)
        {
            //if supplies available
            for (int i = 0; i < PO.supplyListItems.Count; i++)
            {
                var count = supplyInventory.inventoryContents.Where(item => item.itemName == PO.supplyListItems[i].itemName).Count();
                print(count);
                if (count < PO.supplyListNumbers[i]) { print("insufficient supplies"); return; }
            }
            //build item
            var newobject = Instantiate(currentObject.prefab, currentPos, Quaternion.Euler(currentRot));
            BuildingObject newBuildingObject = newobject.GetComponent<BuildingObject>();

            //move supplies into building object
            for (int i = 0; i < PO.supplyListItems.Count; i++)
            {
                for (int j = 0; j < PO.supplyListNumbers[i]; j++)
                {
                    Item consumedItem = (supplyInventory.inventoryContents.Find(delegate (Item searchItem) { return searchItem.itemName == PO.supplyListItems[i].itemName; }));
                    supplyInventory.RemoveItem(consumedItem);
                    newBuildingObject.mats.Add(consumedItem);
                    consumedItem.gameObject.SetActive(false);
                }
            }
            print("buildable built");
        }
    }
}
