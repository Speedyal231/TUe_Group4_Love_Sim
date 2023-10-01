using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour
{

    [Header("Object Declarations")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] Rigidbody RB;
    [SerializeField] PlayerControllerScript playerControllerScript;
    [SerializeField] NPCData enemyData;
    [SerializeField] PlayerData playerData;
    private PlayerInputActions playerInputActions;
    
    [Header("Punch and Attack Values")]
    [SerializeField] float punchHitBoxRadius;
    [SerializeField] float punchHitBoxLength;
    [SerializeField] float punchKnockback;
    [SerializeField] float FinisherKnockback;
    [SerializeField] LayerMask enemyLayer;
    RaycastHit[] enemiesHit;

    [Header("Punch and Attack Timings")]
    [SerializeField] float punchCooldown;
    [SerializeField] float finisherCooldown;
    [SerializeField] float nextPunchTimer;
    [SerializeField] float hitDuration;
    float currentPunchCooldown;
    float currentFinisherCooldown;
    float currentNextPunchTimer;
    bool canPunch;
    bool punched;

    [Header("Hitstun")]
    [SerializeField] float hitStunTime;
    float currentHitStuntTime;

    private enum CombatState
    {
        FirstPunch,
        SecondPunch,
        ThirdPunch,
        Finisher
    }
    private CombatState comboState;


    /// <summary>
    /// 
    /// 
    /// Variable to Unity update and start separator
    /// 
    /// 
    /// </summary>

    private void Awake()
    {
        //enable player input script.
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        punched = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        comboState = CombatState.FirstPunch;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerData.FetchDead()) 
        {
            float punchVal = PunchInput();
            Count();
            HitBoxSpawn(punchVal);
            Punch(punchVal);
        }
    }

    /// <summary>
    /// 
    /// 
    /// Unity update and start to regualr function separator
    /// 
    ///
    /// </summary>

    private float PunchInput()
    {
        return playerInputActions.Keyboard.Punch.ReadValue<float>();
    }

    void Count()
    {
        if (currentPunchCooldown > 0)
            currentPunchCooldown -= Time.fixedDeltaTime;
        if (currentNextPunchTimer > 0)
            currentNextPunchTimer -= Time.fixedDeltaTime;
        if (currentFinisherCooldown > 0)
            currentFinisherCooldown -= Time.fixedDeltaTime;
        if (currentHitStuntTime > 0)
            currentHitStuntTime -= Time.fixedDeltaTime;
    }

    private void HitBoxSpawn(float punchVal)
    {
        if (punchVal > 0) 
        {
            enemiesHit = Physics.SphereCastAll(transform.position + (transform.up.normalized * (capsuleCollider.height * 3 / 4)), punchHitBoxRadius, cameraTransform.forward, punchHitBoxLength, enemyLayer, QueryTriggerInteraction.Ignore);
        }
    }

    private void Punch(float punchVal)
    {
        if (currentNextPunchTimer <= 0)
        {
            comboState = CombatState.FirstPunch;
        }
        if (punched && punchVal > 0)
        {
            canPunch = false;
        }
        else if ((punched && punchVal <= 0) || !punched)
        {
            punched = false;

            if (comboState == CombatState.FirstPunch)
            {
                if (currentPunchCooldown <= 0 && currentFinisherCooldown <= 0)
                {
                    canPunch = true;
                }
                else
                {
                    canPunch = false;
                }
            }
            else if (comboState == CombatState.SecondPunch)
            {
                if (currentPunchCooldown <= 0 && currentNextPunchTimer > 0)
                {
                    canPunch = true;
                }
                else
                {
                    canPunch = false;
                }
            }
            else if (comboState == CombatState.ThirdPunch)
            {
                if (currentPunchCooldown <= 0 && currentNextPunchTimer > 0)
                {
                    canPunch = true;
                }
                else
                {
                    canPunch = false;
                }
            }
            else if (comboState == CombatState.Finisher)
            {
                if (currentPunchCooldown <= 0 && currentNextPunchTimer > 0)
                {
                    canPunch = true;
                }
                else
                {
                    canPunch = false;
                }
            }
        }
        
        
        if (punchVal > 0 && !punched)
        {
            if ((canPunch) )
            {
                if (enemiesHit.Count() > 0)
                {
                    foreach (RaycastHit enemy in enemiesHit)
                    {
                        if (enemy.collider.TryGetComponent(out NPCCombatScript npcCombatScript))
                        {
                            // add charcter velocity to knock back dog.
                            if (comboState == CombatState.FirstPunch || comboState == CombatState.SecondPunch || comboState == CombatState.ThirdPunch)
                            {
                                npcCombatScript.Punched();
                                enemy.rigidbody.velocity += enemy.transform.up.normalized * punchKnockback / 4 + Vector3.ProjectOnPlane(cameraTransform.forward, playerTransform.up).normalized * punchKnockback;
                                currentPunchCooldown = punchCooldown;
                                currentNextPunchTimer = nextPunchTimer;
                                punched = true;
                                

                            }
                            else if (comboState == CombatState.Finisher)
                            {
                                enemy.rigidbody.velocity += enemy.transform.up.normalized * FinisherKnockback / 4 + Vector3.ProjectOnPlane(cameraTransform.forward, playerTransform.up).normalized * FinisherKnockback;
                                currentFinisherCooldown = finisherCooldown;
                                currentNextPunchTimer = nextPunchTimer;
                                npcCombatScript.Finished();
                                punched = true;
                                
                            }
                        }
                    }
                }
                else
                {
                    punched = true;
                }
                
                if (comboState == CombatState.FirstPunch)
                {
                    comboState = CombatState.SecondPunch;
                }
                else if (comboState == CombatState.SecondPunch)
                {
                    comboState = CombatState.ThirdPunch;
                }
                else if (comboState == CombatState.ThirdPunch)
                {
                    comboState = CombatState.Finisher;
                }
                else if (comboState == CombatState.Finisher)
                {
                    comboState = CombatState.FirstPunch;
                }

            }
        }
    }

    public void PlayerDamage() 
    {
        currentHitStuntTime = hitStunTime;
        if (playerData.FetchHealth() <= enemyData.PunchDamage())
        {
            playerData.ChangeHealth(-playerData.FetchHealth());
        }
        else
        {
            playerData.ChangeHealth(-enemyData.PunchDamage());
        }
    }

    public bool FetchDead() 
    { 
        return playerData.FetchDead();
    }

    public bool Stunned() 
    {
        return currentHitStuntTime > 0 ? true : false ;
    }
    

}
