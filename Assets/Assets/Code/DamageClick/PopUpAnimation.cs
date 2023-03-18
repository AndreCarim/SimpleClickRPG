using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpAnimation : MonoBehaviour
{

    private float disappearTimer = 0.5f;
    private float disappearTimerMax = 0.5f;
    private Color textColor;
    private Vector3 moveVector;

    private TMP_Text textMesh;

    private void Awake(){
        textMesh = gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void Start(){
        textColor = textMesh.color;
        moveVector = new Vector3(Random.Range(-.9f, .9f),1) * 15f;
    }
   

    // Update is called once per frame
    void Update()
    {
        
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if(disappearTimer > disappearTimerMax * .5f){
            //firstHalf
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }else{
            //Second half
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        
        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0){
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a <0){
                Destroy(gameObject);
            }
        }
    }
}
