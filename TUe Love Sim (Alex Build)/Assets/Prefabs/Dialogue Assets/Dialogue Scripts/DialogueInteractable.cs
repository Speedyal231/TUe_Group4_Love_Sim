using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueInteractable : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private TMP_Text cueText;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private DialogueNPCData dialogueNPCData;
    [SerializeField] DialogueManager dialogueManager;
    float prevrizz;

    [Header("NPC camera (dialogue)")]
    [SerializeField] private Camera dialogueCam;

    [Header("Amount of time for the player to make a choice [s]")]
    [SerializeField] private float timeForDecision;

    [Header("NPC difficulty level: defines how hard it is to rizz up the NPC. ")]
    [Range(1,4)]
    [SerializeField] private int difficultyLevel;


    private bool activateCue;



    // Start is called before the first frame update
    void Start()
    {
        activateCue = false;
        visualCue.SetActive(false);
    }

    void Update()
    {
        VisualCueHandling();
    }

    void VisualCueHandling()
    {
        if (activateCue && (dialogueNPCData.FetchAttemptsLeft() > 0 && !dialogueNPCData.FetchRizzed()))
        {
            visualCue.SetActive(true);
        }
        else
        {
            visualCue.SetActive(false);
        }
        activateCue = false;
    }

    public void TriggerVisualCue(GameObject player)
    {
        activateCue = true;
        TextFacePlayer(player);
    }

    public void EnterDialogue(GameObject player)
    {
        Debug.Log(dialogueNPCData.FetchRizzed());
        if (dialogueNPCData.FetchAttemptsLeft() > 0 && !dialogueNPCData.FetchRizzed())
        {
            Debug.Log("Dialogue sequence entered");
            playerTransform.position = transform.position + transform.forward * 5;
            playerTransform.forward = -transform.forward;
            dialogueNPCData.changeAttemptsLeft(-1);
            DialogueManager.instance.EnterDialogue(inkJSON, dialogueCam, timeForDecision, difficultyLevel);
            dialogueNPCData.changeRizzed(dialogueManager.FetchRizzed());
        }
        else 
        {
            Debug.Log("Dialogue sequence entered");
        }
            

    }

    void TextFacePlayer(GameObject player)
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 textPosition = visualCue.transform.position;
        Vector3 delta = new Vector3(playerPosition.x - textPosition.x, 
                                    0.00f, 
                                    playerPosition.z - textPosition.z);
        Quaternion rotation = Quaternion.LookRotation(delta);
        visualCue.transform.rotation = rotation;
    }
}
