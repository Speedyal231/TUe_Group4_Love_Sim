using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // declare the DialogueManager class as a singleton class
    public static DialogueManager instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    public bool dialogueIsPlaying { get; private set; }

    [Header("Player Reference")]
    [SerializeField] private GameObject player;
    private PlayerControllerScript playerController;
    private PlayerInputActions playerInputActions;
    private Story currentStory;

    private void Awake()
    {
        // safety check to make sure there is only one DialogueManager in a scene
        if (instance != null && instance != this)
        {
            Debug.Log("More than one instance of the singleton class DialogueManager found in the scene. " +
                      "The new instance has been terminated. ");
            Destroy(this);
            return;
        }
        instance = this;

        // enable player input script.
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        playerController = player.GetComponent<PlayerControllerScript>();
        if (playerController == null)
        {
            Debug.Log("Dialogue Manager couldn't get the Player Controller Script of the Player.");
        }
    }

    public void EnterDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);  
        ContinueStory();
        playerController.gameObject.SetActive(false);
    }
    private IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        playerController.gameObject.SetActive(true);
        
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        // to add: when player presses submit, invoke ContinueStory()
        if(playerInputActions.Keyboard.Jump.ReadValue<float>() == 1)
        {
            ContinueStory();
        }

    }
}
