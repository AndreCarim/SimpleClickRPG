using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellHandler : MonoBehaviour
{
    [SerializeField]private BackPackHandler backPackHandler;
    [SerializeField]private GoldHandler goldHandler;

    
    [SerializeField]private TextMeshProUGUI currentStateOfTheButtonText;

    [SerializeField]private Animator goldFromButtonToBagAnimation;

    

    

    // to the timer
    private bool isReadyToSell = true;
    private float currentTime;
    private float startTime = 30f;
    private string sellStateText = "$$$";

    
    
    [SerializeField]private AudioSource audioSource; 

    void Start(){
        currentTime = startTime;

        
        timeHandler();
    }


    //TIMER
    void FixedUpdate(){ 
        if(isReadyToSell == false){ // checking if the player sold something
            currentTime -= 1 * Time.deltaTime; // decreasing the amount of seconds inside the currentTime(starts at 30)
            currentStateOfTheButtonText.text = Mathf.RoundToInt(currentTime).ToString(); // round it to int and then transform it to string
            if(currentTime <= 0){ //check if the timer is done
                isReadyToSell = true; // set the status back to open/true
                currentStateOfTheButtonText.text = sellStateText; //change the text to sell
                currentTime = startTime; //set the current time back to 30
            }
        }
    }

    public void sellForGold(){
        //sell for gold button will sell the items in the backpack
        //call the clear backpack method inside backpackHandler to clear the backpack
        //call the increaseAmountOfGold methods from GoldHandler script;

        sellForGoldChecker();
        
    }

    private void sellForGoldChecker(){
        //just a checker
        if(backPackHandler.getCurrentBackPack() > 0 && backPackHandler.getTotalItemsValue() > 0){
            if(isReadyToSell == true){
                goldHandler.increaseAmountOfGold(backPackHandler.getTotalItemsValue());
                backPackHandler.clearBackPack();

                

                animationHandler();
                backPackHandler.setText();
                timeHandler();
                playSound();
            } 
        }
    }

    private void animationHandler()
    {
        goldFromButtonToBagAnimation.SetTrigger("OnSellClick");
    }

    private void timeHandler(){
        //if this is false, the time will start to count
        isReadyToSell = false;
    }


    


    private void playSound(){
        audioSource.Play();
    }

    public void startAd(){
        startTime = 15f;
    }

    public void finishAd(){
        startTime = 30f;
    }

    

    
}
