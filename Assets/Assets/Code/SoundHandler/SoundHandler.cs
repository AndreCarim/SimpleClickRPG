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
