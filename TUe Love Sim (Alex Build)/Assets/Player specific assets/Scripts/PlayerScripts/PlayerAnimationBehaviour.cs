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
    string walledTrigger = "Walled";

    string punch1 = "P1";
    string punch2 = "P2";
    string punch3 = "P3";
    string finisher = "Finisher";
    string damaged = "Damaged"; 


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
        if (active) animator.SetTrigger(jumpPressTrigger); else animator.ResetTrigger(jumpPressTrigger);
    }

    public void WalledTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(walledTrigger); else animator.ResetTrigger(walledTrigger);
    }
    public void Punch1Set(bool active)
    {
        if (active) animator.SetTrigger(punch1); else animator.ResetTrigger(punch1);
    }
    public void Punch2Set(bool active)
    {
        if (active) animator.SetTrigger(punch2); else animator.ResetTrigger(punch2);
    }
    public void Punch3Set(bool active)
    {
        if (active) animator.SetTrigger(punch3); else animator.ResetTrigger(punch3);
    }
    public void FinisherSet(bool active)
    {
        if (active) animator.SetTrigger(finisher); else animator.ResetTrigger(finisher);
    }
    public void DamagedSet(bool active) 
    {
        if (active) animator.SetTrigger(damaged); else animator.ResetTrigger(damaged);
    }
}
