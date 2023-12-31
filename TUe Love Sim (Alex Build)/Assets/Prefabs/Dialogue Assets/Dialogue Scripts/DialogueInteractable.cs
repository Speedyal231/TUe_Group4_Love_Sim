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
    [SerializeField] DialogueAnims dialogueAnims;
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
        AnimCheck();
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
        
        if (dialogueNPCData.FetchAttemptsLeft() > 0 && !dialogueNPCData.FetchRizzed())
        {
            Debug.Log("Dialogue sequence entered");
            playerTransform.position = transform.position + transform.forward.normalized * 2;
            playerTransform.forward = -transform.forward;
            DialogueManager.instance.EnterDialogue(inkJSON, dialogueCam, timeForDecision, difficultyLevel, dialogueNPCData, dialogueAnims);
            
        }
        else 
        {
            Debug.Log("Dialogue sequence fail");
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

    private void AnimCheck()
    {
        if (dialogueNPCData.FetchRizzed())
        {
            dialogueAnims.RizzFailTriggerSet(false);
            dialogueAnims.ResetTriggerSet(false);
            dialogueAnims.RizzSuccessTriggerSet(true);
        }
        else if (dialogueNPCData.FetchAttemptsLeft() <= 0 && !dialogueNPCData.FetchRizzed())
        {
            dialogueAnims.RizzFailTriggerSet(true);
            dialogueAnims.ResetTriggerSet(false);
        }
    }
}
