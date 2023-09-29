using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombatScript : MonoBehaviour
{
    [Header("Stun and damadge effects")]
    [SerializeField] string message;
    [SerializeField] float punchStunTime;
    [SerializeField] float finisherStunTime;
    [SerializeField] float UppercutStunTime;
    float currentPunchStunTime;
    float currentFinisherStunTime;

    /// <summary>
    ///
    /// 
    /// 
    /// Spacing between variable declarations and funtion declaration.
    /// 
    /// 
    /// 
    /// </summary>\


    // Start is called before the first frame update

    private void Update()
    {
        Count();
    }

    /// <summary>
    ///
    /// 
    /// 
    /// Spacing between main and general funtion declaration.
    /// 
    /// Add logic to punch player
    /// 
    /// 
    /// </summary>
    
    void Count()
    {
        if (currentPunchStunTime > 0)
            currentPunchStunTime -= Time.fixedDeltaTime;
        if (currentFinisherStunTime > 0)
            currentFinisherStunTime -= Time.fixedDeltaTime;
    }

    public void Punched()
    {
        Debug.Log(message);
        currentPunchStunTime = punchStunTime;
    }

    public void Finished()
    {
        Debug.Log(message);
        currentFinisherStunTime = finisherStunTime;
    }

    public bool StunCheck() 
    {
        if (currentFinisherStunTime > 0 || currentPunchStunTime > 0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
