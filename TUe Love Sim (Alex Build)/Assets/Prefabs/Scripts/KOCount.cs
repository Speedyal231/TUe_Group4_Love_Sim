using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KOCount : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI kills;

    private void Update()
    {
        KillDisp();
    }

    private void KillDisp()
    {
        kills.text = "K.O.'s: " + (playerData.Fetchkills()).ToString();
    }
}
