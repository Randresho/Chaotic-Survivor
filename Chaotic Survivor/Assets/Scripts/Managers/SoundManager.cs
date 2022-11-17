using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private GameManager gameManager;

    public AudioClip[] menus;
    public AudioClip[] levels;

    public AudioMixer audioMixer = null;

    [HideInInspector] public AudioSource music = null;
    public AudioSource buttonSFX = null;

    private int musicIdx = 0;
    public bool isPlayingSong { get; private set; }

    void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        music = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        audioMixer.SetFloat("Music", uiManager.musicSlider.value);
        audioMixer.SetFloat("SoundFX", uiManager.sfxSlider.value);

        if(uiManager.musicSlider.value == -20)
            audioMixer.SetFloat("Music", -80f);

        if (uiManager.sfxSlider.value == -20)
            audioMixer.SetFloat("SoundFX", -80f);

        /*if (gameManager.isInGame)
        {
            ChangeSong();
        }*/

        #region Music Volume
        switch (uiManager.musicSlider.value)
        {
            case -20:
                uiManager.musicVolumeTxt.text = "0";
                break;
            case -19:
                uiManager.musicVolumeTxt.text = "0.5";
                break;
            case -18:
                uiManager.musicVolumeTxt.text = "1";
                break;
            case -17:
                uiManager.musicVolumeTxt.text = "1.5";
                break;
            case -16:
                uiManager.musicVolumeTxt.text = "2";
                break;
            case -15:
                uiManager.musicVolumeTxt.text = "2.5";
                break;
            case -14:
                uiManager.musicVolumeTxt.text = "3";
                break;
            case -13:
                uiManager.musicVolumeTxt.text = "3.5";
                break;
            case -12:
                uiManager.musicVolumeTxt.text = "4";
                break;
            case -11:
                uiManager.musicVolumeTxt.text = "4.5";
                break;
            case -10:
                uiManager.musicVolumeTxt.text = "5";
                break;
            case -9:
                uiManager.musicVolumeTxt.text = "5.5";
                break;
            case -8:
                uiManager.musicVolumeTxt.text = "6";
                break;
            case -7:
                uiManager.musicVolumeTxt.text = "6.5";
                break;
            case -6:
                uiManager.musicVolumeTxt.text = "7";
                break;
            case -5:
                uiManager.musicVolumeTxt.text = "7.5";
                break;
            case -4:
                uiManager.musicVolumeTxt.text = "8";
                break;
            case -3:
                uiManager.musicVolumeTxt.text = "8.5";
                break;
            case -2:
                uiManager.musicVolumeTxt.text = "9";
                break;
            case -1:
                uiManager.musicVolumeTxt.text = "9.5";
                break;
            case 0:
                uiManager.musicVolumeTxt.text = "10";
                break;
            default:
                break;
        }
        #endregion
        #region SFX Volume
        switch (uiManager.sfxSlider.value)
        {
            case -20:
                uiManager.sfxVolumeTxt.text = "0";
                break;
            case -19:
                uiManager.sfxVolumeTxt.text = "0.5";
                break;
            case -18:
                uiManager.sfxVolumeTxt.text = "1";
                break;
            case -17:
                uiManager.sfxVolumeTxt.text = "1.5";
                break;
            case -16:
                uiManager.sfxVolumeTxt.text = "2";
                break;
            case -15:
                uiManager.sfxVolumeTxt.text = "2.5";
                break;
            case -14:
                uiManager.sfxVolumeTxt.text = "3";
                break;
            case -13:
                uiManager.sfxVolumeTxt.text = "3.5";
                break;
            case -12:
                uiManager.sfxVolumeTxt.text = "4";
                break;
            case -11:
                uiManager.sfxVolumeTxt.text = "4.5";
                break;
            case -10:
                uiManager.sfxVolumeTxt.text = "5";
                break;
            case -9:
                uiManager.sfxVolumeTxt.text = "5.5";
                break;
            case -8:
                uiManager.sfxVolumeTxt.text = "6";
                break;
            case -7:
                uiManager.sfxVolumeTxt.text = "6.5";
                break;
            case -6:
                uiManager.sfxVolumeTxt.text = "7";
                break;
            case -5:
                uiManager.sfxVolumeTxt.text = "7.5";
                break;
            case -4:
                uiManager.sfxVolumeTxt.text = "8";
                break;
            case -3:
                uiManager.sfxVolumeTxt.text = "8.5";
                break;
            case -2:
                uiManager.sfxVolumeTxt.text = "9";
                break;
            case -1:
                uiManager.sfxVolumeTxt.text = "9.5";
                break;
            case 0:
                uiManager.sfxVolumeTxt.text = "10";
                break;
            default:
                break;
        }
        #endregion
    }

    public void OnPressButton(AudioClip clip)
    {
        buttonSFX.clip = clip;
        buttonSFX.Play();
    }

    public void ChangeSong()
    {
        music.Stop();
        musicIdx = (musicIdx + 1) % levels.Length;
        music.clip = levels[musicIdx];
        music.Play();
        Debug.Log("Ahora suena " + music.clip.name + " con una duracion de " + music.clip.length);
        //StartCoroutine(ChangeSongPlaying());
    }

    private IEnumerator ChangeSongPlaying()
    {
        Debug.Log("Ahora suena " + music.clip.name + " con una duracion de " + music.clip.length);
        yield return new WaitForSeconds(music.clip.length);
        music.Stop();
        musicIdx = (musicIdx + 1) % levels.Length;
        music.clip = levels[musicIdx];
        music.Play();
    }
}
