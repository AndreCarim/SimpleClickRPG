using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//will handle the enemy hp, sound of the click (damage, kill, full inventory);
//also will be responsible for the healthBar
//it also calls the backpack to give the item dropped
//reload the new enemy
//sets the enemy health and dropItemValue
public class EnemyHandler : MonoBehaviour
{
    private double currentHealth;
    private double currentMaxHealth; //have to save this
    private double dropItemValue; //have to save this
    private double behindTheSceneHealth;//incremental number

    private double healAmount;

    


    [SerializeField] private BackPackHandler backPackHandler;

    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI amountKilledText;

    [SerializeField] private Animator enemyKillAnimation;
    [SerializeField] private SpriteRenderer enemyKillSpriteRenderer;


    private double amountKilled; // have to save this
    private double currentStage; // have to save this
    

    private AudioSource audioSource;
    [SerializeField] private AudioClip diedSound;
    [SerializeField] private AudioClip inventoryFullSound;

    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    
    [SerializeField] private SpriteRenderer backGroundSpriteRenderer;


    private SpriteRenderer spriteRenderer;

    //boss
    private bool isBoss; //check if it is a boss on save and load
    [SerializeField] Sprite heartSprite;
    [SerializeField] Sprite bossSprite;
    [SerializeField] Image healthBarIcon;

    [SerializeField] Sprite[] enemySprites;
    [SerializeField] Sprite[] backGroundSprites;
    private Sprite currentBackGroundSprite;



    //timer
    private float currentTime;
    private float healEveryXSeconds;



    void Start(){
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        healEveryXSeconds = 2f;

        load();

       

        setIconHealthBar();
        changeSprite();
        healthBarUpdate();
        setTexts();
    }

    void OnApplicationPause(bool stats){
        if(stats == true)
        {
            save();
        }
    }




    //TIMER for the enemy healing
    void FixedUpdate()
    {

        currentTime -= 1 * Time.deltaTime; // decreasing the amount of seconds inside the currentTime(starts at 1)
        if (currentTime <= 0)
        { //check if the timer is done

            currentTime = healEveryXSeconds; //set the current time back to 30
            if (currentHealth < currentMaxHealth)
            {
                healEnemy();
            }

        }

    }

    public void setDamage(double damageAmount){
        

        if(damageAmount >= 1 && currentHealth >= 0){
            if(currentHealth - damageAmount > 0){
                //enemy keeps alive
                currentHealth -= damageAmount;
                

                

            }else if(currentHealth - damageAmount <= 0 ){
                currentHealth = 0;
                playDiedSound();
                enemyDied();
            }
        }
        
        healthBarUpdate();
    }

    

    private void enemyDied(){
        killAnimationHandler();//needs to be first because it will get the information such as change sprite and change color of the enemy killed
        backPackHandler.enemyDroppedItem(dropItemValue);

        //upgrade the enemy
        currentHealth = currentMaxHealth;
        amountKilled += 1;

        

        if (amountKilled % 10 == 0){
            //stage is multiple of 10
            isBoss = false; //its not on a boss anymore
            handleUpgradeStage(); //change sprite for the enemy
            changeSprite(); //from enemy and enemy killed
        }
        else if(amountKilled % 10 == 9)//every 9 stages there will be a boss;
        {
            isBoss = true;
            handleBoss();
        }else
        {
            isBoss = false;
        }

        setIconHealthBar();
        setTexts();
        changeColor(); //from the enemy and enemy killed
        healthBarUpdate();
    }


    /*
     * Inimigo
    Vida inicial = 1000
    valor incremental da vida = 500*3(50,70,110,190,350,670...)
    Drop inicial = 5
    incremento de drop = 8x 
     */
    private void handleUpgradeStage(){
        //changing back from a boss
        //it always will upgrade after a boss
        currentMaxHealth = currentMaxHealth / 4;
        dropItemValue = dropItemValue / 8;

        currentMaxHealth = currentMaxHealth + behindTheSceneHealth; //hp handler
        behindTheSceneHealth = behindTheSceneHealth + 550; //550 every 10 kills
        healAmount = healAmount + 50; // increasing the amount of healing
        currentHealth = currentMaxHealth;
        currentStage += 1; //increase stage
        dropItemValue = dropItemValue + 25; // incriese drop values
    }

    private void handleBoss()
    {
        currentMaxHealth = currentMaxHealth * 4;
        currentHealth = currentMaxHealth;
        dropItemValue = dropItemValue * 8;
    }

    private void setIconHealthBar()
    {
        if (isBoss)
        {
            healthBarIcon.sprite = bossSprite;
        }
        else
        {
            healthBarIcon.sprite = heartSprite;
        }
    }


    private void healEnemy()
    {
        if(currentHealth + healAmount <= currentMaxHealth)
        {
            //if the healAmount is not enough to make the full max health
            currentHealth = currentHealth + healAmount;
        }
        else
        {
            //if the healAmount heals all the way to full max health;
            currentHealth = currentMaxHealth;
        }

       
        healthBarUpdate();
    }

    private void changeColor(){
        Color newColor = new Color(Random.value, Random.value, Random.value);
        spriteRenderer.color = newColor;
    }

    private void playDiedSound(){
        if(!backPackHandler.isFull()){
            audioSource.clip = diedSound;
            audioSource.Play();
        }else{
            audioSource.clip = inventoryFullSound;
            audioSource.Play();
        }
    }

    private void healthBarUpdate(){
        

        if(currentMaxHealth > 1000000000)
        {
            healthBarSlider.maxValue = Mathf.RoundToInt((float)currentMaxHealth / 1000000);
            healthBarSlider.value = Mathf.RoundToInt((float)currentHealth / 1000000);
        }
        else
        {
            healthBarSlider.maxValue = Mathf.RoundToInt((float)currentMaxHealth / 50);
            healthBarSlider.value = Mathf.RoundToInt((float)currentHealth / 50);
        }

        if (currentHealth < 1)
        {
            healthText.text = NumberAbrev.ParseDouble(1, 0) + "/" + NumberAbrev.ParseDouble(currentMaxHealth, 0);
        }
        else if(currentHealth > 10000)
        {
            healthText.text = NumberAbrev.ParseDouble(currentHealth, 2) + "/" + NumberAbrev.ParseDouble(currentMaxHealth, 2);
        }
        else
        {
            healthText.text = NumberAbrev.ParseDouble(currentHealth, 0) + "/" + NumberAbrev.ParseDouble(currentMaxHealth, 0);
        }
            
    }

    private void changeSprite(){
        //cheking if there is still sprites out there
        if((int)currentStage <= enemySprites.Length -1)
        {
            spriteRenderer.sprite = enemySprites[(int)currentStage - 1];
        }
        else
        {
            spriteRenderer.sprite = enemySprites[enemySprites.Length - 1];
        }
        
        //checking if is multiple of 50 so it can change the backGround
        if(amountKilled % 50 == 0 && amountKilled != 0)
        {
           
            //if the current stage is not bigger than the amount of sprites
            if((int)amountKilled / 50 <= backGroundSprites.Length - 1)
            {
                currentBackGroundSprite = backGroundSprites[(int)amountKilled / 50];
            }
            else
            {
                currentBackGroundSprite = backGroundSprites[backGroundSprites.Length - 1];
            }
            
        }

        backGroundSpriteRenderer.sprite = currentBackGroundSprite;

    }

    private void setTexts(){
        stageText.text = NumberAbrev.ParseDouble(currentStage);
        amountKilledText.text = NumberAbrev.ParseDouble(amountKilled);
    }

    private void killAnimationHandler()
    {
        enemyKillSpriteRenderer.sprite = spriteRenderer.sprite;
        enemyKillAnimation.SetTrigger("EnemyKilled");
    }

    public void save()
    {
        //nao mudar depois de lançar
        ES3.Save("currentMaxHealth", currentMaxHealth);
        ES3.Save("dropItemValue", dropItemValue);
        ES3.Save("amountKilled", amountKilled);
        ES3.Save("currentStage", currentStage);
        ES3.Save("currentHealth", currentHealth);
        ES3.Save("behindTheSceneHealth", behindTheSceneHealth);
        ES3.Save("isBoss", isBoss);
        ES3.Save("currentBackGroundSprite", currentBackGroundSprite);
        ES3.Save("healAmount", healAmount);
    }

    private void load()
    {
        //nao mudar os nomes depois de lançar
        currentMaxHealth = ES3.Load<double>("currentMaxHealth123124533", 1500);
        dropItemValue = ES3.Load<double>("dropItemValue123123", 100);
        amountKilled = ES3.Load<double>("amountKilled123123", 0);
        currentStage = ES3.Load<double>("currentStage123123", 1);
        currentHealth = ES3.Load<double>("currentHealth234324", currentMaxHealth);
        behindTheSceneHealth = ES3.Load<double>("behindTheSceneHealth123123", 250);
        isBoss = ES3.Load<bool>("isBoss123123", false);
        currentBackGroundSprite = ES3.Load<Sprite>("currentBackGroundSprite123213", backGroundSprites[0]);
        healAmount = ES3.Load<double>("healAmount123213", 50);
    }



}
