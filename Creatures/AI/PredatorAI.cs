using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PredatorAI : MonoBehaviour {
    public BodyManager playerBody;
    public Transform player, eyes;
    public Animator anim;
    public AICharacterControl aiCharControl;
    public BodyManager bodyManager;
    bool isAttacking, isRunning, seesPlayer;
    [Range(1f, 180f)]
    public float peripheralVisionAngle;
    [Range(0.01f, 1000f)]
    public float peripheralVisionDistance;
    public LayerMask visionLayerMask = 3844609;


    // Use this for initialization
    void Start()
    {
        player = UnityStandardAssets.Characters.FirstPerson.PlayerControls.playerControlsGameObject.transform;
        playerBody = player.GetComponent<BodyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, this.transform.position) < peripheralVisionDistance)
        {
            Vector3 direction = (player.position + Vector3.up) - eyes.position;
            float angle = Vector3.Angle(direction, this.transform.forward);
            if (angle < peripheralVisionAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(eyes.position, direction, out hit, peripheralVisionAngle, visionLayerMask))
                {
                    if (hit.transform.gameObject.layer == 20 && playerBody.alive)
                    {
                        if (!seesPlayer)
                        {
                            seesPlayer = true;
                            bodyManager.AggressiveBellow();
                        }
                        aiCharControl.target = player.position;
                        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                        //Quaternion.LookRotation(direction), 0.2f
                        if (direction.magnitude > aiCharControl.agent.stoppingDistance)
                        {
                            if (!isRunning)
                            {
                                isRunning = true;
                                anim.SetBool("isRunning", true);
                            }
                        }
                        if (direction.magnitude < aiCharControl.agent.stoppingDistance * 2)
                        {
                            if (!isAttacking)
                            {
                                isAttacking = true;
                                StartCoroutine(PrimaryAttack());
                            }
                        }
                    }
                }
                else
                    seesPlayer = false;
            }
            else
                seesPlayer = false;
        }
        else
            seesPlayer = false;

        if (!seesPlayer)
        {
            if (isRunning)
            {
                isRunning = false;
                anim.SetBool("isRunning", false);
            }
            if (isAttacking)
            {
                isAttacking = false;
                StopCoroutine(PrimaryAttack());
            }
        }
    }

    IEnumerator PrimaryAttack()
    {
        while (isAttacking)
        {
            bodyManager.PrimaryAttack();
            yield return new WaitForSeconds(2);
        }
    }

    void OnDeserialize()
    {
        player = GameObject.Find("PlayerObject").transform;
        playerBody = player.GetComponent<BodyManager>();
    }
}
