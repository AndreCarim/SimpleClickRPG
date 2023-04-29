using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPlayFab : MonoBehaviour
{
    /*
    *player can login with email and password and username or
    *the player can login anonymous which takes his phone id
    *
    *if he goes with anonymous, he will have an option to create an account latter in the game
    *if he goes with email and password, everytime he logs in he will delete
    *the previus phone and add the new one. 
    *
    *at the start, it will check if the player has a account with a phone id
    */



    [Header("Sellection Buttons")]
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject createAccountPage;
    [SerializeField] private GameObject signInPage;
    [SerializeField] private GameObject resetPage;
    [SerializeField] private GameObject playButton;

    [Header("loadData")]
    [SerializeField] private LoadData loadData;

    [SerializeField] private GameObject loadingText;

    public TextMeshProUGUI messageText;

    [Header("UI signInPage")]
    public InputField emailSignInInput;
    public InputField passwordSignInInput;

    [Header("UI createAccount")]
    public InputField emailCreateAccountInput;
    public InputField passwordCreateAccountInput;
    public InputField usernameCreateAccountInput;

    [Header("UI resetPassword")]
    public InputField emailResetPassword;

    private bool anonymoysIsTrying;
    private string displayNamePrefix = "Anonymous";
    private string randomDisplayName;



    void Start()
    {
        //check if the player has an account linkid with his android ip
        checkIfThePlayerHasAnAccountAlready();

    }



    //register / login / resetPassword / anonymous
    #region register
    public void RegisterButton() //register button call
    {
        NetworkReachability reachability = Application.internetReachability;
        if (reachability == NetworkReachability.NotReachable)
        {
            messageText.text = "Check your internet connection";
            return;//checking if the player has internet.
        }

        if (passwordCreateAccountInput.text.Length < 6)
        {
            messageText.text = "Password too short!";
            return;
        }

        if(usernameCreateAccountInput.text.Length < 3)
        {
            messageText.text = "UserName too short!";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = emailCreateAccountInput.text,
            Password = passwordCreateAccountInput.text,
            Username = usernameCreateAccountInput.text
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess,OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Logged in";

        signInPage.SetActive(false);
        buttons.SetActive(false);
        createAccountPage.SetActive(false);
        SubmitName(usernameCreateAccountInput.text);

        

        loadData.GetPlayerData();
        
    }
    #endregion


    #region login
    public void LoginButton() // login button call
    {
        NetworkReachability reachability = Application.internetReachability;
        if (reachability == NetworkReachability.NotReachable)
        {
            messageText.text = "Check your internet connection";
            return;//checking if the player has internet.
        }
        
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailSignInInput.text,
            Password = passwordSignInInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create");
        signInPage.SetActive(false);
        createAccountPage.SetActive(false);
        resetPage.SetActive(false);
        buttons.SetActive(false);
        

        

        loadData.GetPlayerData();
        
    }
    #endregion


    #region resetPassword
    public void ResetPasswordButton() // reset password call
    {
        NetworkReachability reachability = Application.internetReachability;
        if (reachability == NetworkReachability.NotReachable)
        {
            messageText.text = "Check your internet connection";
            return;//checking if the player has internet.
        }

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailResetPassword.text,
            TitleId = "91E52"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset mail sent!";
    }
    #endregion


    #region anonymous
    public void LoginWithAndroidDeviceAnonymous()
    {

        if(anonymoysIsTrying == false)
        {
            anonymoysIsTrying = true;
            var request = new LoginWithAndroidDeviceIDRequest
            {
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };

            PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccessAnonymous, OnLoginFailure);
        }
        
    }

    private void OnLoginSuccessAnonymous(LoginResult result)
    {
        Debug.Log("Logged in successfully!");

        if (result.InfoResultPayload.PlayerProfile == null || result.InfoResultPayload.PlayerProfile.DisplayName == null)
        {
            SetRandomDisplayName();
        }


        signInPage.SetActive(false);
        createAccountPage.SetActive(false);
        resetPage.SetActive(false);
        buttons.SetActive(false);

        

        loadData.GetPlayerData();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError($"Error logging in: {error.ErrorMessage}");
    }

    private void SetRandomDisplayName()
    {
        randomDisplayName = GenerateRandomDisplayName();

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = randomDisplayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSet, OnDisplayNameError);
    }

    private void OnDisplayNameSet(UpdateUserTitleDisplayNameResult result)
    {
        
    }

    private void OnDisplayNameError(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.NameNotAvailable)
        {
            SetRandomDisplayName();
        }
        else
        {
            Debug.LogError($"Error setting display name: {error.ErrorMessage}");
        }
    }

    private string GenerateRandomDisplayName()
    {
        int randomNum = Random.Range(0, 10000000);
        return $"{displayNamePrefix}{randomNum.ToString("D7")}";
    }
    #endregion





    #region autoLogin
    private void checkIfThePlayerHasAnAccountAlready()
    {
        signInPage.SetActive(false);
        createAccountPage.SetActive(false);
        resetPage.SetActive(false);
        buttons.SetActive(false);
        loadingText.SetActive(true);

        var request = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = false,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnAutoLoginSuccess, OnAutoLoginFail);
    }

    private void OnAutoLoginSuccess(LoginResult result)
    {
        Debug.Log("Logged in successfully!");
        

        signInPage.SetActive(false);
        createAccountPage.SetActive(false);
        resetPage.SetActive(false);
        buttons.SetActive(false);

        loadData.GetPlayerData();
    }

    private void OnAutoLoginFail(PlayFabError error)
    { 

        buttons.SetActive(true);
        loadingText.SetActive(false);
    }


    #endregion




    

    //the displayName
    public void SubmitName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated DisplayName!");
    }
    

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;

        if(error.ErrorMessage == "Cannot resolve destination host")
        {
            messageText.text = "Check your internet connection";
        }
        Debug.Log(error.GenerateErrorReport());
    }




    public void openSignInPage()
    {
        signInPage.SetActive(true);
        buttons.SetActive(false);
        createAccountPage.SetActive(false);
        resetPage.SetActive(false);
        playButton.SetActive(false);
        messageText.text = "";
    }

    public void openCreateAccountPage()
    {
        signInPage.SetActive(false);
        buttons.SetActive(false);
        createAccountPage.SetActive(true);
        resetPage.SetActive(false);
        playButton.SetActive(false);
        messageText.text = "";
    }

    public void closeCreateAndSign()
    {
        signInPage.SetActive(false);
        buttons.SetActive(true);
        createAccountPage.SetActive(false);
        resetPage.SetActive(false);
        playButton.SetActive(false);
        messageText.text = "";
    }


    public void openResetPassword()
    {
        signInPage.SetActive(false);
        buttons.SetActive(false);
        createAccountPage.SetActive(false);
        resetPage.SetActive(true);
        playButton.SetActive(false);
        messageText.text = "";
    }

    
}
