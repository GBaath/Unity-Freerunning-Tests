using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLocker : MonoBehaviour
{
    public bool clampX, clampY, clampZ;
    public float maxX, maxY, maxZ;
    public float minX, minY, minZ;

    public bool lockToRefPoint;
    public Transform refTransform;
    public float maxRefX, maxRefY, maxRefZ;
    public float minRefX, minRefY, minRefZ;
    void Update()
    {
        if (!lockToRefPoint)
        {
            if (clampX)
            {
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, minX, maxX), transform.localPosition.y, transform.localPosition.z);
            }
            else if (clampY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x,Mathf.Clamp(transform.localPosition.y, minY, maxY), transform.localPosition.z);
            }
            else if (clampZ)
            {
                transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y, Mathf.Clamp(transform.localPosition.z, minZ, maxZ));
            }
        }
        else
        {

            if (clampX)
            {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, refTransform.position.x+minRefX, refTransform.position.x+maxRefX), transform.position.y, transform.position.z);
            }
            else if (clampY)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, refTransform.position.y + minRefY, refTransform.position.y + maxRefY), transform.position.z);
            }
            else if (clampZ)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, refTransform.position.z + minRefZ, refTransform.position.z + maxRefZ));
            }
        }
    }
}
