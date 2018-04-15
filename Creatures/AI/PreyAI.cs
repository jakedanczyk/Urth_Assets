using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PreyAI : MonoBehaviour {

    public BodyManager playerBody;
    public Transform player, eyes;
    public Animator anim;
    public AICharacterControl aiCharControl;
    public BodyManager bodyManager;
    bool isAttacking, isRunning, isFleeing, approachingTarget, seesPlayer;
    [Range(1f, 180f)]
    public float peripheralVisionAngle;
    [Range(0.01f, 1000f)]
    public float peripheralVisionDistance;
    public LayerMask visionLayerMask = 3844609;

    TerrainGen terrainGen = new TerrainGen();
    public Vector3 target = new Vector3();

    // Use this for initialization
    void Start()
    {
        player = UnityStandardAssets.Characters.FirstPerson.PlayerControls.playerControlsGameObject.transform;
        playerBody = player.GetComponent<BodyManager>();
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(player.position, transform.position);
        if (playerDistance < peripheralVisionDistance)
        {
            Vector3 direction = (player.position + Vector3.up) - eyes.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < peripheralVisionAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(eyes.position, direction, out hit, peripheralVisionAngle, visionLayerMask))
                {
                    if (Application.isEditor)
                    {
                        Debug.DrawLine(eyes.position, hit.point, Color.blue);
                    }
                    if (hit.transform.gameObject.layer == 20 && playerBody.alive)
                    {
                        seesPlayer = true;
                        if (approachingTarget || !isFleeing)
                        {
                            aiCharControl.agent.speed = 6;
                            NavMeshHit navMeshHit;
                            if (NavMesh.SamplePosition(transform.position - direction.normalized * 100f, out navMeshHit, 100f, NavMesh.AllAreas))
                            {
                                isFleeing = true;
                                aiCharControl.target = aiCharControl.leader.transform;
                                aiCharControl.leader.transform.position = target = navMeshHit.position;
                                approachingTarget = false;
                            }
                        }
                        if (!isRunning)
                        {
                            isRunning = true;
                            bodyManager.Run();
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
        }
        else
            seesPlayer = false;

        if (!isFleeing)
        {
            aiCharControl.agent.speed = 1;
            if (approachingTarget)
            {
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), out navMeshHit, 20f, NavMesh.AllAreas))
                {
                    aiCharControl.target = aiCharControl.leader.transform;
                    aiCharControl.leader.transform.position = target = navMeshHit.position;
                    approachingTarget = false;
                    bodyManager.Walk();
                }
            }
            else if(Vector3.Distance(transform.position, target) < 3)
            {
                approachingTarget = true;
                bodyManager.Idle();
            }
        }


        if (isFleeing && Vector3.Distance(target, transform.position) < 7)
        {
            approachingTarget = true;
            if (!seesPlayer && Vector3.Distance(target, transform.position) < 3)
            {
                if (isRunning)
                {
                    isFleeing = false;
                    isRunning = false;
                    bodyManager.Idle();
                }
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
        player = UnityStandardAssets.Characters.FirstPerson.PlayerControls.playerControlsGameObject.transform;
        playerBody = player.GetComponent<BodyManager>();
    }   
}
