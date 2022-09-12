using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleChecker : MonoBehaviour
{
    public bool blockedForward, blockedAbove, blockedLow, blockedMid;



    public void BlockForward(bool value)
    {
        blockedForward = value;
    }
    public void BlockAbove(bool value)
    {
        blockedAbove = value;
    }
    public void BlockUnder(bool value)
    {
        blockedLow = value;
    }
    public void BlockMid(bool value)
    {
        blockedMid = value;
    }
}
