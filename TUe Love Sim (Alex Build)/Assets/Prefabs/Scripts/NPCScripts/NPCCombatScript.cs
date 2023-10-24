using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombatScript : MonoBehaviour
{
    [Header("Object Declarations")]
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform characterTransform;
    [SerializeField] Transform playerTransform;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] NPCData characterData;
    [SerializeField] PlayerData playerData;
    [SerializeField] NPCMovement npcMovement;
    [SerializeField] EnemyAnimationBehaviour EnemyAnimation;

    [Header("Stun and damadge effects")]
    [SerializeField] float punchStunTime;
    [SerializeField] float finisherStunTime;
    [SerializeField] float sphereRayOffset;
    [SerializeField] float hitRange;
    [SerializeField] float punchCooldown;
    [SerializeField] float knockback;
    float currentPunchCooldown;
    bool canPunch;
    float currentPunchStunTime;
    float currentFinisherStunTime;
    bool inRange;
    RaycastHit hit;
    bool died = false;

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

    private void FixedUpdate()
    {
        if (characterData.FetchDead())
        {
            Count();
            if (!died)
            {
                EnemyAnimation.DeadTriggerSet(true);
                
                if (currentPunchStunTime <= 0) 
                {
                    died = true;
                    playerData.ChangeKills(1);
                    EnemyAnimation.MovingTriggerSet(false);
                    EnemyAnimation.WalkTriggerSet(false);
                    EnemyAnimation.IdlingTriggerSet(false);
                    EnemyAnimation.PunchTriggerSet(false);
                    EnemyAnimation.FlyingTriggerSet(false);
                    EnemyAnimation.HitTriggerSet(false);
                    EnemyAnimation.InAirTriggerSet(false);
                    EnemyAnimation.JumpPressTriggerSet(false);
                    EnemyAnimation.GroundedTriggerSet(false);
                }
            }
            else
            {
                EnemyAnimation.DeadTriggerSet(false);
                EnemyAnimation.MovingTriggerSet(false);
                EnemyAnimation.WalkTriggerSet(false);
                EnemyAnimation.IdlingTriggerSet(false);
                EnemyAnimation.PunchTriggerSet(false);
                EnemyAnimation.FlyingTriggerSet(false);
                EnemyAnimation.HitTriggerSet(false);
                EnemyAnimation.InAirTriggerSet(false);
                EnemyAnimation.JumpPressTriggerSet(false);
                EnemyAnimation.GroundedTriggerSet(false);
            }
        }
        else 
        {
            EnemyAnimation.DeadTriggerSet(false);
            Count();
            CanHit();
            Punch();
            if (!StunCheck()) 
            {
                EnemyAnimation.HitTriggerSet(false);
                EnemyAnimation.FlyingTriggerSet(false);
            }
        }
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
        if (currentPunchCooldown > 0)
            currentPunchCooldown -= Time.fixedDeltaTime;
    }

    public void Punched()
    {
        
        currentPunchStunTime = punchStunTime;
        npcMovement.playerInRange = true;
        if (!died) 
        {
            EnemyAnimation.HitTriggerSet(true);
        }
        if (characterData.FetchHealth() <= playerData.PunchDamage())
        {
            characterData.ChangeHealth(-characterData.FetchHealth());
        }
        else 
        {
            characterData.ChangeHealth(-playerData.PunchDamage());
        }

    }

    public void Finished()
    {
        currentFinisherStunTime = finisherStunTime;
        currentPunchStunTime = punchStunTime;
        if (!died) 
        {
            EnemyAnimation.FlyingTriggerSet(true);
        }
        if (characterData.FetchHealth() <= playerData.FinisherDamage())
        {
            characterData.ChangeHealth(-characterData.FetchHealth());
        }
        else
        {
            characterData.ChangeHealth(-playerData.FinisherDamage());
        }
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

    private void Punch() 
    {
        if (inRange && currentPunchCooldown <=0) 
        {
            canPunch = true;
        }
        else
        {
            canPunch = false;
        }

        if (canPunch)
        {
            if (hit.collider.TryGetComponent(out PlayerCombatScript playerCombatScript))
            {
                EnemyAnimation.PunchTriggerSet(true);
                playerCombatScript.PlayerDamage(characterData.PunchDamage());
                currentPunchCooldown = punchCooldown;
                playerRB.velocity += characterTransform.forward.normalized * knockback + playerTransform.up.normalized * knockback / 4;
            }
        }
        else
        {
            EnemyAnimation.PunchTriggerSet(false);
        }
        
    }

    private void CanHit()
    {
        inRange = Physics.SphereCast(characterTransform.position + (characterTransform.up.normalized * (capsuleCollider.height / 2)) - (sphereRayOffset * characterTransform.forward.normalized), capsuleCollider.radius, characterTransform.forward, out hit, hitRange + sphereRayOffset, playerLayer);
    }

    public bool FetchDead()
    {
        return characterData.FetchDead();
    }
}
