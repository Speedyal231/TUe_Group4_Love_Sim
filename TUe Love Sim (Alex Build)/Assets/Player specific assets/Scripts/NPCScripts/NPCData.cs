using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [Header("Starting Stats")]
    [SerializeField] float startHealth;
    [SerializeField] float punchDamage;


    private float health;

    private void Start()
    {
        health = startHealth;
    }

    /// <summary>
    /// 
    /// Fetching functions
    /// 
    /// <returns></returns>
    public float FetchHealth()
    {
        return health;
    }

    public float PunchDamage()
    {
        return punchDamage;
    }

    /// <summary>
    /// 
    /// updating functions
    /// 
    /// <returns></returns>

    public void ChangeHealth(float change)
    {
        health += change;
    }

}
