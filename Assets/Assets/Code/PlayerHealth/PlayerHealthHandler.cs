using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthHandler : MonoBehaviour
{
    private double currentPlayerHealth;
    private double currentPlayerMaxHealth;

    private double playerHealAmount; //amount of hp healed
    private float healPlayerEveryXSeconds; // time between healing

    private int timesPlayerDied;

    private double petBonusAmount; //%, 1000 + 100 == 105
    private double extraMaxHealthPet; // this will hold the value that the pet added to the player max health
    
    
    private float currentTime;

    [SerializeField] private DeadMenuHandler deadMenuHandler;
    [SerializeField] private GoldHandler goldHandler;

    
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    private bool isTutorialOn;

    //handles the pause menu
    private double totalHealthLostEver;

    [SerializeField] private Transform healthBarTransform;

    //sounds for damage and healing and death
    [SerializeField] private AudioSource playerDamageSound;
    [SerializeField] private AudioSource playerDiedSound;
    [SerializeField] private AudioSource playerHealedSound;



    //died menu
    [SerializeField] private GameObject diedMenu;
    [SerializeField] private TextMeshProUGUI goldLostAmountText;



    //for the health menu
    [SerializeField] private TextMeshProUGUI everyXsecondsStatsText;
    [SerializeField] private TextMeshProUGUI amountHealedStatsText;

    [SerializeField] private TextMeshProUGUI priceHealSecondsText;
    [SerializeField] private TextMeshProUGUI priceHealAmountText;

    [SerializeField] private TextMeshProUGUI priceMaxHealthText;
    

   
    [SerializeField] private AudioSource audioSourceSeconds;
    [SerializeField] private AudioSource audioSourceAmount;

    //upgrade prices
    private double healPlayerEveryXSecondsPrice;
    private double playerHealAmountPrice;
    private double maxHealthPrice;


    void Start()
    {
        currentTime = 2f;

        petBonusAmount = 0;

        load();
        healthBarUpdate();
        setTextHealthMenu();
    }

    void OnApplicationPause(bool stats)
    {
        if (stats == true)
        {
            save();
        }
    }


    //TIMER for the player healing
    void FixedUpdate()
    {

        currentTime -= 1 * Time.deltaTime; // decreasing the amount of seconds inside the currentTime(starts at 1)
        if (currentTime <= 0)
        { //check if the timer is done

            currentTime = healPlayerEveryXSeconds; //set the current time back to 30
            if (currentPlayerHealth < currentPlayerMaxHealth)
            {
                healPlayer();
            }

        }

    }



    //deal damage on player
    public void dealDamage(double amount)
    {
        //if tutorial is on, we dont want to recieve damage
        if(isTutorialOn == false) {
            totalHealthLostEver += amount; //pause menu

            if (currentPlayerHealth - amount > 0)
            {
                //player still has hp
                currentPlayerHealth = currentPlayerHealth - amount;
                healthBarUpdate();
            }
            else
            {
                //player died
                currentPlayerHealth = 0;
                healthText.text = ":(";
                healthBarSlider.value = 0;
                timesPlayerDied++;
                playerDied();
            }

            damageSound();
        } 
    }

    private void playerDied()
    {
        //pause the game, open the died tab
        deadMenuHandler.playerDied();

        diedMenu.SetActive(true);
        
        deadSound();
    }

    private void healPlayer()
    {
        //if tutorial is on, we dont want to recieve healing
        if (isTutorialOn == false && playerHealAmount > 0) {
            if (playerHealAmount + currentPlayerHealth <= currentPlayerMaxHealth)
            {
                //there still room to heal
                currentPlayerHealth = currentPlayerHealth + playerHealAmount;
            }
            else
            {
                //there is no room to heal, so put max health
                currentPlayerHealth = currentPlayerMaxHealth;
            }

            healingSound();
        }

        

        
        healthBarUpdate();
    }


    public void reloadPlayer()
    {
        currentPlayerHealth = currentPlayerMaxHealth;
        healthBarUpdate();
    }


    public void setTutorialIsOn(bool value)
    {
        isTutorialOn = value;
    }

    //upgrade the amount of hp recovered by heal
    public void upgradePlayerHealAmount()
    {
        if(playerHealAmountPrice <= goldHandler.getCurrentAmountOfGold())
        {
            //player have the money
            goldHandler.decreaseAmountOfGold(playerHealAmountPrice);

            //increase the amount healed
            playerHealAmount = playerHealAmount + 40;

            //increase the price
            playerHealAmountPrice = playerHealAmountPrice + 1000;

            audioSourceAmount.Play();// playing the sound
        }
        setTextHealthMenu();
    }

    //upgrade the seconds between healing
    public void upgradeHealPlayerEveryXSeconds()
    {
        if (healPlayerEveryXSecondsPrice <= goldHandler.getCurrentAmountOfGold() && healPlayerEveryXSeconds > 2)
        {
            //player have the money
            goldHandler.decreaseAmountOfGold(healPlayerEveryXSecondsPrice);


            //decrease the seconds between healing
            healPlayerEveryXSeconds = healPlayerEveryXSeconds - .1f;

            //increase the price
            healPlayerEveryXSecondsPrice = healPlayerEveryXSecondsPrice + 10000;

            audioSourceSeconds.Play();//playing the sound
        }
        setTextHealthMenu();
        
    }

    public void upgradeMaxHealth()
    {
        if (maxHealthPrice <= goldHandler.getCurrentAmountOfGold() && healPlayerEveryXSeconds > 2)
        {
            //removing the amount given by the player
            if(petBonusAmount > 0)
            {
                currentPlayerMaxHealth = currentPlayerMaxHealth - extraMaxHealthPet;
            }

            //player have the money
            goldHandler.decreaseAmountOfGold(maxHealthPrice);

            //increase the max health
            currentPlayerMaxHealth = currentPlayerMaxHealth + 300;

            //increase the price
            maxHealthPrice = maxHealthPrice + 1000;

            audioSourceSeconds.Play();//playing the sound

            if(petBonusAmount > 0) { changePetAmountBonus(); }
            setTextHealthMenu();
            healthBarUpdate();
        }      
    }


    //HANDLES PET
    public void setPetBonusAmount(double value)
    {
        petBonusAmount = value;//setting the new value

        changePetAmountBonus();

        healthBarUpdate();
    }

    public void removePetBonusAmount()
    {
        petBonusAmount = 0;

        currentPlayerMaxHealth = currentPlayerMaxHealth - extraMaxHealthPet;
        extraMaxHealthPet = 0; //cleaning the pet max health

        healthBarUpdate();
    }

    private void changePetAmountBonus()
    {
        //this will calculate how much the pet will give as bonus
        extraMaxHealthPet = currentPlayerMaxHealth * petBonusAmount; // ex: 100 power * 0.3 == 130

        //add the extra amount to the max health
        currentPlayerMaxHealth = currentPlayerMaxHealth + extraMaxHealthPet;

    }




    private void setTextHealthMenu()
    {
        everyXsecondsStatsText.text = NumberAbrev.ParseDouble(healPlayerEveryXSeconds) + "s";
        amountHealedStatsText.text = NumberAbrev.ParseDouble(playerHealAmount) + " hp";
        
        if(healPlayerEveryXSeconds > 2)
        {
            priceHealSecondsText.text = NumberAbrev.ParseDouble(healPlayerEveryXSecondsPrice);
        }
        else
        {
            priceHealSecondsText.text = "MAX";
        }


        priceMaxHealthText.text = NumberAbrev.ParseDouble(maxHealthPrice);
        priceHealAmountText.text = NumberAbrev.ParseDouble(playerHealAmountPrice);
    }



    private void healthBarUpdate()
    {
        string tempCurrentPlayerHealth;
        string tempCurrentPlayerMaxHealth;

        if (currentPlayerHealth > 1000000000)//handles the slider itself
        {
            healthBarSlider.maxValue = Mathf.RoundToInt((float)currentPlayerMaxHealth / 1000000);
            healthBarSlider.value = Mathf.RoundToInt((float)currentPlayerHealth / 1000000);
        }
        else
        {
            healthBarSlider.maxValue = Mathf.RoundToInt((float)currentPlayerMaxHealth / 50);
            healthBarSlider.value = Mathf.RoundToInt((float)currentPlayerHealth / 50);
        }



        if (currentPlayerHealth < 1) //handles the text inside the bar for the current Health
        {
            tempCurrentPlayerHealth = "1";
        }
        else if (currentPlayerHealth > 10000)
        {
            tempCurrentPlayerHealth = NumberAbrev.ParseDouble(currentPlayerHealth, 2);
        }
        else
        {
            tempCurrentPlayerHealth = NumberAbrev.ParseDouble(currentPlayerHealth, 0);
        }


        if (currentPlayerMaxHealth > 10000)//handles the text inside the bar for the max health
        {
            tempCurrentPlayerMaxHealth = NumberAbrev.ParseDouble(currentPlayerMaxHealth, 2);
        }
        else
        {
            tempCurrentPlayerMaxHealth = NumberAbrev.ParseDouble(currentPlayerMaxHealth, 0);
        }



        healthText.text = tempCurrentPlayerHealth + "/" + tempCurrentPlayerMaxHealth;
    }

    

    private void damageSound()
    {
        playerDamageSound.Play();
    }

    private void deadSound()
    {
        playerDiedSound.Play();
    }

    private void healingSound()
    {
        playerHealedSound.Play();
    }

    

    

    public string getTimesPlayerDied()
    {
        return timesPlayerDied.ToString();
    }

    public string getTotalHealthLostEver()
    {
        return NumberAbrev.ParseDouble(totalHealthLostEver);
    }

    public string getExtraMaxHealthPet()
    {
        return NumberAbrev.ParseDouble(extraMaxHealthPet,0);
    }

    








    public void save()
    {
        ES3.Save("currentPlayerMaxHealth", currentPlayerMaxHealth);
        ES3.Save("currentPlayerHealth", currentPlayerHealth);
        ES3.Save("playerHealAmount", playerHealAmount);
        ES3.Save("healPlayerEveryXSeconds", healPlayerEveryXSeconds);
        ES3.Save("timesPlayerDied", timesPlayerDied);
        ES3.Save("playerHealAmountPrice", playerHealAmountPrice);
        ES3.Save("healPlayerEveryXSecondsPrice", healPlayerEveryXSecondsPrice);
        ES3.Save("maxHealthPrice", maxHealthPrice);
        ES3.Save("totalHealthLostEver", totalHealthLostEver);
    }

    private void load()
    {
        currentPlayerMaxHealth = ES3.Load<double>("currentPlayerMaxHealth", 1000);
        currentPlayerHealth = ES3.Load<double>("currentPlayerHealth", currentPlayerMaxHealth);
        playerHealAmount = ES3.Load<double>("playerHealAmount", 0);
        healPlayerEveryXSeconds = ES3.Load<float>("healPlayerEveryXSeconds", 5);
        timesPlayerDied = ES3.Load<int>("timesPlayerDied", 0);
        healPlayerEveryXSecondsPrice = ES3.Load<double>("healPlayerEveryXSecondsPrice", 5000);
        playerHealAmountPrice = ES3.Load<double>("playerHealAmountPrice", 500);
        maxHealthPrice = ES3.Load<double>("maxHealthPrice", 1000);
        totalHealthLostEver = ES3.Load<double>("totalHealthLostEver", 0);
    }
}
