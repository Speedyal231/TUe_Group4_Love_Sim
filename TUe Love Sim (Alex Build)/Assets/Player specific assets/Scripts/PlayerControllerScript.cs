using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{

    //Declare reference to Input System, Rigid body, transform etc.
    [Header("Object Declarations")]
    [SerializeField] Rigidbody RB;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] CapsuleCollider capsuleCollider;
    private PlayerInputActions playerInputActions;

    //Variables that contribute to the general character movement
    [Header("Movement")]
    [SerializeField] float acceleration;
    [SerializeField] float accelerationFriction;
    [SerializeField] float groundMaxSpeed;
    [SerializeField] float wallTouchThreshold;
    [SerializeField] float floorSphereOffset;
    Vector3 velocity;
    bool wallBound;
    RaycastHit wallPointHit;

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
    [SerializeField] float airDrag;
    [SerializeField] float airMaxSpeed;



    // enumeration of states used to denote several states.
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
        PhysicsCalcInit();
        GroundedCheck();
        StateSwitch();
        WallCheck();
        
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

        ApplyForces();
    }


/// <summary>
///
/// 
/// 
/// Spacing between main and general funtion declaration.
/// 
/// 
/// TBD:
/// - add air drag funtion to stop infinite acceleration when in air
/// - add 
/// 
/// </summary>

    //stores the  move input direction as a Vector2
    private Vector2 GetMovementVectorNormalized() 
    {
        //getting the input
        Vector2 inputVector = playerInputActions.Keyboard.Move.ReadValue<Vector2>();
        inputVector = inputVector.magnitude > 1 ? inputVector.normalized : inputVector;
    
        return inputVector;
    }

    //stores the jump input as a float
    private float JumpInput() 
    {
        return playerInputActions.Keyboard.Jump.ReadValue<float>();
    }

    //set physics to initial state before being calculated each physics cycle
    private void PhysicsCalcInit() 
    {
        velocity = Vector3.zero;
    }

    //applies the calculated velocity of the current cycle to the Player
    private void ApplyForces() 
    {
        RB.velocity += velocity;
    }

    //function to keep player facing same direction as camera
    private void Turn() 
    {
        Vector3 cameraFlatForward = Vector3.ProjectOnPlane(cameraTransform.forward, playerTransform.up).normalized;
        playerTransform.forward = cameraFlatForward;
    }

    // funtion to apply movement forces to velocity
    private void Move() 
    {
        Vector2 inputVector = GetMovementVectorNormalized();
        Vector3 frontalForce = inputVector.y * playerTransform.forward.normalized;
        Vector3 rightForce = inputVector.x * playerTransform.right.normalized;

        Vector3 movingForce = (frontalForce + rightForce) * acceleration;

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

    private void Jump() 
    {
        
        velocity += playerTransform.up.normalized * jumpHeight * JumpInput();

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

    private void WallCheck() 
    {
        wallBound = Physics.SphereCast(transform.position + (transform.up.normalized * (capsuleCollider.height - capsuleCollider.radius)), capsuleCollider.radius + wallTouchThreshold, transform.forward, out wallPointHit, capsuleCollider.height - capsuleCollider.radius * 2 - wallTouchThreshold - floorSphereOffset);
        
        Debug.Log(wallBound);
        
    }
} 
