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

//it also handles the number of enemys to the next boss

//handles the enemy, stage and 

//level is the current level the player is, not how many enemies he killed
//stage is the amount of stages passed, every 10 levels it pass one stage
//the totalEnemyKills is the general amount of kills the player has
public class EnemyHandler : MonoBehaviour
{
    private double enemyCurrentHealth;
    private double enemyCurrentMaxHealth; //have to save this
    private double dropItemValue; //have to save this
    private double dropGemValue; //quantity of gems that the player will get when he kills a boss
    private double behindTheSceneHealth;//incremental number

    private double enemyDamageToPlayerAmount; //amount of damage the enemy will give to the player

    private double enemyHealAmount;


    //handling the pause menu
    private double totalDamageGivenEver;


    //handlers cheatcheck
    //this will check if the player has passed more than 20 stages in 1 minute
    //it will send a message in discord;
    private double lastStageChecked;
    private float currentTimeLastCheckedCheater;
    private float checkCheatEveryXSeconds;
    [SerializeField] private  CheatHandler cheatHandler;

    [Header("Scripts")]
    [SerializeField] private BackPackHandler backPackHandler;
    [SerializeField] private PlayerHealthHandler playerHealthHandler;
    [SerializeField] private GemHandler gemHandler;
    [SerializeField] private PetsHandler petsHandler;
    [SerializeField] private LeaderBoardHandler leaderBoardHandler;

   

    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI totalAmountKilledText;

    [SerializeField] private Animator enemyKillAnimation;
    [SerializeField] private SpriteRenderer enemyKillSpriteRenderer;



    private double totalAmountKilled; // have to save this
    private double currentEnemyLevel; //this is to see the level
    private double currentStage; // have to save this
    

    private AudioSource audioSource;
    [SerializeField] private AudioClip diedSound;
    [SerializeField] private AudioClip inventoryFullSound;


    //how many enemies left until the boss
    [SerializeField] private TextMeshProUGUI howManyEnemiesLeftUntilTheBossText;
    [SerializeField] private GameObject howManyEnemiesLeft;
    [SerializeField] private GameObject bossButton;
    [SerializeField] private GameObject bossText;
    

    //enemy health bar
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
    [SerializeField] private AudioSource bossLaughAudio;
    [SerializeField] private AudioSource bossKillAudio;
    [SerializeField] private AudioSource bossBackGroundMusic;

    [SerializeField] private BossGlowAnimation bossGlowAnimation;

    


    //timer
    private float currentTimeHeal;
    private float healEveryXSeconds;
    private float currentTimeDamage;
    private float damageEveryXSeconds;

    //timer for the boss;
    private float currentTimeColor;
    private float changeColorEveryXSeconds;

    //Makes Life Easier

    //Makes Life Easier




    void Start(){
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        healEveryXSeconds = 2f; // heal for the enemy
        damageEveryXSeconds = 3f;//damage to the player
        currentTimeDamage = damageEveryXSeconds;
        changeColorEveryXSeconds = 1.5f; //setting the boss to change color every .5 seconds
        checkCheatEveryXSeconds = 60f; //60 seconds to check for cheater

        lastStageChecked = currentStage;

        load();//loads the saves

        

        
        
        setIconHealthBar();
        changeSprite();
        healthBarUpdate();
        setTexts();
    }

   




    //TIMER for the enemy healing and damage
    void FixedUpdate()
    {

        currentTimeHeal -= 1 * Time.deltaTime; // decreasing the amount of seconds inside the currentTime(starts at 1)
        currentTimeDamage -= 1 * Time.deltaTime;
        currentTimeLastCheckedCheater -= 1 * Time.deltaTime; // checking the cheater

        currentTimeColor -= 1 * Time.deltaTime;

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

        //handles the boss changing color every .5 seconds
        if(isBoss == true && currentTimeColor <= 0)
        {
            changeColor();
            currentTimeColor = changeColorEveryXSeconds;
        }

        if(currentTimeLastCheckedCheater <= 0)
        {
            if(lastStageChecked > 0)
            {
                currentTimeLastCheckedCheater = checkCheatEveryXSeconds;
                cheatHandler.checkCheater((int)(currentStage - lastStageChecked));

                lastStageChecked = currentStage;
            }
            
        }

    }

    //damage to the enemy
    public void setDamage(double damageAmount){

        totalDamageGivenEver += damageAmount; //sum the total damage given ever so it can be showed on the pause

        if (damageAmount >= 1 && enemyCurrentHealth >= 0){
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
        backPackHandler.enemyDroppedItem(dropItemValue); //add the gold to the bag

        

        //upgrade the enemy
        enemyCurrentHealth = enemyCurrentMaxHealth;
        currentTimeDamage = 5f;//this will set the time to atack back to 5 seconds

        totalAmountKilled += 1;
        petsHandler.checkIfEncounterAPet(totalAmountKilled);


        if (currentEnemyLevel % 10 == 8) { currentEnemyLevel++; } //this is adding one to the current so we can check in the conditionals that follows, otherwise it will go directly to the else statement.

        //reload the hp for the player
        playerHealthHandler.reloadPlayer();

        

        if (isBoss){//right after killed a boss
            //will handle whatever comes after a boss

            //first, give the player some gems.
            gemHandler.increaseAmountOfGem(20);

            handleUpgradeStage(); //change sprite for the enemy
            changeSprite(); //from enemy and enemy killed
            setHowManyEnemiesLeftUntilBoss();//will set the amount of enemies left until the boss
            bossAnimationsFinish();
        }
        else if(currentEnemyLevel % 10 == 9)//this will open the boss button, it will happen every level before a boss
        {
            //while the player not kills the boss, this will keep showing
            //it will not count to the currentEnemy because it can be played more than once
            isBoss = false;
            setBossButton();
        }
        else//its not a lvl before the boss and not right after the boss
        {
            isBoss = false;
            currentEnemyLevel += 1;

            
            setHowManyEnemiesLeftUntilBoss();//will set the amount of enemies left until the boss
        }


        leaderBoardHandler.SendKillsLeaderBoard((int)totalAmountKilled); //update leaderBoard
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
        //upgrade the enemy after a boss
        //it always will upgrade after a boss

        

        isBoss = false;

        //if you change here, need to change inside the goBackOneLevel()
        enemyCurrentMaxHealth = enemyCurrentMaxHealth / 5;
        dropItemValue = dropItemValue / 8;
        enemyDamageToPlayerAmount = enemyDamageToPlayerAmount / 2;

        enemyCurrentMaxHealth = enemyCurrentMaxHealth + behindTheSceneHealth; //hp handler
        behindTheSceneHealth = behindTheSceneHealth + 400; //550 every 10 kills
        enemyHealAmount = enemyHealAmount + 50; // increasing the amount of healing
        enemyCurrentHealth = enemyCurrentMaxHealth;
        enemyDamageToPlayerAmount = enemyDamageToPlayerAmount + 20;//increase the amount of damage to player
        currentStage += 1; //increase stage
        dropItemValue = dropItemValue + 100; // incriese drop values

        currentEnemyLevel += 1;


        leaderBoardHandler.SendStageLeaderBoard((int)currentStage); //updates the leaderboard
        bossKillAudio.Play();//play the audio
    }

    private void handleBoss()//handles the boss upgrades
    {
        enemyCurrentMaxHealth = enemyCurrentMaxHealth * 5;
        enemyCurrentHealth = enemyCurrentMaxHealth;
        dropItemValue = dropItemValue * 8;
        enemyDamageToPlayerAmount = enemyDamageToPlayerAmount * 2;
    }


    

    public void clickBossButton()//handles to enter in boss mode
    {
        if (!isBoss)
        {
            handleBoss();

            isBoss = true;
            playerHealthHandler.reloadPlayer(); //healing the player

            currentTimeColor = changeColorEveryXSeconds; //just setting the amount of time for the boss to change color

            setIconHealthBar();
            setTexts();
            changeColor();
            healthBarUpdate();

            bossAnimationsStart(); //play sound and animations

        }
        
    }


    private void setBossButton() //activate the boss button
    {
        bossButton.SetActive(true);
        howManyEnemiesLeft.SetActive(false);
        bossText.SetActive(false);
    }

    

    private void setHowManyEnemiesLeftUntilBoss() //activate the how many enemies text
    {
        //this will handle if its gonna show the button or the number
        bossButton.SetActive(false); // sets the boss button to not active
        howManyEnemiesLeft.SetActive(true);//sets the how many active
        bossText.SetActive(false);

        howManyEnemiesLeftUntilTheBossText.text = NumberAbrev.ParseDouble(9 - (currentEnemyLevel % 10));// number of enemies per stage without the boss - the rest of the currentLevel
    }

    //reaload the enemy when the player died for a boss enemy
    public void reloadFromBoss()
    {
        if (isBoss) //if the player is really in a boss
        {
            isBoss = false;

            enemyCurrentMaxHealth = enemyCurrentMaxHealth / 5;
            dropItemValue = dropItemValue / 8;
            enemyDamageToPlayerAmount = enemyDamageToPlayerAmount / 2;
            enemyCurrentHealth = enemyCurrentMaxHealth;


            bossAnimationsFinish();

            setBossButton();
            setIconHealthBar();
            setTexts();
            changeColor();
            healthBarUpdate();
        }
    }
   
    //reaload the enemy when the player died for a normal enemy
    public void reloadFromNormalEnemy()
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

        string tempCurrentHealth;
        string tempMaxHealth;
        

        if(enemyCurrentMaxHealth > 1000000000)//handles the health bar it self
        {
            healthBarSlider.maxValue = Mathf.RoundToInt((float)enemyCurrentMaxHealth / 1000000);
            healthBarSlider.value = Mathf.RoundToInt((float)enemyCurrentHealth / 1000000);
        }
        else
        {
            healthBarSlider.maxValue = Mathf.RoundToInt((float)enemyCurrentMaxHealth / 50);
            healthBarSlider.value = Mathf.RoundToInt((float)enemyCurrentHealth / 50);
        }

        if (enemyCurrentHealth < 1) //handles the text inside the bar for the current Health
        {
            tempCurrentHealth = "1";
        }
        else if(enemyCurrentHealth > 10000)
        {
            tempCurrentHealth = NumberAbrev.ParseDouble(enemyCurrentHealth, 2);
        }
        else
        {
            tempCurrentHealth = NumberAbrev.ParseDouble(enemyCurrentHealth, 0);
        }


        if (enemyCurrentMaxHealth > 10000)//handles the text inside the bar for the max health
        {
            tempMaxHealth = NumberAbrev.ParseDouble(enemyCurrentMaxHealth, 2);
        }
        else
        {
            tempMaxHealth = NumberAbrev.ParseDouble(enemyCurrentMaxHealth, 0);
        }



        healthText.text = tempCurrentHealth + "/" + tempMaxHealth;

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
        if(currentStage % 50 == 0 && currentStage != 0)
        {
           
            //if the current stage is not bigger than the amount of sprites
            if((int)currentStage / 50 <= backGroundSprites.Length - 1)
            {
                currentBackGroundSprite = backGroundSprites[(int)currentStage / 50];
            }
            else
            {
                currentBackGroundSprite = backGroundSprites[backGroundSprites.Length - 1];
            }
            
        }

        backGroundSpriteRenderer.sprite = currentBackGroundSprite;

    }

    private void bossAnimationsStart()
    {
        bossButton.SetActive(false);
        howManyEnemiesLeft.SetActive(false);
        bossText.SetActive(true);

        bossLaughAudio.Play();//starts the laugh
        bossBackGroundMusic.Play();//starts the background music
        bossGlowAnimation.setIsBossActive(true);//starts the glowing
    }

    public void bossAnimationsFinish()
    {
        bossBackGroundMusic.Stop();//stops the backGround music
        
        bossGlowAnimation.setIsBossActive(false);//stops the glowing
    }

    private void setTexts(){
        stageText.text = NumberAbrev.ParseDouble(currentStage);
        totalAmountKilledText.text = NumberAbrev.ParseDouble(totalAmountKilled);
    }

    private void killAnimationHandler()
    {
        enemyKillSpriteRenderer.sprite = spriteRenderer.sprite;
        enemyKillAnimation.SetTrigger("EnemyKilled");
    }

    public bool getIsBoss()
    {
        return isBoss;
    }

    public string getTotalDamageGivenEver()
    {
        return NumberAbrev.ParseDouble(totalDamageGivenEver);
    }

    public string getTotalAmountKilled()
    {
        return NumberAbrev.ParseDouble(totalAmountKilled);
    }

    public string getCurrentStage()
    {
        return NumberAbrev.ParseDouble(currentStage);
    }

    public double getDropItemValue()
    {
        return dropItemValue;
    }

    

    public void save()
    {
        //nao mudar depois de lançar
        ES3.Save("enemyCurrentMaxHealth", enemyCurrentMaxHealth);
        ES3.Save("dropItemValue", dropItemValue);
        ES3.Save("currentEnemyLevel", currentEnemyLevel);
        ES3.Save("currentStage", currentStage);
        ES3.Save("enemyCurrentHealth", enemyCurrentHealth);
        ES3.Save("behindTheSceneHealth", behindTheSceneHealth);
        ES3.Save("isBoss", isBoss);
        ES3.Save("currentBackGroundSprite", currentBackGroundSprite);
        ES3.Save("enemyHealAmount", enemyHealAmount);
        ES3.Save("enemyDamageToPlayerAmount", enemyDamageToPlayerAmount);
        ES3.Save("totalAmountKilled", totalAmountKilled);
        ES3.Save("totalDamageGivenEver", totalDamageGivenEver);
        
    }

    private void load()
    {
        //nao mudar os nomes depois de lançar
        enemyCurrentMaxHealth = ES3.Load<double>("enemyCurrentMaxHealth", 1500);
        dropItemValue = ES3.Load<double>("dropItemValue", 100);
        currentEnemyLevel = ES3.Load<double>("currentEnemyLevel", 0);
        currentStage = ES3.Load<double>("currentStage", 1);
        enemyCurrentHealth = ES3.Load<double>("enemyCurrentHealth", enemyCurrentMaxHealth);
        behindTheSceneHealth = ES3.Load<double>("behindTheSceneHealth", 250);
        isBoss = ES3.Load<bool>("isBoss", false);
        currentBackGroundSprite = ES3.Load<Sprite>("currentBackGroundSprite", backGroundSprites[0]);
        enemyHealAmount = ES3.Load<double>("enemyHealAmount", 50);
        enemyDamageToPlayerAmount = ES3.Load<double>("enemyDamageToPlayerAmount", 50);//50
        totalAmountKilled = ES3.Load<double>("totalAmountKilled", 0);
        totalDamageGivenEver = ES3.Load<double>("totalDamageGivenEver", 0);

        //handles the enemy load to check if it is a boss, a regular enemy or a enemy before the boss (so it needs to show the boss button)
        if (isBoss)
        {
            bossAnimationsStart();
        }
        else if (currentEnemyLevel % 10 == 9)//this will open the boss button, it will happen every level before a boss
        {
            setBossButton();
        }
        else//its not a lvl before the boss and not right after the boss
        { 
            setHowManyEnemiesLeftUntilBoss();//will set the amount of enemies left until the boss
        }
    }



}
