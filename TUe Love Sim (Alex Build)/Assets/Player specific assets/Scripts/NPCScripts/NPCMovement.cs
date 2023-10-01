using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{

    //Declare reference to Input System, Rigid body, transform etc.
    [Header("Object Declarations")]
    [SerializeField] Rigidbody RB;
    [SerializeField] Transform characterTransform;
    [SerializeField] Transform playerTransform;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] NPCCombatScript NPCCombat;


    [Header("Movement")]
    [SerializeField] float docileMaxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationFriction;
    [SerializeField] float groundMaxSpeed;
    [SerializeField, Range(0,1)] float turnSpeed;
    [SerializeField] float wallTouchThreshold;
    [SerializeField] float wallJumpThreshold;
    [SerializeField] float turnRange;
    [SerializeField] float randyTime;
    Vector3 randomVector;
    float currentRandyTime;
    bool shouldTurn;
    Vector3 velocity;

    //Variables that contribute to and store object collisions and raycast data
    [Header("Ground Detection")]
    [SerializeField] float rayOriginOffset;
    [SerializeField] float groundHitRange;
    [SerializeField] LayerMask groundLayer;
    private RaycastHit groundedRayPointNPC;
    bool grounded;

    [Header("Air Variables")]
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;
    [SerializeField] float airDrag;
    [SerializeField] float airMaxSpeed;

    [Header("Combat Movement")]
    [SerializeField] float groundMaxSpeedInRange;
    [SerializeField] float airMaxSpeedInRange;
    [SerializeField] float comabtRadius;
    [SerializeField] float sphereRayOffset;
    [SerializeField] LayerMask playerLayer;
    RaycastHit wallRangeRay;
    RaycastHit pointWallTouchData;
    bool playerInRange;
    bool stunned;
    bool walled;
    bool canJump;

    private enum State
    {
        Ground,
        Air
    }
    private State state;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        PhysicsCalcInit();
        GroundedCheck();
        StateSwitch();
        CombatRangeCheck();
        HitStunCheck();
        JumpCheck();
        WallCheck();
        WallUnstick();
        MoveCheck();
        RandomDirectionGen();
        Count();

        if (stunned || NPCCombat.FetchDead())
        {
            if (state == State.Air)
            {
                Turn();
                Gravity();
                AirDrag();
            }
            else if (state == State.Ground)
            {
                GroundSnap();
                Turn();
                Friction();
            }
        }
        else 
        {
            if (state == State.Air)
            {
                Turn();
                Move();
                Gravity();
                AirDrag();
            }
            else if (state == State.Ground)
            {
                GroundSnap();
                Turn();
                Move();
                Friction();
                Jump();
            }
        }
        
        ApplyForces();
    }
    /// <summary>
    ///
    /// 
    /// 
    /// Spacing between main and general funtion declaration.
    /// 
    /// add logic to cause NPC to move around walls
    /// add logic for NPC to jump.
    /// 
    /// 
    /// </summary>

    private void PhysicsCalcInit()
    {
        velocity = Vector3.zero;
    }

    private void ApplyForces()
    {
        RB.velocity += velocity;
    }

    void Count()
    {
        if (currentRandyTime > 0)
            currentRandyTime -= Time.fixedDeltaTime;
    }

    private void Turn() 
    {
        Vector3 NPCDirection;
        if (playerInRange) 
        {
            NPCDirection = playerTransform.position - characterTransform.position;
        }
        else if (shouldTurn) 
        {
             NPCDirection = Quaternion.AngleAxis(20, characterTransform.up) * characterTransform.forward;
        }
        else
        {
            NPCDirection = Vector3.ProjectOnPlane(randomVector,characterTransform.up);
        }


        NPCDirection = new Vector3(NPCDirection.x, 0, NPCDirection.z);
        Vector3 NPCMoveDir = Vector3.Slerp(characterTransform.forward, NPCDirection,turnSpeed);
        Quaternion rotate = Quaternion.LookRotation(NPCMoveDir, characterTransform.up);
        characterTransform.rotation = rotate;
    }

    private void Move()
    {
        
        velocity += characterTransform.forward.normalized * acceleration;
         
    }

    private void Friction()
    {
        float maxSpeed = playerInRange ? groundMaxSpeed : docileMaxSpeed;

        if (RB.velocity.magnitude != 0f)
        {
            if (stunned) 
            {
                if (RB.velocity.magnitude < accelerationFriction / 5)
                {
                    velocity -= RB.velocity;
                }
                else
                {
                    velocity -= RB.velocity.normalized * accelerationFriction/5;
                }
            } 
            else if (RB.velocity.magnitude >= maxSpeed)
            {
                velocity -= RB.velocity.normalized * acceleration;
            }
            else
            {

                if (RB.velocity.magnitude < accelerationFriction)
                {
                    velocity -= RB.velocity;
                }
                else
                {
                    velocity -= RB.velocity.normalized * accelerationFriction;
                }
            }
        }
    }

    private void GroundSnap()
    {
        characterTransform.position = groundedRayPointNPC.point;
    }

    private void GroundedCheck()
    {
        grounded = Physics.Raycast(characterTransform.position + (characterTransform.up.normalized * rayOriginOffset), -characterTransform.up, out groundedRayPointNPC, groundHitRange + rayOriginOffset, groundLayer);
    }
    private void Gravity()
    {
        velocity += Vector3.down.normalized * gravity;
    }

    private void StateSwitch()
    {
        if (grounded)
        {
            state = State.Ground;
        }
        else
        {
            state = State.Air;
        }
    }

    private void AirDrag()
    {
        Vector3 XZPlanarVelocity = Vector3.ProjectOnPlane(RB.velocity, characterTransform.up);
        float maxSpeed = playerInRange ? groundMaxSpeed : docileMaxSpeed;
        if (XZPlanarVelocity.magnitude != 0f)
        {

            if (stunned)
            {
                if (XZPlanarVelocity.magnitude < airDrag/5)
                {
                    velocity -= XZPlanarVelocity;
                }
                else
                {
                    velocity -= XZPlanarVelocity.normalized * airDrag/5;
                }
            }
            else if (XZPlanarVelocity.magnitude >= maxSpeed)
            {
                velocity -= XZPlanarVelocity.normalized * acceleration;
            }
            else
            {

                if (XZPlanarVelocity.magnitude < airDrag)
                {
                    velocity -= XZPlanarVelocity;
                }
                else
                {
                    velocity -= XZPlanarVelocity.normalized * airDrag;
                }
            }
        }
    }

    private void CombatRangeCheck()
    {
        playerInRange = comabtRadius > (playerTransform.position - characterTransform.position).magnitude;
    }

    private void WallCheck()
    {
        Vector3 NPCDirection = playerTransform.position - characterTransform.position;
        walled = Physics.SphereCast(characterTransform.position + (characterTransform.up.normalized * (capsuleCollider.height / 2)) - (sphereRayOffset * NPCDirection.normalized), capsuleCollider.radius, NPCDirection, out pointWallTouchData, wallTouchThreshold + sphereRayOffset, groundLayer);

    }

    private void JumpCheck()
    {
        Vector3 NPCDirection = playerTransform.position - characterTransform.position;
        canJump = Physics.SphereCast(characterTransform.position + (characterTransform.up.normalized * (capsuleCollider.height / 2)) - (sphereRayOffset * NPCDirection.normalized), capsuleCollider.radius, NPCDirection, out pointWallTouchData, wallJumpThreshold + sphereRayOffset, groundLayer);

    }

    public void HitStunCheck()
    {
        stunned = NPCCombat.StunCheck();
    }

    private void Jump()
    {
        if (this.state == State.Ground && walled)
        {
            velocity += characterTransform.up.normalized * jumpHeight;
        }
    }
    private void WallUnstick()
    {
        if (walled)
        {
            velocity += Vector3.ProjectOnPlane(pointWallTouchData.normal, Vector3.up).normalized * acceleration;
        }
    }

    private void MoveCheck()
    {
        shouldTurn = Physics.Raycast(characterTransform.position + (characterTransform.up.normalized * capsuleCollider.height/2), characterTransform.forward, out wallRangeRay, turnRange, groundLayer);
    }

    private void RandomDirectionGen() 
    {
        if (currentRandyTime <= 0)
        {
            randomVector = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            currentRandyTime = randyTime;
        }
    }

}