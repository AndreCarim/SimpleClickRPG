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

    [SerializeField] private GameObject gettingPlayerDataText;
    [SerializeField] private GameObject checkingVersionText;


    [SerializeField] private GameObject updateVersionMessage;

    private string url;
    private string currentGameVersion;





    private string key = "myData";
    
    void Start()
    {  
        ES3.DeleteFile();
        url = "https://play.google.com/store/apps/details?id=com.AcaiSlayerStudio.SimpleClickRpg";
    }

    public void GetPlayerData()
    {
        gettingPlayerDataText.SetActive(true);
        checkingVersionText.SetActive(false);
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

        //link the android to the account

        checkAndroidLinked();

        InitializeVersionCheck(); 
    }

    private void OnError(PlayFabError error)
    {
        

        Debug.LogError("Error loading data: " + error.GenerateErrorReport());
    }


    private void checkAndroidLinked()
    {
        string newAndroidDeviceID = SystemInfo.deviceUniqueIdentifier;
        if (!string.IsNullOrEmpty(newAndroidDeviceID))
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
            {
                // Check if an Android device is already linked to the account
                if (result.AccountInfo.AndroidDeviceInfo != null)
                {
                    // Get the ID of the linked Android device
                    var linkedAndroidDeviceID = result.AccountInfo.AndroidDeviceInfo.AndroidDeviceId;

                    // Check if the linked device ID matches the ID of the current Android device
                    if (linkedAndroidDeviceID != null && linkedAndroidDeviceID == newAndroidDeviceID)
                    {
                        // Android device ID is already linked to the player's account, so there's nothing more to do
                        return;
                    }

                    // If the linked device ID doesn't match the ID of the current Android device, we need to update the linked device
                    var request = new LinkAndroidDeviceIDRequest
                    {
                        AndroidDeviceId = newAndroidDeviceID
                    };

                    // Call the LinkAndroidDeviceID API to update the linked Android device ID
                    PlayFabClientAPI.LinkAndroidDeviceID(request, OnAndroidUpdateSuccess, OnError);
                }
                else
                {
                    // If there is no Android device linked to the account, we need to link the current device
                    var request = new LinkAndroidDeviceIDRequest
                    {
                        AndroidDeviceId = newAndroidDeviceID
                    };

                    // Call the LinkAndroidDeviceID API to link the current Android device to the player's account
                    PlayFabClientAPI.LinkAndroidDeviceID(request, OnAndroidUpdateSuccess, OnError);
                }
            }, OnError);
        }
    }


    private void OnAndroidUpdateSuccess(LinkAndroidDeviceIDResult result)
    {
        // The Android device ID has been linked or updated successfully
        // You can add your own code here to handle the success case
        Debug.Log("Android device ID linked or updated successfully!");
    }


    #region checkVersion
    private void InitializeVersionCheck()
    {
        gettingPlayerDataText.SetActive(false);
        checkingVersionText.SetActive(true);

        currentGameVersion = Application.version;

        if (PlayFabSettings.TitleId == null)
        {
            PlayFabSettings.TitleId = "91E52"; // Replace with your actual PlayFab TitleId
        }

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            CheckGameVersion();
        }
        else
        {
            Debug.LogError("Player is not logged in.");
            // Implement your login logic here
        }
    }

    private void CheckGameVersion()
    {
        var request = new GetTitleDataRequest
        {
            Keys = new List<string> { "GameVersion" }
        };

        PlayFabClientAPI.GetTitleData(request, OnGetTitleDataSuccess, OnGetTitleDataError);
    }

    private void OnGetTitleDataSuccess(GetTitleDataResult result)
    {
        if (result.Data.ContainsKey("GameVersion"))
        {
            string latestGameVersion = result.Data["GameVersion"];

            if (currentGameVersion != latestGameVersion)
            {
                //the game is not uptodate
                updateVersionMessage.SetActive(true);
                // Implement additional logic to notify the player or prevent gameplay
            }
            else
            {
                //the game is uptodate
                updateVersionMessage.SetActive(false);
                gettingPlayerDataText.SetActive(false);
                checkingVersionText.SetActive(false);
                playButton.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("GameVersion not found in TitleData.");
        }
    }

    private void OnGetTitleDataError(PlayFabError error)
    {
        Debug.LogError("Error fetching TitleData: " + error.GenerateErrorReport());
    }
    #endregion



    public void openPlaystore()
    {
        Application.OpenURL(url);
    }

}






