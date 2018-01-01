using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class Chase : MonoBehaviour {

    public BodyManager playerBody;
    public Transform player;
    public Animator anim;
    public AICharacterControl aiCharControl;
    public BodyManager_Skeleton skeletonBody;
    public bool attacking = false;

	// Use this for initialization
	void Start () {
        if (LevelSerializer.IsDeserializing) return;

        player = GameObject.Find("PlayerObject").transform;
        playerBody = player.GetComponent<BodyManager>();
        anim = GetComponent<Animator>();
        anim.speed = 2.0f;
    }

    // Update is called once per frame
    void Update() {
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        if (Vector3.Distance(player.position, this.transform.position) < 64 && angle < 90)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, direction, out hit, 64))
            {
                if (hit.transform.gameObject.layer == 20 && playerBody.alive)
                {
                    aiCharControl.target = player;
                    //this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                    //Quaternion.LookRotation(direction), 0.2f);
                    anim.SetBool("isIdle", false);
                    if (direction.magnitude > 1.5)
                    {
                        //this.transform.Translate(0, 0, 0.510f);
                        anim.SetBool("isWalking", true);
                        anim.SetBool("isAttacking", false);
                        attacking = false;

                    }
                    else if (!attacking)
                    {
                        attacking = true;
                        anim.SetBool("isAttacking", true);
                        anim.SetBool("isWalking", false);
                        print(665);
                        InvokeRepeating("MainAttack", .5f, 3f);
                    }
                }
            }
        }



        else
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            CancelInvoke();
            skeletonBody.StopCoroutine("Claw");
        }
    }

    void MainAttack()
    {
        print(664);

        skeletonBody.MainAttack();
    }

    void OnDeserialize()
    {
        player = GameObject.Find("PlayerObject").transform;
        playerBody = player.GetComponent<BodyManager>();
    }
}
