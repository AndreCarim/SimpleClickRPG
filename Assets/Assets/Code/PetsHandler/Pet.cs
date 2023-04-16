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
        MaxHp,
        Damage
    }

    public int level; //0 to 5
    public double bonusAmountDouble;//for the percentage and gems
    public int bonusAmountInt; //for the backpack
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    


    
}
