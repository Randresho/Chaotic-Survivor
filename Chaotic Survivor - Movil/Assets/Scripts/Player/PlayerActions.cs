using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerActions : MonoBehaviour
{
    private PlayerInput m_PlayerInput;
    private OptionsManager m_OptionsManager;

    [SerializeField] private LevelManager m_LevelManager;
    [SerializeField] private ObjectPolling objectPolling;
    [SerializeField] private AbilityScriptableObject m_AbilityScriptableObject;
    [Header("Rotate Aim")]
    public Transform shootPoint;
    [SerializeField] private Camera m_camera;
    private Vector3 m_MousePos;
    [SerializeField] private float rotationAim;
    [SerializeField] private float rotationSpeed;
    private Vector2 mousePos;

    [Header("Player Data")]
    [SerializeField] private int playerLevel = 0;
    public float playerHP = 0;
    public float playerMaxHP = 0;
    [SerializeField] private Slider playerHPSlider = null;
    public Collider2D playerCollider = null;
    [SerializeField] private PlayerAbilities playerAbilities = null;
    private PlayerMovement playerMovement = null;
    public float timerToReduceLife = 1;
    [SerializeField] private float reducerLife = 0.5f;
    [SerializeField] private Animator animator = null;
    public bool panicMode = false;
    [SerializeField] private Volume postProcesingVolume = null;
    [SerializeField] private OffScreenIndicator offScreenIndicator = null;

    [Header("Hit FX")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;
    [SerializeField] private Material originalMaterial;
    private Coroutine flashRoutine;

    // Start is called before the first frame update
    void Awake()
    {
        m_OptionsManager = FindObjectOfType<OptionsManager>();
        m_PlayerInput = new PlayerInput();
        m_LevelManager = FindObjectOfType<LevelManager>();
        objectPolling = FindObjectOfType<ObjectPolling>();
        m_AbilityScriptableObject = FindObjectOfType<AbilityScriptableObject>();
        m_LevelManager.playerActions = this;
        m_camera = FindObjectOfType<Camera>();
        playerAbilities = GetComponent<PlayerAbilities>();
        playerHPSlider.maxValue = playerHP;
        playerCollider.enabled = true;
        m_AbilityScriptableObject.SetPlayerActions(this);
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        animator = GetComponent<Animator>();
        postProcesingVolume = FindObjectOfType<Volume>();
        offScreenIndicator = FindObjectOfType<OffScreenIndicator>();
        offScreenIndicator.SetPlayer(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //vector 3D
        #region Funcional
        m_MousePos = -m_PlayerInput.PlayerMovement.RotateAim.ReadValue<Vector2>();
        #endregion

        if (playerHP > 0.05f)
        {
            HoldMousePos(m_MousePos);
            playerAbilities.SpawnAbilities();

            if(m_LevelManager.canShoot)
            {
                playerAbilities.SpawnBullet();
            }
            else
            {
                for (int i = 0; i < playerAbilities.abilities.Length; i++)
                {
                    playerAbilities.abilities[i].abilityEnable = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < playerAbilities.abilities.Length; i++)
            {
                playerAbilities.abilities[i].ability.SetActive(false);
            }
        }    

        UpdateHp();
    }

    #region Hold Mouse Pos
    public void HoldMousePos(Vector2 pos)
    {
        if(m_OptionsManager.easyMod)
        {
            mousePos = shootPoint.position - m_LevelManager.enemyScriptables[0].transform.position;
            float angles = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            shootPoint.rotation = Quaternion.Euler(0, 0, angles);
        }
        else
        {
            mousePos = pos;
            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            shootPoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    #endregion

    #region Update HP
    public void UpdateHp()
    {
        playerHPSlider.value = playerHP;

        if (playerMovement.m_rigidbody2D.velocity == Vector2.zero)
        {
            if (timerToReduceLife > 0)
                timerToReduceLife -= Time.fixedDeltaTime;
            else
            {
                if(playerHP > 15)
                    playerHP -= Time.fixedDeltaTime * reducerLife;
                else
                { 
                    for (int i = 0; i < m_LevelManager.enemies.Count; i++)
                    {
                        m_LevelManager.enemies[i].moveRight = false;
                    }
                }                
            }
        }
        else
            timerToReduceLife = 1;

        if (playerHP > 300)
        {
            playerHP = 300;
            Debug.Log("Se tiene la vida completa");
        }

        animator.SetFloat("HP", playerHP);
        postProcesingVolume.weight = (1 - (playerHP / playerMaxHP));
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<EnemyScriptableObject>() != null)
        {
            playerHP -= obj.GetComponent<EnemyScriptableObject>().damagePlayer;
            m_LevelManager.enemiesSpawned--;
            obj.GetComponent<EnemyScriptableObject>().gameObject.SetActive(false);

            if (playerHP < 0.05)
                m_LevelManager.GameOver();

            if(playerHP > 0)
            {
                Flash();
            }
        }
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }

    /*private void OnCollisionStay2D(Collision2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<EnemyScriptableObject>() != null)
        {
            playerHP -= obj.GetComponent<EnemyScriptableObject>().damagePlayer;
        }
    }*/

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
