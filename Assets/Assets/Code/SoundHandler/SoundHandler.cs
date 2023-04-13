using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] AudioSource petFoundSound;
    [SerializeField] AudioSource clickSound;


    public void petFoundSoundHandle()
    {
        petFoundSound.Play();
    }

    public void clickSoundHandler()
    {
        clickSound.Play();
    }
}
