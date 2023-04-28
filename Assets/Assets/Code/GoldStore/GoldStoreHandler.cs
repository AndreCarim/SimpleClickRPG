using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoldStoreHandler : MonoBehaviour
{
    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private GemHandler gemHandler;
    [SerializeField] private GoldHandler goldHandler;
    [SerializeField] private SoundHandler soundHandler;

    [SerializeField] private GameObject storeWindow;
    [SerializeField] private AutoSaveHandler autoSaveHandler;

    [Header("amountTexts")]
    [SerializeField] private TextMeshProUGUI smallPackAmountText;
    [SerializeField] private TextMeshProUGUI mediumPackAmountText;
    [SerializeField] private TextMeshProUGUI bigPackAmountText;


    private double currentDropAmount; //the current amount of gold the enemy is dropping;

    private double currentSmallPackAmount;
    private double currentMediumPackAmount;
    private double currentBigPackAmount;

    public void buySmallGoldPack()
    {
        //give the player what it wants
        if(gemHandler.getCurrentAmountOfGem() >= 75)
        {
            gemHandler.decreaseAmountOfGem(75);
            goldHandler.increaseAmountOfGold(currentSmallPackAmount);

            soundHandler.upgradeSoundHandler();
            autoSaveHandler.save();
        }        
    }

    public void buyMediumGoldPack()
    {
        //give the player what it wants
        if (gemHandler.getCurrentAmountOfGem() >= 200)
        {
            gemHandler.decreaseAmountOfGem(200);
            goldHandler.increaseAmountOfGold(currentMediumPackAmount);

            soundHandler.upgradeSoundHandler();
            autoSaveHandler.save();
        }
    }

    public void buyBigGoldPack()
    {
        //give the player what it wants
        if (gemHandler.getCurrentAmountOfGem() >= 500)
        {
            gemHandler.decreaseAmountOfGem(500);
            goldHandler.increaseAmountOfGold(currentBigPackAmount);

            soundHandler.upgradeSoundHandler();
            autoSaveHandler.save();
        }
    }


    public void openStoreButton()
    {
        if (!enemyHandler.getIsBoss())
        {
            //its not a boss
            currentDropAmount = enemyHandler.getDropItemValue();
        }
        else
        {
            //it is a boss
            currentDropAmount = enemyHandler.getDropItemValue() / 8;
        }

        currentSmallPackAmount = currentDropAmount * 20;//amount of times the value will be
        currentMediumPackAmount = currentDropAmount * 60;
        currentBigPackAmount = currentDropAmount * 160;

        smallPackAmountText.text = NumberAbrev.ParseDouble(currentSmallPackAmount);
        mediumPackAmountText.text = NumberAbrev.ParseDouble(currentMediumPackAmount);
        bigPackAmountText.text = NumberAbrev.ParseDouble(currentBigPackAmount);

        storeWindow.SetActive(true);
        pauseGame();
    }

    public void closeStoreButton()
    {
        storeWindow.SetActive(false);
        resumeGame();
    }

    private void pauseGame()
    {
        Time.timeScale = 0f;
    }

    private void resumeGame()
    {
        Time.timeScale = 1f;
    }
}
