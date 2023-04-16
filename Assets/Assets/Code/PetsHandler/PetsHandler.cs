using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Handles the pet Encouter
public class PetsHandler : MonoBehaviour
{
    [SerializeField] private Pet[] comumPets;
    [SerializeField] private Pet[] rarePets;
    [SerializeField] private Pet[] epicPets;

    


    [SerializeField] private EnemyHandler enemyHandler;
    [SerializeField] private GemHandler gemHandler;
    [SerializeField] private MagicHandler magicHandler;
    [SerializeField] private PetsPlayerOwn petsPlayerOwn;
    [SerializeField] private SoundHandler soundHandler;
    
    [SerializeField] private AutoSaveHandler autoSaveHandler;//so it saves the game 

    
    
    //encounter Handler
    [SerializeField] private Animator animatorBush; //this will keep the animation of the bush running
    [SerializeField] private Animator animatorButton; //this will keep the animation of the catch button running
    [SerializeField] private Animator animatorPet;
    [SerializeField] private Animator animatorOkButton;
    [SerializeField] private Animator animatorShiny;


    [SerializeField] private GameObject petShow;
    [SerializeField] private GameObject bush;

    [SerializeField] private GameObject petEncounterMenu; //this is where everthing will happen;
    [SerializeField] private GameObject PetEncounterUI; //when the player chooses any option, hide the buttons
    [SerializeField] private GameObject questionMenu;
    [SerializeField] private TextMeshProUGUI nameAndRarityText;
    [SerializeField] private Button catchButtonUI;
    [SerializeField] private GameObject petRanAwayText;
    [SerializeField] private GameObject petCatchedText;
    [SerializeField] private GameObject petDuplicateText;
    
    [SerializeField] private TextMeshProUGUI petNameAndRarity;
    [SerializeField] private GameObject okButton;
    [SerializeField] private Image petImage;

    [SerializeField] private TextMeshProUGUI gemCurrentAmount;


    private double findAPetEveryXAmountOfKills; //this will be the handler to get the amount of kills you will need between one find and another;
    private double nextPetEncounter; //the current kill + the findAPetEveryXAmountOfKills

    private Pet petFound;
    //encounter Handler


    void Start()
    {
        load();
    }

    void OnApplicationPause(bool stats)
    {
        if (stats == true)
        {
            save();
        }
    }






    //petEncounter //petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter
    //petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter
    //petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter
    public void checkIfEncounterAPet(double amountOfKills)
    {
        //this will be called from the enemyHandler script
        //when the player kills any enemy (boss or normal enemies)

        

        if(amountOfKills >= nextPetEncounter)
        {
            //now, we are going to update the next pet encouter
            nextPetEncounter = UnityEngine.Random.Range(5,7) + amountOfKills;

            //the player encoutered a new pet
            petEncouter();
        }
    }


    private void petEncouter()
    {
        //cleaning everything
        autoSaveHandler.save(); //save the new amount

        handlesOppening();//will close and open everything it needs
        //cleaning everything

        petFound = choosePet();//getting the pet

        soundHandler.petFoundSoundHandle();
        pauseGame();
    }


    /*
     * 
     * it will randomly choose a pet and return it
     */
    private Pet choosePet()
    {
        int rarityRoll = UnityEngine.Random.Range(1, 101); // gera um número aleatório entre 1 e 100 para determinar a raridade do pet
        
        
        if (rarityRoll <= 70) // 70% de chance de ser um pet comum
        {
            int petRollComun = UnityEngine.Random.Range(0, comumPets.Length); // gera um número aleatório entre  para determinar o pet na lista
            return comumPets[petRollComun];
        }
        else if (rarityRoll <= 99) // 29% de chance de ser um pet raro
        {
            int petRollRare = UnityEngine.Random.Range(0, rarePets.Length);
            return rarePets[petRollRare];
        }
        else // 1% de chance de ser um pet épico
        {
            int petRollEpic = UnityEngine.Random.Range(0, epicPets.Length);
            return epicPets[petRollEpic];
        }
   
    }



    public void catchButton()
    {
        if (gemHandler.getCurrentAmountOfGem() >= 90)
        {
            //the player clicked to catch the pet
            finishPetEncounter();
            gemHandler.decreaseAmountOfGem(90);
            checkIfPlayerAlreadyHaveThePetFound(); //catch the pet


            soundHandler.clickSoundHandler();
        }
        
    }

    


    private void checkIfPlayerAlreadyHaveThePetFound()
    {
        //checking if the player has the pet or not
        foreach (Pet pet in petsPlayerOwn.getPetsPlayerOwn())
        {
            if (pet.petId == petFound.petId)
            {
                playerAlreadyHaveThePetFound();
                return;
            }
        }

        playerDontHaveThePetFound();
    }


    private void playerDontHaveThePetFound()
    {
        //new pet
        petsPlayerOwn.setPetsPlayerOwn(Instantiate(petFound));//add a copy of pet to the petsPlayerOwnList

        petCatchedText.SetActive(true);//set the text
    }

    private void playerAlreadyHaveThePetFound()
    {
        //duplicate pet
        magicHandler.increaseAmountOfMagic(35);
        petDuplicateText.SetActive(true);
    }



    public void leaveButton()
    {
        //the player clicked to leave the pet alone


        finishPetEncounter();
        petRanAwayText.SetActive(true);

        soundHandler.clickSoundHandler();
    }


    public void questionOpenButton()
    {
        questionMenu.SetActive(true);

        soundHandler.clickSoundHandler();
    }

    public void QuestionCloseMenu()
    {
        questionMenu.SetActive(false);

        soundHandler.clickSoundHandler();
    }





    private void finishPetEncounter()
    {
        //once the player clicks on either the catch or leave button.
        //all the handling with the showing of the pet will be here
        PetEncounterUI.SetActive(false);

        //handles pet information
        petImage.sprite = petFound.sprite; //setting the image to show the pet found
        animatorPet.runtimeAnimatorController = petFound.animator;
        petNameAndRarity.text = petFound.petName + " - " + petFound.rarity;

        //once the animation finishs, it will triger the function inside the bush script

        animatorBush.SetTrigger("ShowPetAnimation");
        
    }

    public void okButtonClose()
    {
        resumeGame();
        petEncounterMenu.SetActive(false);

        soundHandler.clickSoundHandler();
    }



    private void handlesOppening()
    {
        petEncounterMenu.SetActive(true); // setting the main page true
        petFound = null;
        PetEncounterUI.SetActive(true);
        bush.SetActive(true);
        petDuplicateText.SetActive(false);
        

        

        petShow.SetActive(false);
        okButton.SetActive(false);

        nameAndRarityText.text = "";
        petCatchedText.SetActive(false);
        petRanAwayText.SetActive(false);

        gemCurrentAmount.text = NumberAbrev.ParseDouble(gemHandler.getCurrentAmountOfGem());

        if(gemHandler.getCurrentAmountOfGem() >= 90)
        {
            catchButtonUI.interactable = true;
        }
        else
        {
            catchButtonUI.interactable = false;
        }
    }

    //petEncounter //petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter
    //petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter
    //petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter//petEncounter


    void pauseGame()
    {
        Time.timeScale = 0;


        // Continue playing the animation while the game is paused
        animatorBush.updateMode = AnimatorUpdateMode.UnscaledTime;
        animatorButton.updateMode = AnimatorUpdateMode.UnscaledTime;


    }

    void resumeGame()
    {
        Time.timeScale = 1;


        // Reset the update mode to normal
        animatorBush.updateMode = AnimatorUpdateMode.Normal;
        animatorButton.updateMode = AnimatorUpdateMode.Normal;
        animatorPet.updateMode = AnimatorUpdateMode.Normal;
        animatorOkButton.updateMode = AnimatorUpdateMode.Normal;
        animatorOkButton.updateMode = AnimatorUpdateMode.Normal;
    }
   



    public void save()
    {
        ES3.Save("nextPetEncounter", nextPetEncounter);
        ES3.Save("findAPetEveryXAmountOfKills", findAPetEveryXAmountOfKills);
    }

    private void load()
    {
        findAPetEveryXAmountOfKills = ES3.Load<double>("findAPetEveryXAmountOfKills", UnityEngine.Random.Range(5, 100));
        nextPetEncounter = ES3.Load<double>("nextPetEncounter", findAPetEveryXAmountOfKills);
    }




    


}
