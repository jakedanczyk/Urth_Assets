using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGBodyPart : RPGVital {

    public BodyPartType bodyPartType;
    public int[] protection = new int[6]; // blunt cut pierce insulation water wind
    public float wetness = 0;
    public float damageModifer = 1;

    public virtual int[] ProtValue
    {
        get { return protection; }
    }

}
