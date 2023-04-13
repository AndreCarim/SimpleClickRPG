using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GemHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gemAmountText;

    [SerializeField] private double currentAmountOfGem;

    //handles the pause menu
    [SerializeField] private double totalAmountOfGemEver;


    // Start is called before the first frame update
    void Start()
    {
        load();


        setGemAmountText();
    }


    void OnApplicationPause(bool stats)
    {
        if (stats == true)
        {
            save();
        }
    }


    public void increaseAmountOfGem(double value)
    {
        //this will be used by the sell button for example
        currentAmountOfGem = currentAmountOfGem + value;

        totalAmountOfGemEver += value;//pause menu

        setGemAmountText();
    }

    public void decreaseAmountOfGem(double value)
    {


        //this will be used by upgrades like backpack and strength

        currentAmountOfGem = currentAmountOfGem - value;


        setGemAmountText();
    }



    private void setGemAmountText()
    {
        if (currentAmountOfGem > 10000)
        {
            gemAmountText.text = NumberAbrev.ParseDouble(currentAmountOfGem, 2);
        }
        else
        {
            gemAmountText.text = NumberAbrev.ParseDouble(currentAmountOfGem, 0);
        }

    }

    public double getCurrentAmountOfGem()
    {
        return currentAmountOfGem;
    }

    public string getTotalAmountOfGemEver()
    {
        return NumberAbrev.ParseDouble(totalAmountOfGemEver);
    }

    public void save()
    {
        ES3.Save("currentAmountOfGem", currentAmountOfGem);
        ES3.Save("totalAmountOfGemEver", totalAmountOfGemEver);
    }

    private void load()
    {
        currentAmountOfGem = ES3.Load<double>("currentAmountOfGem", 0);
        totalAmountOfGemEver = ES3.Load<double>("totalAmountOfGemEver", 0);
    }
}
