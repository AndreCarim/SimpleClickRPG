using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{

    private AudioSource audioSource;

    

    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject nextButton;
    
    [SerializeField] private GameObject[] phases;



    //timer
    private float currentTime;
    private float timePerPhase; // amount of seconds that the player is alowed to click next

    private bool canPassPhase;
    private int currentPhaseIndex;
    private bool isFirstTime;


    void Start()
    {
        load();
        checkIsFirstTime();

        audioSource = gameObject.GetComponent<AudioSource>();

        timePerPhase = 4f; //5 seconds
        currentTime = timePerPhase;
        canPassPhase = false;
        currentPhaseIndex = 1;

        
        
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
        if (currentTime <= 0)
        { //check if the timer is done

            
            
            nextButtonShow();


        }

    }

    private void checkIsFirstTime()
    {
        if(isFirstTime == true)
        {
            isFirstTime = false; //if it is the first time, change to false and save, so the next time it wont show
            save();
        }
        else
        {
            endTutorial();
        }
    }



    //show the next button and allow the player to click to next phase anywhere
    private void nextButtonShow()
    {
        nextButton.SetActive(true);
        canPassPhase = true;
    }


    public void nextPhaseButton()
    {
        if (canPassPhase)
        {
            if(currentPhaseIndex == phases.Length) 
            { 
                endTutorial();
                return ;
            } //ending the tutorial if the current phase is the max phase


            if (currentPhaseIndex == phases.Length - 1) //if its the last phase, it will show next button in 2 seconds
            {
                currentTime = 2f;
            }
            else
            {
                currentTime = timePerPhase;
            }

            phases[currentPhaseIndex - 1].SetActive(false);//set the last phase inactive
            phases[currentPhaseIndex].SetActive(true); //set the next one active

            nextButton.SetActive(false); //turn off the next button

            currentPhaseIndex++; //increase the index phase
            canPassPhase = false; // the player cant click

            audioSource.Play();
            
            
        }
    }


    private void endTutorial()
    {
        gameObject.SetActive(false);
    }


    private void save()
    {
        ES3.Save("isFirstTime", isFirstTime);
    }

    private void load()
    {
        isFirstTime = ES3.Load("isFirstTime", true);
    }

}
