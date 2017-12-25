using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTest : MonoBehaviour {

    public RPGStatCollection stats;
    public List<RPGStat> statlist;
    public List<RPGStatType> typelist;
    // Use this for initialization
    void Start () {
        foreach (KeyValuePair<RPGStatType, RPGStat> entry in stats.StatDict)
        {
            statlist.Add(entry.Value);
            typelist.Add(entry.Key);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
