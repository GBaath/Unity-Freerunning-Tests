using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimator : MonoBehaviour
{
    public Animator animator;
    public void SetTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
    public void SetAnimSpeed(float speed)
    {
        animator.speed = speed;
    }
    public void DisableAnim()
    {
        animator.enabled = false;
    }
    public void Roll()
    {
        StartCoroutine(Roll(0.7f));
    }
    public IEnumerator Roll(float rollSpeed)
    {
        float time = 0;
        float startVal = Camera.main.transform.localEulerAngles.x;
        Quaternion startRot = Camera.main.transform.localRotation;
        while (time < rollSpeed)
        {
            Camera.main.transform.localEulerAngles= new Vector3(Mathf.Lerp(startVal, startVal+360, time / rollSpeed),0,0);
            time += Time.deltaTime;
            yield return null;
        }
       // Camera.main.transform.rotation = startRot;
    }
}
