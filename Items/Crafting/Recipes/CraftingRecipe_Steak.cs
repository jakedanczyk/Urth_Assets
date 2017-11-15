using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe_Steak : CraftingRecipe {

    public Fire fire;
    public StartFireButtonScript fireButton;

    public override bool RequirementsCheck()
    {
        fire = fireButton.currentFire;
        return fire.isLit;
    }

}
