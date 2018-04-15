using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroScript : MonoBehaviour {

    public GameObject gasGiant, urthOrbit, urth, moon, youAreHere;
    public WorldTime worldTime;
    float prevTime;
    public Light planetLight, sunLight, moonLight;
	// Use this for initialization
	void Start () {
        prevTime = worldTime.totalGameSeconds;
        StartCoroutine(HeavenlyMotion());
	}

    IEnumerator HeavenlyMotion()
    {
        while (true)
        {
            //transform.Rotate(Vector3.right * Time.deltaTime);
            urthOrbit.transform.Rotate(0,(worldTime.totalGameSeconds - prevTime) / 14400,0);

            //urthOrbit.transform.Rotate(urthOrbit.transform.up * (worldTime.totalGameSeconds - prevTime) / 14400);
            urth.transform.Rotate(0, (worldTime.totalGameSeconds - prevTime) / 240, 0);
            //urth.transform.Rotate(Vector3.up);
            float sunAngle = Vector3.Angle(youAreHere.transform.up, transform.position - youAreHere.transform.position);
            if (sunAngle < 95)
            {
                sunLight.intensity = Mathf.Min((95 - sunAngle) / 6, 1) * 2f;
                sunLight.enabled = true;
                sunLight.transform.rotation = Quaternion.LookRotation(youAreHere.transform.InverseTransformDirection(youAreHere.transform.position - transform.position));
            }
            else
                planetLight.enabled = false;
            float planetAngle = Vector3.Angle(youAreHere.transform.up, gasGiant.transform.position - youAreHere.transform.position);
            if(planetAngle < 95)
            {
                planetLight.intensity = Mathf.Min((95 - planetAngle) / 6, 1) * 0.3f;
                planetLight.enabled = true;
                planetLight.transform.rotation = Quaternion.LookRotation(youAreHere.transform.InverseTransformDirection(youAreHere.transform.position - gasGiant.transform.position));
            }
            else
                planetLight.enabled = false;
            prevTime = worldTime.totalGameSeconds;
            yield return new WaitForSeconds(1);
        }
    }
}
