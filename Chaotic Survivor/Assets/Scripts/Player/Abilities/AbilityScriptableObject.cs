using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public enum AbilityType { Bullet, Normal, Spin, Magnet, }
public class AbilityScriptableObject : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private LocalSettingsManager localSettingsManager;

    [System.Serializable]
    public class Language
    {
        public string name;
        public string title;
        public string[] description;
    }

    public AbiltyButtonInfo[] buttons;
    
    [Header("Abilities Type")]
    public int minAbilities;
    public int minCurAbilities;
    public int maxAbilities;
    public int maxCurAbilities;

    [Header("Active Abilities")]
    public bool isMagnetActive;
    public bool isSpinActive;
    public GameObject[] buttonActivatorObj;
    public AbiltyButtonInfo[] buttonsAcivator;

    [Header("Magnet")]
    [SerializeField] private MagnetItem magnetItem;
    [SerializeField] private float increaseMagnet = 0.25f;
    [SerializeField] private float increaseMaxMagnet = 3f;
    [Space]
    [SerializeField] private int magnetCurChoose;
    [SerializeField] private int magnetChoose;
    [SerializeField] private Sprite magnetSprite;
    [SerializeField] private string magnetTitle;
    [SerializeField] private string[] magnetDescription;
    [SerializeField] private Language[] _magnetLanguage;
   

    [Header("Spin")]
    [SerializeField] private SpinWeapon spinWeapon = null;
    [SerializeField] private int increaseWeapon = 1;
    [SerializeField] private float increaseSpinSpeed;
    [SerializeField] private float increaseSpinMaxSpeed;
    [Space]
    [SerializeField] private int spinCurChoose;
    [SerializeField] private int spinChoose;
    [SerializeField] private Sprite spinSprite;
    [SerializeField] private string spinTitle;
    [SerializeField] private string[] spinDescription;
    [SerializeField] private Language[] spinLanguage;

    [Header("Normal Shoot")]
    [SerializeField] private PlayerAbilities abilities;
    [Space]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private string normalTitle;
    [SerializeField] private string normalDescription;
    [SerializeField] private Language[] _normalLanguage;

    /*[Header("Life")]
    [SerializeField] private int lifeRandom;
    [Space]
    [SerializeField] private int lifeCurChoose;
    [SerializeField] private int lifeChoose;
    [SerializeField] private int[] lifeAdder;
    [SerializeField] private Sprite lifeSprite;
    [SerializeField] private string lifeTitle;
    [SerializeField] private string[] lifeDescription;*/

    [Header("Buttons")]
    [SerializeField] private GameObject[] bulletPrefab;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private string bulletTitle;
    [SerializeField] private string[] bulletDescription;
    [SerializeField] private Language[] bulletLanguage;
    [Space]
    public GameObject[] optionsButtons;
    public float shootTimer = 0.05f;
    public float shootMaxTimer = 0.1f;
    [Space]
    [SerializeField] private int bulletCurChoose;
    [SerializeField] private int bulletChoose;
    public float damage;
    public int maxEnemiesHit;
    public float bulletSpeed;

    [Header("Update")]
    public int abiltityRandomA;
    public int abiltityRandomB;

    [Space]
    [SerializeField] private int ChangeOption;
    [SerializeField] private int maxChangeOption = 3;
    public GameObject changeOptionsObj;

    
    [SerializeField] private PlayerActions playerActions;

    private void Awake()
    {
        localSettingsManager = FindObjectOfType<LocalSettingsManager>();
        changeOptionsObj.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (abiltityRandomB == abiltityRandomA)
            abiltityRandomB = Random.Range(minAbilities, maxAbilities);

        //Update
        if (isMagnetActive && isSpinActive)
        {
            Debug.Log("Ambas armas estan activadas");

            buttons[0].type = (AbilityType)abiltityRandomA;
            buttons[1].type = (AbilityType)abiltityRandomB;

            if (ChangeOption > maxChangeOption)
            {
                changeOptionsObj.SetActive(false);
            }
            else
            {
                changeOptionsObj.SetActive(true);
            }
        }
        else if(isMagnetActive && !isSpinActive)
        {
            buttons[0].type = (AbilityType)abiltityRandomA;
            buttons[1].type = AbilityType.Spin;
            maxCurAbilities = 3;
        }
        else if(!isMagnetActive && isSpinActive)
        {
            buttons[0].type = AbilityType.Magnet;
            buttons[1].type = (AbilityType)abiltityRandomB;
        }
        else
        {
            changeOptionsObj.SetActive(false);
        }

        switch (buttons[0].type)
        {
            case AbilityType.Bullet:
                buttons[0].SetInfo(bulletSprite, bulletLanguage[localSettingsManager.languageNumber].title, bulletLanguage[localSettingsManager.languageNumber].description[bulletChoose]);
                break;
            case AbilityType.Normal:
                buttons[0].SetInfo(normalSprite, _normalLanguage[localSettingsManager.languageNumber].title, _normalLanguage[localSettingsManager.languageNumber].description[0]);
                break;
            case AbilityType.Spin:
                buttons[0].SetInfo(spinSprite, spinLanguage[localSettingsManager.languageNumber].title, spinLanguage[localSettingsManager.languageNumber].description[spinChoose]);
                break;
            case AbilityType.Magnet:
                buttons[0].SetInfo(magnetSprite, _magnetLanguage[localSettingsManager.languageNumber].title, _magnetLanguage[localSettingsManager.languageNumber].description[magnetChoose]);
                break;
            default:
                break;
        }

        switch (buttons[1].type)
        {
            case AbilityType.Bullet:
                buttons[1].SetInfo(bulletSprite, bulletLanguage[localSettingsManager.languageNumber].title, bulletLanguage[localSettingsManager.languageNumber].description[bulletChoose]);
                break;
            case AbilityType.Normal:
                buttons[1].SetInfo(normalSprite, _normalLanguage[localSettingsManager.languageNumber].title, _normalLanguage[localSettingsManager.languageNumber].description[0]);
                break;
            case AbilityType.Spin:
                buttons[1].SetInfo(spinSprite, spinLanguage[localSettingsManager.languageNumber].title, spinLanguage[localSettingsManager.languageNumber].description[spinChoose]);
                break;
            case AbilityType.Magnet:
                buttons[1].SetInfo(magnetSprite, _magnetLanguage[localSettingsManager.languageNumber].title, _magnetLanguage[localSettingsManager.languageNumber].description[magnetChoose]);
                break;
            default:
                break;
        }
    }

    #region Set scripts
    public void SetLevelManager(LevelManager newLevelManageR)
    {
        levelManager = newLevelManageR;
        for (int i = 0; i < bulletPrefab.Length; i++)
        {
            bulletPrefab[i].GetComponent<BulletBehavior>().damage = bulletPrefab[i].GetComponent<BulletBehavior>().damageDefault;
            bulletPrefab[i].GetComponent<BulletBehavior>().maxEnemiesHit = bulletPrefab[i].GetComponent<BulletBehavior>().maxEnemiesHitDefault;
            bulletPrefab[i].GetComponent<BulletBehavior>().maxEnemiesHit = bulletPrefab[i].GetComponent<BulletBehavior>().maxEnemiesHitDefault;
        }

        isMagnetActive = false;
        isSpinActive = false;
    }

    public void SetPlayerAbilities(PlayerAbilities newAbilities)
    {
        abilities = newAbilities;
    }

    public void SetPlayerActions(PlayerActions newActions)
    {
        playerActions = newActions;
    }

    public void SetMagnet(MagnetItem newMagnetItem)
    {
        magnetItem = newMagnetItem;
    }

    public void SetSpinWeapon(SpinWeapon newSpinWeapon)
    {
        spinWeapon = newSpinWeapon;
    }
    #endregion


    #region Select Options
    public void SelectRandomNumbers()
    {
        //Magnet Check
        if (magnetItem.transform.localScale.x <= increaseMaxMagnet || magnetItem.transform.localScale.y <= increaseMaxMagnet || magnetItem.transform.localScale.z <= increaseMaxMagnet)
            maxCurAbilities = 3;
        else
            maxCurAbilities = maxAbilities;  

        //Spin Check
        if (spinWeapon.currentBulletPoint <= 0)
            spinCurChoose = 2;

        if (spinWeapon.rotationSpeed >= increaseSpinMaxSpeed)
            spinCurChoose = 3;

        //Random
        abiltityRandomA = Random.Range(minCurAbilities, maxCurAbilities);
        abiltityRandomB = Random.Range(minCurAbilities, maxCurAbilities);
        //lifeRandom = Random.Range(0, buttons.Length);
        bulletChoose = Random.Range(0, bulletDescription.Length);
        magnetChoose = Random.Range(magnetCurChoose, magnetDescription.Length);
        spinChoose = Random.Range(spinCurChoose, spinDescription.Length);
        //lifeChoose = Random.Range(lifeCurChoose, lifeTitle.Length);

        //Acctive Magnet
        if (!isMagnetActive)
        {
            //buttonsAcivator[0].SetInfo(magnetSprite, magnetTitle, magnetDescription[0]);
            buttonsAcivator[0].SetInfo(magnetSprite, _magnetLanguage[localSettingsManager.languageNumber].title, _magnetLanguage[localSettingsManager.languageNumber].description[0]);
            buttonActivatorObj[0].SetActive(true);
            //optionsButtons[0].SetActive(false);
        }

        //Active Spin
        if (!isSpinActive)
        {
            //buttonsAcivator[1].SetInfo(spinSprite, spinTitle, spinDescription[0]);
            buttonsAcivator[1].SetInfo(spinSprite, spinLanguage[localSettingsManager.languageNumber].title, spinLanguage[localSettingsManager.languageNumber].description[0]);
            buttonActivatorObj[1].SetActive(true);
            //optionsButtons[1].SetActive(false);
        }

        //Restore life
        if (isMagnetActive && isSpinActive)
        {
            for (int i = 0; i < optionsButtons.Length; i++)
            {
                optionsButtons[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < optionsButtons.Length; i++)
            {
                optionsButtons[i].SetActive(true);
            }
        }

        //Change Add
        ChangeOption++;
    }
    #endregion

    #region Active abilities
    public void ActiveAbilityOne()
    {
        isMagnetActive = true;
        buttonActivatorObj[0].SetActive(false);
        //buttonsAcivator[0].SetInfo(spinSprite, spinLanguage[localSettingsManager.languageNumber].title, spinLanguage[localSettingsManager.languageNumber].description[spinChoose]);
        abilities.enableAbilities[1] = true;
        ChangeOption = 0;
    }

    public void ActiveAbilityTwo()
    {
        isSpinActive = true;
        buttonActivatorObj[1].SetActive(false);
        //buttonsAcivator[1].SetInfo(spinSprite, spinLanguage[localSettingsManager.languageNumber].title, spinLanguage[localSettingsManager.languageNumber].description[spinChoose]);
        spinWeapon.currentBulletPoint -= increaseWeapon;
        ChangeOption = 0;
    }
    #endregion

    #region Update Abilities
    public void UpdateAbility()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            switch (buttons[i].type)
            {
                //Bullet
                case AbilityType.Bullet:
                    for (int B = 0; B < bulletPrefab.Length; B++)
                    {
                        switch (bulletChoose)
                        {
                            case 0:
                                for (int b = 0; b < bulletPrefab.Length; b++)
                                {
                                    bulletPrefab[b].GetComponent<BulletBehavior>().damage += damage;
                                }
                                break;
                            case 1:
                                for (int b = 0; b < bulletPrefab.Length; b++)
                                {
                                    bulletPrefab[b].GetComponent<BulletBehavior>().maxEnemiesHit += maxEnemiesHit;
                                }
                                break;
                            case 2:
                                for (int b = 0; b < bulletPrefab.Length; b++)
                                {
                                    bulletPrefab[b].GetComponent<BulletBehavior>().bulletSpeed += bulletSpeed;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                //Normal
                case AbilityType.Normal:
                    if (abilities.timerToSpawn >= shootMaxTimer)
                    {
                        abilities.timerToSpawn -= shootTimer;
                    }
                    else
                        abilities.timerToSpawn = shootMaxTimer;
                    break;
                case AbilityType.Spin:
                    switch (spinChoose)
                    {
                        case 0:
                            break;
                        case 1:
                            spinWeapon.currentBulletPoint -= increaseWeapon;
                            break;
                        case 2:
                            spinWeapon.rotationSpeed += increaseSpinSpeed;
                            break;
                        case 3:
                            spinWeapon.timerToSpawn -= shootTimer;
                            break;
                        default:
                            break;
                    }
                    break;
                case AbilityType.Magnet:
                    magnetItem.transform.localScale += new Vector3(increaseMagnet,increaseMagnet, increaseMagnet);
                    break;
                default:
                    break;
            }
            optionsButtons[i].SetActive(false);
        }

        ChangeOption = 0;
    }
    #endregion
}
