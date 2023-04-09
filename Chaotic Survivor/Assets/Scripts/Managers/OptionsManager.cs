using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    private UiManager uiManager;
    private SaveNLoad saveNLoad;

    public bool useMilliseconds = false;
    public int millisecondsInt;
    [Space]
    public bool muteMusic = false;
    public float musicInt;
    [Space]
    public bool muteSFX = false;
    public float sfxInt;
    [Space]
    public bool autoAimBool = false;
    public int autoAimInt;

    [Header("Game Objects")]
    public GameObject continueBtn;

    public bool UseMilliseconds()
    { 
        return useMilliseconds; 
    }

    public bool MuteMusic()
    {
        return muteMusic;
    }

    public bool MuteSFX()
    {
        return muteSFX;
    }

    // Start is called before the first frame update
    void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        saveNLoad = FindObjectOfType<SaveNLoad>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseMillisecondsDispay()
    {
        useMilliseconds = uiManager.useMillisecondsTgl.isOn;

        if (useMilliseconds)
            millisecondsInt = 1;
        else
            millisecondsInt = 0;

        saveNLoad.SaveMilliseconds();
    }

    public void MuteMusicDisplay()
    {
        //muteMusic = uiManager.muteMusic.isOn;

        musicInt = uiManager.musicSlider.value;

        saveNLoad.SaveMusic();
    }

    public void MuteSFXDisplay()
    {
        //muteSFX = uiManager.muteSFX.isOn;

        sfxInt = uiManager.sfxSlider.value;

        saveNLoad.SaveSFX();
    }

    public void UseEasyMode()
    {
        autoAimBool = uiManager.autoAim.isOn;

        if (autoAimBool)
            autoAimInt = 1;
        else
            autoAimInt = 0;

        saveNLoad.SaveAutoAim();
    }
}
