using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckInternet : MonoBehaviour
{
    [SerializeField] private GameObject noInternetObject;

    

    void Start()
    {
        StartCoroutine(CheckForInternet());
    }

    IEnumerator CheckForInternet()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // check every 2 seconds

            NetworkReachability reachability = Application.internetReachability;
            if (reachability == NetworkReachability.NotReachable)
            {
                SceneManager.LoadScene(0); // 0 is the main menu, 1 is the game;
            }
            else
            {
                Debug.Log("Player has internet");
            }
        }
    }
   

    
}
