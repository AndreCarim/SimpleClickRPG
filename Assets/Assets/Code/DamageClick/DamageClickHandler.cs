using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DamageClickHandler : MonoBehaviour
{
    [SerializeField]private EnemyHandler enemyHandlerCode;
    [SerializeField]private StrengthHandler strengthHandler;

    [SerializeField]private GameObject popUpText;

    [SerializeField]private AudioSource audioSource;
    
    

    private float clickEveryXSeconds; //1 is one second, 0.1 is one tenth of a second
    private bool isReadyToClick;
    private float currentTime;


    void Start()
    {
        clickEveryXSeconds = 0.1f; //setting it to be one tenth of a second, so the player can click 10 times per second max
        currentTime = clickEveryXSeconds;
        isReadyToClick = true;
    }


    //TIMER
    void FixedUpdate()
    {
        if (isReadyToClick == false)
        { // checking if the player clicked
            currentTime -= 1 * Time.deltaTime; // decreasing the amount of seconds inside the currentTime(starts at 1)
            if (currentTime <= 0)
            { //check if the timer is done
                isReadyToClick = true; // set the status back to open/
                currentTime = clickEveryXSeconds; //set the current time back to 30
            }
        }
    }




    public void click(){
        if(isReadyToClick == true) // check to see if the player can click
        {
            enemyHandlerCode.setDamage(strengthHandler.getStrengthPower());

            isReadyToClick = false;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                GameObject newObj = Instantiate(popUpText, touchPosition, Quaternion.identity);
                TMP_Text newText = newObj.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();

                if(strengthHandler.getStrengthPower() > 10000)
                {
                    newText.text = NumberAbrev.ParseDouble(strengthHandler.getStrengthPower(), 2);
                }
                else
                {
                    newText.text = NumberAbrev.ParseDouble(strengthHandler.getStrengthPower(), 0);
                }


                playSound();
                
            }
        } 
    }


    private void playSound()
    {
        audioSource.Play();
    }


    

}
