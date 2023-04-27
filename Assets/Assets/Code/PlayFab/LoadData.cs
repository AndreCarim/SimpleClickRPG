using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Networking;
using TMPro;


public class LoadData : MonoBehaviour
{

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject loading;

    private string key = "myData";

    void Start()
    {  
        ES3.DeleteFile();
        
    }

    public void GetPlayerData()
    {
        loading.SetActive(true);

        // Create a request to load the player data
        var request = new GetUserDataRequest()
        {
            Keys = new List<string>() { key }
        };

        // Call the PlayFab API to load the player data
        PlayFabClientAPI.GetUserData(request, OnDataLoaded, OnError);
    }

    private void OnDataLoaded(GetUserDataResult result)
    {
        

        // Check if the player data contains "myData"
        if (result.Data.ContainsKey(key))
        {
            //if there is a save, load the save
            var cacheSettings = new ES3Settings(ES3.Location.File);//gets the location file stored
            ES3.SaveRaw(result.Data[key].Value, cacheSettings); //overwrite the save with the save from server
            

            Debug.Log("Data loaded successfully!");
           
        }
        else
        {
            
            // Do something else, such as initializing the game state
            Debug.Log("No data found.");

        }
        loading.SetActive(false);
        playButton.SetActive(true);
    }

    private void OnError(PlayFabError error)
    {
        

        Debug.LogError("Error loading data: " + error.GenerateErrorReport());
    }
}
   





