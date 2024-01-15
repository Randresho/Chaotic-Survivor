using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveNLoad : MonoBehaviour
{
    #region Scripts
    [Header("Scripts")]
    private GameManager gameManager = null;
    private TransitionManager transitionManager = null;
    private OptionsManager optionsManager = null;
    private UiManager uiManager = null;
    private SoundManager soundManager = null;
    #endregion

    [SerializeField] private bool clearData;

    [Header("Last Match")]
    public float timerData = 0f;
    public int enemiesKilledData = 0;
    public int coinGrabData = 0;
    public int levelData = 0;

    [Header("Options")]
    public int millisecondsOn;
    public int musicOn;
    public int sfxOn;
    public int easyMod;

    [Header("Game Data")]
    public int firstTimePlayingInt;
    public int enemiesFreezed;
    public int enemiesBurned;
    public int enemiesElectroshocked;

    public int deadsAmount;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        transitionManager = FindObjectOfType<TransitionManager>();
        optionsManager = FindObjectOfType<OptionsManager>();
        uiManager = FindObjectOfType<UiManager>();
        soundManager = FindObjectOfType<SoundManager>();

#if UNITY_EDITOR
        if (clearData)
            ClearData();
#endif 
    }

    public void SaveData(float timer, int enemiesKilled, int coinGrab, int level)
    {
        PlayerPrefs.SetFloat("Timer", timer);
        PlayerPrefs.SetInt("EnemiesKilled", enemiesKilled);
        PlayerPrefs.SetInt("CoinsGrab", coinGrab);
        PlayerPrefs.SetInt("LevelD", level);
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetInt("FirstTimePlaying", firstTimePlayingInt);
    }

    public void SaveStatsInfo(int coins, int enemies, int deads)
    {
        PlayerPrefs.SetInt("CoinsStats", coins);
        PlayerPrefs.SetInt("EnemiesStats", enemies);
        PlayerPrefs.SetInt("DeadsStats", deads);
    }

    public void SaveGameplayData(int enemiesF, int enemiesB, int enemiesE)
    {
        PlayerPrefs.SetInt("EnemiesFreezed", enemiesF);
        PlayerPrefs.SetInt("EnemiesBurned", enemiesB);
        PlayerPrefs.SetInt("EnemiesElectroshocked", enemiesE);
    }

    #region Options
    public void SaveMilliseconds()
    {
        millisecondsOn = optionsManager.millisecondsInt;
        PlayerPrefs.SetInt("MillisecondsSave", millisecondsOn);
    }

    public void SaveMusic()
    {
        musicOn = optionsManager.musicInt;
        PlayerPrefs.SetInt("MusicSave", musicOn);
    }

    public void SaveSFX()
    {
        sfxOn = optionsManager.sfxInt;
        PlayerPrefs.SetInt("SFXSave", sfxOn);
    }

    public void SaveEasyMode()
    {
        easyMod = optionsManager.easyModInt;
        PlayerPrefs.SetInt("EasyModeSave", easyMod);
    }
    #endregion

    public void LoadData()
    {
        timerData = PlayerPrefs.GetFloat("Timer");
        enemiesKilledData = PlayerPrefs.GetInt("EnemiesKilled");
        coinGrabData = PlayerPrefs.GetInt("CoinsGrab");
        levelData = PlayerPrefs.GetInt("LevelD");

        enemiesFreezed = PlayerPrefs.GetInt("EnemiesFreezed");
        enemiesBurned = PlayerPrefs.GetInt("EnemiesBurned");
        enemiesElectroshocked = PlayerPrefs.GetInt("EnemiesElectroshocked");
        
        uiManager.DisplayTimer(timerData, uiManager.highscoreTimerTxt, uiManager.highscoreTimerInfoTxt);
        uiManager.DisplayMatchInfo(uiManager.highscoreLevelTxt, levelData, uiManager.highscoreEnemiesTxt, enemiesKilledData, uiManager.highscoreCoinsTxt, coinGrabData);        
    }

    public void LoadGameData()
    {
        firstTimePlayingInt = PlayerPrefs.GetInt("FirstTimePlaying");

        gameManager.coinsAmount = PlayerPrefs.GetInt("CoinsStats");
        gameManager.enemiesAmount = PlayerPrefs.GetInt("EnemiesStats");
        gameManager.deadsAmount = PlayerPrefs.GetInt("DeadsStats");


        uiManager.DisplayStats();
    }

    public void LoadOptions()
    {
        //Milliseconds
        millisecondsOn = PlayerPrefs.GetInt("MillisecondsSave");

        if(millisecondsOn == 0)
            optionsManager.useMilliseconds = false;
        else
            optionsManager.useMilliseconds = true;

        uiManager.useMillisecondsTgl.isOn = optionsManager.useMilliseconds;

        //Music
        musicOn = PlayerPrefs.GetInt("MusicSave");

        if(musicOn == 0)
            optionsManager.muteMusic = false;
        else
            optionsManager.muteMusic = true;

        uiManager.muteMusic.isOn = optionsManager.muteMusic;
        soundManager.MuteMusic();

        //Sound FX
        sfxOn = PlayerPrefs.GetInt("SFXSave");

        if(sfxOn == 0)
            optionsManager.muteSFX = false;
        else
            optionsManager.muteSFX = true;

        uiManager.muteSFX.isOn = optionsManager.muteSFX;
        soundManager.MuteSFX();

        //Easy mod
        easyMod = PlayerPrefs.GetInt("EasyModeSave");

        if(easyMod == 0)
            optionsManager.easyMod = true;
        else
            optionsManager.easyMod = false;

        uiManager.easyModTgl.isOn = optionsManager.easyMod;
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Todos los datos fueron borrados");
    }
}
