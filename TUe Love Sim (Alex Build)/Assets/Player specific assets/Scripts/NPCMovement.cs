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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
