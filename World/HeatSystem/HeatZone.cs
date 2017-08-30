using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatZone : MonoBehaviour {

    public float temperatureDifference;
    public Collider zone;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider coll)
    {
        print("enter");
        if(coll.gameObject.tag == "Thermometer")
        {
            coll.gameObject.GetComponent<Thermometer>().heatZones.Add(this);   
        }
    }
}
