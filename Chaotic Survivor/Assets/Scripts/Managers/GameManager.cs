using System.Collections;
//using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Scripts
    [Header("Scripts")]
    private TransitionManager transitionManager = null;
    private OptionsManager optionsManager = null;
    private UiManager uiManager = null;
    private SoundManager soundManager = null;
    private SaveNLoad saveNLoad = null;
    private AbilityScriptableObject abilityScriptableObject = null;
    private LocalSettingsManager localSettingsManager = null;
    [SerializeField] private LevelManager levelManager = null;
    #endregion

    [Header("Persistent Objects")]
    [SerializeField] private GameObject[] persistentObjects = null;

    [Header("Level Data")]
    public int currentLevelNumber = 0;
    public float porcentageLevel = 0;
    public int fadeLevelAnimator = 0;
    public int playerCurLevel = 5;
    public int playerAdCurLevel = 5;
    public bool firstTimePlaying = false;
    public int instruccionInt = 0;

    [Header("Transition Controller")]
    [SerializeField] private float transitionTimer = 1f;

    [Header("Pause")]
    public float pauseTimer;
    public bool isPause = false;
    public int pauseAnimationNumber = 0;

    [Header("Player")]
    [SerializeField] private PlayerActions playerActions = null;
    [SerializeField] private PlayerAbilities abilities = null;
    [SerializeField] private MagnetItem magnetItem = null;
    [SerializeField] private SpinWeapon spinWeapon = null;
    #region Non in the inspector
    [HideInInspector] public bool isInGame = false;
    #endregion

    void Awake()
    {
        #region Scripts
        transitionManager = FindObjectOfType<TransitionManager>();
        optionsManager = FindObjectOfType<OptionsManager>();
        uiManager = FindObjectOfType<UiManager>();
        soundManager = FindObjectOfType<SoundManager>();
        levelManager = FindObjectOfType<LevelManager>();
        saveNLoad = FindObjectOfType<SaveNLoad>();
        localSettingsManager = FindObjectOfType<LocalSettingsManager>();
        #endregion        

        #region Player
        playerActions = FindObjectOfType<PlayerActions>();
        abilities = FindObjectOfType<PlayerAbilities>();
        abilityScriptableObject = FindObjectOfType<AbilityScriptableObject>();
        #endregion       

        uiManager.versionTxt.text = Application.version;
        uiManager.companyNameTxt.text = Application.companyName + " ®";        

        foreach (GameObject obj in persistentObjects)
            Object.DontDestroyOnLoad(obj);

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        LoadMainMenuStart();
              
        yield return new WaitForSeconds(uiManager.animationUiTimer);
        saveNLoad.LoadData();
        saveNLoad.LoadGameData();
        saveNLoad.LoadOptions();
        uiManager.PlayAnimations();
        
        if (saveNLoad.firstTimePlayingInt == 0)
            firstTimePlaying = true;

        if(firstTimePlaying)
        {
            uiManager.ActiveAnimationWithTimer(9);
            switch (Application.systemLanguage) 
            {
                case SystemLanguage.English:
                    Debug.Log("El idioma original es Ingles");
                    localSettingsManager.languageNumber = 0;
                    break;
                case SystemLanguage.French:
                    Debug.Log("El idioma original es Frances");
                    localSettingsManager.languageNumber = 1;
                    break;
                case SystemLanguage.German:
                    Debug.Log("El idioma original es Aleman");
                    //localSettingsManager.languageNumber = 2;
                    localSettingsManager.languageNumber = 0;
                    break;
                case SystemLanguage.Italian:
                    Debug.Log("El idioma original es Italiano");
                    localSettingsManager.languageNumber = 3;
                    break;
                case SystemLanguage.Norwegian:
                    Debug.Log("El idioma original es Noruego");
                    localSettingsManager.languageNumber = 4;
                    break;
                case SystemLanguage.Spanish:
                    Debug.Log("El idioma original es Spañol");
                    localSettingsManager.languageNumber = 5;
                    break;
                default:
                    localSettingsManager.languageNumber = 0;
                    break;
            }
        }
    }
    private void Start()
    {
            
    }

    public void Player(PlayerAbilities playerAbilities)
    {

    }


    #region Load scenes

    //Main Menu
    public void LoadMainMenuStart()
    {
        isInGame = false;

        Time.timeScale = 1f;

        isPause = false;

        transitionManager.TransitionScene(0, transitionTimer);
        //transitionManager.TransitionAsyncScenes(0);
        transitionManager.TransitionMusicMainMenuStart(0, transitionTimer);

        playerCurLevel = playerAdCurLevel;
    }
    public void LoadMainMenu()
    {
        isInGame = false;

        Time.timeScale = 1f;

        isPause = false;

        transitionManager.TransitionScene(0, transitionTimer);
        //transitionManager.TransitionAsyncScenes(0);
        transitionManager.TransitionMusic(0, transitionTimer);

        saveNLoad.LoadData();
        playerCurLevel = playerAdCurLevel;
    }

    //Level
    public void LoadLevel()
    {
        currentLevelNumber = Random.Range(0, transitionManager.levels.Length);

        isInGame = true;

        Time.timeScale = 1f;


        if (!firstTimePlaying)
        {
            transitionManager.TransitionScene(currentLevelNumber, transitionTimer);
            //transitionManager.TransitionAsyncScenes(currentLevelNumber);
            transitionManager.TransitionMusic(currentLevelNumber, transitionTimer);
            uiManager.ActiveAnimation(0);
#if UNITY_ANDROID
            PlayGameLogros.instance.WinAchivement();
#endif
        }
        else
        {
            uiManager.ActiveAnimation(instruccionInt);
            ActiveContinue(true);          
        }
    }

    public void NofirstTime()
    {
        firstTimePlaying = false;
        saveNLoad.firstTimePlayingInt++;
        saveNLoad.SaveGameData();
    }

    public void ActiveContinue(bool active)
    {
        optionsManager.continueBtn.SetActive(active);
    }

    //Exit Level
    public void ExitLevel()
    {
        isInGame = false;

        Time.timeScale = 1f;

        isPause = false;

        transitionManager.TransitionScene(0, transitionTimer);
        transitionManager.TransitionMusic(0, transitionTimer);

        levelManager = null;
    }

    //Restart Level 
    public void RestartLevel()
    {
        isInGame = true;

        Time.timeScale = 1f;

        isPause = false;

        transitionManager.TransitionScene(currentLevelNumber, transitionTimer);
        transitionManager.TransitionMusic(currentLevelNumber, transitionTimer);
        abilityScriptableObject.isMagnetActive = false;
        abilityScriptableObject.isSpinActive = false;
        playerCurLevel = playerAdCurLevel;
    }
    #endregion

    #region Pause
    public void PauseNResume()
    {
        isPause = !isPause;
        StartCoroutine(PauseDelay());
    }

    IEnumerator PauseDelay()
    {
        uiManager.ActiveAnimation(pauseAnimationNumber);
        yield return new WaitForSecondsRealtime(pauseTimer);
        Time.timeScale = isPause ? 0f : 1f;
    }

    IEnumerator Pause()
    {
        yield return null;
    }
    #endregion

    #region Level Manager
    public void SetLevelManager(LevelManager newLevelManageR)
    {
        levelManager = newLevelManageR;
    }

    public void ResumeFromLevelUp()
    {
        levelManager.ResumeFromLevelUp();
    }
    #endregion

    #region Player 
    //Abilities
    public void SetPlayerAbilities(PlayerAbilities newAbilities)
    {
        abilities = newAbilities;
    }

    public void SelectAbility(int option)
    {

    }

    //Action
    public void SetPlayerAction(PlayerActions newAction)
    {
        playerActions = newAction;
    }

    public void SelectAction(int option)
    {

    }

    //Spin Weapon
    public void SetSpinWeapon(SpinWeapon newSpinW)
    {
        spinWeapon = newSpinW;
    }

    public void SelectSpin(int option)
    {

    }
    #endregion

    #region Links
    public void Link(string link)
    {
        Application.OpenURL(link);
    }
    #endregion

    #region Exit
    public void AppExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    #endregion
}
