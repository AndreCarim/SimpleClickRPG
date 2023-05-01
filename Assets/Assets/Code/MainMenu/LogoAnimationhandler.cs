using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimationhandler : MonoBehaviour
{
    [SerializeField] private MainMenuPlayFab mainMenuPlayFab;
 
    [Space]
    [Header("Sounds")]
    [SerializeField] private AudioSource swordSound;
    [SerializeField] private AudioSource punchSound;
    [SerializeField] private AudioSource playSwapSound;

    public void animationEnded()
    {
        mainMenuPlayFab.checkIfThePlayerHasAnAccountAlready();
    }

    public void playSwordSound()
    {
        swordSound.Play();
    }

    public void playExplosion()
    {
        punchSound.Play();
    }

    public void playSwap()
    {
        playSwapSound.Play();
    }
}
