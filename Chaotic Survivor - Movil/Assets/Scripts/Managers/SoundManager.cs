using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private OptionsManager optionsManager;
    [SerializeField] private TransitionManager transitionManager;

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
        optionsManager = FindObjectOfType<OptionsManager>();
        transitionManager = FindObjectOfType<TransitionManager>();

        music = GetComponent<AudioSource>();
    }

    public void MuteMusic()
    {
        //Music
        if (uiManager.muteMusic.isOn)
            audioMixer.SetFloat("Music", -80);
        else
            audioMixer.SetFloat("Music", 0);
    }

    public void MuteSFX()
    {
        //Sound FX
        if (uiManager.muteSFX.isOn)
            audioMixer.SetFloat("SoundFX", -80);
        else
            audioMixer.SetFloat("SoundFX", 0);
    }

    public void OnPressButton(AudioClip clip)
    {
        buttonSFX.clip = clip;
        buttonSFX.Play();
    }

    public void ChangeSongInLevel()
    {
        StartCoroutine(ChangeToNextSongInLevel());
    }

    IEnumerator ChangeToNextSongInLevel()
    {
        transitionManager.transitionMusic.SetTrigger("FadeStart");
        yield return new WaitForSeconds(0.5f);
        music.Stop();
        musicIdx = (musicIdx + 1) % levels.Length;
        music.clip = levels[musicIdx];
        Debug.Log("Ahora suena " + music.clip.name + " con una duracion de " + music.clip.length);
        music.Play();
        transitionManager.transitionMusic.SetTrigger("FadeEnd");
    }

    public void ChangeSongMainMenu()
    {
        StartCoroutine(ChangeToNextSongMainMenu());
    }

    IEnumerator ChangeToNextSongMainMenu()
    {
        transitionManager.transitionMusic.SetTrigger("FadeStart");
        yield return new WaitForSeconds(0.5f);
        music.Stop();
        musicIdx = (musicIdx + 1) % menus.Length;
        music.clip = menus[musicIdx];
        Debug.Log("Ahora suena " + music.clip.name + " con una duracion de " + music.clip.length);
        music.Play();
        transitionManager.transitionMusic.SetTrigger("FadeEnd");
    }
}
