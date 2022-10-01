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

    #region Choose Levels
    public void TransitionChooseLevels(int idx, float timer)
    {
        StartCoroutine(LoadChooseLevels(idx, timer));
    }

    IEnumerator LoadChooseLevels(int idx, float timer)
    {
        transitionScene.SetTrigger("Start");
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene("Scenes/Menus" + scenes[idx]);
        //Ui
        transitionScene.SetTrigger("End");
    }
    #endregion

    #region Winning
    public void TransitionWinning(int idx, float timer)
    {
        StartCoroutine(LoadWinning(idx, timer));
    }

    IEnumerator LoadWinning(int idx, float timer)
    {
        transitionScene.SetTrigger("Start");
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene("Scenes/Menu" + scenes[idx]);
        //Ui
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
            soundManager.music.clip = soundManager.levels[Random.Range(0, soundManager.levels.Length)];
        else
            soundManager.music.clip = soundManager.menus[Random.Range(0, soundManager.menus.Length)];
        soundManager.music.Play();
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
            soundManager.music.clip = soundManager.levels[Random.Range(0, soundManager.levels.Length)];
        else
            soundManager.music.clip = soundManager.menus[idx];
        soundManager.music.Play();
        transitionMusic.SetTrigger("FadeEnd");
    }
    #endregion
}
