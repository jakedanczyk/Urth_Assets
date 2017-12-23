using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterInfoPanel : MonoBehaviour {

    public GridLayoutGroup grid;
    public RectTransform parentPanel, grandParentPanel, siblingPanel;

    void OnRectTransformDimensionsChange()
    {
        //parentPanel.rect.width = parentPanel.rect.width - 
        Vector2 vec = Vector2.zero;
        vec.Set(parentPanel.rect.width / 2, parentPanel.rect.height);
        grid.cellSize = vec;
    }

}
