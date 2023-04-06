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

//handles the enemy, stage and 
public class EnemyHandler : MonoBehaviour
{
    private double enemyCurrentHealth;
    private double enemyCurrentMaxHealth; //have to save this
    private double dropItemValue; //have to save this
    private double behindTheSceneHealth;//incremental number

    private double enemyDamageToPlayerAmount; //amount of damage the enemy will give to the player

    private double enemyHealAmount;

    


    [SerializeField] private BackPackHandler backPackHandler;
    [SerializeField] private PlayerHealthHandler playerHealthHandler;

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
    private float currentTimeHeal;
    private float healEveryXSeconds;
    private float currentTimeDamage;
    private float damageEveryXSeconds;



    void Start(){
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        healEveryXSeconds = 2f; // heal for the enemy
        damageEveryXSeconds = 5f;//damage to the player
        currentTimeDamage = damageEveryXSeconds;

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

        currentTimeHeal -= 1 * Time.deltaTime; // decreasing the amount of seconds inside the currentTime(starts at 1)
        currentTimeDamage -= 1 * Time.deltaTime;

        if (currentTimeHeal <= 0)//dealing heal
        { //check if the timer is done

            currentTimeHeal = healEveryXSeconds;
            if (enemyCurrentHealth < enemyCurrentMaxHealth)
            {
                healEnemy();
            }

        }

        if(currentTimeDamage <= 0)//dealing the damage
        {
            currentTimeDamage = damageEveryXSeconds;
            dealDemageToPlayer();
        }

    }

    //damage to the enemy
    public void setDamage(double damageAmount){
        

        if(damageAmount >= 1 && enemyCurrentHealth >= 0){
            if(enemyCurrentHealth - damageAmount > 0){
                //enemy keeps alive
                enemyCurrentHealth -= damageAmount;
                

                

            }else if(enemyCurrentHealth - damageAmount <= 0 ){
                enemyCurrentHealth = 0;
                playDiedSound();
                enemyDied();
            }
        }
        
        healthBarUpdate();
    }

    //damage to the player
    public void dealDemageToPlayer()
    {
        playerHealthHandler.dealDamage(enemyDamageToPlayerAmount);
    }
    

    private void enemyDied(){
        killAnimationHandler();//needs to be first because it will get the information such as change sprite and change color of the enemy killed
        backPackHandler.enemyDroppedItem(dropItemValue);

        //upgrade the enemy
        enemyCurrentHealth = enemyCurrentMaxHealth;
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
        enemyCurrentMaxHealth = enemyCurrentMaxHealth / 4;
        dropItemValue = dropItemValue / 8;
        enemyDamageToPlayerAmount = enemyDamageToPlayerAmount / 2;

        enemyCurrentMaxHealth = enemyCurrentMaxHealth + behindTheSceneHealth; //hp handler
        behindTheSceneHealth = behindTheSceneHealth + 550; //550 every 10 kills
        enemyHealAmount = enemyHealAmount + 50; // increasing the amount of healing
        enemyCurrentHealth = enemyCurrentMaxHealth;
        enemyDamageToPlayerAmount = enemyDamageToPlayerAmount + 50;//increase the amount of damage to player
        currentStage += 1; //increase stage
        dropItemValue = dropItemValue + 25; // incriese drop values
    }

    private void handleBoss()
    {
        enemyCurrentMaxHealth = enemyCurrentMaxHealth * 4;
        enemyCurrentHealth = enemyCurrentMaxHealth;
        dropItemValue = dropItemValue * 8;
        enemyDamageToPlayerAmount = enemyDamageToPlayerAmount * 2;
    }


    public void reloadEnemy()
    {
        enemyCurrentHealth = enemyCurrentMaxHealth;
        healthBarUpdate();
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
        if(enemyCurrentHealth + enemyHealAmount <= enemyCurrentMaxHealth)
        {
            //if the healAmount is not enough to make the full max health
            enemyCurrentHealth = enemyCurrentHealth + enemyHealAmount;
        }
        else
        {
            //if the healAmount heals all the way to full max health;
            enemyCurrentHealth = enemyCurrentMaxHealth;
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
        

        if(enemyCurrentMaxHealth > 1000000000)
        {
            healthBarSlider.maxValue = Mathf.RoundToInt((float)enemyCurrentMaxHealth / 1000000);
            healthBarSlider.value = Mathf.RoundToInt((float)enemyCurrentHealth / 1000000);
        }
        else
        {
            healthBarSlider.maxValue = Mathf.RoundToInt((float)enemyCurrentMaxHealth / 50);
            healthBarSlider.value = Mathf.RoundToInt((float)enemyCurrentHealth / 50);
        }

        if (enemyCurrentHealth < 1)
        {
            healthText.text = NumberAbrev.ParseDouble(1, 0) + "/" + NumberAbrev.ParseDouble(enemyCurrentMaxHealth, 0);
        }
        else if(enemyCurrentHealth > 10000)
        {
            healthText.text = NumberAbrev.ParseDouble(enemyCurrentHealth, 2) + "/" + NumberAbrev.ParseDouble(enemyCurrentMaxHealth, 2);
        }
        else
        {
            healthText.text = NumberAbrev.ParseDouble(enemyCurrentHealth, 0) + "/" + NumberAbrev.ParseDouble(enemyCurrentMaxHealth, 0);
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

    private void healAnimation()
    {

    }

    public void save()
    {
        //nao mudar depois de lançar
        ES3.Save("enemyCurrentMaxHealth", enemyCurrentMaxHealth);
        ES3.Save("dropItemValue", dropItemValue);
        ES3.Save("amountKilled", amountKilled);
        ES3.Save("currentStage", currentStage);
        ES3.Save("enemyCurrentHealth", enemyCurrentHealth);
        ES3.Save("behindTheSceneHealth", behindTheSceneHealth);
        ES3.Save("isBoss", isBoss);
        ES3.Save("currentBackGroundSprite", currentBackGroundSprite);
        ES3.Save("enemyHealAmount", enemyHealAmount);
        ES3.Save("enemyDamageToPlayerAmount", enemyDamageToPlayerAmount);
    }

    private void load()
    {
        //nao mudar os nomes depois de lançar
        enemyCurrentMaxHealth = ES3.Load<double>("enemyCurrentMaxHealth", 1500);
        dropItemValue = ES3.Load<double>("dropItemValue", 100);
        amountKilled = ES3.Load<double>("amountKilled", 0);
        currentStage = ES3.Load<double>("currentStage", 1);
        enemyCurrentHealth = ES3.Load<double>("enemyCurrentHealth", enemyCurrentMaxHealth);
        behindTheSceneHealth = ES3.Load<double>("behindTheSceneHealth", 250);
        isBoss = ES3.Load<bool>("isBoss", false);
        currentBackGroundSprite = ES3.Load<Sprite>("currentBackGroundSprite", backGroundSprites[0]);
        enemyHealAmount = ES3.Load<double>("enemyHealAmount", 50);
        enemyDamageToPlayerAmount = ES3.Load<double>("enemyDamageToPlayerAmount", 50);
    }



}
