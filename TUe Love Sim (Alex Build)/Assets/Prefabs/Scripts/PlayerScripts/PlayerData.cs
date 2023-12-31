using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Starting Stats")]
    [SerializeField] float startHealth;
    [SerializeField] float punchDamage;
    [SerializeField] float finisherDamage;
    private float health;
    private float combo;
    private float targetRizzedCount;
    private bool dead;
    private float time;
    private float kills;
    RizzStatus rizzStatus;
    public enum RizzStatus 
    {
        MegaMinger,
        Minger,
        Normie,
        Rizzler,
        RizzGod,
        GigaChad
    }

    private void Start()
    {
        time = 0;
        health = startHealth; 
        combo = 0;
        targetRizzedCount = 0;
        rizzStatus = RizzStatus.MegaMinger;
        kills = 0;
    }

    private void FixedUpdate()
    {
        UpdateDead();
        TimeUpdate();
        Debug.Log(kills);
        //Debug.Log(health);
        //Debug.Log(combo);
        //Debug.Log(targetRizzedCount);
        //Debug.Log(rizzStatus);

    }


    /// <summary>
    /// 
    /// Fetching functions
    /// 
    /// <returns></returns>
    /// 

    private void TimeUpdate() 
    {
        time += Time.deltaTime;
    }

    public float PunchDamage() 
    {
        return punchDamage;
    }

    public float FinisherDamage()
    {
        return finisherDamage;
    }

    public float FetchHealth() 
    {
        return health;
    } 

    public float FetchCombo() 
    {
        return combo;
    }

    public float FetchTime() 
    {
        return time;
    }

    public float FetchTargetRizzedCount() 
    {
        return targetRizzedCount;
    }

    public RizzStatus GetRizzStatus() 
    { 
        return rizzStatus;
    }

    public bool FetchDead() 
    {
        return dead;
    }

    public float Fetchkills() 
    {
        return kills;
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

    public void ChangeCombo(float change)
    {
        combo += change;
    }

    public void ChangeTargetRizzedCount(float change)
    {
        targetRizzedCount += change;
    }

    public void ChangeKills(float change)
    {
        kills += change;
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
