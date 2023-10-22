using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationBehaviour : MonoBehaviour
{

    [SerializeField] private Animator animator;

    string idleTrigger = "Idling";
    string movingTrigger = "Runs";
    string groundedTrigger = "Grounded";
    string inAirTrigger = "InAir";
    string jumpPressTrigger = "Jump";
    string walkTrigger = "Walk";
    string flyingTrigger = "Flying";
    string deadTrigger = "Dead";
    string punchTrigger = "Punch";
    string hitTrigger = "Hit";

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
    public void PunchTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(punchTrigger); else animator.ResetTrigger(punchTrigger);
    }
    public void WalkTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(walkTrigger); else animator.ResetTrigger(walkTrigger);
    }
    public void FlyingTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(flyingTrigger); else animator.ResetTrigger(flyingTrigger);
    }
    public void DeadTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(deadTrigger); else animator.ResetTrigger(deadTrigger);
    }
    public void HitTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(hitTrigger); else animator.ResetTrigger(hitTrigger);
    }
}
