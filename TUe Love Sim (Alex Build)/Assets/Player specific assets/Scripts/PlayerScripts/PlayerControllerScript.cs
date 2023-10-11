using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{

    //Declare reference to Input System, Rigid body, transform etc.
    [Header("Object Declarations")]
    [SerializeField] Rigidbody RB;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] PlayerCombatScript playerCombatScript;
    private PlayerInputActions playerInputActions;

    //Variables that contribute to the general character movement
    [Header("Movement")]
    [SerializeField] float acceleration;
    [SerializeField] float accelerationFriction;
    [SerializeField] float groundMaxSpeed;
    [SerializeField] float wallTouchThreshold;
    [SerializeField] float sphereRayOffset;
    Vector3 velocity;
    bool wallBound;
    RaycastHit wallPointHit;
    RaycastHit wallJumpPointHit;
    Vector3 prevWallNormal;
    Collider prevObjectWall;
    bool stunned;

    //Variables that contribute to and store object collisions and raycast data
    [Header("Ground Detection")]
    [SerializeField] float rayOriginOffset;
    [SerializeField] float groundHitRange;
    [SerializeField] LayerMask groundLayer;
    RaycastHit groundedRayPoint;
    bool grounded;

    // Air behaviour variables
    [Header("Air Variables")]
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;
    [SerializeField] float wallJumpMultiplier;
    [SerializeField] float wallBounceMultiplier;
    [SerializeField] float wallJumpThreshold;
    [SerializeField] float wallClingTimer;
    [SerializeField] float wallSpeedThreshHold;
    [SerializeField] float airDrag;
    [SerializeField] float airMaxSpeed;
    float currentWallClingTimer;
    bool canWallJump;
    bool hasWallJumped;
    bool canCling;
    bool endCling;
    bool canJump;



    // enumeration of states used to denote several states.
    public enum State
    {
        Ground,
        Air
    }
    public State statePlayer;


/// <summary>
///
/// 
/// 
/// Spacing between variable declarations and funtion declaration.
/// 
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
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // FixedUpdate is called once per Unity physics cycle
    void FixedUpdate()
    {
        Vector3 movingForce = GetMovementVectorNormalized();
        PhysicsCalcInit();
        GroundedCheck();
        StateSwitch();
        Count();
        CheckStun();
        WallCheck(movingForce);
        WallJumpCheck(movingForce);

        if (stunned || playerCombatScript.FetchDead()) 
        {
            if (statePlayer == State.Air)
            {
                Turn();
                Gravity();
                AirDrag();
            }
            else if (statePlayer == State.Ground)
            {
                GroundSnap();
                Turn();
                Friction();
            }
        }
        else if (statePlayer == State.Air)
        {
            Turn();
            Move(movingForce);
            WallUnstick(movingForce);
            Gravity();
            AirDrag();
            NewJump();
            DYNAWallCling();
        }
        else if (statePlayer == State.Ground)
        {
            GroundSnap();
            Turn();
            Move(movingForce);
            WallUnstick(movingForce);
            Friction();
            NewJump();
        }

        ApplyForces();
    }


/// <summary>
///
/// 
/// 
/// Spacing between main and general funtion declaration.
/// 
/// 
/// 
/// </summary>

    //stores the  move input direction as a Vector2
    // consider implement input collection at start of update to sfae precessing times
    private Vector3 GetMovementVectorNormalized() 
    {
        //getting the input
        Vector2 inputVector = playerInputActions.Keyboard.Move.ReadValue<Vector2>();
        inputVector = inputVector.magnitude > 1 ? inputVector.normalized : inputVector;
        Vector3 frontalForce = inputVector.y * playerTransform.forward.normalized;
        Vector3 rightForce = inputVector.x * playerTransform.right.normalized;

        Vector3 movingForce = (frontalForce + rightForce) * acceleration;
    
        return movingForce;
    }

    //stores the jump input as a float
    private float JumpInput() 
    {
        return playerInputActions.Keyboard.Jump.ReadValue<float>();
    }
    
    private float ShiftInput() 
    {
        return playerInputActions.Keyboard.Cling.ReadValue<float>();
    }

    //set physics to initial state before being calculated each physics cycle
    private void PhysicsCalcInit() 
    {
        velocity = Vector3.zero;
        if (statePlayer == State.Ground)
        {
            prevWallNormal = Vector3.up;
            hasWallJumped = false;
        }
    }

    //applies the calculated velocity of the current cycle to the Player
    private void ApplyForces() 
    {
        RB.velocity += velocity;
    }

    void Count()
    {
        if(currentWallClingTimer > 0)
            currentWallClingTimer -= Time.fixedDeltaTime;
    }

    //function to keep player facing same direction as camera
    private void Turn() 
    {
        Vector3 cameraFlatForward = Vector3.ProjectOnPlane(cameraTransform.forward, playerTransform.up).normalized;
        playerTransform.forward = cameraFlatForward;
    }

    // funtion to apply movement forces to velocity
    private void Move(Vector3 movingForce) 
    {
        velocity += movingForce;
    }

    // funtion to apply friction to motion of player on ground (note: must be adapted to only work in xz plane)
    private void Friction() 
    {
        if (RB.velocity.magnitude != 0f) 
        {
            if (RB.velocity.magnitude >= groundMaxSpeed) 
            {
                velocity -= RB.velocity.normalized * acceleration;
            } 
            else 
            {
                
                if (RB.velocity.magnitude <  accelerationFriction) 
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

    private void GroundedCheck() 
    {
        grounded = Physics.Raycast(playerTransform.position + (playerTransform.up.normalized * rayOriginOffset), -playerTransform.up, out groundedRayPoint, groundHitRange + rayOriginOffset, groundLayer);
    }

    private void Gravity() 
    {
        velocity += Vector3.down.normalized * gravity;
    }

    private void NewJump() 
    {
        if (statePlayer == State.Ground)
        {
            prevObjectWall = null;
        }
        float jumpVal = JumpInput();
        if (statePlayer == State.Ground)
        {
            velocity += playerTransform.up.normalized * jumpHeight * jumpVal;
        }
        else if (statePlayer == State.Air)
        {
            if (canWallJump && jumpVal > 0 && !hasWallJumped && canCling) 
            {
                hasWallJumped = true;
                velocity.y -= (-RB.velocity.y > gravity) ? RB.velocity.y : 0;
                velocity += playerTransform.up.normalized * jumpHeight * wallJumpMultiplier *jumpVal + Vector3.ProjectOnPlane(wallJumpPointHit.normal, transform.up).normalized * wallBounceMultiplier * jumpHeight;
            }
        } 
    }

    private void DYNAWallCling() 
    {
        float clingVal = ShiftInput();
        if (canWallJump)
        {
            if (clingVal > 0 && (ExtensionMethods.Round(prevWallNormal.normalized) - ExtensionMethods.Round(wallJumpPointHit.normal.normalized) != Vector3.zero))
            {
                currentWallClingTimer = wallClingTimer;
                canCling = true;
                prevWallNormal = wallJumpPointHit.normal.normalized;
                prevObjectWall = wallJumpPointHit.collider;
                hasWallJumped = false;
                
            }
            else if (clingVal > 0 && (ExtensionMethods.Round(prevWallNormal.normalized) - ExtensionMethods.Round(wallJumpPointHit.normal.normalized) == Vector3.zero)) 
            {
                if (currentWallClingTimer > 0 && !hasWallJumped) 
                {
                    canCling = true;
                    endCling = false;
                } 
                else if (Vector3.ProjectOnPlane(RB.velocity, wallJumpPointHit.normal).magnitude >= wallSpeedThreshHold && !endCling && !hasWallJumped) 
                {
                    canCling = true;
                }
                else
                {
                    canCling = false;
                    endCling = true;
                }
            }
        }
        else 
        {
            canCling = false;
        }

        if (canCling) 
        {
            velocity += -playerTransform.up * RB.velocity.y * ShiftInput();
        }
    }

    private void StateSwitch() 
    {
        if (grounded)
        {
            statePlayer = State.Ground;
        }
        else 
        {
            statePlayer = State.Air;
        }
    }

    // improve snap
    private void GroundSnap() 
    {
        playerTransform.position = groundedRayPoint.point;
    }

    private void AirDrag() 
    {
        Vector3 XZPlanarVelocity = Vector3.ProjectOnPlane(RB.velocity, playerTransform.up);
        if (XZPlanarVelocity.magnitude != 0f) 
        {
            if (XZPlanarVelocity.magnitude >= airMaxSpeed) 
            {
                velocity -= XZPlanarVelocity.normalized * acceleration;
            } 
            else 
            {
                
                if (XZPlanarVelocity.magnitude <  airDrag) 
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

    private void WallCheck(Vector3 movingForce) 
    {

        wallBound = Physics.SphereCast(transform.position + (transform.up.normalized * (capsuleCollider.height/2)) - (sphereRayOffset * movingForce.normalized), capsuleCollider.radius, movingForce, out wallPointHit, wallTouchThreshold + sphereRayOffset);        
        //wallBound = Physics.CapsuleCast(transform.position - (sphereRayOffset * movingForce.normalized), transform.position + (transform.up.normalized * capsuleCollider.height) - (sphereRayOffset * movingForce.normalized), capsuleCollider.radius, movingForce, out wallPointHit, wallTouchThreshold + sphereRayOffset);
    }

    private void WallJumpCheck(Vector3 movingForce) 
    {

        canWallJump = Physics.SphereCast(transform.position + (transform.up.normalized * (capsuleCollider.height/2)) - (sphereRayOffset * movingForce.normalized), capsuleCollider.radius, movingForce, out wallJumpPointHit, wallJumpThreshold + sphereRayOffset);        
        //wallBound = Physics.CapsuleCast(transform.position - (sphereRayOffset * movingForce.normalized), transform.position + (transform.up.normalized * capsuleCollider.height) - (sphereRayOffset * movingForce.normalized), capsuleCollider.radius, movingForce, out wallPointHit, wallTouchThreshold + sphereRayOffset);
    }

    private void WallUnstick(Vector3 movingForce) 
    {
        if(wallBound) 
        {
            velocity += Vector3.ProjectOnPlane(wallPointHit.normal,Vector3.up).normalized * movingForce.magnitude;
        }
    }

    public bool AirFetch() 
    {
        return statePlayer == State.Air ? true : false ;
    }

    public float JumpHeightFetch()
    {
        return jumpHeight;
    }

    private void CheckStun() 
    {
        stunned = playerCombatScript.Stunned();
    }

}

static class ExtensionMethods
{
    /// <summary>
    /// Rounds Vector3.
    /// </summary>
    /// <param name="vector3"></param>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }
}
