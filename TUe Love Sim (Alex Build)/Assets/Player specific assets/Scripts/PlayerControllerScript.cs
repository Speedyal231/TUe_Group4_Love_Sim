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
    private PlayerInputActions playerInputActions;

    //Variables that contribute to the general character movement
    [Header("Movement")]
    [SerializeField] float acceleration;
    [SerializeField] float accelerationFriction;
    [SerializeField] float groundMaxSpeed;
    Vector3 velocity;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        PhysicsCalcInit();
        GroundedCheck();
        StateSwitch();
        
        if (state == State.Air) 
        {
            Turn();
            Move();
            Gravity();
            
        }
        else if (state == State.Ground)
        {
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
/// 
/// </summary>


    public Vector2 GetMovementVectorNormalized() 
    {
        //getting the input
        Vector2 inputVector = playerInputActions.Keyboard.Move.ReadValue<Vector2>();
        inputVector = inputVector.magnitude > 1 ? inputVector.normalized : inputVector;
    
        return inputVector;
    }

    private float JumpInput() 
    {
        return playerInputActions.Keyboard.Jump.ReadValue<float>();
    }

    private void PhysicsCalcInit() 
    {
        velocity = Vector3.zero;
    }

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

    // funtion to apply movement forces to RB velocity
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
} 
