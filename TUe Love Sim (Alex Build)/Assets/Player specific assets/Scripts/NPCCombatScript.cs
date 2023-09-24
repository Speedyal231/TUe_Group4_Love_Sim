using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombatScript : MonoBehaviour
{
    [SerializeField] string message;
    // Start is called before the first frame update
    public void Damaged()
    {
        Debug.Log(message);
    }

}
