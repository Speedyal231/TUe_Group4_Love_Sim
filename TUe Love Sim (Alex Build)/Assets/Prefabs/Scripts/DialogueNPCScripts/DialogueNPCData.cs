using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPCData : MonoBehaviour
{
    [SerializeField] float startAttempts;
    private float attemptsLeft;
    private bool prevAttempt;
    private bool rizzed;

    private void Start()
    {
        rizzed = false;
        attemptsLeft = startAttempts;
    }

    private void Update()
    {
        //Debug.Log(attemptsLeft);
        //Debug.Log(rizzed);
    }

    public float FetchAttemptsLeft() 
    {
        return attemptsLeft;    
    }

    public bool FetchRizzed()
    {
        return rizzed;
    }

    public bool FetchPrevAttempt() 
    { 
        return prevAttempt;
    }

    public void changeAttemptsLeft(int change)
    {
        attemptsLeft += change;
    }

    public void changeRizzed(bool change) 
    { 
        rizzed = change;
    }

    public void changePrevAttempt(bool change) 
    {
        prevAttempt = change;
    }
}
