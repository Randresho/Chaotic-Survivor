using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum RandomAbilityEnum{ElectroShock, Freeze, Burn, InstantKill}

[RequireComponent(typeof(CircleCollider2D))]
public class RandomAbilities : MonoBehaviour
{
    #region scripts
    private PlayerInput m_PlayerInput;
    private GameManager gameManager;
    private UiManager uiManager;
    private LevelManager levelManager;
    #endregion

    [Header("Abilities")]
    [SerializeField] private RandomAbilityEnum randomAbilities;
    public RandomAbilityEnum RandomAbility
    {
        get { return randomAbilities; }
    }
    [HideInInspector] public CircleCollider2D circleCollider;

    [Header("Enemies")]
    public List<EnemyScriptableObject> enemyScripts;
    public int enemyCount;
    public int enemyCountLimit;

    [Header("Timer")]
    public float maxTimerToShowNewAbility;
    private float timerToShowNewAbility;
    private int randomAbiltyNumber;
    private float fillAmount;
    private bool canActive = false;

    void Awake()
    {
        m_PlayerInput = new PlayerInput();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.RandomAbilities(this);
        uiManager = FindObjectOfType<UiManager>();
        levelManager = FindObjectOfType<LevelManager>();
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
            else
            {
                if (m_PlayerInput.PlayerMovement.UseAbilities.IsPressed())
                    UseAbility();
            }
        }
    }
    public void UseAbility()
    {
        //Check if are enemies around
        if (enemyScripts.Count <= 0)
        {
            Debug.Log("No hay enemigos para atacar");
            return;
        }        

        //Use ability

        foreach (var enemyScriptableObject in enemyScripts) 
        {
            enemyScriptableObject.AbilityReaction();
        }

        SetNewAbility();
    }

    private void SetNewAbility()
    {
        //Set new cycle
        canActive = true;
        timerToShowNewAbility = 0;

        //Reset Graphics
        uiManager.manaButton.interactable = false;
        uiManager.manaCoolDown.fillAmount = 1f;

        //Set a random new ability
        randomAbiltyNumber = Random.Range(0, uiManager.manaNewImage.Length);
        randomAbilities = (RandomAbilityEnum)randomAbiltyNumber;
        uiManager.manaRandomAbility.sprite = uiManager.manaNewImage[randomAbiltyNumber];
    }

    private void CanUseAbility()
    {
        uiManager.manaButton.interactable = true;

        uiManager.manaCoolDown.fillAmount = 0f;
        canActive = false;
    }

    #region Input Enable / Disable
    private void OnEnable()
    {
        m_PlayerInput.Enable();
    }
    private void OnDisable()
    {
        m_PlayerInput.Disable();
    }
    #endregion
}
