using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    private PlayerInput m_PlayerInput;
    [SerializeField] private LevelManager m_LevelManager;
    [SerializeField] private ObjectPolling objectPolling;
    [SerializeField] private AbilityScriptableObject m_AbilityScriptableObject;
    [Header("Rotate Aim")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Camera m_camera;
    private Vector3 m_MousePos;
    private Vector3 mouseWorlPos;
    private Vector3 targetDir;
    private Vector2 mousePos;
    private Vector2 mouseWorlPos2;

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

    [Header("Hit FX")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;
    [SerializeField] private Material originalMaterial;
    private Coroutine flashRoutine;

    // Start is called before the first frame update
    void Awake()
    {
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //vector 3D
        m_MousePos = m_PlayerInput.PlayerMovement.ShootPoint.ReadValue<Vector2>();
        mouseWorlPos = m_camera.ScreenToWorldPoint(m_MousePos);
        

        if(playerHP > 0.05f)
        {
            HoldMousePos(mouseWorlPos);
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

    private void HoldMousePos(Vector3 pos)
    {
        targetDir = pos - shootPoint.position;
        float angle = Mathf.Atan2(-targetDir.y, -targetDir.x) * Mathf.Rad2Deg;
        shootPoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    #region Vector 2D
    public void ReadMouseInput(InputAction.CallbackContext context)
    {
        //Vector 2D
        mousePos = m_PlayerInput.PlayerMovement.ShootPoint.ReadValue<Vector2>();
        mouseWorlPos2 = (Vector2) m_camera.WorldToScreenPoint(transform.position);
        Vector2 direction = (mousePos-mouseWorlPos2).normalized;
        //mouseWorlPos2 = m_camera.ScreenToWorldPoint(mousePos);
    }

    public void ReadStick(InputAction.CallbackContext context)
    {
        Vector2 stickDirection = context.ReadValue<Vector2>().normalized;
        RotateAim(stickDirection);
    }

    public void RotateAim(Vector2 directon)
    {
        float angle = Mathf.Atan2(directon.y, directon.x) * Mathf.Rad2Deg;
        shootPoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    #endregion

    private void UpdateHp()
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
    }

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
