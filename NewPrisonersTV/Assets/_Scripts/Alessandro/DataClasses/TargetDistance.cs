using UnityEngine;
using System;
[System.Serializable]
public class TargetDistance : IComparable<TargetDistance>
{

    public int targetIndex;
    public float distance;

    public TargetDistance()
    {
        targetIndex = -1;
        distance = Mathf.Infinity;
    }
    public TargetDistance(int index, float dist)
    {
        targetIndex = index;
        distance = dist;
    }
    // compare for Array.sort
    public int CompareTo(TargetDistance obj) 
    {
        return distance.CompareTo(obj.distance);
    }
}


