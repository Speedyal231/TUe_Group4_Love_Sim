using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI time;

    private void Update()
    {
        TimeDisp();
    }

    private void TimeDisp() 
    {
        time.text = "Time: " + (Mathf.Round(playerData.FetchTime() * 10.0f) * 0.1).ToString();
    }
}
