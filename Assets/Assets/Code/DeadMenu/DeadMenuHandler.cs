using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//handles everything related to the player
//reload enemy and player
//decrease the gold
//pauses the game, resume the game...
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


        setText(goldLost);//setting the text for the dead menu

        setGold(goldLost);
        //show how many gold handler

        //handler for upcoming lost updates


        //setting the amount of deaths the player have
        timesPlayerDiedText.text = playerHealthHandler.getTimesPlayerDied();

        

        //this is so it will save the game when it pause
        //with both player and enemy health back on full
        //so the player cant come in and come out
        

        pauseGame();
    }
    
    //this will activate once the player clicks on the reload button
    public void reloadWorld()
    {

        if (enemyHandler.getIsBoss())//checking who killed the player
        {
            //it was a boss who killed the player
            diedForBoss();
        }
        else
        {
            //it was not a boss
            diedForNormalEnemy();
        }
        resumeGame();

        gameObject.SetActive(false);
    }

    //reload health from player and from enemy
    private void diedForNormalEnemy()
    {
        playerHealthHandler.reloadPlayer();
        enemyHandler.reloadFromNormalEnemy();
    }

    private void diedForBoss()
    {
        playerHealthHandler.reloadPlayer();
        enemyHandler.reloadFromBoss();
    }


    private void setText(double amount)
    {
        if (amount > 10000)
        {
            lostGoldText.text = NumberAbrev.ParseDouble(amount, 2);
        }
        else if(amount < 1)
        {
            lostGoldText.text = "0";
        }else
        {
            lostGoldText.text = NumberAbrev.ParseDouble(amount, 0);
        }

        //it will simply stop the background music of the boss before the user clicks the reloadWorld button
        if (enemyHandler.getIsBoss()) { enemyHandler.bossAnimationsFinish(); }
    }

    private void setGold(double amount)
    {

        if(amount < 1)
        { 
            goldHandler.decreaseAmountOfGold(amount *2);
        }
        else
        {
            goldHandler.decreaseAmountOfGold(amount);
        }
        
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
