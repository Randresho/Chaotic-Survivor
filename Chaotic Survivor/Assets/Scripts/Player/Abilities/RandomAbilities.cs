using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum RandomAbilityEnum{ElectroShock, Freeze, Burn, }

[RequireComponent(typeof(CircleCollider2D))]
public class RandomAbilities : MonoBehaviour
{
    #region scripts
    private PlayerInput m_PlayerInput;
    private GameManager gameManager;
    private UiManager uiManager;
    private LevelManager levelManager;
    private AbilityScriptableObject abilityScriptableObject;
    #endregion

    [Header("Abilities")]
    [SerializeField] private RandomAbilityEnum randomAbilities;
    public RandomAbilityEnum RandomAbility
    {
        get { return randomAbilities; }
    }
    [HideInInspector] public CircleCollider2D circleCollider;
    //Electro Shock
    [Space]
    public float electroShockDamage = 5f;
    public float electroShockManaUse;
    public float maxElectroShockTimer = 3f;
    //Freeze
    [Space]
    public float FreezeManaUse;
    public float maxFreezeTimer = 20f;
    //Burn
    [Space]
    public float maxBurningTimer = 15f;
    public float burnManaUse;
    public float burnDamage = 2f;
    private float maxNextBurning = 5f;
    public float MaxNextBurning
    {
        get { return maxNextBurning; }
    }

    [Header("Enemies")]
    public List<EnemyScriptableObject> enemyScripts;
    public int enemyCount;
    public int enemyCountLimit;

    [Header("Timer")]
    public float maxTimerToShowNewAbility;
    private float timerToShowNewAbility;
    private int randomAbiltyNumber;
    public int RandomAbilityNumber
    {
        get { return randomAbiltyNumber; }
    }
    private float fillAmount;
    private bool canActive = false;
    [SerializeField] private float flashTimer;

    public bool CanActive 
    { 
        get { return canActive; }
    }

    void Awake()
    {
        m_PlayerInput = new PlayerInput();

        gameManager = FindObjectOfType<GameManager>();
        gameManager.RandomAbilities(this);
        uiManager = FindObjectOfType<UiManager>();
        levelManager = FindObjectOfType<LevelManager>();
        abilityScriptableObject = FindObjectOfType<AbilityScriptableObject>();
        abilityScriptableObject.SetRandomAbilities(this);
        
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = 2f;

        SetNewAbility();
    }

    private void FixedUpdate()
    {
        if (levelManager.TimerRunning)
        {
            if (canActive)
            {
                //Cooldown
                if (!uiManager.manaButton.interactable && timerToShowNewAbility <= maxTimerToShowNewAbility)
                {
                    timerToShowNewAbility += Time.fixedDeltaTime;
                    fillAmount = (maxTimerToShowNewAbility / timerToShowNewAbility) / 10f;
                    uiManager.manaCoolDown.fillAmount = fillAmount;
                }
                else
                    CanUseAbility();
            }
        }

        uiManager.enemiesManaCount.text = "x" + enemyScripts.Count;
        uiManager.manaText.text = levelManager.playerMana + "/" + levelManager.playerMaxMana;
    }

    public void UseAbility()
    {
        if (canActive)
            return;

        //Check if are enemies around
        if (enemyScripts.Count <= 0)
        {
            StartCoroutine(FlashMana());
            return;
        }

        //Check if have mana
        if (levelManager.playerMana <= 0)
        {
            StartCoroutine(FlashMana());
            return;
        }

        //Use ability
        switch (randomAbilities)
        {
            case RandomAbilityEnum.ElectroShock:
                if (levelManager.playerMana < electroShockManaUse)
                {
                    StartCoroutine(FlashMana());
                    return;
                }

                levelManager.ManaUsage(electroShockManaUse);
                break;

            case RandomAbilityEnum.Freeze:
                if (levelManager.playerMana < FreezeManaUse)
                {
                    StartCoroutine(FlashMana());
                    return;
                }

                levelManager.ManaUsage(FreezeManaUse);
                break;

            case RandomAbilityEnum.Burn:
                if (levelManager.playerMana < burnManaUse)
                {
                    StartCoroutine(FlashMana());
                    return;
                }

                levelManager.ManaUsage(burnManaUse);
                break;
            default:
                break;
        }

        foreach (var enemyScriptableObject in enemyScripts) 
            enemyScriptableObject.AbilityReaction();

        SetNewAbility();
    }

    private void SetNewAbility()
    {
        //Reset Graphics
        uiManager.manaButton.interactable = false;
        uiManager.manaCoolDown.fillAmount = 1f;
        uiManager.manaConsume.text = "";

        //Set a random new ability
        randomAbiltyNumber = Random.Range(0, uiManager.manaNewImage.Length);
        randomAbilities = (RandomAbilityEnum)randomAbiltyNumber;
        uiManager.manaRandomAbility.sprite = uiManager.manaNewImage[randomAbiltyNumber];

        //Set new cycle
        canActive = true;
        timerToShowNewAbility = 0;
    }

    private void CanUseAbility()
    {
        uiManager.manaButton.interactable = true;
        uiManager.manaCoolDown.fillAmount = 0f;

        switch (randomAbilities)
        {
            case RandomAbilityEnum.ElectroShock:
                uiManager.manaConsume.text = electroShockManaUse.ToString();
                break;
            case RandomAbilityEnum.Freeze:
                uiManager.manaConsume.text = FreezeManaUse.ToString();
                break;
            case RandomAbilityEnum.Burn:
                uiManager.manaConsume.text = burnManaUse.ToString();
                break;
            default:
                break;
        }
        canActive = false;
    }

    #region Flash
    private IEnumerator FlashMana()
    {
        uiManager.manaButton.interactable = false;
        levelManager.ManaUsage(1f);
        yield return new WaitForSeconds(flashTimer);
        uiManager.manaButton.interactable = true;
    }
    #endregion

    #region Input Enable / Disable
    private void OnEnable()
    {
        m_PlayerInput.Enable();
        m_PlayerInput.PlayerMovement.UseAbilities.performed += UsAbilty;
    }

    private void UsAbilty(InputAction.CallbackContext obj)
    {
        UseAbility();
    }

    private void OnDisable()
    {
        m_PlayerInput.Disable();
        m_PlayerInput.PlayerMovement.UseAbilities.performed -= UsAbilty;
    }
    #endregion
}
