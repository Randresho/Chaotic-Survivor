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
    public int musicInt;
    [Space]
    public bool muteSFX = false;
    public int sfxInt;
    [Space]
    public bool easyMod = false;
    public int easyModInt;

    [Header("Game Objects")]
    public GameObject continueBtn;

    // Start is called before the first frame update
    void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        saveNLoad = FindObjectOfType<SaveNLoad>();
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
        muteMusic = uiManager.muteMusic.isOn;  
        
        if(muteMusic)
            musicInt = 1;
        else
            musicInt = 0;

        saveNLoad.SaveMusic();
    }

    public void MuteSFXDisplay()
    {
        muteSFX = uiManager.muteSFX.isOn;

        if(muteSFX)
            sfxInt = 1; 
        else
            sfxInt = 0;

        saveNLoad.SaveSFX();
    }

    public void UseEasyMode()
    {
        easyMod = uiManager.easyModTgl.isOn;

        if(easyMod)
            easyModInt = 1;
        else
            easyModInt = 0;

        saveNLoad.SaveEasyMode();
    }
}
