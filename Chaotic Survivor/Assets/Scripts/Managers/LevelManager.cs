using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    #region Scripts
    [Header("Scripts")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private OptionsManager optionsManager;
    [SerializeField] private SaveNLoad saveNLoad;
    [SerializeField] private int fadeAnimator = 0;
    #endregion

    [Header("Timer")]
    public float timerValue = 0;
    [SerializeField] private string[] infoTextString = null;
    [SerializeField] private bool timerRunning = false;

    [Header("Camera Spawner")]
    public Camera cameraMain = null;
    [SerializeField] private float timerToSpawn = 0;
    [SerializeField] private GameObject[] enemyPrefab = null;
    [SerializeField] private Vector3 outsideCam;
    [SerializeField] private Transform enemyParent = null;
    public int enemiesSpawned;
    public int maxEnemiesSpawn;
    private float timerToSpawnCurrent = 0;
    private OffScreenIndicator offScreenIndicator;

    [Header ("Player Data")]
    public int playerLevel = 1;
    public int playerLevelLate = 1;
    public float playerLevelFloat = 0;
    public float playerLevelMaxFloat = 0;
    public PlayerActions playerActions = null;
    public PlayerMovement playerMovement = null;
    [SerializeField] private Animator animator;

    [Header("Game Data")]
    public int enemiesKilled = 0;
    public int coinsGrab = 0;
    public List<EnemyScriptableObject> enemies = null;
    public List<BulletBehavior> bullets = null;
    public List<ItemCollector> items = null;
    public int UiAnimatorActive;
    public int gameOverAnimator;
    public bool canShoot = true;
    public bool leveUp = false;
    public AudioSource levelUpSound = null;

    [Header("Life Spawner")]
    public GameObject[] lifePrefab;
    public Transform[] lifeSpawnPoint;
    public int spawnPointLifeNumber;
    [Space]
    public GameObject[] levelUpItemPrefab;
    public Transform[] levelUpItemSpawnPoint;
    public int spawnPointlevelUpItemNumber;

    [Space]
    [SerializeField] private AbilityScriptableObject abilityScriptable;
    

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.SetLevelManager(this);
        uiManager = FindObjectOfType<UiManager>();
        optionsManager = FindObjectOfType<OptionsManager>();
        cameraMain = FindObjectOfType<Camera>();
        playerActions = FindObjectOfType<PlayerActions>();
        uiManager.levelSlider.maxValue = playerLevelMaxFloat;
        animator = playerActions.GetComponent<Animator>();
        saveNLoad = FindObjectOfType<SaveNLoad>();
        //uiManager.ActiveAnimation(fadeAnimator);
        timerRunning = true;
        abilityScriptable = FindObjectOfType<AbilityScriptableObject>();
        abilityScriptable.SetLevelManager(this);
        offScreenIndicator = FindObjectOfType<OffScreenIndicator>();
        offScreenIndicator.Awake();
        levelPlayer();
        SpawnLife();
        SpawnLevelUpItem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region Timer
        if(timerRunning)
        {
            if (timerValue >= 524285)
                GameOver();
            else
                timerValue += Time.fixedDeltaTime;

            uiManager.DisplayTimer(timerValue, uiManager.timerTxt, uiManager.infoTxt);
        }
        #endregion

        if(leveUp)
            playerActions.timerToReduceLife = 1f;

        //Enemies
        UpdateCameraSpawner();
        SpawnEnemies();

        animator.SetFloat("HP", playerActions.playerHP);

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
                items.Remove(items[i]);
        }
    }

    private void LateUpdate()
    {
        offScreenIndicator.DrawIndicators();
    }

    #region Level Player
    //Level Player
    public void levelPlayer()
    {
        if (playerLevelFloat >= playerLevelMaxFloat)
        if (playerLevelFloat >= playerLevelMaxFloat)
        {
            playerLevelFloat = 0;
            playerLevelMaxFloat += (playerLevelMaxFloat / gameManager.porcentageLevel);
                if(playerLevel == gameManager.playerCurLevel-1)
                {
                    LevelUp();
                    gameManager.playerCurLevel += gameManager.playerAdCurLevel;
                }
            playerLevel++;
        }

        uiManager.levelSlider.value = playerLevelFloat;
        uiManager.levelSlider.maxValue = playerLevelMaxFloat;
        uiManager.levelNumber.text = " " + playerLevel;
    }

    //Level Up
    public void LevelUp()
    {
        playerActions.playerCollider.enabled = false;
        abilityScriptable.SelectRandomNumbers();

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].speed = 0;
        }

        for (int i = 0;i < bullets.Count; i++)
        {
            bullets[i].DestroyNRemove();
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].collider2D.enabled = false;
        }

        uiManager.levelUpTxt.text = "LEVEL " + (playerLevel) + " -> " + (playerLevel + 1);

        timerRunning = false;
        canShoot = false;
        playerMovement.m_PlayerSpeed = 0;
        playerMovement.m_rigidbody2D.velocity = new Vector2(0f, 0f);
        playerMovement.enabled = false;
        leveUp = true;

        uiManager.ActiveAnimation(UiAnimatorActive);
        levelUpSound.Play();
    }

    public void ResumeFromLevelUp()
    {
        playerActions.GetComponent<Collider2D>().enabled = true;

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].speed = enemies[i].maxSpeed;
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].collider2D.enabled = true;
        }

        timerRunning = true;
        canShoot = true;
        playerMovement.enabled = true;
        playerMovement.m_PlayerSpeed = playerMovement.m_PlayerSpeedMax;
        leveUp = false;

        playerLevelLate++;
    }
    #endregion

    #region Enemies 
    private void UpdateCameraSpawner()
    {
        outsideCam = cameraMain.ViewportToWorldPoint(new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 10));
    }

    private void SpawnEnemies()
    {
        if (enemiesSpawned < maxEnemiesSpawn)
        {
            if (timerToSpawnCurrent > 0)
                timerToSpawnCurrent -= Time.fixedDeltaTime;
            else
            {
                Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], outsideCam, Quaternion.identity, enemyParent);
                enemiesSpawned++;
                timerToSpawnCurrent = timerToSpawn;
            }
        }
    }
    #endregion

    #region Spawners
    public void SpawnLife()
    {
        spawnPointLifeNumber = Random.Range(0, lifeSpawnPoint.Length);

        Instantiate(lifePrefab[Random.Range(0, lifePrefab.Length)], lifeSpawnPoint[spawnPointLifeNumber].position, Quaternion.identity);
    }

    public void SpawnLevelUpItem()
    {
        spawnPointlevelUpItemNumber = Random.Range(0, levelUpItemSpawnPoint.Length);

        Instantiate(levelUpItemPrefab[Random.Range(0, levelUpItemPrefab.Length)], levelUpItemSpawnPoint[spawnPointlevelUpItemNumber].position, Quaternion.identity);
    }
    #endregion

    #region Game Over
    public void GameOver()
    {
        playerActions.playerCollider.enabled = false;
        playerMovement.m_rigidbody2D.velocity = new Vector2(0f, 0f);
        animator.SetFloat("VelX", 0);
        animator.SetFloat("VelY", 0);
        playerMovement.enabled = false;
        playerActions.playerHP = 0f;
        playerActions.playerCollider.enabled = false;
        timerRunning = false;

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].collider.enabled = false;
            enemies[i].moveRight = true;
        }

        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].DestroyNRemove();
        }

        saveNLoad.SaveData(timerValue, enemiesKilled, coinsGrab, playerLevel);

        uiManager.DisplayTimer(timerValue, uiManager.scoreTimerTxt, uiManager.scoreTimerInfoTxt);
        uiManager.DisplayMatchInfo(uiManager.scoreLevelTxt, playerLevel, uiManager.scoreEnemiesTxt, enemiesKilled, uiManager.scoreCoinsTxt, coinsGrab);

        uiManager.ActiveAnimation(gameOverAnimator);      
    }
    #endregion
}
