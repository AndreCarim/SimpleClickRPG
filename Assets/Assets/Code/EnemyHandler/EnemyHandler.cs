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
    
    //enemys sprites
    [SerializeField] private Sprite stageSprite1;
    [SerializeField] private Sprite stageSprite2;
    [SerializeField] private Sprite stageSprite3;
    [SerializeField] private Sprite stageSprite4;
    [SerializeField] private Sprite stageSprite5;
    [SerializeField] private Sprite stageSprite6;
    [SerializeField] private Sprite stageSprite7;
    [SerializeField] private Sprite stageSprite8;
    [SerializeField] private Sprite stageSprite9;
    [SerializeField] private Sprite stageSprite10;
    [SerializeField] private Sprite stageSprite11;
    [SerializeField] private Sprite stageSprite12;
    [SerializeField] private Sprite stageSprite13;
    [SerializeField] private Sprite stageSprite14;
    [SerializeField] private Sprite stageSprite15;

    //backGroundSprite
    [SerializeField] private Sprite backGroundSprite1;
    [SerializeField] private Sprite backGroundSprite2;
    [SerializeField] private Sprite backGroundSprite3;
    [SerializeField] private Sprite backGroundSprite4;
    [SerializeField] private Sprite backGroundSprite5;
    


    private SpriteRenderer sprite;



    void Start(){
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

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
        currentMaxHealth = currentMaxHealth / 10;
        dropItemValue = dropItemValue / 10;

        currentMaxHealth = currentMaxHealth + behindTheSceneHealth; //hp handler
        behindTheSceneHealth = behindTheSceneHealth * 1.6; //40% every 10 kills
        currentHealth = currentMaxHealth;
        currentStage += 1; //increase stage
        dropItemValue = dropItemValue * 4; // incriese drop values
    }

    private void handleBoss()
    {
        currentMaxHealth = currentMaxHealth * 8;
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
        switch(currentStage){
            case 1:
                spriteRenderer.sprite = stageSprite1;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 2:
                spriteRenderer.sprite = stageSprite2;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 3:
                spriteRenderer.sprite = stageSprite3;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 4:
                spriteRenderer.sprite = stageSprite4;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 5:
                spriteRenderer.sprite = stageSprite5;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 6:
                spriteRenderer.sprite = stageSprite6;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 7:
                spriteRenderer.sprite = stageSprite7;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 8:
                spriteRenderer.sprite = stageSprite8;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 9:
                spriteRenderer.sprite = stageSprite9;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 10:
                spriteRenderer.sprite = stageSprite10;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 11:
                spriteRenderer.sprite = stageSprite11;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 12:
                spriteRenderer.sprite = stageSprite12;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 13:
                spriteRenderer.sprite = stageSprite13;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 14:
                spriteRenderer.sprite = stageSprite14;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 15:
                spriteRenderer.sprite = stageSprite15;
                backGroundSpriteRenderer.sprite = backGroundSprite1;
                break;
            case 16:
                spriteRenderer.sprite = stageSprite15;
                backGroundSpriteRenderer.sprite = backGroundSprite2;
                break;
            case 17:
                spriteRenderer.sprite = stageSprite15;
                backGroundSpriteRenderer.sprite = backGroundSprite3;
                break;
            case 18:
                spriteRenderer.sprite = stageSprite15;
                backGroundSpriteRenderer.sprite = backGroundSprite4;
                break;
            case 19:
                spriteRenderer.sprite = stageSprite15;
                backGroundSpriteRenderer.sprite = backGroundSprite5;
                break;
            default:
                spriteRenderer.sprite = stageSprite15;
                backGroundSpriteRenderer.sprite = backGroundSprite5;
                break;
            
        }
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
    }

    private void load()
    {
        //nao mudar os nomes depois de lançar
        currentMaxHealth = ES3.Load<double>("currentMaxHealth", 1500);
        dropItemValue = ES3.Load<double>("dropItemValue", 50);
        amountKilled = ES3.Load<double>("amountKilled", 0);
        currentStage = ES3.Load<double>("currentStage", 1);
        currentHealth = ES3.Load<double>("currentHealth", currentMaxHealth);
        behindTheSceneHealth = ES3.Load<double>("behindTheSceneHealth", 500);
        isBoss = ES3.Load<bool>("isBoss", false);
    }



}
