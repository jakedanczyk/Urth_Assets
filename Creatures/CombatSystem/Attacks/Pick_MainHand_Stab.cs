using UnityEngine;
using System.Collections;

public class Pick_MainHand_Stab : Attack {


    // Use this for initialization
    void Start()
    {

            //anim.SetTrigger("Attack");
            // if (col.gameObject) {
            //   EditTerrain.SetBlock(col.gameObject, new BlockAir());
            //}

            RaycastHit hit;
            if (Physics.Raycast(aimPoint.position, aimPoint.forward, out hit, reach))
            {
                GameObject target = hit.transform.gameObject;
                if (target.tag == "Terrain")
                { //Block block = EditTerrain.GetBlock(hit);

                    EditTerrain.HitBlock(hit, 6000);
                    Debug.Log("hit terrain");
                }
                if (hit.transform.tag == "Creature")
                { targetDC = target.GetComponentInParent(typeof(DamageController)) as DamageController;
                    //targetDC.TakeDamage(tag.tag, baseDamage);   
                }
            }
    }
}
