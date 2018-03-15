using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static GameObject audioManagerGameObject;

    void Awake()
    {
        audioManagerGameObject = this.gameObject;
    }

    public AudioClip ImpactSound(GameObject impactor, GameObject target)
    {
        return metalOnWood;
    }

    public AudioClip ImpactSound(Item_Weapon impactor, Collision target)
    {
        if ((int)impactor.primaryMaterialType < 100)// == MaterialType.Iron || impactor.primaryMaterialType == MaterialType.Copper)
        {
            if (target.gameObject.tag == "BodyPart")
            {
                return hitBody;
            }
            else if (target.gameObject.tag == "Tree" || target.gameObject.tag == "FelledTree")
            {
                return metalOnWood;
            }
            else if(target.gameObject.layer == 13)
            {
                Block16 block = World16.worldGameObject.GetComponent<World16>().GetBlock16(target.contacts[0].point);
                if (block is Block16)
                {
                    return metalOnStone;
                }
                else if (block is Block16Grass || block is Block16RiverGrass)
                    return hitDirt;
                else
                    return hitDirt;
            }
            else if (target.gameObject.layer == 15)
            {
                Block4 block = World4.worldGameObject.GetComponent<World4>().GetBlock4(target.contacts[0].point);
                if (block is Block4)
                {
                    return metalOnStone;
                }
                else if (block is Block4Grass)// || block is Block16RiverGrass)
                    return hitDirt;
                else
                    return hitDirt;
            }
            else if (target.gameObject.layer == 17)
            {
                Block1 block = World1.worldGameObject.GetComponent<World1>().GetBlock1(target.contacts[0].point);
                if (block is Block1)
                {
                    return metalOnStone;
                }
                else if (block is Block1Grass)// || block is Block16RiverGrass)
                    return hitDirt;
                else
                    return hitDirt;
            }
            else if (target.gameObject.layer == 19)
            {
                Block block = World.worldGameObject.GetComponent<World>().GetBlock(target.contacts[0].point);
                if (block is Block)
                {
                    return metalOnStone;
                }
                else if (block is BlockGrass)// || block is Block16RiverGrass)
                    return hitDirt;
                else
                    return hitDirt;
            }
        }
        return metalOnWood;
    }

    //public AudioClip ImpactSound(Item_Weapon impactor, string tag)
    //{
    //    if(impa)
    //    return metalOnWood;
    //}

    //public AudioClip ImpactSound(Item_Weapon impactor, Collider target)
    //{
    //    return metalOnWood;
    //}

    //Dictionary<MaterialType,Dictionary<string,AudioClip>> itemImpactDict = new Dictionary<MaterialType, Dictionary<string, AudioClip>> { { MaterialType.Iron, new Dictionary<string,AudioClip>} 

    public AudioClip inventoryTransfer, openInventory,
        metalOnStone, metalOnWood, hitDirt,
        draw, sheathe, swing, equip, hitBody, 
        drink, eat, fall, teethChatter;
}
