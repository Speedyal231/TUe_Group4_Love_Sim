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

    [Header("Air Variables")]
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;
    [SerializeField] float airDrag;
    [SerializeField] float airMaxSpeed;

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
        GroundSnap();
        Turn();
        Move();
        Friction();
        ApplyForces();
    }
    /// <summary>
    ///
    /// 
    /// 
    /// Spacing between main and general funtion declaration.
    /// 
    /// Fix weird  teleport bug
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

    private void Turn() 
    {
        Vector3 NPCDirection = playerTransform.position - characterTransform.position;
        NPCDirection = new Vector3(NPCDirection.x, 0, NPCDirection.z);
        Quaternion rotate = Quaternion.LookRotation(NPCDirection, characterTransform.up);
        characterTransform.rotation = rotate;
    }

    private void Move()
    {
        velocity += characterTransform.forward.normalized * acceleration;
    }

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
        characterTransform.position = groundedRayPoint.point;
    }

    private void GroundedCheck()
    {
        grounded = Physics.Raycast(characterTransform.position + (characterTransform.up.normalized * rayOriginOffset), -characterTransform.up, out groundedRayPoint, groundHitRange + rayOriginOffset, groundLayer);
    }
}
