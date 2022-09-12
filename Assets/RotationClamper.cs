using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationClamper : MonoBehaviour
{
    public Transform rotationToFollow;
    public bool clampX, clampY, clampZ;
    public float minX, minY, minZ, maxX, maxY, maxZ;
    [SerializeField]private Vector3 rot;

    void Update()
    {
        rot = transform.rotation.eulerAngles;
        if (rotationToFollow != null)
            rot = rotationToFollow.rotation.eulerAngles;
        FixRotation();
        Quaternion rotation = Quaternion.Euler(Mathf.Clamp(rot.x, minX, maxX), Mathf.Clamp(rot.y, minY, maxY), Mathf.Clamp(rot.z, minZ, maxZ));
        transform.localRotation = Quaternion.Euler(Mathf.Clamp(rot.x, minX, maxX), Mathf.Clamp(rot.y, minY, maxY), Mathf.Clamp(rot.z, minZ, maxZ));
        //transform.localRotation = Quaternion.Euler(rot);
    }

    //switches to negative numbers to match value in inspector
    private void FixRotation()
    {
        if (clampX)
        {
            if (rot.x > 180)
                rot.x -= 360;
            else if (rot.x < -180)
                rot.x += 360;
            rot.x = Mathf.Clamp(rot.x, minX, maxX);
        }
        if (clampY)
        {
            if (rot.y > maxY && rot.x - 360 >= minY)
                rot.y -= 360;
            rot.y = Mathf.Clamp(rot.y, minY, maxY);
        }
        if (clampZ)
        {
            if (rot.z > maxZ && rot.x - 360 >= minZ)
                rot.z -= 360;
            rot.z = Mathf.Clamp(rot.z, minZ, maxZ);
        }
    }
}
