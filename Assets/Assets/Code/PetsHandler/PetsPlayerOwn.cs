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







    //t



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


    private void unequipPet()
    {

    }

    private void equipPet(Pet pet)
    {
        //checking if the player has the pet he is trying to equip
        if (petsPlayerOwn.Contains(pet))
        {
            //changes the pet
            equippedPet = pet;
            
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


    
}
