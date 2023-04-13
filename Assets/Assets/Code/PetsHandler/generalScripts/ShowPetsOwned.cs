using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowPetsOwned : MonoBehaviour
{
    [SerializeField] private PetsPlayerOwn petsPlayerOwn;

    [SerializeField] private GameObject petsOwnedMenu;

    [SerializeField] private SoundHandler soundHandler;


    [SerializeField] private SlotHandler[] comunPets; //slots
    [SerializeField] private SlotHandler[] rarePets; //slots
    [SerializeField] private SlotHandler[] epicPets; //slots

    [SerializeField] private GameObject petInfo;
    [SerializeField] private Image infoImage;
    [SerializeField] private TextMeshProUGUI infoName;
    [SerializeField] private TextMeshProUGUI infoRarity;
    [SerializeField] private TextMeshProUGUI infoBonus;
    [SerializeField] private TextMeshProUGUI infoDescription;
    [SerializeField] private TextMeshProUGUI infoLevel;

    [SerializeField] private GameObject heartIcon;
    [SerializeField] private GameObject strengthIcon;
    [SerializeField] private GameObject backPackIcon;
    [SerializeField] private GameObject gemIcon;
    [SerializeField] private GameObject goldIcon;

    [SerializeField] private GameObject equipButton; 

    private Color commonColor = new Color(.7f, 1f, .34f);
    private Color rareColor = new Color(1f, .20f, .22f);
    private Color epicColor = new Color(0.98f, .33f, 1f);

    [SerializeField] private Animator animatorEgg; //this will keep the animation of the bush running


    private Pet selectedPet; //when the player selects a pet


    

    public void openPetsPlayerOwnMenu()
    {
        //cleaning
        clean();
        

        List<Pet> currentPetsOwned = petsPlayerOwn.getPetsPlayerOwn();
        int indexCountCommon = 0;
        int indexCountRare = 0;
        int indexCountEpic = 0;

        foreach (Pet pet in currentPetsOwned){
            switch (pet.rarity) // it will check which type of rarity the pet is
            {
                case Pet.Rarity.Common:
                    //common
                    comunPets[indexCountCommon].setPetInSlot(pet);
                    indexCountCommon++;
                    break;
                case Pet.Rarity.Rare:
                    rarePets[indexCountRare].setPetInSlot(pet);
                    indexCountRare++;
                    //rare
                    break;
                case Pet.Rarity.Epic:
                    //epic
                    epicPets[indexCountEpic].setPetInSlot(pet);
                    indexCountEpic++;
                    break;
            }
        }



        petsOwnedMenu.SetActive(true);
        soundHandler.clickSoundHandler();
        pauseGame();
    }

    //click equip
    public void setNewEquippedPet()
    {
        petsPlayerOwn.setEquippedPet(selectedPet); //equipping new pet

        equipButton.SetActive(false); //closing the equip button so the player cant keep pressing

        soundHandler.clickSoundHandler();
    }



    public void closePetsPlayerOwnMenu()
    {



        resumeGame();
        petsOwnedMenu.SetActive(false);


        soundHandler.clickSoundHandler();
    }

    public void setSelectedPet(Pet pet)
    {
        //if the pet selected is the one he have equipped already, leave the equipp button closed
        if(pet != petsPlayerOwn.getEquippedPet()) { equipButton.SetActive(true); }

        selectedPet = pet;
        petInfo.SetActive(true);

        //handles info when player selects
        infoName.text = pet.petName;
        infoImage.sprite = pet.sprite;
        infoDescription.text = pet.description;
        infoLevel.text = "LVL " + pet.level;


        switch (pet.rarity)
        {
            case Pet.Rarity.Common:
                infoRarity.text = "Common";
                infoRarity.color = commonColor;
                break;
            case Pet.Rarity.Rare:
                infoRarity.text = "Rare";
                infoRarity.color = rareColor;
                break;
            case Pet.Rarity.Epic:
                infoRarity.text = "Epic";
                infoRarity.color = epicColor;
                break;
        }


        switch (pet.bonus)
        {
            case Pet.Bonus.Damage:
                //damage percentage
                strengthIcon.SetActive(true);
                infoBonus.text = pet.bonusAmount + "%";
                break;
            case Pet.Bonus.HpAndrecovery:
                //hpandRecovery percentage
                heartIcon.SetActive(true);
                infoBonus.text = pet.bonusAmount + "%";
                break;
            case Pet.Bonus.Gold:
                //gold percentage
                goldIcon.SetActive(true);
                infoBonus.text = pet.bonusAmount + "%";
                break;
            case Pet.Bonus.Backpack:
                //unit of backpack
                backPackIcon.SetActive(true);
                infoBonus.text = pet.bonusAmount.ToString();
                break;
            case Pet.Bonus.Gems:
                //gems bonus
                gemIcon.SetActive(true);
                infoBonus.text = pet.bonusAmount.ToString();
                break;
        }



        soundHandler.clickSoundHandler();
    }



    private void clean()
    {
        selectedPet = null;
        petInfo.SetActive(false);


        backPackIcon.SetActive(false);
        heartIcon.SetActive(false);
        gemIcon.SetActive(false);
        goldIcon.SetActive(false);
        strengthIcon.SetActive(false);
        equipButton.SetActive(false);
    }


    void pauseGame()
    {
        Time.timeScale = 0;

        animatorEgg.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void resumeGame()
    {
        Time.timeScale = 1;

        animatorEgg.updateMode = AnimatorUpdateMode.Normal;
    }
}
