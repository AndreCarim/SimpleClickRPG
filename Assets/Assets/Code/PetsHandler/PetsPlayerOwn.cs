using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PetsPlayerOwn : MonoBehaviour
{
    [SerializeField] private List<Pet> petsPlayerOwn = new List<Pet>();

    [SerializeField] private Pet equippedPet;
    


    

















    public void setPetsPlayerOwn(Pet newPet)
    {
        if (!petsPlayerOwn.Contains(newPet))
            petsPlayerOwn.Add(newPet);
    }

    public List<Pet> getPetsPlayerOwn()
    {
        return petsPlayerOwn;
    }


    public void setEquippedPet(Pet pet)
    {
        if (petsPlayerOwn.Contains(pet))
        {
            equippedPet = pet;
            //change power
            //change sprite showing on screen
        }
    }

    public Pet getEquippedPet()
    {
        return equippedPet;
    }


    
}
