using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoClickHandler : MonoBehaviour
{
    private float autoClickEveryXSeconds; //1 is one second, this can upgrade
    private double autoClickStrength;

    private double autoClickStrengthPrice;
    private double autoClickTimePrice;
 
    private float currentTime;

    private bool isOn;

    

    [SerializeField] private GameObject popUpText;
    [SerializeField] private EnemyHandler enemyHandlerCode;

    [SerializeField] private TextMeshProUGUI currentAutoClickTimePriceText;
    [SerializeField] private TextMeshProUGUI currentAutoClickStrengthPriceText;

    [SerializeField] private GoldHandler goldHandler; // to check if the player has gold to upgrade

    [SerializeField] private AudioSource audioSourceDamageSound;
    [SerializeField] private AudioSource audioSourceTimeButton;
    [SerializeField] private AudioSource audioSourceStrengthButton;

    [SerializeField] private Image onOffSwitchImage;
    [SerializeField] private Sprite onSwitchSprite;
    [SerializeField] private Sprite offSwitchSprite;

    [SerializeField] private AudioSource switchAudioSource;
    [SerializeField] private TextMeshProUGUI currentAmountOfAutoClickStrengthText;

    



    void Start()
    {
        load();
        setText();
        setSwitchImage();
    }


    void OnApplicationPause(bool stats)
    {
        if (stats == true)
        {
            save();
        }
    }


    //TIMER
    void FixedUpdate()
    {
        
            currentTime -= 1 * Time.deltaTime; // decreasing the amount of seconds inside the currentTime(starts at 1)
            if (currentTime <= 0)
            { //check if the timer is done
          
                currentTime = autoClickEveryXSeconds; //set the current time back to 30
                if (isOn && autoClickStrength > 0)
                {
                    autoClick();
                }
                
            }
        
    }



    public void upgradeStrengthClick()
    {
        if (autoClickStrengthPrice <= goldHandler.getCurrentAmountOfGold())
        {
            goldHandler.decreaseAmountOfGold(autoClickStrengthPrice); //decrease the amount of gold

            //handle auto click strength
            if (autoClickStrength == 0)
            {
                autoClickStrength = 500;//first strength
            }
            else
            {
                autoClickStrength = autoClickStrength * 4;
            }


            //handle the new price for the upgrade
            autoClickStrengthPrice = autoClickStrengthPrice * 10;

            playSoundStrength();
            setText();
        }
        
    }

    public void upgradeTimeClick()
    {
        if (autoClickTimePrice <= goldHandler.getCurrentAmountOfGold()) 
        {
            

            //handling the amount of seconds between auto clicks
            //checking if the player is not at level max already (1 click every 1 second)
            if(autoClickEveryXSeconds > 1f)
            {
                goldHandler.decreaseAmountOfGold(autoClickTimePrice); //decrease the amount of gold

                autoClickEveryXSeconds = autoClickEveryXSeconds - 0.1f; //decrease the total amount of seconds to wait by 0.1 seconds (one tenth of a second)


                autoClickTimePrice = autoClickTimePrice * 11; // price will increase by 1000x every upgrade
                playSoundTime();
                setText();
            }
            
        }
        
        
    }


    public void onOffButton()
    {
        if (isOn)
        {
            isOn = false;
        }
        else
        {
            isOn = true;
        }

        switchAudioSource.Play();

        setSwitchImage();
    }




    //handles the auto click every x seconds
    private void autoClick()
    {
        
        enemyHandlerCode.setDamage(autoClickStrength);


        GameObject newObj = Instantiate(popUpText, Vector3.zero, Quaternion.identity);
        TMP_Text newText = newObj.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();//
        newText.text = NumberAbrev.ParseDouble(autoClickStrength);


        playDamageSound();

    }


    private void playSoundTime()
    {
        audioSourceTimeButton.Play();
    }

    private void playSoundStrength()
    {
        audioSourceStrengthButton.Play();
    }

    private void playDamageSound()
    {
        audioSourceDamageSound.Play();
    }


    private void setText()
    {
        
        if(autoClickEveryXSeconds > 1f)
        {
            currentAutoClickTimePriceText.text = NumberAbrev.ParseDouble(autoClickTimePrice);
        }
        else
        {
            currentAutoClickTimePriceText.text = "MAX";
        }

     

        if(autoClickStrength > 10000)
        {
            currentAmountOfAutoClickStrengthText.text = NumberAbrev.ParseDouble(autoClickStrength, 2);
            currentAutoClickStrengthPriceText.text = NumberAbrev.ParseDouble(autoClickStrengthPrice, 2);
            currentAutoClickTimePriceText.text = NumberAbrev.ParseDouble(autoClickTimePrice, 2);
        }
        else
        {
            currentAmountOfAutoClickStrengthText.text = NumberAbrev.ParseDouble(autoClickStrength, 0);
            currentAutoClickStrengthPriceText.text = NumberAbrev.ParseDouble(autoClickStrengthPrice, 0);
            currentAutoClickTimePriceText.text = NumberAbrev.ParseDouble(autoClickTimePrice, 0);
        }
        
    }

    private void setSwitchImage()
    {
        if (isOn)
        {
            onOffSwitchImage.sprite = onSwitchSprite;
        }
        else
        {
            onOffSwitchImage.sprite = offSwitchSprite;
        }
    }


    public void save()
    {
        ES3.Save("autoClickStrength", autoClickStrength);
        ES3.Save("autoClickEveryXSeconds", autoClickEveryXSeconds);
        ES3.Save("autoClickStrengthPrice", autoClickStrengthPrice);
        ES3.Save("autoClickTimePrice", autoClickTimePrice);
        ES3.Save("isOn", isOn);
    }



    private void load()
    {
        autoClickStrength = ES3.Load<double>("autoClickStrength", 0);
        autoClickEveryXSeconds = ES3.Load<float>("autoClickEveryXSeconds", 2f);
        autoClickStrengthPrice = ES3.Load<double>("autoClickStrengthPrice", 10000);
        autoClickTimePrice = ES3.Load<double>("autoClickTimePrice", 20000);
        isOn = ES3.Load<bool>("isOn", false);
    }



}
