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

    //handles the pets for backpack and gold
    private int petBackpackBonusAmount;// for ex 1 == 1 extra space
    private double petBackpackTotalItemsValue; //this is the amount of gold the pet will carry (only the gold passing the max amount of gold)
    private double petGoldBonusAmount; //handles the gold pets, % ex: 100 gold + 30% == 130 gold


    [SerializeField]private SellHandler sellHandler; // this is just to change the text
    [SerializeField]private GoldHandler goldHandler; // to check if the player has gold to upgrade

    
    [SerializeField]private TextMeshProUGUI floatingBackPackCurrentAmountText;
    [SerializeField]private TextMeshProUGUI upgradePriceText;
    [SerializeField]private TextMeshProUGUI currentAmountGoldBackpack;

    [SerializeField] private GameObject noMoreSpaceIcon;

    private bool isAdActive = false;


    [SerializeField] private AudioSource audioSource;

    //makes life easier
    private double upgradeBackPackPriceAmount;
    //makes life easier


    void Start(){
        //makes life easier
        upgradeBackPackPriceAmount = 10; //amount of times the price will increase after a upgrade
        //makes life easier


        petBackpackBonusAmount = 0;
        petGoldBonusAmount = 0;

        load();


        checkNoMoreSpaceIcon();
        setText();
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

       
        currentUpgradePrice = currentUpgradePrice  * upgradeBackPackPriceAmount;//upgrade the current price of an upgrade
        
        

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
            totalItemsValue = totalItemsValue + value + getPetGoldAmountBonus(value);//current + dropped + pet bonus

            setText();
            checkNoMoreSpaceIcon();


        }else if(currentBackPack < currentBackPackMaxSize + petBackpackBonusAmount)
        {
            //checking if there is a pet

            //adding the gold to the pet
            petBackpackTotalItemsValue += value;
            currentBackPack++;

            setText();
            checkNoMoreSpaceIcon();
        }//else do nothing

    }



    //HANDLES PET
    public void setPetBackpackBonusAmount(int value)
    {
        petBackpackBonusAmount = value;//setting the new value

        setText();
        checkNoMoreSpaceIcon();
    }

    public void removePetBackpackBonusAmount()
    {

        if (currentBackPack > currentBackPackMaxSize)
        {
            //checking if the player is removing the pet with the full backpack, if so, it
            //needs to remove gold amount
            totalItemsValue = totalItemsValue - petBackpackTotalItemsValue;
            currentBackPack = currentBackPack - petBackpackBonusAmount;
            petBackpackTotalItemsValue = 0;
        }

        petBackpackBonusAmount = 0;


        checkNoMoreSpaceIcon();
        setText();
    }

    public void setPetGoldBonusAmount(double value)
    {
        petGoldBonusAmount = value;//setting the new value
    }

    public void removePetGoldBonusAmount()
    {
        petGoldBonusAmount = 0;
    }

    public double getPetGoldAmountBonus(double value)
    {
        //the value is the amount dropped by the monster
        return value * petGoldBonusAmount; // ex: 100 power * 0.3 == 130
    }





    public void clearBackPack(){
        //this will handle the backPack once you sell the items on it
        //it just need to make the currentBackPack = 0 and
        //the totalItemsValue = 0 too and change the text;
        //this will be called from the SellHandler script
        currentBackPack = 0;
        totalItemsValue = 0;
        petBackpackTotalItemsValue = 0;

        checkNoMoreSpaceIcon();
        setText();
    }

    public double getTotalItemsValue(){
        return totalItemsValue + petBackpackTotalItemsValue;
    }


    private void setCurrentBackPackMaxSize(double newMaxSize){
        currentBackPackMaxSize = newMaxSize;
    }

    public double getCurrentBackPack(){
        return currentBackPack;
    }

    public void setText(){
        //this will set both the price and space text
        floatingBackPackCurrentAmountText.text = NumberAbrev.ParseDouble(currentBackPack) + "/" + NumberAbrev.ParseDouble(currentBackPackMaxSize + petBackpackBonusAmount);
        

        if(totalItemsValue > 10000)
        {
            currentAmountGoldBackpack.text = NumberAbrev.ParseDouble(totalItemsValue + petBackpackTotalItemsValue, 2); 
        }
        else
        {
            currentAmountGoldBackpack.text = NumberAbrev.ParseDouble(totalItemsValue + petBackpackTotalItemsValue, 0);
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
        return currentBackPackMaxSize + petBackpackBonusAmount == currentBackPack;
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


    private void checkNoMoreSpaceIcon()
    {
        //this will handle the animation of no more space left in the back pack

        if(currentBackPack < currentBackPackMaxSize + petBackpackBonusAmount)
        {
            //there are still space left
            noMoreSpaceIcon.SetActive(false);
        }
        else
        {
            noMoreSpaceIcon.SetActive(true);
        }
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
        ES3.Save("petBackpackTotalItemsValue", petBackpackTotalItemsValue);
    }

    private void load()
    {
        currentBackPackMaxSize = ES3.Load<double>("currentBackPackMaxSize", 5);
        currentBackPack = ES3.Load<double>("currentBackPack", 0);
        currentUpgradePrice = ES3.Load<double>("currentBackPackUpgradePrice", 10000);
        totalItemsValue = ES3.Load<double>("totalItemsValue", 0);
        petBackpackTotalItemsValue = ES3.Load<double>("petBackpackTotalItemsValue", 0);
    }
}
