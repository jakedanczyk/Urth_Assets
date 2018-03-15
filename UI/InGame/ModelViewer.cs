using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ModelViewer : MonoBehaviour, IDragHandler, IScrollHandler
{
    public static RectTransform modelViewerObject;
    public GameObject currModel;
    public RectTransform panel;
    public RectTransform parentCanvas;
    public static GameObject modelViewObject;
    public Text infoText;


    int defaultSize = 2;

    void Awake()
    {
        modelViewerObject = this.GetComponent<RectTransform>();
        modelViewObject = this.gameObject;
    }

    public void ChangeModel(GameObject newModel)
    {
        Destroy(currModel);
        currModel = newModel;
        newModel.transform.localPosition = Vector3.zero;
        SetLayerRecursively(currModel, 5);
        OnRectTransformDimensionsChange();
    }

    public void ChangeModel(Item newItem)
    {
        Destroy(currModel);
        GameObject model = Instantiate(newItem.itemModel);
        model.transform.SetParent(transform);
        model.transform.localPosition = Vector3.zero;
        currModel = model;
        model.transform.localPosition = Vector3.zero;
        SetLayerRecursively(currModel, 5);
        OnRectTransformDimensionsChange();
        if (newItem is Item_Weapon)
        {
            Item_Weapon weapon = (Item_Weapon)newItem;
            infoText.text = "Damage\nBlunt:  " + weapon.baseBlunt + "\n Cut:     " + weapon.baseCut + "\n Pierce: " + weapon.basePierce + 
                            "\n Woodchop rate: " + weapon.TreeChopRateFactor();            
        }
        else if(newItem is Item_Food)
        {
            Item_Food food = (Item_Food)newItem;
            infoText.text = "Calories: " + food.calories + "\n Water:    " + food.water + "\n Protein:  " + food.protein + "\n Fat:      " + food.fat + "\n Carb:     " + food.carb;
        }
        else if (newItem is ItemStackFood)
        {
            ItemStackFood food = (ItemStackFood)newItem;
            infoText.text = "Calories: " + food.calories + "\n Water:    " + food.water + "\n Protein:  " + food.protein + "\n Fat:      " + food.fat + "\n Carb:     " + food.carb + "\n Quantity: " + food.numItems;
        }
        else if(newItem is Item_Garment)
        {
            Item_Garment garment = (Item_Garment)newItem;
            infoText.text = "Protection\nBlunt:  " + garment.protection[0] + "\n Cut:     " + garment.protection[1] + "\n Pierce: " + garment.protection[2] + "\n Warmth: " + garment.protection[3] + "\n Water:  " + garment.protection[4] + "\n Wind:   " + garment.protection[5];
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer )
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }


    public void OnDrag(PointerEventData data)
    {
        Debug.Log("Draggable Mouse Down");
        PointerEventData ped = (PointerEventData)data;
        currModel.transform.Rotate(Vector3.down * ped.delta.x);
        currModel.transform.Rotate(Vector3.left * ped.delta.y);
    }

    public void OnScroll(PointerEventData data)
    {
        Debug.Log("Mouse Wheel");
        PointerEventData ped = (PointerEventData)data;
        currModel.transform.localScale += ped.scrollDelta.y * Vector3.one * 3;
        if (currModel.transform.localScale.x > (2*(panel.rect.xMax - panel.rect.xMin) / defaultSize) || currModel.transform.localScale.x > (2 * (panel.rect.yMax - panel.rect.yMin) / defaultSize))
        {
            float scale = (panel.rect.xMax - panel.rect.xMin) < (panel.rect.yMax - panel.rect.yMin) ? (panel.rect.xMax - panel.rect.xMin) / defaultSize : (panel.rect.yMax - panel.rect.yMin) / defaultSize;
            currModel.transform.localScale = 2* scale * Vector3.one;
        }
        else if (currModel.transform.localScale.x < (.2 * (panel.rect.xMax - panel.rect.xMin) / defaultSize) || currModel.transform.localScale.x < (.2 * (panel.rect.yMax - panel.rect.yMin) / defaultSize))
        {
            float scale = (panel.rect.xMax - panel.rect.xMin) > (panel.rect.yMax - panel.rect.yMin) ? (panel.rect.xMax - panel.rect.xMin) / defaultSize : (panel.rect.yMax - panel.rect.yMin) / defaultSize;
            currModel.transform.localScale = .1f * scale * Vector3.one;
        }
    }

    void OnRectTransformDimensionsChange()
    {
        if (currModel != null)
        {
            float scale = (panel.rect.xMax - panel.rect.xMin) < (panel.rect.yMax - panel.rect.yMin) ? (panel.rect.xMax - panel.rect.xMin) / defaultSize : (panel.rect.yMax - panel.rect.yMin) / defaultSize;
            currModel.transform.localScale = 1.8f * scale * Vector3.one;
        }
    }
}

