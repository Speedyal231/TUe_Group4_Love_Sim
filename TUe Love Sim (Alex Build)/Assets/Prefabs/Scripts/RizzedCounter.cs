using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RizzedCounter : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI Rizz;

    private void Update()
    {
        RizzDisp();
    }

    private void RizzDisp()
    {
        Rizz.text = "Targets Rizzed: " + (playerData.FetchTargetRizzedCount()).ToString();
    }
}
