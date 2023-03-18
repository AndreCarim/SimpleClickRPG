using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInitializer : MonoBehaviour
{
    private bool isFirstTime;
    [SerializeField] private GameObject tutorialObject;



    
    void Start()
    {
        tutorialObject.SetActive(true);
    }
}
