using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossGlowAnimation : MonoBehaviour
{
    public Image image;
    private float minAlpha;
    private float maxAlpha;
    private float speed;

    private bool increasing = true;
    private Color normalColor = new Color(255, 0, 0, 0);

    private bool isBossActive;

    void Start()
    {
        image = GetComponent<Image>();
        minAlpha = 0.1f;
        maxAlpha = 0.5f;
        speed = 0.1f;
    }

    void Update()
    {

        if (isBossActive)
        {
            Color color = image.color;
            float alpha = color.a;

            if (increasing)
            {
                alpha += Time.deltaTime * speed;
                if (alpha >= maxAlpha)
                {
                    alpha = maxAlpha;
                    increasing = false;
                }
            }
            else
            {
                alpha -= Time.deltaTime * speed;
                if (alpha <= minAlpha)
                {
                    alpha = minAlpha;
                    increasing = true;
                }
            }

            color.a = alpha;
            image.color = color;
        }
        
    }



    public void setIsBossActive(bool value)
    {
        if(value == true)
        {
            //starts the glowing
            isBossActive = true;
        }
        else
        {
            //stops the glowing
            isBossActive = false;
            image.color = normalColor;
        }
    }
}
