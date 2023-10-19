using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using Ink.Runtime;

public class DialogueTimer : MonoBehaviour
{
    [Header("Timer bar")]
    [SerializeField] private GameObject timerBar;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void RunTimer(float time, Story currentStory)
    {
        this.gameObject.SetActive(true);

        // scale the bar in a set amount of time, then call dialogue manager when finished
        LeanTween.scaleX(timerBar.gameObject, 0, time).setOnComplete(() =>
        {
            DialogueManager.instance.TimerFinished(currentStory);
            CancelTimer(); 

        });
    }

    public void CancelTimer()
    {
        LeanTween.cancel(timerBar.gameObject);
        timerBar.transform.localScale = Vector3.one;
        this.gameObject.SetActive(false);
    }

}
