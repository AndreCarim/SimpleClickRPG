using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject openBasicMenu;
    [SerializeField] private GameObject openAutoClickMenu;
    [SerializeField] private GameObject openHealthMenu;
    [SerializeField] private GameObject openRealMoneyStore;

    [SerializeField] private GameObject autoClickMenu;
    [SerializeField] private GameObject basicMenu;
    [SerializeField] private GameObject healthMenu;
    [SerializeField] private GameObject RealMoneyStoreMenu;

    private Color clickedColor = new Color(1f, 1f, 1f, .25f);
    private Color notClickedColor = new Color(1f, 1f, 1f, 0f);


    //just for when the player buys gems
    [SerializeField] private GameObject finishBuyingDisplay;




    private AudioSource audioSource;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    



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

    public void openRealMoneyStoreMenuClick()
    {
        if (!RealMoneyStoreMenu.activeInHierarchy)
        {
            deactiveAll();
            RealMoneyStoreMenu.SetActive(true);
            openRealMoneyStore.GetComponent<Image>().color = clickedColor;
        }
    }





    private void deactiveAll()
    {
        audioSource.Play();
        //deactivating all of the menus
        //so we can open the right one in the function
        autoClickMenu.SetActive(false);
        basicMenu.SetActive(false);
        healthMenu.SetActive(false);
        RealMoneyStoreMenu.SetActive(false);
        finishBuyingDisplay.SetActive(false);

        openHealthMenu.GetComponent<Image>().color = notClickedColor;
        openBasicMenu.GetComponent<Image>().color = notClickedColor;
        openAutoClickMenu.GetComponent<Image>().color = notClickedColor;
        openRealMoneyStore.GetComponent<Image>().color = notClickedColor;
    }


    public void closeRealMoneyStoreMenuClick()
    {
        deactiveAll();
        openBasicMenuClick();
    }
}
