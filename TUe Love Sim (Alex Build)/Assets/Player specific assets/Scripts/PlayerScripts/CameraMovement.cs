using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Mathematics;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [Header("Object Declarations")]
    [SerializeField] Transform camTransform;
    [SerializeField] Transform containerTransform;
    private PlayerInputActions playerInputActions;


    [Header("Camera Control")]
    [SerializeField] float sensitivity;
    float rotationX = 0;
    float rotationY = 0;

    private void Awake() 
    {
        //enable player input script.
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        OrientCam();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
        Look();
    }

    private Vector2 GetMouseDeltaVectorNormalized() 
    {
        //getting the input
        Vector2 mouseDeltaVector = playerInputActions.Keyboard.Look.ReadValue<Vector2>();
    
        return mouseDeltaVector;
    }


    private void Follow()
    {
        camTransform.position = containerTransform.position;
    }

    private void OrientCam()
    {
        camTransform.position = containerTransform.position;
        camTransform.forward = containerTransform.forward;

    }
    
    /// <summary>
    /// Black magic, please research. ???????? how his work?
    /// </summary>
    private void Look() 
    {
        Vector3 cameraFlatForward = Vector3.ProjectOnPlane(camTransform.forward, Vector3.up).normalized;
        Vector2 deltaVector = GetMouseDeltaVectorNormalized();
        float deltaXAngle = deltaVector.x * sensitivity;
        float deltaYAngle = deltaVector.y * sensitivity;
        
        rotationX -= deltaYAngle;
        rotationX = Mathf.Clamp(rotationX, -85f, 85f);
        rotationY -= -deltaXAngle;
        //rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        camTransform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);


        //camTransform.Rotate(-deltaYAngle, deltaXAngle, 0f, Space.Self);
    }
}
