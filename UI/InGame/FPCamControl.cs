using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamControl : MonoBehaviour {

    UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControls;

    void OnCollisionEnter(Collision collision)
    {
        playerControls.RevertCam();
    }
}
