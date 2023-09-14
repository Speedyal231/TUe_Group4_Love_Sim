using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Player : MonoBehaviour
{
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactionRange = 2f;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange))
            {
                if (hit.collider.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact();
                }
            }   
        }
    }
}
