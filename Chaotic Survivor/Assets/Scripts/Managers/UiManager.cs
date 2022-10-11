using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region Animation Ui Class
    [System.Serializable]
    public class AnimationUiController
    {
        public string fontName;
        public UiAnimationController[] uiAnimationControllers;

        public void PlayAnimations()
        {
            for (int i = 0; i < uiAnimationControllers.Length; i++)
            {
                uiAnimationControllers[i].PlayAnimationParallel();
                uiAnimationControllers[i].PlayAnimationFixed();
            }
        }
    }
    
    private GameManager gameManager;
    private OptionsManager optionsManager;

    [Header("Transition Controller")]
    public float animationUiTimer = 1f;

    #endregion
    [Header("Animation Controllers By Section")]
    [SerializeField] private AnimationUiController[] animationUiControllers;

    [Header("All Animation Controllers")]
    [SerializeField] private UiAnimationController[] uiAnimationControllers = null;

    [Header("Main Menu")]
    public Text versionTxt = null;
    public Text companyNameTxt = null;
    [Space]
    public Text highscoreTimerTxt = null;
    public Text highscoreTimerInfoTxt = null;
    public Text highscoreLevelTxt = null;
    public Text highscoreEnemiesTxt = null;
    public Text highscoreCoinsTxt = null;

    [Header("Options")]
    public Toggle muteMusic = null;
    public Toggle muteSFX = null;
    public Toggle useMillisecondsTgl = null;
    public Slider musicSlider = null;
    public Slider sfxSlider = null;
    public Text musicVolumeTxt = null;
    public Text sfxVolumeTxt = null;
    public Dropdown dropdownLanguage = null;

    [Header("InGame")]
    public Text timerTxt = null;
    public Text infoTxt = null;
    [Space]
    public Slider levelSlider = null;
    public Text levelNumber = null;
    public Text levelUpTxt = null;
    [Space]
    public Text scoreTimerTxt = null;
    public Text scoreTimerInfoTxt = null;
    public Text scoreLevelTxt = null;
    public Text scoreEnemiesTxt = null;
    public Text scoreCoinsTxt = null;

    [Header("Timer")]
    public string[] infoTextString = null;


    [Header("Loading")]
    public Slider loadingSlider = null;
    public Text loadingText = null;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        optionsManager = FindObjectOfType<OptionsManager>();
    }

    public void PlayAnimations()
    {
        for (int i = 0; i < uiAnimationControllers.Length; i++)
        {
            uiAnimationControllers[i].PlayAnimationParallel();
            uiAnimationControllers[i].PlayAnimationFixed();
        }
    }

    #region Animations Ui
    //No Timer
    public void ActiveAnimation(int idx)
    {
        animationUiControllers[idx].PlayAnimations();
    }


    //With Timer
    public void ActiveAnimationWithTimer(int idx)
    {
        StartCoroutine(ActiveAnimationTimer(idx));
    }

    IEnumerator ActiveAnimationTimer(int idx)
    {
        yield return new WaitForSeconds(animationUiTimer);
        animationUiControllers[idx].PlayAnimations();
    }
    #endregion

    #region Display Texts
    //Timer
    public void DisplayTimer(float timeToDisplay, Text text, Text infoText)
    {
        if (timeToDisplay < 0)
            timeToDisplay = 0;

        float days = Mathf.FloorToInt(timeToDisplay / 86400) % 7;
        float hour = Mathf.FloorToInt((timeToDisplay / 3600) % 24);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60) % 60;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = timeToDisplay % 1 * 1000;

        if (timeToDisplay > 0f)
        {
            if (!optionsManager.useMilliseconds)
            {
                text.text = string.Format("{0:00}", seconds);
                infoText.text = infoTextString[0];
            }
            else
            {
                text.text = string.Format("{0:00}:{1:000}", seconds, milliseconds);
                infoText.text = infoTextString[1];
            }
        }
        if (timeToDisplay > 60f)
        {
            if (!optionsManager.useMilliseconds)
            {
                text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                infoText.text = infoTextString[2];
            }
            else
            {
                text.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
                infoText.text = infoTextString[3];
            }
        }
        if (timeToDisplay > 3600f)
        {
            if (!optionsManager.useMilliseconds)
            {
                text.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minutes, seconds);
                infoText.text = infoTextString[4];
            }
            else
            {
                text.text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", hour, minutes, seconds, milliseconds);
                infoText.text = infoTextString[5];
            }
        }
        if (timeToDisplay > 86400f)
        {
            if (!optionsManager.useMilliseconds)
            {
                text.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", days, hour, minutes, seconds);
                infoText.text = infoTextString[6];
            }
            else
            {
                text.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}:{4:000}", days, hour, minutes, seconds, milliseconds);
                infoText.text = infoTextString[7];
            }
        }
    }

    //Match Data
    public void DisplayMatchInfo(Text levelTxt, int levelData, Text enemiesTxt, int enemiesData, Text coinsTxt, int coinsData)
    {
        levelTxt.text = "" + levelData;
        enemiesTxt.text = "" + enemiesData;
        coinsTxt.text = "" + coinsData;
    }
    #endregion
}
