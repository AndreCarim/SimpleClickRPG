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
        clickEveryXSeconds = 0.05f; //setting it to be one tenth of a second, so the player can click 10 times per second max
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

    public void touched()
    {
        if (Input.touchCount > 0)
        {

            // Percorre todos os toques na tela
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Verifica se o toque foi iniciado ou terminado
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    // Cria um raio a partir do ponto do toque na tela
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);

                    // Verifica se o raio atingiu o objeto com a tag desejada
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                    
 
                    if (isReadyToClick == true)
                    {
                        handleTouch(Input.GetTouch(i));
                    } // check to see if the player can click
                }
            }
        }
    }



    private void handleTouch(Touch touch){
        
        
        
        if(isReadyToClick == true) // check to see if the player can click
        {
            isReadyToClick = false;
           
            enemyHandlerCode.setDamage(strengthHandler.getStrengthPower());

                

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


    private void playSound()
    {
        audioSource.Play();
    }


    

}
