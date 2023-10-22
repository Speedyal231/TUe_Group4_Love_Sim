using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPCData : MonoBehaviour
{
    [SerializeField] float startAttempts;
    private float attemptsLeft;
    private bool rizzed;

    private void Start()
    {
        rizzed = false;
        attemptsLeft = startAttempts;
    }

    public float FetchAttemptsLeft() 
    {
        return attemptsLeft;    
    }

    public bool FetchRizzed()
    {
        return rizzed;
    }

    public void changeAttemptsLeft(int change)
    {
        attemptsLeft += change;
    }

    public void changeRizzed(bool change) 
    { 
        rizzed = change;
    }
}
