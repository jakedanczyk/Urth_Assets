using UnityEngine;
using System.Collections;

public class Mining : MonoBehaviour {

    public Animator anim;
    public Transform camTrans;

    public GameObject drop;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Attack");
            // if (col.gameObject) {
            //   EditTerrain.SetBlock(col.gameObject, new BlockAir());
            //}

            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 12f))
            {
                Block block = EditTerrain.GetBlock(hit);

                EditTerrain.HitBlock(hit, 60);

                Instantiate(drop, hit.point, Quaternion.identity);
                //EditTerrain.SetBlock(hit, new BlockTouchedGrass());
            }
        }
    }

    void BreakBlock(Item breakingTool, Block block)
    {
        anim.SetBool("isMining", true);

    }
}
