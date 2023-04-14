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


    private GameObject selectIcon;
    private GameObject equippedIcon;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);


        //getting the select and equipped icons
        Transform findSelected = transform.Find("Selected");
        Transform findEquipped = transform.Find("Equipped");
        selectIcon = findSelected.gameObject;
        equippedIcon = findEquipped.gameObject;
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
        equippedIcon.SetActive(true);
    }

    public void slotGotUnequipped()
    {
        //this will remove the surrounding
        equippedIcon.SetActive(false);
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
