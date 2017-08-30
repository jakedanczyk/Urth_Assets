using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DamageController : MonoBehaviour {

    public float health = 100f;
    [Range(0.0f, 100.0f)]
    public float headHealth;
    [Range(0.0f, 100.0f)]
    public float neckHealth;
    [Range(0.0f, 100.0f)]
    public float chestHealth;
    [Range(0.0f, 100.0f)]
    public float stomachHealth;
    [Range(0.0f, 100.0f)]
    public float hipHealth;
    [Range(0.0f, 100.0f)]
    public float lShoulderHealth;
    [Range(0.0f, 100.0f)]
    public float rShoulderHealth;
    [Range(0.0f, 100.0f)]
    public float lArmHealth;
    [Range(0.0f, 100.0f)]
    public float rArmHealth;
    [Range(0.0f, 100.0f)]
    public float lForearmHealth;
    [Range(0.0f, 100.0f)]
    public float rForearmHealth;
    [Range(0.0f, 100.0f)]
    public float lHandHealth;
    [Range(0.0f, 100.0f)]
    public float rHandHealth;
    [Range(0.0f, 100.0f)]
    public float lThighHealth;
    [Range(0.0f, 100.0f)]
    public float rThighHealth;
    [Range(0.0f, 100.0f)]
    public float lLowerLegHealth;
    [Range(0.0f, 100.0f)]
    public float rLowerLegHealth;
    [Range(0.0f, 100.0f)]
    public float lFootHealth;
    [Range(0.0f, 100.0f)]
    public float rFootsHealth;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(BodyPartTag bodyPartHit, float damage)
    {

    }
}
