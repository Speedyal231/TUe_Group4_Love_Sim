using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour
{

    [Header("Object Declarations")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] CapsuleCollider capsuleCollider;
    private PlayerInputActions playerInputActions;


    [Header("Punch and Attack Values")]
    [SerializeField] float punchHitBoxRadius;
    [SerializeField] float punchHitBoxLength;
    [SerializeField] float punchKnockback;
    [SerializeField] float FinisherKnockback;
    [SerializeField] float UppercutKnockback;
    [SerializeField] LayerMask enemyLayer;
    RaycastHit[] enemiesHit;
    bool hitLanded;

    [Header("Punch and Attack Timings")]
    [SerializeField] float punchCooldown;
    [SerializeField] float finisherCooldown;
    [SerializeField] float uppercutCooldown;
    [SerializeField] float nextPunchTimer;
    [SerializeField] float uppercutTransitionTime;
    float currentPunchCooldown;
    float currentFinisherCooldown;
    float currentUppercutCooldown;
    float currentPunchTimer;
    float currentUppercutTransitionTimer;
    bool canPunch;


    private enum CombatState
    {
        FirstPunch,
        SecondPunch,
        ThirdPunch,
        Finisher,
        AirFinisher
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float punchVal = PunchInput();
        HitBoxSpawn(punchVal);
        Punch(punchVal);
        Count();
        Debug.Log(currentPunchCooldown);
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
        if (comboState == CombatState.FirstPunch) 
        {
            if (currentPunchCooldown <= 0)
            {
                canPunch = true;
            }
            else 
            {
                canPunch = false;
            }
        }




        if (canPunch && punchVal > 0) 
        {
            foreach (RaycastHit enemy in enemiesHit)
            {
                if (enemy.collider.TryGetComponent(out NPCCombatScript npcCombatScript))
                {
                    if (comboState == CombatState.FirstPunch) 
                    {
                        npcCombatScript.Damaged();
                        enemy.rigidbody.velocity += enemy.transform.up.normalized * punchKnockback / 4 + Vector3.ProjectOnPlane(cameraTransform.forward, playerTransform.up).normalized * punchKnockback;
                        currentPunchCooldown = punchCooldown;
                    }
                }
            }
        }
    }
}
