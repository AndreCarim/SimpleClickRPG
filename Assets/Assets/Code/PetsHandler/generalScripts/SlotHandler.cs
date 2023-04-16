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

    


    [SerializeField]private GameObject selectIcon;
    [SerializeField] private GameObject equipedIcon;

    


    public void setPetInSlot(Pet pet)
    {
        
        image = gameObject.GetComponent<Image>(); //needs to be here so it will set the image before opens
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);





        petInSlot = pet;

        setPetImage();
    }

    private void setPetImage()
    {
        if (petInSlot)
        {
            image.sprite = petInSlot.sprite;
        }
    }


    public void OnButtonClick()
    {
        //when the player selects the slot

        if (petInSlot)
        {
            showPetsOwned.setSelectedPet(gameObject);
        }
        
        //setting the select image.
    }

    public void slotGotEquipped()
    {
        //this will only set the surrounding
        equipedIcon.SetActive(true);
    }

    public void slotGotUnequipped()
    {
        //this will remove the surrounding
        equipedIcon.SetActive(false);
    }

    public void slotGotSelected()
    {
       
        selectIcon.SetActive(true); //player sellected this slot
    }

    public void slotGotUnselected()
    {
        selectIcon.SetActive(false);//the player equipped this pet
    }



    public Pet getPetInSlot()
    {
        return petInSlot;
    }
}
