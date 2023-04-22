using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] AudioSource petFoundSound;
    [SerializeField] AudioSource clickSound;

    [SerializeField] AudioSource bushSound;
    [SerializeField] AudioSource revielingSound;

    [SerializeField] AudioSource upgradeSound;

    [SerializeField] AudioSource boughtGemsSound;
    [SerializeField] AudioSource clickEnemySound;

    [SerializeField] AudioSource openBottomMenusSound;


    public void petFoundSoundHandle()
    {
        petFoundSound.Play();
    }

    public void clickSoundHandler()
    {
        clickSound.Play();
    }

    public void upgradeSoundHandler()
    {
        upgradeSound.Play();
    }

    public void boughtGemsSoundHandler()
    {
        boughtGemsSound.Play();
    }

    public void clickEnemySoundHandler()
    {
        clickEnemySound.Play();
    }

    public void openBottomMenusSoundHandler()
    {
        openBottomMenusSound.Play();
    }




    //bush animation for pet encounter
    public void bushSoundHandler()
    {
        bushSound.Play();
    }

    public void revielingSoundHandler()
    {
        revielingSound.Play();
    }


    
}
