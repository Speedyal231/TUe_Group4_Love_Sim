using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] PlayerData playerData; 

    public Slider slider;

    public void SetHealth(int health) 
    { 
        slider.value = health;
    }

    private void Update()
    {
        SetHealth((int)playerData.FetchHealth());
    }

}
