using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBodyPartsPanel : MonoBehaviour {

    public GridLayoutGroup grid;
    public RectTransform parentPanel;

    void OnRectTransformDimensionsChange()
    {
        Vector2 vec = Vector2.zero;
        vec.Set(parentPanel.rect.width / 3, parentPanel.rect.height / 7);
        grid.cellSize = vec;
    }

}
