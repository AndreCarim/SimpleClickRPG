using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeadMenuHandler : MonoBehaviour
{
    [SerializeField] private GoldHandler goldHandler;
    [SerializeField] private TextMeshProUGUI lostGoldText;

    [SerializeField] private PlayerHealthHandler playerHealthHandler;
    [SerializeField] private EnemyHandler enemyHandler;

    [SerializeField] private TextMeshProUGUI timesPlayerDiedText;


    public void playerDied()
    {
        //show how many gold handler
        double goldLost = goldHandler.getCurrentAmountOfGold() / 2;
        lostGoldText.text = goldLost.ToString();
        goldHandler.decreaseAmountOfGold(goldLost);
        //show how many gold handler

        //handler for upcoming lost updates


        //setting the amount of deaths the player have
        timesPlayerDiedText.text = timesPlayerDiedText.text = playerHealthHandler.getTimesPlayerDied();


        //this is so it will save the game when it pause
        //with both player and enemy health back on full
        //so the player cant come in and come out
        reloadHealth();

        pauseGame();
    }
    
    //this will activate once the player clicks on the reload button
    public void reloadWorld()
    {
        

        gameObject.SetActive(false);

        reloadHealth();
        resumeGame();
    }

    //reload health from player and from enemy
    private void reloadHealth()
    {
        playerHealthHandler.reloadPlayer();
        enemyHandler.reloadEnemy();
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
