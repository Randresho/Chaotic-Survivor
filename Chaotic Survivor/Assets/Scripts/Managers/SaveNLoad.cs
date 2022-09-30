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
    public float musicOn;
    public float sfxOn;

    [Header("Game Data")]
    public int firstTimePlayingInt;

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

    public void SaveOptions()
    {
        millisecondsOn = optionsManager.millisecondsInt;
        musicOn = optionsManager.musicInt;
        sfxOn = optionsManager.sfxInt;

        PlayerPrefs.SetInt("MillisecondsSave", millisecondsOn);
        PlayerPrefs.SetFloat("MusicSave", musicOn);
        PlayerPrefs.SetFloat("SFXSave", sfxOn);
    }

    public void LoadData()
    {
        timerData = PlayerPrefs.GetFloat("Timer");
        enemiesKilledData = PlayerPrefs.GetInt("EnemiesKilled");
        coinGrabData = PlayerPrefs.GetInt("CoinsGrab");
        levelData = PlayerPrefs.GetInt("LevelD");
        
        uiManager.DisplayTimer(timerData, uiManager.highscoreTimerTxt, uiManager.highscoreTimerInfoTxt);
        uiManager.DisplayMatchInfo(uiManager.highscoreLevelTxt, levelData, uiManager.highscoreEnemiesTxt, enemiesKilledData, uiManager.highscoreCoinsTxt, coinGrabData);        
    }

    public void LoadGameData()
    {
        firstTimePlayingInt = PlayerPrefs.GetInt("FirstTimePlaying");
    }

    public void LoadOptions()
    {
        millisecondsOn = PlayerPrefs.GetInt("MillisecondsSave");
        musicOn = PlayerPrefs.GetFloat("MusicSave");
        sfxOn = PlayerPrefs.GetFloat("SFXSave");

        //Milliseconds
        if(millisecondsOn == 0)
        {
            optionsManager.useMilliseconds = false;
        }
        else
        {
            optionsManager.useMilliseconds = true;
        }
        uiManager.useMillisecondsTgl.isOn = optionsManager.useMilliseconds;

        uiManager.musicSlider.value = musicOn;
        uiManager.sfxSlider.value = sfxOn;
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Todos los datos fueron borrados");
    }
}
