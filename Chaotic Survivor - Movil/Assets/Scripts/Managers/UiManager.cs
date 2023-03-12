using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI versionTxt = null;
    public TextMeshProUGUI companyNameTxt = null;
    [Space]
    public TextMeshProUGUI highscoreTimerTxt = null;
    public TextMeshProUGUI highscoreTimerInfoTxt = null;
    public TextMeshProUGUI highscoreLevelTxt = null;
    public TextMeshProUGUI highscoreEnemiesTxt = null;
    public TextMeshProUGUI highscoreCoinsTxt = null;

    [Header("Options")]
    public Toggle useMillisecondsTgl = null;
    [Space]
    public Toggle muteMusic = null;
    [Space]
    public Toggle muteSFX = null;
    [Space]
    public Toggle easyModTgl = null;

    [Header("InGame")]
    public TextMeshProUGUI timerTxt = null;
    public TextMeshProUGUI infoTxt = null;
    [Space]
    public Slider levelSlider = null;
    public TextMeshProUGUI levelNumber = null;
    public TextMeshProUGUI levelUpTxt = null;
    [Space]
    public TextMeshProUGUI scoreTimerTxt = null;
    public TextMeshProUGUI scoreTimerInfoTxt = null;
    public TextMeshProUGUI scoreLevelTxt = null;
    public TextMeshProUGUI scoreEnemiesTxt = null;
    public TextMeshProUGUI scoreCoinsTxt = null;
    [Space]
    public GameObject[] joystickEasyMode = null;
    public GameObject oneJoystickEasyMode = null;

    [Header("Timer")]
    public string[] infoTextString = null;


    [Header("Loading")]
    public Slider loadingSlider = null;
    public TextMeshProUGUI loadingText = null;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        optionsManager = FindObjectOfType<OptionsManager>();

        CheckCanvasScaler();
    }

    public void PlayAnimations()
    {
        for (int i = 0; i < uiAnimationControllers.Length; i++)
        {
            uiAnimationControllers[i].PlayAnimationParallel();
            uiAnimationControllers[i].PlayAnimationFixed();
        }
    }

    #region AppChecker
    private void CheckCanvasScaler()
    {
        if (((float)Screen.width / Screen.height) < (9f / 18f))
            return;
        CanvasScaler[] scalers = FindObjectsOfType<CanvasScaler>();
        for (int i = 0; i < scalers.Length; i++)
            scalers[i].matchWidthOrHeight = 1f;
    }
    #endregion

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
    public void DisplayTimer(float timeToDisplay, TextMeshProUGUI text, TextMeshProUGUI infoText)
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
    public void DisplayMatchInfo(TextMeshProUGUI levelTxt, int levelData, TextMeshProUGUI enemiesTxt, int enemiesData, TextMeshProUGUI coinsTxt, int coinsData)
    {
        levelTxt.text = "" + levelData;
        enemiesTxt.text = "" + enemiesData;
        coinsTxt.text = "" + coinsData;
    }
    #endregion
}
