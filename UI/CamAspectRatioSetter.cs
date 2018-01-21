using UnityEngine;
using System.Collections;

public class CamAspectRatioSetter : MonoBehaviour
{
    public Camera cam;
    public float ratio;
    float previousRatio;
    
    void Start()
    {
        if(cam == null)
        {
            cam = GetComponent<Camera>();
        }
    }

    void OnPreRender()
    {
        previousRatio = cam.aspect;

        if (previousRatio != ratio)
        {
            cam.aspect = ratio;
        }

    }
}