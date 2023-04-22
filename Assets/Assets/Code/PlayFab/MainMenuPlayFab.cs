using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPlayFab : MonoBehaviour
{
    [Header("Sellection Buttons")]
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject createAccountPage;
    [SerializeField] private GameObject signInPage;
    [SerializeField] private GameObject resetPage;
    [SerializeField] private GameObject playButton;


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

    //register / login / resetPassword
    public void RegisterButton()
    {
        if(passwordCreateAccountInput.text.Length < 6)
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
        playButton.SetActive(true);
        SubmitName();
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailSignInInput.text,
            Password = passwordSignInInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccess, OnError);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailResetPassword.text,
            TitleId = "91E52"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    //the displayName
    public void SubmitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = usernameCreateAccountInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated DisplayName!");
    }
    
    /*
    void Start()
    {
        login();
    }

    void login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    */


    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset mail sent!";
    }


    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create");
        signInPage.SetActive(false);
        playButton.SetActive(true);
        createAccountPage.SetActive(false);
        resetPage.SetActive(false);
        buttons.SetActive(false);
        messageText.text = "Welcome ";
    }

    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
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
