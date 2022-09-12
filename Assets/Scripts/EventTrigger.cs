using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent linkedEvent;
    public UnityEvent linkedEventOnExit;
    public string colliderTag;
    public LayerMask colliderLayer;
    public LayerMask ignoreLayer;

    public Collider TriggerCollider;
    public bool disableColliderOnTrigger;
    public bool disableColliderOnTriggerExit;

    public bool triggerOnExit;
    public bool separateEventOnTriggerExit;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == colliderTag)
        {
            linkedEvent.Invoke();
            if(disableColliderOnTrigger)
                TriggerCollider.enabled = false;
        }
        else if(other.gameObject.layer == colliderLayer.value)
        {
            linkedEvent.Invoke();
            if (disableColliderOnTrigger)
                TriggerCollider.enabled = false;
        }
        else if (other.gameObject.layer != ignoreLayer)
        {
            linkedEvent.Invoke();
            if (disableColliderOnTrigger)
                TriggerCollider.enabled = false;
        }
        else if (colliderTag == null)
        {
            linkedEvent.Invoke();
            if (disableColliderOnTrigger)
                TriggerCollider.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (triggerOnExit)
        {
            if (separateEventOnTriggerExit)
            {
                if (linkedEventOnExit!=null)
                {
                    if (other.tag == colliderTag)
                    {
                        linkedEventOnExit.Invoke();
                        if (disableColliderOnTriggerExit)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.gameObject.layer == colliderLayer.value)
                    {
                        linkedEventOnExit.Invoke();
                        if (disableColliderOnTriggerExit)
                            TriggerCollider.enabled = false;
                    }
                    else if (other.gameObject.layer != ignoreLayer)
                    {
                        linkedEventOnExit.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                    else if (colliderTag == null)
                    {
                        linkedEventOnExit.Invoke();
                        if (disableColliderOnTrigger)
                            TriggerCollider.enabled = false;
                    }
                }
            }
            else
            {
                if (other.tag == colliderTag)
                {
                    linkedEventOnExit.Invoke();
                    if (disableColliderOnTriggerExit)
                        TriggerCollider.enabled = false;
                }
                else if (other.gameObject.layer == colliderLayer.value)
                {
                    linkedEventOnExit.Invoke();
                    if (disableColliderOnTriggerExit)
                        TriggerCollider.enabled = false;
                }
                else if (colliderTag == null)
                {
                    linkedEventOnExit.Invoke();
                    if (disableColliderOnTrigger)
                        TriggerCollider.enabled = false;
                }
            }
        }
    }
}
