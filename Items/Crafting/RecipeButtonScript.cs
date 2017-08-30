using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class RecipeButtonScript : MonoBehaviour, ISelectHandler
{
    public CraftingSystem parentCrafter;
    public CraftingRecipe parentRecipe;

    //void SelectOnClick()
    //{
    //    print(1);

    //    parentCrafter.selectedRecipe = parentRecipe;
    //}
    public void OnSelect(BaseEventData eventData)
    {
        print(1);
        parentCrafter.selectedRecipe = parentRecipe;
    }
}
