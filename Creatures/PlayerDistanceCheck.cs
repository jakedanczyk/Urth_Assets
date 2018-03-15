using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceCheck : MonoBehaviour {


    public BodyManager playerBody;
    public Transform player;
    bool inRange;
    // Use this for initialization
    void Start()
    {
        player = UnityStandardAssets.Characters.FirstPerson.PlayerControls.playerControlsGameObject.transform;
        playerBody = player.GetComponent<BodyManager>();
        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance()
    {
        while (!inRange)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < 350)
                inRange = true;
            yield return new WaitForSeconds(10);
        }
    }


}
