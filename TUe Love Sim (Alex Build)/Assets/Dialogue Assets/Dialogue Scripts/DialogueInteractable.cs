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

    [Header("NPC camera (dialogue)")]
    [SerializeField] private Camera dialogueCam;

    [Header("Amount of time for the player to make a choice")]
    [SerializeField] private float timeForDecision;

    private bool activateCue;

    // Start is called before the first frame update
    void Awake()
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
        if (activateCue)
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
        Debug.Log("Dialogue sequence entered");
        DialogueManager.instance.EnterDialogue(inkJSON, dialogueCam, timeForDecision);
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
