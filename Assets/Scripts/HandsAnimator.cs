using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HandsAnimator : MonoBehaviour
{
    public Animator animator;
    private bool _sprinting;
    public bool sprinting
    {
        get
        {return _sprinting;}
        set
        {
            _sprinting = value;
            animator.SetBool("Sprinting", value);
        }
    }
    public void SetTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
    public void SetAnimSpeed(float speed)
    {
        animator.speed = speed;
    }
}
