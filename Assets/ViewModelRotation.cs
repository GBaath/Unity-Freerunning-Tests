using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewModelRotation : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(Vector3.Slerp(transform.rotation.eulerAngles, transform.parent.rotation.eulerAngles, 1f));
    }
}
