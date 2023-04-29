using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GemHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gemAmountText;
    [SerializeField] private TextMeshProUGUI gemAmountTextPetEncounter;

    [SerializeField] private double currentAmountOfGem;

    //handles the pause menu
    [SerializeField] private double totalAmountOfGemEver;

    [SerializeField] private LeaderBoardHandler leaderBoardHandler;

    private double petBonusAmount;// ex: 5 will get extra 5 gems per boss
    //common +5 gems
    //rare +10 gems
    //epic +20 gems

    // Start is called before the first frame update
    void Start()
    {
        load();

        petBonusAmount = 0;

        setGemAmountText();
    }


    


    public void increaseAmountOfGem(double value)
    {
        
        currentAmountOfGem = currentAmountOfGem + value + petBonusAmount;

        totalAmountOfGemEver += value + petBonusAmount;//pause menu


        leaderBoardHandler.SendGemsLeaderBoard((int)totalAmountOfGemEver);
        setGemAmountText();
    }

    public void decreaseAmountOfGem(double value)
    {

        currentAmountOfGem = currentAmountOfGem - value;

        leaderBoardHandler.SendGemsLeaderBoard((int)totalAmountOfGemEver);
        setGemAmountText();
    }


    //HANDLES PET
    public void setPetBonusAmount(double value)
    {
        petBonusAmount = value;//setting the new value
    }

    public void removePetBonusAmount()
    {
        petBonusAmount = 0;
    }



    private void setGemAmountText()
    {
        if (currentAmountOfGem > 10000)
        {
            gemAmountText.text = NumberAbrev.ParseDouble(currentAmountOfGem, 2);
            gemAmountTextPetEncounter.text = NumberAbrev.ParseDouble(currentAmountOfGem, 2);
        }
        else
        {
            gemAmountText.text = NumberAbrev.ParseDouble(currentAmountOfGem, 0);
            gemAmountTextPetEncounter.text = NumberAbrev.ParseDouble(currentAmountOfGem, 0);
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
