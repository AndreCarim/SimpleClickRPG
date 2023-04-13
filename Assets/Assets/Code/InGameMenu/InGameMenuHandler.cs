using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class InGameMenuHandler : MonoBehaviour
{
    [SerializeField] private GoldHandler goldHandler;
    [SerializeField] private PlayerHealthHandler playerHealthHandler;
    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private StrengthHandler strengthHandler;

    //for the stats
    [SerializeField] private GameObject statusMenu;
    [SerializeField] private GameObject statusButton;

    [SerializeField] private TextMeshProUGUI totalGoldEverText;
    [SerializeField] private TextMeshProUGUI totalPlayerHpLostEverText;
    [SerializeField] private TextMeshProUGUI totalDamageGivenEverText;
    [SerializeField] private TextMeshProUGUI currentStageText;
    [SerializeField] private TextMeshProUGUI currentKillsText;
    [SerializeField] private TextMeshProUGUI currentPlayerPowerText;
    [SerializeField] private TextMeshProUGUI currentPlayerDeathsText;
    //for the stats

    [SerializeField] private GameObject inGameMenuObject;

    [SerializeField] private AudioSource clickButton;


    //for the history
    [SerializeField] private GameObject gameHistoryMenu;
    [SerializeField] private GameObject gameHistoryButton;

    [SerializeField] private GameObject kingdomHistoryOpenButton; // this is the buttons to change text
    [SerializeField] private GameObject magicBackPackHistoryOpenButton;// this is the button to change text
    [SerializeField] private GameObject monstersHistoryOpenButton;//this is the button to change
    [SerializeField] private GameObject petsHistoryOpenButton;//this is the button to change
    [SerializeField] private GameObject thomasMisteryOpenButton;

    [SerializeField] private GameObject kingdomHistoryText;
    [SerializeField] private GameObject magicBackPackText;
    [SerializeField] private GameObject monsterHistoryText;
    [SerializeField] private GameObject petsHistoryText;
    [SerializeField] private GameObject thomasMisteryText;

    private Color historyClickedColor = new Color(1f, 0.32f, 0.32f, 1f);
    private Color historyNotClicked = new Color(1f, 0.32f, 0.32f, .35f);
    //for the history


    //for handling the menu buttons inside the game Menu
    private Color menuClickedColor = new Color(1f, .45f, .45f, 1f); // a little transparent when not clicked
    private Color menuNotClickedColor = new Color(1f, 1f, 1f, .35f); //full color when clicked


    //handles the thomas history easteregg
    private int countForEasterEgg;
    [SerializeField] private AudioSource misterySound;
    


    //opens the general menu showing the stats first
    public void clickOpenInGameMenu()
    {
        inGameMenuObject.SetActive(true);

        openStatusMenu(); //every time we open the menu the status will be there;

        playSound();
        pauseGame();
    }
    //opens the general menu showing the stats first




    //handles the menus inside the menu
    //opens status menu
    public void openStatusMenu()
    {
        if (!statusMenu.activeInHierarchy)//checking to see if it is open already
        {
            //this one will open the status menu when the player clicks the status button
            closeAllMenus();//making sure all the menus are closed


            handleStatus();
            statusMenu.SetActive(true);

            setMenuButtonTransparency(statusButton);
            playSound();
        }
        
    }

    //opens game history menu
    public void openGameHistoryMenu()
    {
        if (!gameHistoryMenu.activeInHierarchy)//checking to see if it is open already
        {
            //this one will open the history menu when the player clicks the hitory button
            closeAllMenus();//making sure all the menus are closed
            gameHistoryMenu.SetActive(true);

            openKingdomHistory();

            setMenuButtonTransparency(gameHistoryButton);
            playSound();
        }
        
    }
    //handles the menus inside the menu



    //handles the buttons inside the game history menu
    public void openKingdomHistory()
    {
        if (!kingdomHistoryText.activeInHierarchy)//checking to see if it is open already
        {
            closeAllHistoryTexts();

            kingdomHistoryText.SetActive(true);
            setHistoryButtonsTransparency(kingdomHistoryOpenButton);
            playSound();
        }
    }

    public void openBackPackHistory()
    {
        if (!magicBackPackText.activeInHierarchy)//checking to see if it is open already
        {
            closeAllHistoryTexts();

            magicBackPackText.SetActive(true);
            setHistoryButtonsTransparency(magicBackPackHistoryOpenButton);
            playSound();
        }
    }


    public void openMonstersHistory()
    {
        if (!monsterHistoryText.activeInHierarchy)//checking to see if it is open already
        {
            closeAllHistoryTexts();

            monsterHistoryText.SetActive(true);
            setHistoryButtonsTransparency(monstersHistoryOpenButton);
            playSound();
        }
    }


    public void openPetsHistory()
    {
        if (!petsHistoryText.activeInHierarchy)//checking to see if it is open already
        {
            closeAllHistoryTexts();

            petsHistoryText.SetActive(true);
            setHistoryButtonsTransparency(petsHistoryOpenButton);
            playSound();
        }
    }


    public void openThomasMisteryHistory()
    {
        if (!thomasMisteryText.activeInHierarchy)//checking to see if it is open already
        {
            closeAllHistoryTexts();

            thomasMisteryText.SetActive(true);
            setHistoryButtonsTransparency(thomasMisteryOpenButton);
            playSound();
        }
    }
    //handles the buttons inside the game history menu







    private void handleStatus()
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
    }



    private void closeAllMenus()
    {
        //this will make sure every menu is closed
        statusMenu.SetActive(false);
        gameHistoryMenu.SetActive(false);

        //this will also change all the transperancy back to transparent 
        gameHistoryButton.GetComponent<Image>().color = menuNotClickedColor;
        statusButton.GetComponent<Image>().color = menuNotClickedColor;
    }



    private void closeAllHistoryTexts()
    {
        //this will make sure all the texts are closed
        kingdomHistoryText.SetActive(false);
        magicBackPackText.SetActive(false);
        monsterHistoryText.SetActive(false);
        petsHistoryText.SetActive(false);
        thomasMisteryText.SetActive(false);

        //this will also change all the transperancy back to transparent
        kingdomHistoryOpenButton.GetComponent<Image>().color = historyNotClicked;
        magicBackPackHistoryOpenButton.GetComponent<Image>().color = historyNotClicked;
        monstersHistoryOpenButton.GetComponent<Image>().color = historyNotClicked;
        petsHistoryOpenButton.GetComponent<Image>().color = historyNotClicked;
        thomasMisteryOpenButton.GetComponent<Image>().color = historyNotClicked;
    }



    public void setMenuButtonTransparency(GameObject button)
    {
        //will change the button transparency when the player clicks on it
        button.GetComponent<Image>().color = menuClickedColor;
    }


    public void openEasterEggButton()
    {
        countForEasterEgg++;
        
        if(countForEasterEgg == 15)
        {
            thomasMisteryOpenButton.SetActive(true);
            misterySound.Play();
        }
    }

    public void setHistoryButtonsTransparency(GameObject button)
    {
        //will change the button transparency when the player clicks on it
        button.GetComponent<Image>().color = historyClickedColor;
    }



    public void clickResume()
    {
        inGameMenuObject.SetActive(false);
        playSound();
        resumeGame();
    }


    private void playSound()
    {
        clickButton.Play();
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
