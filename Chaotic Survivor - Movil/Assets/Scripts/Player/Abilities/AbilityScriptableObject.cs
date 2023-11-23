using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public enum AbilityType { Bullet, Normal, Spin, RandomAbility, Magnet }
public class AbilityScriptableObject : MonoBehaviour
{
    #region Languages
    [System.Serializable]
    public class Language
    {
        public string name;
        public string title;
        public string[] description;
    }

    [System.Serializable]
    public class RandomDescriptionLanguage
    {
        public string name;
        public string[] description;
    }

    [System.Serializable]
    public class RandomLanguage
    {
        public string name;
        public string[] title;
        public RandomDescriptionLanguage[] description;
    }
    #endregion

    #region Scripts
    [SerializeField] private LevelManager levelManager;
    private UiManager uiManager;
    [SerializeField] private LocalSettingsManager localSettingsManager;
    #endregion 

    #region Active Abilities
    [Header("Active Abilities")]
    public bool isMagnetActive;
    public bool isSpinActive;
    #endregion

    #region Buttons
    [Header("Buttons")]
    public GameObject[] optionsButtons;
    public AbiltyButtonInfo[] buttons;
    [Space]
    public GameObject changeOptionsObj;
    [Space]
    public GameObject[] buttonActivatorObj;
    public AbiltyButtonInfo[] buttonsAcivator;
    #endregion

    #region Abilities Types
    [Header("Abilities Type")]
    public int minAbilities;
    public int minCurAbilities;
    public int maxAbilities;
    public int maxCurAbilities;
    #endregion

    #region Abilities
    //To Fix this
    #region Magnet
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
    #endregion

    #region Spin
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
    #endregion

    #region Normal Shoot
    [Header("Normal Shoot")]
    [SerializeField] private PlayerAbilities abilities;
    [Space]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private string normalTitle;
    [SerializeField] private string normalDescription;
    [SerializeField] private Language[] _normalLanguage;
    #endregion

    #region Random Abilities
    [Header("Rando Abilities")]
    [SerializeField] private RandomAbilities randomAbilities = null;
    [SerializeField] private Sprite[] randomAbilitySprite = null;
    [SerializeField] private int randomAbilityCurChoose;
    [SerializeField] private int randomAbilityChoose;
    //Electro Shock
    [Space]
    [SerializeField] private float electroShockDamage;
    [SerializeField] private float electroShockTimer;
    //Freeze
    [Space]
    [SerializeField] private float freezeTimer;
    //Burn
    [Space]
    [SerializeField] private float burnDamage;
    [SerializeField] private float burnTimer;
    //Enemies
    [Space]
    [SerializeField] private int enemiesLimit;
    [SerializeField] private float manaLimit;
    [Space]

    [SerializeField] private string[] randomAbilityTitle;
    [SerializeField] private RandomLanguage[] _randomAbilitiesLanguage;
    #endregion
    #endregion

    #region Bullets
    [Header("Bullets")]
    [SerializeField] private GameObject[] bulletPrefab;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private string bulletTitle;
    [SerializeField] private string[] bulletDescription;
    [SerializeField] private Language[] bulletLanguage;
    [Space]
    public float shootTimer = 0.05f;
    public float shootMaxTimer = 0.1f;
    [Space]
    [SerializeField] private int bulletCurChoose;
    [SerializeField] private int bulletChoose;
    public float damage;
    public int maxEnemiesHit;
    public float bulletSpeed;
    #endregion

    #region Updates
    [Header("Update")]
    public int abiltityRandomA;
    public int abiltityRandomB;
    [Space]
    [SerializeField] private int ChangeOption;
    [SerializeField] private int maxChangeOption = 3;
    #endregion
    
    [SerializeField] private PlayerActions playerActions;

    private void Awake()
    {
        localSettingsManager = FindObjectOfType<LocalSettingsManager>();
        uiManager = FindObjectOfType<UiManager>();
        changeOptionsObj.SetActive(false);
    }

    private void UpdateData()
    {
        while (abiltityRandomB == abiltityRandomA)
        {
            abiltityRandomB = Random.Range(minAbilities, maxAbilities);
            return;
        }
        Debug.Log("No eran iguales");

        //Update
        if (isMagnetActive && isSpinActive)
        {
            //Debug.Log("Ambas armas estan activadas");

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
        else if (isMagnetActive && !isSpinActive)
        {
            buttons[0].type = (AbilityType)abiltityRandomA;
            buttons[1].type = AbilityType.Spin;
        }
        else if (!isMagnetActive && isSpinActive)
        {
            buttons[0].type = AbilityType.Magnet;
            buttons[1].type = (AbilityType)abiltityRandomB;
        }
        //f (!isMagnetActive && !isSpinActive)
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
            case AbilityType.RandomAbility:
                buttons[0].SetInfo(randomAbilitySprite[randomAbilityChoose], _randomAbilitiesLanguage[localSettingsManager.languageNumber].title[randomAbilityChoose], _randomAbilitiesLanguage[localSettingsManager.languageNumber].description[randomAbilityChoose].description[randomAbilityCurChoose]);
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
            case AbilityType.RandomAbility:
                buttons[1].SetInfo(randomAbilitySprite[randomAbilityChoose], _randomAbilitiesLanguage[localSettingsManager.languageNumber].title[randomAbilityChoose], _randomAbilitiesLanguage[localSettingsManager.languageNumber].description[randomAbilityChoose].description[randomAbilityCurChoose]);
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

    public void SetRandomAbilities(RandomAbilities newRandomAbilities)
    {
        randomAbilities = newRandomAbilities;
    }
    #endregion

    #region Select Options
    public void SelectRandomNumbers()
    {

        //Magnet Check
        if (magnetItem.transform.localScale.x >= increaseMaxMagnet && magnetItem.transform.localScale.y >= increaseMaxMagnet && magnetItem.transform.localScale.z >= increaseMaxMagnet)
            maxCurAbilities = 4;
        else
            maxCurAbilities = maxAbilities;

        //Spin Check
        if (spinWeapon.currentBulletPoint <= 0)
            spinCurChoose = 2;

        if (spinWeapon.rotationSpeed >= increaseSpinMaxSpeed)
            spinCurChoose = 3;

        //Enemies check
        if (randomAbilities.enemyCountLimit >= levelManager.maxEnemiesSpawn)
        {
            while (randomAbilityChoose == 3)
            {
                randomAbilityChoose = Random.Range(0, randomAbilityTitle.Length);
                randomAbilityCurChoose = Random.Range(0, (_randomAbilitiesLanguage[randomAbilityChoose].description[randomAbilityChoose].description.Length));
                return;
            }
        }

        //Random
        abiltityRandomA = Random.Range(minCurAbilities, maxCurAbilities);
        abiltityRandomB = Random.Range(minCurAbilities, maxCurAbilities);

        bulletChoose = Random.Range(0, bulletDescription.Length);
        magnetChoose = Random.Range(magnetCurChoose, magnetDescription.Length);
        spinChoose = Random.Range(spinCurChoose, spinDescription.Length);
        randomAbilityChoose = Random.Range(0, randomAbilityTitle.Length);
        randomAbilityCurChoose = Random.Range(0, (_randomAbilitiesLanguage[randomAbilityChoose].description[randomAbilityChoose].description.Length));

        //Acctive Magnet
        if (!isMagnetActive)
        {
            buttonsAcivator[0].SetInfo(magnetSprite, _magnetLanguage[localSettingsManager.languageNumber].title, _magnetLanguage[localSettingsManager.languageNumber].description[0]);
            buttonActivatorObj[0].SetActive(true);
        }

        //Active Spin
        if (!isSpinActive)
        {
            buttonsAcivator[1].SetInfo(spinSprite, spinLanguage[localSettingsManager.languageNumber].title, spinLanguage[localSettingsManager.languageNumber].description[0]);
            buttonActivatorObj[1].SetActive(true);
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

        UpdateData();
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
                //Spin
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
                //Magnet
                case AbilityType.Magnet:
                    magnetItem.transform.localScale += new Vector3(increaseMagnet, increaseMagnet, increaseMagnet);
                    break;
                //Random
                case AbilityType.RandomAbility:
                    switch (randomAbilityChoose)
                    {
                        case 0: //Electro Shock
                            switch (randomAbilityCurChoose)
                            {
                                case 0:
                                    randomAbilities.electroShockDamage += electroShockDamage;
                                    break;
                                case 1:
                                    randomAbilities.maxElectroShockTimer += electroShockTimer;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 1: //Freeze
                            switch (randomAbilityCurChoose)
                            {
                                case 0:
                                    randomAbilities.maxFreezeTimer += freezeTimer;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 2: //Burn
                            switch (randomAbilityCurChoose)
                            {
                                case 0:
                                    randomAbilities.burnDamage += burnDamage;
                                    break;
                                case 1:
                                    randomAbilities.maxBurningTimer += burnTimer;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case 3: //Enemies
                            switch (randomAbilityCurChoose)
                            {
                                case 0:
                                    randomAbilities.enemyCountLimit += enemiesLimit;
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case 4: //Mana
                            switch (randomAbilityCurChoose)
                            {
                                case 0:
                                    levelManager.playerMaxMana += manaLimit;
                                    uiManager.manaSlider.maxValue = levelManager.playerMaxMana;
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }
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
