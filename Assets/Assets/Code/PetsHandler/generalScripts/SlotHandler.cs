using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotHandler : MonoBehaviour
{
    private Pet petInSlot;

    private Image image;
    private Button button;

    [SerializeField] private ShowPetsOwned showPetsOwned;
    

    //handles information
    [SerializeField] 

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }


    public void setPetInSlot(Pet pet)
    {
        petInSlot = pet;

        setPetInfo();
    }

    private void setPetInfo()
    {
        if (petInSlot)
        {
            image.sprite = petInSlot.sprite;
        }
    }


    public void OnButtonClick()
    {
        if (petInSlot)
        {
            showPetsOwned.setSelectedPet(petInSlot);
        }
        
    }
}
