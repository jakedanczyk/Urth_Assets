using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DynamicDOF : MonoBehaviour {

    public PostProcessingProfile profile;
    public Camera mainCam;
    public LayerMask mask;
    // Update is called once per frame
    void Update() {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0F));
        RaycastHit hit; //Variable reading information about the collider hit.

        //Cast ray from center of the screen towards where the player is looking.
        if (Physics.Raycast(ray, out hit, 1000,mask))
        {
            profile.depthOfField.settings = new DepthOfFieldModel.Settings
            {
                focusDistance = hit.distance,
                aperture = 5.6f,
                focalLength = 50f,
                useCameraFov = false,
                kernelSize = DepthOfFieldModel.KernelSize.Medium
            };
        }
    }
}
