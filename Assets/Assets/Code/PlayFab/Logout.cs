using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    [SerializeField] private GameObject logoutMenu;
    [SerializeField] private GameObject ingameMenu;

    [SerializeField] private AutoSaveHandler autoSaveHandler;

    public void yesLogoutButton()
    {
        PlayFabClientAPI.UnlinkAndroidDeviceID(new UnlinkAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
        }, OnUnlinkSuccess, OnUnlinkFailure);
    }


    private void OnUnlinkSuccess(UnlinkAndroidDeviceIDResult result)
    {
        autoSaveHandler.save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // 0 is the main menu, 1 is the game;
    }

    private void OnUnlinkFailure(PlayFabError error)
    {
        logoutMenu.SetActive(false);
        ingameMenu.SetActive(false);
        Time.timeScale = 1f;
    }


    public void noLogoutButton()
    {
        logoutMenu.SetActive(false);
        ingameMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
