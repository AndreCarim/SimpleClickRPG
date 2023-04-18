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

    private SlotHandler slotSelected;
    private SlotHandler equippedSlot;

    [SerializeField] private GameObject petInfo;
    [SerializeField] private Image infoImage;
    [SerializeField] private TextMeshProUGUI infoName;
    [SerializeField] private TextMeshProUGUI infoRarity;
    [SerializeField] private TextMeshProUGUI infoBonus;
    [SerializeField] private TextMeshProUGUI infoLevel;
    [SerializeField] private TextMeshProUGUI upgradePrice;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private GameObject heartIcon;
    [SerializeField] private GameObject strengthIcon;
    [SerializeField] private GameObject backPackIcon;
    [SerializeField] private GameObject gemIcon;
    [SerializeField] private GameObject goldIcon;
    [SerializeField] private MagicHandler magicHandler;

    [SerializeField] private TabSwitcher tabSwitcher;
    

    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unequipButton;

    private Color commonColor = new Color(.7f, 1f, .34f);
    private Color rareColor = new Color(1f, .20f, .22f);
    private Color epicColor = new Color(0.98f, .33f, 1f);

    [SerializeField] private Animator animatorEgg; //this will keep the animation of the bush running
    [SerializeField] private Animator animatorPet;
    



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



    public void openPetsPlayerOwnMenu()
    {
        //cleaning
        clean();
  

        
        int indexCountCommon = 0;
        int indexCountRare = 0;
        int indexCountEpic = 0;

        foreach (Pet pet in petsPlayerOwn.getPetsPlayerOwn())
        {
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

        //handles the equipped icon
        if (equippedSlot)
        {
            equippedSlot.slotGotEquipped();
            equippedSlot.slotGotSelected();
            setPetInfo(equippedSlot.getPetInSlot());
            unequipButton.SetActive(true);
            slotSelected = equippedSlot;
        }



        petsOwnedMenu.SetActive(true);
        soundHandler.openBottomMenusSoundHandler();
        pauseGame();
    }

    //click equip
    public void setNewEquippedPet()
    {
        //setting the select surrouding


        petsPlayerOwn.setEquippedPet(slotSelected.getPetInSlot()); //equipping new pet

        equipButton.SetActive(false); //closing the equip button so the player cant keep pressing
        unequipButton.SetActive(true);

        soundHandler.clickSoundHandler();

        //handles the equipped icon;
        slotSelected.slotGotEquipped();
        if (equippedSlot) { equippedSlot.slotGotUnequipped(); } //check if it is the first equip of the day
        equippedSlot = slotSelected;
    }


    //unequip the pet
    public void unequipPet()
    {
        if (equippedSlot)
        {
            equippedSlot.slotGotUnequipped();
            equippedSlot.slotGotUnselected();
            equipButton.SetActive(false);
            unequipButton.SetActive(false);
            petsPlayerOwn.unequipPet();
            equippedSlot = null;
            slotSelected = null;
            petInfo.SetActive(false);

        }
    }


    



    public void closePetsPlayerOwnMenu()
    {

        
        petsOwnedMenu.SetActive(false);


        soundHandler.clickSoundHandler();
        tabSwitcher.closePetMenuClick();
        resumeGame();
    }


    //Handles the pet upgrade
    public void upgradePetButton()
    {
        if (slotSelected)
        {
            petsPlayerOwn.upgradePet(slotSelected.getPetInSlot());
            setPetInfo(slotSelected.getPetInSlot());
        }
    }



    //this will be called from the slotHandler since any one of the slots can call;
    public void setSelectedPet(GameObject slot)
    {
        clean();// clean everything
        SlotHandler tempSlot = slot.GetComponent<SlotHandler>();

        //handling the select and equipped icon
        if (slotSelected) { slotSelected.slotGotUnselected(); }
        tempSlot.slotGotSelected(); // adding the select icon
        if (equippedSlot) { equippedSlot.slotGotEquipped(); }

        slotSelected = tempSlot; //so we can know which slot is selected
    

        //if the pet selected is the one he have equipped already, leave the equipp button closed
        if(slotSelected.getPetInSlot() != petsPlayerOwn.getEquippedPet()) 
        { 
            equipButton.SetActive(true); 
        }else if(slotSelected.getPetInSlot() == petsPlayerOwn.getEquippedPet()) 
        { 
            //the pet is the same
            unequipButton.SetActive(true); 
        }

        setPetInfo(slotSelected.getPetInSlot());

        soundHandler.clickSoundHandler();
    }


    private void setPetInfo(Pet pet)
    {
       

        petInfo.SetActive(true);

        //handles info when player selects
        infoName.text = pet.petName;
        infoImage.sprite = pet.sprite;
        infoLevel.text = "LVL " + pet.level;
        animatorPet.runtimeAnimatorController = pet.animator;

        //handles the upgradePrice`common
        //pet level 0 > 1 == 50 magic
        //pet level 1 > 2 == 75 magic
        //pet level 2 > 3 == 100 magic
        //pet level 3 > 4 == 150 magic
        //pet level 4 > 5 == 200 magic

        //handles the upgradePrice`rare
        //pet level 0 > 1 == 75 magic
        //pet level 1 > 2 == 125 magic
        //pet level 2 > 3 == 175 magic
        //pet level 3 > 4 == 225 magic
        //pet level 4 > 5 == 300 magic

        //handles the upgradePrice`epic
        //pet level 0 > 1 == 100 magic
        //pet level 1 > 2 == 150 magic
        //pet level 2 > 3 == 200 magic
        //pet level 3 > 4 == 250 magic
        //pet level 4 > 5 == 350 magic
        if (pet.rarity == Pet.Rarity.Common)
        {
            switch (pet.level)
            {
                case 0:
                    upgradePrice.text = "50";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 50) { upgradeButton.interactable = false; }
                    break;
                case 1:
                    upgradePrice.text = "75";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 75) { upgradeButton.interactable = false; }
                    break;
                case 2:
                    upgradePrice.text = "100";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 100) { upgradeButton.interactable = false; }
                    break;
                case 3:
                    upgradePrice.text = "150";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 150) { upgradeButton.interactable = false; }
                    break;
                case 4:
                    upgradePrice.text = "200";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 200) { upgradeButton.interactable = false; }
                    break;
                case 5:
                    upgradePrice.text = "MAX";
                    upgradeButton.interactable = false;
                    break;
            }
        }else if(pet.rarity == Pet.Rarity.Rare)
        {
            switch (pet.level)
            {
                case 0:
                    upgradePrice.text = "75";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 75) { upgradeButton.interactable = false; }
                    break;
                case 1:
                    upgradePrice.text = "125";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 125) { upgradeButton.interactable = false; }
                    break;
                case 2:
                    upgradePrice.text = "175";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 175) { upgradeButton.interactable = false; }
                    break;
                case 3:
                    upgradePrice.text = "225";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 225) { upgradeButton.interactable = false; }
                    break;
                case 4:
                    upgradePrice.text = "300";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 300) { upgradeButton.interactable = false; }
                    break;
                case 5:
                    upgradePrice.text = "MAX";
                    upgradeButton.interactable = false;
                    break;
            }
        }else if(pet.rarity == Pet.Rarity.Epic)
        {
            switch (pet.level)
            {
                case 0:
                    upgradePrice.text = "100";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 100) { upgradeButton.interactable = false; }
                    break;
                case 1:
                    upgradePrice.text = "150";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 150) { upgradeButton.interactable = false; }
                    break;
                case 2:
                    upgradePrice.text = "200";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 200) { upgradeButton.interactable = false; }
                    break;
                case 3:
                    upgradePrice.text = "250";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 250) { upgradeButton.interactable = false; }
                    break;
                case 4:
                    upgradePrice.text = "350";
                    upgradeButton.interactable = true;
                    if (magicHandler.getCurrentAmountOfMagic() < 350) { upgradeButton.interactable = false; }
                    break;
                case 5:
                    upgradePrice.text = "MAX";
                    upgradeButton.interactable = false;
                    break;
            }
        }
        

        


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
                infoBonus.text = (pet.bonusAmountDouble * 100) + "%";
                break;
            case Pet.Bonus.MaxHp:
                //hpandRecovery percentage
                heartIcon.SetActive(true);
                infoBonus.text = (pet.bonusAmountDouble * 100) + "%";
                break;
            case Pet.Bonus.Gold:
                //gold percentage
                goldIcon.SetActive(true);
                infoBonus.text = (pet.bonusAmountDouble * 100) + "%";
                break;
            case Pet.Bonus.Backpack:
                //unit of backpack
                backPackIcon.SetActive(true);
                infoBonus.text = pet.bonusAmountInt.ToString();
                break;
            case Pet.Bonus.Gems:
                //gems bonus
                gemIcon.SetActive(true);
                infoBonus.text = pet.bonusAmountDouble.ToString();
                break;
        }
    }


    private void clean()
    {
        
        petInfo.SetActive(false);

        if (slotSelected) { slotSelected.slotGotUnselected(); }
        if (equippedSlot) { equippedSlot.slotGotUnequipped(); }
        if (equippedSlot) { equippedSlot.slotGotUnselected(); }

        slotSelected = null;
        backPackIcon.SetActive(false);
        heartIcon.SetActive(false);
        gemIcon.SetActive(false);
        goldIcon.SetActive(false);
        strengthIcon.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        upgradeButton.interactable = false;
    }


    void pauseGame()
    {
        Time.timeScale = 0;

        animatorEgg.updateMode = AnimatorUpdateMode.UnscaledTime;
        animatorPet.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void resumeGame()
    {
        Time.timeScale = 1;

        animatorEgg.updateMode = AnimatorUpdateMode.Normal;
        animatorPet.updateMode = AnimatorUpdateMode.Normal;
    }


    public void save()
    {
        ES3.Save("equippedSlot", equippedSlot);
    }

    public void load()
    {
        if (ES3.KeyExists("equippedSlot"))
            equippedSlot = ES3.Load<SlotHandler>("equippedSlot");
    }

}
