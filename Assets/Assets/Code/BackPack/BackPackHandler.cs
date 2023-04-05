using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackPackHandler : MonoBehaviour
{
    private double currentBackPackMaxSize; //currentBackPackMaxSize
    private double currentBackPack; // current number of items backpack is holding
    private double totalItemsValue; //the sum of all the items values

    private double currentUpgradePrice;//current price
    

    [SerializeField]private SellHandler sellHandler; // this is just to change the text
    [SerializeField]private GoldHandler goldHandler; // to check if the player has gold to upgrade

    
    [SerializeField]private TextMeshProUGUI floatingBackPackCurrentAmountText;
    [SerializeField]private TextMeshProUGUI upgradePriceText;
    [SerializeField]private TextMeshProUGUI currentAmountGoldBackpack;

    private bool isAdActive = false;


    [SerializeField] private AudioSource audioSource;

    void Start(){
        

        load();

        setText();
    }

     void OnApplicationPause(bool stats){
        if (stats == true)
        {
            save();
            setText();
        }
     }




    public void upgradeBackPack(){
        if(currentUpgradePrice <= goldHandler.getCurrentAmountOfGold()){
            handleUpgrade();
            setText();
            playSound();
        }
    }



    /*
    Mochila:
    valor inicial = 1B
    quantidade inicial = 0/5
    multiplicação por nivel = x100 (1B, 10B, 100B, 1T, 10T, 100T...) 
     */
    private void handleUpgrade(){
        goldHandler.decreaseAmountOfGold(currentUpgradePrice);

       
        currentUpgradePrice = currentUpgradePrice   * 10;//upgrade the current price of an upgrade
        
        

        if(isAdActive == false){
            currentBackPackMaxSize = currentBackPackMaxSize + 1;//upgrade the current backPack max
        }else if(isAdActive == true){
            currentBackPackMaxSize = currentBackPackMaxSize + 2;//upgrade the current backPack max
        }
    }


    public void enemyDroppedItem(double value){
        //this will be called from the enemyHandler script
        //this will get as a parameter the value of the item dropped
        //when a enemy is killed
        if(currentBackPack < currentBackPackMaxSize){
            //backPack is not full
            currentBackPack++;
            totalItemsValue += value;
            
            setText();

            
        }else if(currentBackPack == currentBackPackMaxSize || currentBackPack > currentBackPackMaxSize){
            //backPack is full
        }
    }






    public void clearBackPack(){
        //this will handle the backPack once you sell the items on it
        //it just need to make the currentBackPack = 0 and
        //the totalItemsValue = 0 too and change the text;
        //this will be called from the SellHandler script
        currentBackPack = 0;
        totalItemsValue = 0;

        setText();
    }

    public double getTotalItemsValue(){
        return totalItemsValue;
    }


    private void setCurrentBackPackMaxSize(double newMaxSize){
        currentBackPackMaxSize = newMaxSize;
    }

    public double getCurrentBackPack(){
        return currentBackPack;
    }

    public void setText(){
        //this will set both the price and space text
        floatingBackPackCurrentAmountText.text = NumberAbrev.ParseDouble(currentBackPack) + "/" + NumberAbrev.ParseDouble(currentBackPackMaxSize);
        

        if(totalItemsValue > 10000)
        {
            currentAmountGoldBackpack.text = NumberAbrev.ParseDouble(totalItemsValue,2); 
        }
        else
        {
            currentAmountGoldBackpack.text = NumberAbrev.ParseDouble(totalItemsValue, 0);
        }

        if(currentUpgradePrice > 10000)
        {
            upgradePriceText.text = NumberAbrev.ParseDouble(currentUpgradePrice, 2);
        }
        else
        {
            upgradePriceText.text = NumberAbrev.ParseDouble(currentUpgradePrice, 0);
        }
        
    }

    public bool isFull(){
        return currentBackPackMaxSize == currentBackPack;
    }

    private void playSound(){
        audioSource.Play();
    }

    public void startAd(){
        currentBackPackMaxSize = currentBackPackMaxSize * 2;
        isAdActive = true;
        setText();
    }

    public void finishAd(){
        currentBackPackMaxSize = currentBackPackMaxSize / 2;
        isAdActive = false;
        setText();
    }

    public void save()
    {
        if (isAdActive == false)
        {
            ES3.Save("currentBackPackMaxSize", currentBackPackMaxSize);
        }
        else
        {
            ES3.Save("currentBackPackMaxSize", currentBackPackMaxSize / 2);
        }

        ES3.Save("currentBackPack", currentBackPack);
        ES3.Save("currentBackPackUpgradePrice", currentUpgradePrice);
        ES3.Save("totalItemsValue", totalItemsValue);    
    }

    private void load()
    {
        currentBackPackMaxSize = ES3.Load<double>("currentBackPackMaxSize", 5);
        currentBackPack = ES3.Load<double>("currentBackPack", 0);
        currentUpgradePrice = ES3.Load<double>("currentBackPackUpgradePrice", 10000);
        totalItemsValue = ES3.Load<double>("totalItemsValue", 0);
    }
}
