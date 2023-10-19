using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    // declare the DialogueManager class as a singleton class
    public static DialogueManager instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    public bool dialogueIsPlaying { get; private set; }

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Player Reference")]
    [SerializeField] private GameObject player;
    private PlayerControllerScript playerController;
    private PlayerInputActions playerInputActions;
    private Rigidbody playerRigidbody;

    [Header("Timer UI")]
    [SerializeField] private DialogueTimer dialogueTimer;
    private float timerDuration = 0;

    private Story currentStory;
    private Camera dialogueCam;
    private int NPC_difficulty;


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
            Debug.Log("Dialogue Manager couldn't get the Player Controller script of the Player.");
        }

        playerRigidbody = player.GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.Log("Dialogue Manager couldn't get the Rigidbody component of the Player.");
        }

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void EnterDialogue(TextAsset inkJSON, Camera NPCcam, float timeForDecision, int difficulty)
    {
        // prepare timer
        timerDuration = timeForDecision;

        // manage inky dialogue
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);  
        ContinueStory();

        FreezePlayerPosition();

        // manage camera
        dialogueCam = NPCcam;
        CameraManager.instance.DisablePlayerCameraMovement();
        CameraManager.instance.SwitchToCamera(dialogueCam);

        // set NPC difficulty
        NPC_difficulty = difficulty;

    }

    private void FreezePlayerPosition()
    {
        playerController.enabled = false;
    }

    private void UnFreezePlayerPosition()
    {
        playerController.enabled = true;
    }

    private IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        UnFreezePlayerPosition();

        // manage camera
        CameraManager.instance.EnablePlayerCameraMovement();
        CameraManager.instance.ReturnToMainCamera(dialogueCam);
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // set the dialogue line
            dialogueText.text = currentStory.Continue();

            // set the player choices for the dialogue line
            DisplayDialogueChoices();

            // set off the timer if there are still some choices to be made by the player
            if (currentStory.currentChoices.Count > 0)
            {
                dialogueTimer.RunTimer(timerDuration, currentStory);
            }
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

        // When player presses space (jump), next dialogue line is read
        if(playerInputActions.Keyboard.Jump.ReadValue<float>() == 1)
        {
            ContinueStory();
        }

    }

    private void DisplayDialogueChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // send warning if at some point the ink file passes more choices than the set UI limit
        if (currentChoices.Count > choices.Length)
        {
            Debug.Log("The number of dialogue choices provided in the INK file exceeds the UI limit. " +
                "Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        // set all UI choices being used to active and set their text according to the INK file
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // disable the visibility of unused choice boxes
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].SetActive(false);
        }

        // Unity's Event System requires us to select the first choice | we clear it first and then set it in the next frame
        StartCoroutine(SelectFirstChoice());
    }
    
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        dialogueTimer.CancelTimer();
        ContinueStory();
    }

    public void TimerFinished(Story storyOnFinish)
    {
        if (currentStory.state == storyOnFinish.state)
        {
            StartCoroutine(ExitDialogue());
            Debug.Log("The player didn't make a choice in time. He loses points or whatever ");
            // add logic to punish the player for not choosing in time
        } 
    }
}
