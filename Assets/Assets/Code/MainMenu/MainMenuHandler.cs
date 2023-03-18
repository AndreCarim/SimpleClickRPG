using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] AudioSource playClickAudioSource;
    
    private int developerEasterEggCount = 0;
    [SerializeField] GameObject developerObject;

    public void playClickButton()
    {
        SceneManager.LoadScene(1); // 0 is the main menu, 1 is the game;
        playClickAudioSource.Play();
    }


    //if the person clicks 10 times on the logo
    public void developersEasterEgg()
    {
        if(developerEasterEggCount == 10)
        {
            developerObject.SetActive(true);
        }
        else
        {
            developerEasterEggCount++;
        }
    }
}
