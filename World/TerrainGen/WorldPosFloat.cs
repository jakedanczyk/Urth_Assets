using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct WorldPosFloat
{
    public float x, y, z;

    public WorldPosFloat(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 47;

            hash = hash * 227 + x.GetHashCode();
            hash = hash * 227 + y.GetHashCode();
            hash = hash * 227 + z.GetHashCode();

            return hash;
        }
    }

    public override bool Equals(object obj)
    {
        if (GetHashCode() == obj.GetHashCode())
            return true;
        return false;
    }
}