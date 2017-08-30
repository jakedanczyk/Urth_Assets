using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemAssembly : ItemAttribute
{
    public List<Item> components;
    public List<Transform> componentTransforms;
    int totalWeight;
    int totalVolume;
    string constructedName;
    string customName;
    
}
