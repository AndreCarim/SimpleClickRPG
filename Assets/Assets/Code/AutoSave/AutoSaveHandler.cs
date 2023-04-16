using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveHandler : MonoBehaviour
{
    [SerializeField] private GoldHandler goldHandler;
    [SerializeField] private BackPackHandler backPackHandler;
    [SerializeField] private AutoClickHandler autoClickHandler;
    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private PlayerHealthHandler playerHealthHandler;
    [SerializeField] private TutorialHandler tutorialHandler;
    [SerializeField] private StrengthHandler strengthHandler;
    [SerializeField] private PetsHandler petsHandler;
    [SerializeField] private PetsPlayerOwn petsPlayerOwn;
    [SerializeField] private ShowPetsOwned showPetsOwned;

    [SerializeField] private GameObject autoSaveText;

    private float currentTime;
    private float saveEveryXSeconds;

    private float autoSaveTextTime;
    private bool isTextOn;


    void Start()
    {
        saveEveryXSeconds = 120f;
        currentTime = saveEveryXSeconds;
    }

    //TIMER
    void FixedUpdate()
    {

        currentTime -= 1 * Time.deltaTime; // decreasing the amount of seconds inside the currentTime(starts at 1)
        autoSaveTextTime -= 1 * Time.deltaTime;

        if (currentTime <= 0)
        { //check if the timer is done

            currentTime = saveEveryXSeconds; //set the current time back to 20

            autoSaveTextTime = 3f;


            save();
        }

        if(autoSaveTextTime > 0 && isTextOn == false)
        {
            isTextOn = true;
            autoSaveText.SetActive(true);
        }else if(autoSaveTextTime < 0 && isTextOn) {
            isTextOn = false;
            autoSaveText.SetActive(false);
        }
        
        

    }


    public void save()//save all the game
    {
        //need to add everything I want to save

        //Gold
        goldHandler.save();

        //backPack
        backPackHandler.save();

        //AutoClick
        autoClickHandler.save();

        //EnemyHandler
        enemyHandler.save();

        //player health
        playerHealthHandler.save();

        //Tutorial
        tutorialHandler.save();

        //player strength
        strengthHandler.save();

        //pets
        petsHandler.save();
        petsPlayerOwn.save();
        showPetsOwned.save();
    }
}
