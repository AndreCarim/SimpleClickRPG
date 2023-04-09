using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//this script will handle the pause menu 
public class PauseHandler : MonoBehaviour
{
    [SerializeField] private GoldHandler goldHandler;
    [SerializeField] private PlayerHealthHandler playerHealthHandler;
    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private StrengthHandler strengthHandler;

    

    [SerializeField] private TextMeshProUGUI totalGoldEverText;
    [SerializeField] private TextMeshProUGUI totalPlayerHpLostEverText;
    [SerializeField] private TextMeshProUGUI totalDamageGivenEverText;
    [SerializeField] private TextMeshProUGUI currentStageText;
    [SerializeField] private TextMeshProUGUI currentKillsText;
    [SerializeField] private TextMeshProUGUI currentPlayerPowerText;
    [SerializeField] private TextMeshProUGUI currentPlayerDeathsText;

    [SerializeField] private GameObject pauseFrame;

    [SerializeField] private AudioSource clickButton;


    public void clickPause()
    {
        //enemy
        totalDamageGivenEverText.text = enemyHandler.getTotalDamageGivenEver();
        currentKillsText.text = enemyHandler.getTotalAmountKilled();
        currentStageText.text = enemyHandler.getCurrentStage();

        //player
        totalPlayerHpLostEverText.text = playerHealthHandler.getTotalHealthLostEver();
        currentPlayerDeathsText.text = playerHealthHandler.getTimesPlayerDied();

        //strength
        currentPlayerPowerText.text = strengthHandler.getStrengthPowerText();

        //gold 
        totalGoldEverText.text = goldHandler.getTotalAmountOfGoldEver();


        
        pauseFrame.SetActive(true);
        clickButton.Play();
        pauseGame();
    }


    public void clickResume()
    {
        pauseFrame.SetActive(false);
        clickButton.Play();
        
        resumeGame();
    }



    void pauseGame()
    {
        Time.timeScale = 0;
    }
    void resumeGame()
    {
        Time.timeScale = 1;
    }
}
