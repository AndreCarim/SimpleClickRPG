using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MagicStore : MonoBehaviour
{
    [SerializeField] private GameObject storeWindow;

    [SerializeField] private MagicHandler magichandler;
    [SerializeField] private GemHandler gemHandler;
    [SerializeField] private AutoSaveHandler autoSaveHandler;
    [SerializeField] private SoundHandler soundHandler;


    private double smallPackPrice;
    private double mediumPackPrice;
    private double bigPackPrice;

    void Start()
    {
        smallPackPrice = 75;
        mediumPackPrice = 200;
        bigPackPrice = 500;
    }

    public void buySmallMagicPack()
    {
        if(gemHandler.getCurrentAmountOfGem() >= smallPackPrice)
        {
            //the player has the gems to buy the small pack
            gemHandler.decreaseAmountOfGem(smallPackPrice);
            magichandler.increaseAmountOfMagic(50);

            soundHandler.upgradeSoundHandler();
            autoSaveHandler.save();
        }
    }

    public void buyMediumMagicPack()
    {
        if (gemHandler.getCurrentAmountOfGem() >= mediumPackPrice)
        {
            //the player has the gems to buy the small pack
            gemHandler.decreaseAmountOfGem(mediumPackPrice);
            magichandler.increaseAmountOfMagic(150);

            soundHandler.upgradeSoundHandler();
            autoSaveHandler.save();
        }
    }


    public void buyBigMagicPack()
    {
        if (gemHandler.getCurrentAmountOfGem() >= bigPackPrice)
        {
            //the player has the gems to buy the small pack
            gemHandler.decreaseAmountOfGem(bigPackPrice);
            magichandler.increaseAmountOfMagic(600);

            soundHandler.upgradeSoundHandler();
            autoSaveHandler.save();
        }
    }








    public void openStoreButton()
    {
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
