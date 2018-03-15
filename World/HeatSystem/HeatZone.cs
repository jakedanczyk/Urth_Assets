using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatZone : MonoBehaviour {

    public float temperatureDifference;
    public Collider zone;

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Thermometer")
        {
            coll.gameObject.GetComponent<Thermometer>().heatZones.Add(this);   
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Thermometer")
        {
            coll.gameObject.GetComponent<Thermometer>().heatZones.Remove(this);
        }
    }
}
