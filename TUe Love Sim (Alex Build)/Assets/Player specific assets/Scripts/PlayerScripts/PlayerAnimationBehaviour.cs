using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehaviour : MonoBehaviour
{

    [SerializeField] private Animator animator;

    string idleTrigger = "Idling";
    string movingTrigger = "Running";
    string groundedTrigger = "Grounded";
    string inAirTrigger = "InAir";
    string jumpPressTrigger = "JumpPress";

    public void IdlingTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(idleTrigger); else animator.ResetTrigger(idleTrigger);
    }

    public void MovingTriggerSet(bool active) 
    {
        if (active) animator.SetTrigger(movingTrigger); else animator.ResetTrigger(movingTrigger);
    }

    public void GroundedTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(groundedTrigger); else animator.ResetTrigger(groundedTrigger);
    }

    public void InAirTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(inAirTrigger); else animator.ResetTrigger(inAirTrigger);
    }

    public void JumpPressTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(inAirTrigger); else animator.ResetTrigger(inAirTrigger);
    }
}
