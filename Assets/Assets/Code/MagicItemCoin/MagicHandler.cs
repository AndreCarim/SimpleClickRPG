using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MagicHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI magicAmountText;

    [SerializeField] private double currentAmountOfMagic;

    //handles the pause menu
    private double totalAmountOfMagicEver;


    // Start is called before the first frame update
    void Start()
    {
        load();


        setMagicAmountText();
    }


    void OnApplicationPause(bool stats)
    {
        if (stats == true)
        {
            save();
        }
    }


    public void increaseAmountOfMagic(double value)
    {
        //this will be used by the sell button for example
        currentAmountOfMagic = currentAmountOfMagic + value;

        totalAmountOfMagicEver += value;//pause menu

        setMagicAmountText();
    }

    public void decreaseAmountOfMagic(double value)
    {


        //this will be used by upgrades like backpack and strength

        currentAmountOfMagic = currentAmountOfMagic - value;


        setMagicAmountText();
    }



    private void setMagicAmountText()
    {
        if (currentAmountOfMagic > 10000)
        {
            magicAmountText.text = NumberAbrev.ParseDouble(currentAmountOfMagic, 2);
        }
        else
        {
            magicAmountText.text = NumberAbrev.ParseDouble(currentAmountOfMagic, 0);
        }

    }

    public double getCurrentAmountOfMagic()
    {
        return currentAmountOfMagic;
    }

    public string getTotalAmountOfmagicEver()
    {
        return NumberAbrev.ParseDouble(totalAmountOfMagicEver);
    }

    public void save()
    {
        ES3.Save("currentAmountOfMagic", currentAmountOfMagic);
        ES3.Save("totalAmountOfMagicEver", totalAmountOfMagicEver);
    }

    private void load()
    {
        currentAmountOfMagic = ES3.Load<double>("currentAmountOfMagic", 0);
        totalAmountOfMagicEver = ES3.Load<double>("totalAmountOfMagicEver", 0);
    }
}
