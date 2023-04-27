using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.IO;

public class AutoSaveHandler : MonoBehaviour
{
    [SerializeField] private GoldHandler goldHandler;
    [SerializeField] private BackPackHandler backPackHandler;
    [SerializeField] private AutoClickHandler autoClickHandler;
    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private PlayerHealthHandler playerHealthHandler;
    [SerializeField] private TutorialHandler tutorialHandler;
    [SerializeField] private StrengthHandler strengthHandler;
    [SerializeField] private PetsHandler petsHandler;
    [SerializeField] private PetsPlayerOwn petsPlayerOwn;
    [SerializeField] private ShowPetsOwned showPetsOwned;
    [SerializeField] private GemHandler gemHandler;
    [SerializeField] private MagicHandler magichandler;

    [SerializeField] private GameObject autoSaveText;

    private float currentTime;
    private float saveEveryXSeconds;

    private float autoSaveTextTime;
    private bool isTextOn;


    void Start()
    {
        saveEveryXSeconds = 120;
        currentTime = saveEveryXSeconds;

       
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
        autoSaveTextTime -= 1 * Time.deltaTime;

        if (currentTime <= 0)
        { //check if the timer is done

            currentTime = saveEveryXSeconds; //set the current time back to 20

            autoSaveTextTime = 3f;


            save();
        }

        if(autoSaveTextTime > 0 && isTextOn == false)
        {
            isTextOn = true;
            autoSaveText.SetActive(true);
        }else if(autoSaveTextTime < 0 && isTextOn) {
            isTextOn = false;
            autoSaveText.SetActive(false);
        }
        
        

    }


    public void save()//save all the game in the phone cache
    {
        //need to add everything I want to save

        //coins
        goldHandler.save();
        gemHandler.save();
        magichandler.save();

        //backPack
        backPackHandler.save();

        //AutoClick
        autoClickHandler.save();

        //EnemyHandler
        enemyHandler.save();

        //player health
        playerHealthHandler.save();

        //Tutorial
        tutorialHandler.save();

        //player strength
        strengthHandler.save();

        //pets
        petsHandler.save();
        petsPlayerOwn.save();
        showPetsOwned.save();



        //save to the cloud


        saveToServer();
    }




    public void saveToServer()// take the saving from the cache and stores to the cloud
    {
        //saving the file to the server
        var cacheSettings = new ES3Settings(ES3.Location.File);//getting the location where data is saved
        string data = ES3.LoadRawString(cacheSettings);//transform it into a string

        

        // Create a request to save the player data
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                { "myData", data }
            }
            
        };
        Debug.Log(data);

        // Call the PlayFab API to save the player data
        PlayFabClientAPI.UpdateUserData(request, OnDataSaved, OnError);
        
    }


    // Callback for success
    private void OnDataSaved(UpdateUserDataResult result)
    {
        Debug.Log("Data saved successfully!");
    }

    // Callback for failure
    private void OnError(PlayFabError error)
    {
        Debug.LogError("Data save failed: " + error.ErrorMessage);
    }



   



}
