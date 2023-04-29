using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateAccountInGame : MonoBehaviour
{
    //this script will handle the case that the player loggs in anonymous and wants to create an account


    [Header("UI createAccount")]
    public InputField emailCreateAccountInput;
    public InputField passwordCreateAccountInput;
    public InputField usernameCreateAccountInput;

    [Header("UI GameObjects Menus")]
    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject createAccountMenu;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI message;

    private bool isTryingToLink;

    public void LinkEmailAndPassword()
    {
        NetworkReachability reachability = Application.internetReachability;
        if (reachability == NetworkReachability.NotReachable)
        {
            message.text = "Check your internet connection";
            return;//checking if the player has internet.
        }

        if (passwordCreateAccountInput.text.Length < 6)
        {
            message.text = "Password too short!";
            return;
        }

        if (usernameCreateAccountInput.text.Length < 3)
        {
            message.text = "UserName too short!";
            return;
        }

        if(isTryingToLink == false)
        {
            isTryingToLink = true;

            if (emailCreateAccountInput != null && passwordCreateAccountInput != null && usernameCreateAccountInput != null)
            {
                var request = new AddUsernamePasswordRequest
                {
                    Email = emailCreateAccountInput.text,
                    Password = passwordCreateAccountInput.text,
                    Username = usernameCreateAccountInput.text
                };

                PlayFabClientAPI.AddUsernamePassword(request, OnEmailAndPasswordLinked, OnEmailAndPasswordLinkFailed);
            }
            else
            {
                message.text = "One or more input fields are null.";
            }
        }

        
        
    }

    private void OnEmailAndPasswordLinked(AddUsernamePasswordResult result)
    {
        Debug.Log("Email and password linked successfully!");

        inGameMenu.SetActive(false);
        createAccountMenu.SetActive(false);
        SubmitName(usernameCreateAccountInput.text);

        Time.timeScale = 1;
    }

    private void OnEmailAndPasswordLinkFailed(PlayFabError error)
    {
        message.text = error.ErrorMessage;
        isTryingToLink = false;
    }

    public void SubmitName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnEmailAndPasswordLinkFailed);
    }

    public void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Display name updated");
    }


    public void closeWindow()
    {
        createAccountMenu.SetActive(false);
    }


    public void logOut()
    {
        //this will simply go back to the main menu

    }
}
