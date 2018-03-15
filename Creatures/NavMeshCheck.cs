using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class NavMeshCheck : MonoBehaviour {

    public MonoBehaviour aiScript;
    public AICharacterControl characterControl;
    public NavMeshAgent agent;

    bool onNavMesh;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(CheckForNavMesh());
	}

    IEnumerator CheckForNavMesh()
    {
        while (!onNavMesh)
        {
            NavMeshHit hit;

            if (NavMesh.SamplePosition(transform.position, out hit, 1, NavMesh.AllAreas))
            {
                onNavMesh = true;
                aiScript.enabled = true;
                characterControl.enabled = true;
                agent.enabled = true;
            }

            yield return new WaitForSeconds(3);
        }
    }
}
