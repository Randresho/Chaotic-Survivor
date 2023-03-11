using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ActiveUI { MainMenu, Options, Credits, InGame, LevelUp, Pause, HowToPlay, GameOver}
public class BackButtonMobile : MonoBehaviour
{
    private UiManager uiManager;

    //[SerializeField] private UnityEvent onBackKey;
    public ActiveUI activeUI;
    [SerializeField] private ActivateUi[] activateUis;

    public void ChooseUi(int _activeUI)
    {
        activeUI = (ActiveUI)_activeUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        switch (activeUI)
        {
            case ActiveUI.MainMenu:
                activateUis[0].OnActiveUi();
                break;
            case ActiveUI.Options:
                activateUis[1].OnActiveUi();
                break;
            case ActiveUI.Credits:
                activateUis[2].OnActiveUi();
                break;
            case ActiveUI.InGame:
                activateUis[3].OnActiveUi();
                break;
            case ActiveUI.LevelUp:
                activateUis[4].OnActiveUi();
                break;
            case ActiveUI.Pause:
                activateUis[5].OnActiveUi();
                break;
            case ActiveUI.HowToPlay:
                activateUis[6].OnActiveUi();
                break;
            case ActiveUI.GameOver:
                activateUis[7].OnActiveUi();
                break;
            default:
                break;
        }
        
    }
}

[System.Serializable]
public class ActivateUi
{
    public string uiName;
    public UnityEvent onBackKey;

    public void SetBackEvent(UnityEvent backEvent)
    {
        onBackKey = backEvent;
    }

    public void OnActiveUi()
    {
        onBackKey.Invoke();
    }
}
