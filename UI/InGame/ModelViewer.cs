using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ModelViewer : MonoBehaviour, IDragHandler, IScrollHandler
{
    public GameObject currModel;
    public RectTransform panel;
    public RectTransform parentCanvas;

    int defaultSize = 2;

    public void ChangeModel(GameObject newModel)
    {
        Destroy(currModel);
        currModel = newModel;
        newModel.transform.localPosition = Vector3.zero;
        SetLayerRecursively(currModel, 5);
        OnRectTransformDimensionsChange();
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
        float scale = (panel.rect.xMax - panel.rect.xMin) < (panel.rect.yMax - panel.rect.yMin) ? (panel.rect.xMax - panel.rect.xMin)/defaultSize : (panel.rect.yMax - panel.rect.yMin)/defaultSize;
        currModel.transform.localScale = 1.8f*scale*Vector3.one;
    }
}

