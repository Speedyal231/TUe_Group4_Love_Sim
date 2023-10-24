using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathAndReset : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject gameObject;
    

    private void Start()
    {
        gameObject.SetActive(false);
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDead();
    }

    void CheckDead() 
    {

        if (playerData.FetchDead()) 
        { 
            gameObject.SetActive(true);
            if (playerInputActions.Keyboard.Interact.ReadValue<float>() > 0) 
            {
                ResetGame();
            }
        }
        
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
