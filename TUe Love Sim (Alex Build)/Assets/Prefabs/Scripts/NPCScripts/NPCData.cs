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
    private bool dead;

    private void Start()
    {
        health = startHealth;
    }

    private void FixedUpdate()
    {
        UpdateDead();
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

    public bool FetchDead()
    {
        return dead;
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

    private void UpdateDead()
    {
        if (health <= 0)
        {
            dead = true;
        }
        else
        {
            dead = false;
        }
    }
}