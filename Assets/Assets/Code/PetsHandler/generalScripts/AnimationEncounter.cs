using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEncounter : MonoBehaviour
{

    [SerializeField] private GameObject petShow;
    [SerializeField] private GameObject bush;

    [SerializeField] private GameObject okButton;

    [SerializeField] private Animator animatorPet;
    [SerializeField] private Animator animatorOkButton;
    [SerializeField] private Animator animatorShiny;

    [SerializeField] private SoundHandler soundHandler;





    public void handleEndAnimation()
    {
        petShow.SetActive(true);
        bush.SetActive(false);

        animatorPet.updateMode = AnimatorUpdateMode.UnscaledTime;
        animatorOkButton.updateMode = AnimatorUpdateMode.UnscaledTime;
        animatorShiny.updateMode = AnimatorUpdateMode.UnscaledTime;
    }


    public void showButton()
    {
        okButton.SetActive(true);
        animatorOkButton.updateMode = AnimatorUpdateMode.UnscaledTime;
    }


    public void bushSound()
    {
        soundHandler.bushSoundHandler();
    }

    public void reviewSound()
    {
        soundHandler.revielingSoundHandler();
    }

    
}
