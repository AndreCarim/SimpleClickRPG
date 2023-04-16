using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PetsPlayerOwn : MonoBehaviour
{
    private List<Pet> petsPlayerOwn = new List<Pet>();

    private Pet equippedPet;

    [SerializeField] private BackPackHandler backPackHandler;
    [SerializeField] private GoldHandler goldHandler;
    [SerializeField] private GemHandler gemHandler;
    [SerializeField] private MagicHandler magicHandler;
    [SerializeField] private StrengthHandler strengthHandler;
    [SerializeField] private PlayerHealthHandler playerHealthHandler;

    [SerializeField] private SoundHandler soundHandler;

    
    //pet level 0 > 1 == 50 magic
    //pet level 1 > 2 == 75 magic
    //pet level 2 > 3 == 100 magic
    //pet level 3 > 4 == 150 magic
    //pet level 4 > 5 == 200 magic




    void Start()
    {
        load();

        if (equippedPet) 
        { 
            equipPet(equippedPet);
            
        }
    }

    void OnApplicationPause(bool stats)
    {
        if (stats == true)
        {
            save();
        }
    }



    //when the player changes the pet equipped
    //this will be called by the showPetsOwned
    public void setEquippedPet(Pet pet)
    {

        //first removes the pet he has right now, removing the bonus the pet gives
        unequipPet();

        //add the new pet and set its bonus
        //it will also check if the player have the pet
        equipPet(pet);
    }


    public void unequipPet()
    {
        if (equippedPet)
        {
            switch (equippedPet.bonus)
            {
                case Pet.Bonus.Damage:
                    //damage percentage
                    strengthHandler.removePetBonusAmount();
                    break;
                case Pet.Bonus.MaxHp:
                    //hpandRecovery percentage
                    playerHealthHandler.removePetBonusAmount();
                    break;
                case Pet.Bonus.Gold:
                    //gold percentage
                    backPackHandler.removePetGoldBonusAmount();
                    break;
                case Pet.Bonus.Backpack:
                    //unit of backpack
                    backPackHandler.removePetBackpackBonusAmount();
                    break;
                case Pet.Bonus.Gems:
                    //gems bonus
                    gemHandler.removePetBonusAmount();
                    break;
            }
            equippedPet = null;
        }
        
    }

    private void equipPet(Pet pet)
    {
        //checking if the player has the pet he is trying to equip
        if (petsPlayerOwn.Contains(pet))
        {
            //changes the pet
            equippedPet = pet;

            switch (equippedPet.bonus)
            {
                case Pet.Bonus.Damage:
                    //damage percentage
                    strengthHandler.setPetBonusAmount(pet.bonusAmountDouble);
                    break;
                case Pet.Bonus.MaxHp:
                    //hpandRecovery percentage
                    playerHealthHandler.setPetBonusAmount(pet.bonusAmountDouble);
                    break;
                case Pet.Bonus.Gold:
                    //gold percentage
                    backPackHandler.setPetGoldBonusAmount(pet.bonusAmountDouble);
                    break;
                case Pet.Bonus.Backpack:
                    //unit of backpack
                    backPackHandler.setPetBackpackBonusAmount(pet.bonusAmountInt);
                    break;
                case Pet.Bonus.Gems:
                    //gems bonus
                    gemHandler.setPetBonusAmount(pet.bonusAmountDouble);
                    break;
            }

        }
    }


    public void upgradePet(Pet pet)
    {
        if (petsPlayerOwn.Contains(pet))
        {
            //handles the upgradePrice
            //pet level 0 > 1 == 50 magic
            //pet level 1 > 2 == 75 magic
            //pet level 2 > 3 == 100 magic
            //pet level 3 > 4 == 150 magic
            //pet level 4 > 5 == 200 magic
            //now, I need to check the rarity and if the player has the magic
            switch (pet.level)
            {
                case 0:
                    upgrade(50, pet);
                    break;
                case 1:
                    upgrade(75, pet);
                    break;
                case 2:
                    upgrade(100, pet);
                    break;
                case 3:
                    upgrade(150, pet);
                    break;
                case 4:
                    upgrade(200, pet);
                    break;
            }
        }
    }

    private void upgrade(double value, Pet pet)
    {
        
        if (magicHandler.getCurrentAmountOfMagic() >= value)
        {
            switch (pet.bonus)
            {
                case Pet.Bonus.Damage:
                    //damage percentage
                    pet.bonusAmountDouble += 0.1;
                    pet.level += 1;
                    break;
                case Pet.Bonus.MaxHp:
                    //hpandRecovery percentage
                    pet.bonusAmountDouble += 0.1;
                    pet.level += 1;
                    break;
                case Pet.Bonus.Gold:
                    //gold percentage
                    pet.bonusAmountDouble += 0.1;
                    pet.level += 1;
                    break;
                case Pet.Bonus.Backpack:
                    //unit of backpack
                    pet.bonusAmountInt = pet.bonusAmountInt + 1;
                    pet.level += 1;
                    break;
                case Pet.Bonus.Gems:
                    //gems bonus
                    pet.bonusAmountDouble += 3;
                    pet.level += 1;
                    break;
            }
            magicHandler.decreaseAmountOfMagic(value);
            if(equippedPet == pet) { equipPet(pet); }//reload the bonus of the pet after upgrade
            soundHandler.upgradeSoundHandler();
        }
    }




    //when the player catches a new pet. When the player dont have the pet yet
    public void setPetsPlayerOwn(Pet newPet)
    {
        if (!petsPlayerOwn.Contains(newPet))
            petsPlayerOwn.Add(newPet);
  
    }

    public List<Pet> getPetsPlayerOwn()
    {
        return petsPlayerOwn;
    }


    

    public Pet getEquippedPet()
    {
        return equippedPet;
    }



    public void save()
    {
        ES3.Save("equippedPet", equippedPet);

        ES3.Save("petsPlayerOwn", petsPlayerOwn);
    }

    public void load()
    {
        if (ES3.KeyExists("equippedPet"))
            equippedPet = ES3.Load<Pet>("equippedPet");

        if (ES3.KeyExists("petsPlayerOwn"))
            petsPlayerOwn = ES3.Load<List<Pet>>("petsPlayerOwn");
    }
    
}
