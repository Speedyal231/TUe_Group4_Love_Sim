using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Player : MonoBehaviour
{
    [SerializeField]
    private GameObject cameraPlayer;
    private PlayerInputActions playerInputActions;
    private RaycastHit hit;


    /// <summary>
    /// Clean up the funtions and code here, and add a funtion to cause the interactable to glow when you can interact.
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
    void Update()
    {
        if (playerInputActions.Keyboard.Interact.ReadValue<float>() == 1) 
        {
            float interactionRange = 20f;
            if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, interactionRange))
            {
                if (hit.collider.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact();
                }
            }   
        }
    }
}
