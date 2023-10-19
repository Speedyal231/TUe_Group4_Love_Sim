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

        }
        else 
        {
            Count();
            CanHit();
            Punch();
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
                playerCombatScript.PlayerDamage();
                currentPunchCooldown = punchCooldown;
                playerRB.velocity += characterTransform.forward.normalized * knockback + playerTransform.up.normalized * knockback / 4; 
            }
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
