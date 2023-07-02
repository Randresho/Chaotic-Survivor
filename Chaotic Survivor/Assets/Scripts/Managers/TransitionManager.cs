using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    #region Scripts
    private GameManager gameManager = null;
    private UiManager uiManager = null;
    private SoundManager soundManager = null;
    #endregion

    [Header("Transitions")]
    [SerializeField] private Animator transitionScene = null;
    [SerializeField] private Animator transitionMusic = null;
    [Space]
    [SerializeField] private GameObject loadingScreen; 

    [Header("Scenes & Levels")]
    [SerializeField] private string[] scenes = null;
    public string[] levels = null;

    // Start is called before the first frame update
    void Awake()
    {
        #region Scripts
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UiManager>();
        soundManager = FindObjectOfType<SoundManager>();
        #endregion
    }

    //Scenes
    #region Main Menu & Levels
    public void TransitionAsyncScenes(int idx)
    {
        StartCoroutine(LoadAsynchronously(idx));
    }

    IEnumerator LoadAsynchronously(int idx)
    {
        AsyncOperation operation;

        loadingScreen.SetActive(true);

        if (gameManager.isInGame)
        {
            operation = SceneManager.LoadSceneAsync("Scenes/Levels/" + levels[idx]);
            //SceneManager.LoadScene("Scenes/Levels/" + levels[idx]);
            //Ui
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);

                uiManager.loadingSlider.value = progress;
                uiManager.loadingText.text = progress * 100f + "%";

                yield return null;
            }
        }
        else
        {
            operation = SceneManager.LoadSceneAsync("Scenes/Menus/" + scenes[idx]);
            //SceneManager.LoadScene("Scenes/Menus/" + scenes[idx]);
            //Ui
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);

                uiManager.loadingSlider.value = progress;
                uiManager.loadingText.text = progress * 100f + "%";

                yield return null;
            }
        }
    }

    public void TransitionScene(int idx, float timer)
    {
       StartCoroutine(LoadScene(idx, timer));
    }
    IEnumerator LoadScene(int idx, float timer)
    {
        transitionScene.SetTrigger("Start");
        yield return new WaitForSeconds(timer);
        if (gameManager.isInGame)
        {
            SceneManager.LoadScene("Scenes/Levels/" + levels[idx]);
            //Ui
        }
        else
        {
            SceneManager.LoadScene("Scenes/Menus/" + scenes[idx]);
            //Ui
        }
        transitionScene.SetTrigger("End");
    }
    #endregion

    //Music
    #region Music
    public void TransitionMusic(int idx, float timer)
    {
        StartCoroutine(LoadMusic(idx, timer));
    }

    IEnumerator LoadMusic(int idx, float timer)
    {
        transitionMusic.SetTrigger("FadeStart");
        yield return new WaitForSeconds(timer);
        if (gameManager.isInGame)
            soundManager.ChangeSongInLevel();
        else
            soundManager.ChangeSongMainMenu();
        transitionMusic.SetTrigger("FadeEnd");
    }

    public void TransitionMusicMainMenuStart(int idx, float timer)
    {
        StartCoroutine(LoadMusicMainMenuStart(idx, timer));
    }

    IEnumerator LoadMusicMainMenuStart(int idx, float timer)
    {
        transitionMusic.SetTrigger("FadeStart");
        yield return new WaitForSeconds(timer);
        if (gameManager.isInGame)
        {
            soundManager.ChangeSongInLevel();
        }
        else
        {
            soundManager.music.clip = soundManager.menus[idx];
            soundManager.music.Play();
        }
        transitionMusic.SetTrigger("FadeEnd");
    }
    #endregion
}
