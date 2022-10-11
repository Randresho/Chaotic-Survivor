using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalSettingsManager : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField] private bool active = false;
    public int languageNumber;

    private void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        languageNumber = PlayerPrefs.GetInt("Language", 0);
        ChangeLocale(languageNumber);
    }

    public void ChangeLocale(int idx)
    {
        if (active)
            return;
        StartCoroutine(SetLocal(idx));
    }

    IEnumerator SetLocal(int idx)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[idx];
        PlayerPrefs.SetInt("Language", idx);
        active = false;
    }
}
