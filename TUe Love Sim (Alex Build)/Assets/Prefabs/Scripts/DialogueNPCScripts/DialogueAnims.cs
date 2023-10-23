using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnims : MonoBehaviour
{
    [SerializeField] private Animator animator;


    string resetTrigger = "Reset";
    string rizzSuccessTrigger = "RizzSuccess";
    string rizzFailTrigger = "RizzFailed";

    public void ResetTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(resetTrigger); else animator.ResetTrigger(resetTrigger);
    }
    public void RizzSuccessTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(rizzSuccessTrigger); else animator.ResetTrigger(rizzSuccessTrigger);
    }
    public void RizzFailTriggerSet(bool active)
    {
        if (active) animator.SetTrigger(rizzFailTrigger); else animator.ResetTrigger(rizzFailTrigger);
    }
}
