using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    [SerializeField] PlayerData playerData;
    [SerializeField] Transform playerTransform;
    [SerializeField] Rigidbody RB;
    [SerializeField] GameObject screen;
    [SerializeField] TextMeshProUGUI rizzed;
    [SerializeField] TextMeshProUGUI healthBonus;
    [SerializeField] TextMeshProUGUI timeBonus;
    [SerializeField] TextMeshProUGUI killBonus;
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] Transform exit;
    [SerializeField] GameObject enemies;
    [SerializeField] float ExitRange;
    bool ended;
    float timeBonusVal;



    // Update is called once per frame
    private void Start()
    {
        screen.SetActive(false);
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        ended = false;
        RB.isKinematic = false;
    }
    private void Update()
    {
        Detection();
    }

    private void Detection() 
    {

        if ((exit.position - playerTransform.position).magnitude <= ExitRange)
        {
            End();
            if (playerInputActions.Keyboard.Interact.ReadValue<float>() > 0)
            {
                ResetGame();
            }
        }
    }

    private void End() 
    { 
        RB.isKinematic = true;
        screen.SetActive(true);
        enemies.SetActive(false);
        rizzed.text = playerData.FetchTargetRizzedCount().ToString();
        float healthBonusVal = (playerData.FetchHealth() * 100);
        healthBonus.text = healthBonusVal.ToString();

        if (!ended) 
        {
            timeBonusVal = (7000 - Mathf.Round(playerData.FetchTime()) * 10);
            if (timeBonusVal > 0) { timeBonusVal += 1000; } else { timeBonusVal = 1000; }
        }
        timeBonus.text = timeBonusVal.ToString();

        float KillBonusVal = (playerData.Fetchkills() * 200);
        killBonus.text = KillBonusVal.ToString();

        float ScoreVal = playerData.FetchTargetRizzedCount() * 10000 + KillBonusVal + healthBonusVal + timeBonusVal;
        if (playerData.FetchTargetRizzedCount() == 5) { ScoreVal += 20000; }
        Score.text = ScoreVal.ToString();
        ended = true;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
