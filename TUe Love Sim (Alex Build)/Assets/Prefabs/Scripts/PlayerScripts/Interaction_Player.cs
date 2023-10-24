using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody rbNPC;
    [SerializeField] private GameObject cameraPlayer;
    private PlayerInputActions playerInputActions;
    private RaycastHit hit;
    [SerializeField] private float interactionRange = 10f;


    /// <summary>
    /// Clean up the funtions and code here, and add a funtion to cause the interactable to glow when you can interact.
    /// </summary>

    private void Awake()
    {
        //enable player input script.
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    void Update()
    {
        LookForInteraction();
    }

    void LookForInteraction()
    {
        // first fire ray to see if there are any objects to interact with
        // (has to be done this way to implement cues for interaction for the player)
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, interactionRange))
        {
            if (hit.collider.TryGetComponent(out Interactable interactable))
            {
                if (playerInputActions.Keyboard.Interact.ReadValue<float>() == 1)
                {
                    interactable.Interact();
                }
            }

            // check if detected object has a dialogue script
            if (hit.collider.TryGetComponent(out DialogueInteractable dialogueInteractable) 
                                             && !DialogueManager.instance.dialogueIsPlaying)
            {
                // if there is a dialogue script, prompt the user to enter dialogue
                dialogueInteractable.TriggerVisualCue(this.gameObject);
                if (playerInputActions.Keyboard.Interact.ReadValue<float>() == 1)
                {
                    
                    dialogueInteractable.EnterDialogue(this.gameObject);
                    
                }
            }


        }
    }

   
}
