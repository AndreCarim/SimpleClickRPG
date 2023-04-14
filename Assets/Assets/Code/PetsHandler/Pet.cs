using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "NewPet", menuName = "Pets/Pet")]
public class Pet : ScriptableObject
{
    public string petName;

    [SerializeField]
    public Rarity rarity;

    public enum Rarity
    {
        Common,
        Rare,
        Epic
    }

    [SerializeField]
    public Bonus bonus;

    public enum Bonus
    {
        Gold,
        Backpack,
        Gems,
        HpAndrecovery,
        Damage
    }

    public int level; //0 to 5
    public int bonusAmount;
    public Sprite sprite;
    


    public Rarity getRarity()
    {
        return rarity;
    }
}
