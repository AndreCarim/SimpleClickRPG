using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldAmountText;
    
    [SerializeField] private double currentAmountOfGold;

    


    // Start is called before the first frame update
    void Start()
    {
        load();

        
        setGoldAmountText();
    }


    void OnApplicationPause(bool stats){
        if(stats == true)
        {
            save();
        }
    }


    public void increaseAmountOfGold(double value){
        //this will be used by the sell button for example
        currentAmountOfGold = currentAmountOfGold + value;
        setGoldAmountText();
    }

    public void decreaseAmountOfGold(double value){
        //this will be used by upgrades like backpack and strength
        currentAmountOfGold = currentAmountOfGold - value;
        setGoldAmountText();
    }

    

    private void setGoldAmountText(){
        if(currentAmountOfGold > 10000)
        {
            goldAmountText.text = NumberAbrev.ParseDouble(currentAmountOfGold, 2);
        }
        else
        {
            goldAmountText.text = NumberAbrev.ParseDouble(currentAmountOfGold, 0);
        }
        
    }

    public double getCurrentAmountOfGold(){
        return currentAmountOfGold;
    }

    public void save()
    {
        ES3.Save("currentAmountOfGold", currentAmountOfGold);
        
    }

    private void load()
    {
        currentAmountOfGold = ES3.Load<double>("currentAmountOfGold", 0); 
    }

}