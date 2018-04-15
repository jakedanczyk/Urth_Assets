using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class BlockBuildingSystem : MonoBehaviour {
    public Dictionary<BlockType, string> blocks = new Dictionary<BlockType, string>
    {
        { BlockType.Granite, "Block1" },
        { BlockType.Grass, "Block1Grass" }
    };

    public enum Mode
    {
        Inventory,
        StockPile,
    }

    Mode buildMode = Mode.Inventory;

    public Mode BuildMode{
        get { return buildMode; }
        set { buildMode = value; }
    }

    public Inventory supplyInventory;

    public Item_TerrainBlock buildingBlock;

    public buildObjects currentObject;
    private Vector3 currentPos;
    private Vector3 currentRot;
    public GameObject currentPreview;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layer;

    public float offset = 1.0f;
    public float gridSize = 1.0f;

    public bool isBuilding;

    public int i = 0;
    void Update()
    {
        if(buildingBlock != null)
        {
            if(currentPreview != null)
            {
                if (Physics.Raycast(cam.position, cam.forward, out hit, 40, layer))
                {
                    Vector3 previewPos = Vector3.Lerp(cam.position, hit.point, .995f);
                    WorldPos pos = GetBlockPos(previewPos);

                    currentPreview.transform.position = new Vector3(pos.x, pos.y, pos.z);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        print("build");

                        var nb = Activator.CreateInstance(Type.GetType(blocks[buildingBlock.blockType]));
                        SetBlock(pos,(Block1)nb);
                    }
                }
            }
        }
    }

    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
            );

        return blockPos;
    }

    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return GetBlockPos(pos);
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
            {
                pos += (norm / 2);
            }
            else
            {
                pos -= (norm / 2);
            }
        }

        return (float)pos;
    }

    public void ChangeCurrentBlock()
    {

    }

    public void startPreview()
    {
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
        currentPreview.transform.position = currentPos;
        if (Input.GetButtonDown("Fire2"))
            currentRot += new Vector3(0, 90, 0);
        currentPreview.transform.localEulerAngles = currentRot;
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
    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }

    public bool SetBlock(WorldPos pos, Block1 block, bool adjacent = false)
    {
        World1 world = World1.worldGameObject.GetComponent<World1>();
        Chunk1 chunk = world.GetChunk1((int)pos.x, (int)pos.y, (int)pos.z);
        if (chunk == null)
            return false;

        if(buildMode == Mode.Inventory)
        {
            supplyInventory.DropItem(buildingBlock);
            Destroy(buildingBlock.gameObject);
        }
        world.SetBlock1(pos.x, pos.y, pos.z, block);

        return true;
    }
}
