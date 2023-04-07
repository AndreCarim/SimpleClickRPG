using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrengthHandler : MonoBehaviour
{

    [SerializeField]private double currentStrengthPower;//current power
    private double behindTheScenePower;//current behind the scene power

    private double currentUpgradePrice; // the visible price
    private double behindTheScenePrice; //behindTheSceneValue is the value that will keep increasing every update ( currentUpgradePrice + (behindTheSceneValue*2))  

    [SerializeField]private GoldHandler goldHandler;

    [SerializeField] private AudioSource audioSource;


    [SerializeField]private TextMeshProUGUI strengthPowerText;
    [SerializeField]private TextMeshProUGUI upgradeStrengthPriceText;

    private bool isAdActive;

    // Start is called before the first frame update
    void Start()
    {

        load();

        isAdActive = false;
        
        setStrengthPowerText();
    }

    void OnApplicationPause(bool stats){
        if(stats == true)
        {
            save();
        }
    }






    public void upgradeStrength(){
        if(currentUpgradePrice <= goldHandler.getCurrentAmountOfGold()){
            
            
            handleUpgrades();
            playSound();
            setStrengthPowerText();
        }
    }


    /*
     * 
        Força Inicial = 50
        valor de incremento da força= 5*2(10,25,55, 115, 235...)
        Custo inicial da força = 20
        Valor de incremento de custo= 25(10, 60, 160, 360, 760...)
     * 
     */
    private void handleUpgrades(){
        if(currentUpgradePrice <= goldHandler.getCurrentAmountOfGold()){
            
            goldHandler.decreaseAmountOfGold(currentUpgradePrice);//handling gold
            

            if(isAdActive == false){//handling strength   
                currentStrengthPower = currentStrengthPower + behindTheScenePower;
                behindTheScenePower = behindTheScenePower + 5; //increases 10% per upgrade
            }
            else if(isAdActive == true){   
                currentStrengthPower = (currentStrengthPower+20) + behindTheScenePower;
                behindTheScenePower = behindTheScenePower +20;
            }


            behindTheScenePrice = behindTheScenePrice + 15;//handling price
            currentUpgradePrice = currentUpgradePrice + behindTheScenePrice;
        }
    }



    

    public double getStrengthPower(){
        //this will be called by the damageClickerHandler script
        //everytime a click is done, the script will check the amount of strength player has
        return currentStrengthPower;
    }

    private void setStrengthPower(double newStrength){
        currentStrengthPower = newStrength;
    }


    private void setStrengthPowerText(){

        if(currentUpgradePrice > 10000)
        {
            upgradeStrengthPriceText.text = NumberAbrev.ParseDouble(currentUpgradePrice, 2);
        }
        else
        {
            upgradeStrengthPriceText.text = NumberAbrev.ParseDouble(currentUpgradePrice, 0);
            
        }

        if(currentStrengthPower > 10000)
        {
            strengthPowerText.text = NumberAbrev.ParseDouble(currentStrengthPower, 2);
        }
        else
        {
            strengthPowerText.text = NumberAbrev.ParseDouble(currentStrengthPower, 0);
        }
        
        
    }

    private void playSound(){
        audioSource.Play();
    }

    public void startAd(){
        currentStrengthPower = currentStrengthPower * 2;
        isAdActive = true;
        setStrengthPowerText();
    }

    public void finishAd(){
        currentStrengthPower = currentStrengthPower / 2;
        isAdActive = false;
        setStrengthPowerText();
    }


    public void save()
    {
        if (isAdActive == false)
        {
            ES3.Save("currentStrengthPower", currentStrengthPower);
            ES3.Save("behindTheScenePower", behindTheScenePower);
        }
        else
        {
            ES3.Save("currentStrengthPower", currentStrengthPower / 2);
            ES3.Save("behindTheScenePower", behindTheScenePower /2);
        }

       

        ES3.Save("currentStrengthUpgradePrice", currentUpgradePrice);
        ES3.Save("behindTheScenePrice", behindTheScenePrice);
        
    }

    private void load()
    {
        currentStrengthPower = ES3.Load<double>("currentStrengthPower", 50);
        currentUpgradePrice = ES3.Load<double>("currentStrengthUpgradePrice", 20);
        behindTheScenePrice = ES3.Load<double>("behindTheScenePrice", 25);
        behindTheScenePower = ES3.Load<double>("behindTheScenePower", 4);
    }

    
}
