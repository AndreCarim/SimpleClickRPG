using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject openBasicMenu;
    [SerializeField] private GameObject openAutoClickMenu;
    [SerializeField] private GameObject openHealthMenu;
    

    [SerializeField] private GameObject autoClickMenu;
    [SerializeField] private GameObject basicMenu;
    [SerializeField] private GameObject healthMenu;
    

    private Color clickedColor = new Color(1f, 1f, 1f, .25f);
    private Color notClickedColor = new Color(1f, 1f, 1f, 0f);

    [SerializeField] private SoundHandler soundHandler;


    //just for when the player buys gems
    [SerializeField] private GameObject finishBuyingDisplay;




    


   
    



    public void openBasicMenuClick()
    {
        if (!basicMenu.activeInHierarchy)
        {
            deactiveAll();
            basicMenu.SetActive(true);
            openBasicMenu.GetComponent<Image>().color = clickedColor;
        }
    }


    public void openAutoClickMenuClick()
    {
        if (!autoClickMenu.activeInHierarchy)
        {
            deactiveAll();
            autoClickMenu.SetActive(true);
            openAutoClickMenu.GetComponent<Image>().color = clickedColor;
        }
    }

    public void openHealthMenuClick()
    {
        if (!healthMenu.activeInHierarchy)
        {
            deactiveAll();
            healthMenu.SetActive(true);
            openHealthMenu.GetComponent<Image>().color = clickedColor;
        }
    }

   





    private void deactiveAll()
    {
        soundHandler.openBottomMenusSoundHandler();
        //deactivating all of the menus
        //so we can open the right one in the function
        autoClickMenu.SetActive(false);
        basicMenu.SetActive(false);
        healthMenu.SetActive(false);
        
        

        openHealthMenu.GetComponent<Image>().color = notClickedColor;
        openBasicMenu.GetComponent<Image>().color = notClickedColor;
        openAutoClickMenu.GetComponent<Image>().color = notClickedColor;
        
    }


    public void closePetMenuClick()
    {
        deactiveAll();
        openBasicMenuClick();
        
    }

}
