using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotationLocker : MonoBehaviour
{
    public bool lockX, lockY, lockZ;
    private float startRotX, startRotY, startRotZ;
    [SerializeField] private Transform refResetPoint;

    private int count=0;

    private void Start()
    {
        startRotX = transform.rotation.eulerAngles.x;
        startRotY = transform.rotation.eulerAngles.y;
        startRotZ = transform.rotation.eulerAngles.z;
    }

    private void Update()
    {
        if (lockX)
        {
            transform.rotation = Quaternion.Euler(startRotX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        if (lockY)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, startRotY, transform.rotation.eulerAngles.z);
        }
        if (lockZ)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, startRotZ);
        }
        count++;
        if (count >= 10)
        {
            count = 0;
            Update10();
        }
    }
    private void Update10()
    {
        if (refResetPoint != null)
        {
            transform.rotation = refResetPoint.rotation;
        }
    }
}
