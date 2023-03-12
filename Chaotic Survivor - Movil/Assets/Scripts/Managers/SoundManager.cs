using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private OptionsManager optionsManager;

    public AudioClip[] menus;
    public AudioClip[] levels;

    public AudioMixer audioMixer = null;

    [HideInInspector] public AudioSource music = null;
    public AudioSource buttonSFX = null;

    void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        optionsManager = FindObjectOfType<OptionsManager>();

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
}
