using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum enemyType { Ghost, Slime, Spider, Bat}
//[CreateAssetMenu(fileName = "EnemyType",menuName = "Enemy/Enemy")]
public class EnemyScriptableObject : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ObjectPolling objectPolling;
    [SerializeField] private RandomAbilities randomAbilities;
    [Header("Enemy Type")]
    [SerializeField] private enemyType enemyType;
    [SerializeField] private Canvas canvas;

    [Header("Player")]
    [SerializeField] private Transform playerPos;
    [SerializeField] private PlayerActions playerActions;

    [Header("Enemy Data")]
    [SerializeField] private Sprite sprite;
    [SerializeField] private Rigidbody2D m_rigidbodys;
    [SerializeField] private Vector2 enemyPosition;
    public float speed;
    public float maxSpeed;
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float hpAdder;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Space]
    [SerializeField] private float experience;
    [Space]
    public Collider2D collider;
    public float damagePlayer;
    [Space]
    public bool moveRight;
    private bool canMoveIt;
    [SerializeField] private float timerToDestroy;

    [Header("Spawner Item")]
    [SerializeField] private GameObject[] itemDropPrefab;
    [Space]
    [SerializeField] private GameObject deadObjVfx = null;
    [SerializeField] private Animator deadVfx = null;
    [SerializeField] private AudioSource deadSound;

    [Header("Hit FX")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;
    [SerializeField] private Material originalMaterial;
    private Coroutine flashRoutine;

    [Header("Reations Effects")]
    //Electro Shock
    public float electroShocktimer;
    [SerializeField] private GameObject electroFX;
    [SerializeField] private AudioSource electroSound;

    //Freeze
    [Space]
    [SerializeField] private Material freezeMaterial;
    [SerializeField] private AudioSource freezeSound;
    private bool isFreezeActive;

    //Burn
    [Space]
    [SerializeField] private GameObject burnFX;
    [SerializeField] private AudioSource burnSound;
    private float timeBurning;
    private bool isBurnActive;

    // Start is called before the first frame update
    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        objectPolling = FindObjectOfType<ObjectPolling>();
        randomAbilities = FindObjectOfType<RandomAbilities>();
        m_rigidbodys = GetComponent<Rigidbody2D>();
        playerPos = FindObjectOfType<PlayerMovement>().transform;
        canvas.worldCamera = FindObjectOfType<Camera>();
        levelManager.enemies.Add(this);
        collider = GetComponent<Collider2D>();
        maxSpeed = speed;
        originalMaterial = spriteRenderer.material;
        playerActions = FindObjectOfType<PlayerActions>();
        //ActiveEnemy();
    }

    private void Start()
    {
        ActiveEnemy();
    }

    public void ActiveEnemy()
    {
        transform.position = levelManager.outsideCam;
        hp = maxHp;
        hpSlider.maxValue = hp;
        collider.enabled = true;
        deadObjVfx.SetActive(false);
        moveRight = false;
        spriteRenderer.material = originalMaterial;
        levelManager.enemyScriptables.Add(this);
        EnemyLife();
        levelManager.UpdateCameraPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!levelManager.leveUp)
        {
            speed = maxSpeed;
        }
        else
        {
            speed = 0;
        }

        MoveEnemy();
    }

    private void MoveEnemy()
    {
        //Move
        if(!moveRight)
        {
            Vector2 move = playerPos.position - transform.position;
            Vector2 velocity = move * speed * Time.fixedDeltaTime;
            m_rigidbodys.velocity = velocity;
            collider.enabled = true;
        }
        else
        {
            Vector2 move = transform.right;
            Vector2 velocity = move * speed * Time.fixedDeltaTime;
            m_rigidbodys.velocity = velocity;
            collider.enabled = false;
        }

        //Sprite Renderer
        Vector2 screenPos = Camera.main.WorldToScreenPoint(m_rigidbodys.position);
        if (screenPos.y > Screen.height || screenPos.y < 0 || screenPos.x > Screen.width || screenPos.x < 0)
        {
            spriteRenderer.enabled = false;
            collider.enabled = false;
            return;
        }
        else
        {
            if (hp <= 0.05)
                spriteRenderer.enabled = false;
            else
                spriteRenderer.enabled = true;
        }
    }

    private void EnemyLife()
    {
        //Hp
        hpSlider.value = hp;
        if (hp <= 0.05)
        {
            deadObjVfx.SetActive(true);
            deadVfx.Play("Anim");
            StartCoroutine(DestroyObj());
        }      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<BulletBehavior>() != null)
        {
            hp -= obj.GetComponent<BulletBehavior>().damage;
            EnemyLife();
            Flash();
            if (hp <= 0.06)
            {
                deadSound.Play();
                spriteRenderer.enabled = false;                
            }
        }
    }

    public void Flash()
    {
        if( flashRoutine != null)
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

    #region Damage
    private void ReciveDamage(float damage)
    {
        hp -= damage;
        hpSlider.value = hp;

        if (!isFreezeActive)
            Flash();

        if(isBurnActive)
            StartCoroutine(DamageBurnFX());        

        if (hp <= 0.05)
        {
            deadSound.Play();
            spriteRenderer.enabled = false;

            deadObjVfx.SetActive(true);
            deadVfx.Play("Anim");
            StartCoroutine(DestroyObj());
        }
    }

    //Burn Damage
    private IEnumerator DamageBurnFX()
    {
        burnFX.SetActive(true);
        burnSound.Play();
        yield return new WaitForSeconds(0.5f);
        burnFX.SetActive(false);
    }

    //Electro Damage
    private IEnumerator DamageElectroFX()
    {
        electroFX.SetActive(true);
        electroSound.Play();
        yield return new WaitForSeconds(0.5f);
        electroFX.SetActive(false);
    }

    public IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(timerToDestroy);
        levelManager.enemiesSpawned--;
        levelManager.enemiesKilled++;
        levelManager.playerLevelFloat += experience;
        levelManager.levelPlayer();
        levelManager.AddMana(1f);

        //Instantiate(itemDropPrefab[Random.Range(0, itemDropPrefab.Length)], transform.position, transform.rotation, levelManager.transform);
        GameObject coinObj = ObjectPoolCoins.instance.GetPooledObject();
        if (coinObj != null)
        {
            coinObj.transform.position = transform.position;
            coinObj.transform.rotation = Quaternion.identity;
            coinObj.SetActive(true);
        }

        gameObject.SetActive(false);

        levelManager.enemyScriptables.Remove(this);

        maxHp += hpAdder;

        ActiveEnemy();
    }
    #endregion

    #region Reations
    public void AbilityReaction()
    {
        switch (randomAbilities.RandomAbility)
        {
            case RandomAbilityEnum.ElectroShock:
                StartCoroutine(ElectroShocking());
                break;

            case RandomAbilityEnum.Freeze:
                StartCoroutine(Freezed());
                break;

            case RandomAbilityEnum.Burn:
                isBurnActive = true;
                StartCoroutine(ReciveBurnDamage());
                break;
        }
    }

    #region Electro Shock
    private IEnumerator ElectroShocking()
    {
        while (electroShocktimer <= randomAbilities.maxElectroShockTimer)
        {
            electroShocktimer += Time.fixedDeltaTime;
            ReciveDamage(randomAbilities.electroShockDamage);
            StartCoroutine(DamageElectroFX());

            isFreezeActive = true;
        }
        yield return new WaitForSeconds(randomAbilities.maxElectroShockTimer);
        isFreezeActive = false;
    }
    #endregion

    #region Freeze
    private IEnumerator Freezed()
    {
        isFreezeActive = true;
        m_rigidbodys.bodyType = RigidbodyType2D.Static;
        animator.speed = 0f;
        spriteRenderer.material = freezeMaterial;
        freezeSound.Play();
        levelManager.enemyScriptables.Remove(this);

        yield return new WaitForSeconds(randomAbilities.maxFreezeTimer);

        spriteRenderer.material = originalMaterial;

        isFreezeActive = false;

        m_rigidbodys.bodyType = RigidbodyType2D.Dynamic;
        animator.speed = 1f;

        if (hp > 0.05f)
            levelManager.enemyScriptables.Add(this);
    }
    #endregion

    #region Burn
    private IEnumerator ReciveBurnDamage()
    {
        while (isBurnActive)
        {
            do
            {
                timeBurning += 5f;

                yield return new WaitForSeconds(randomAbilities.MaxNextBurning);

                ReciveDamage(randomAbilities.burnDamage);

                yield return new WaitForSeconds(0.5f);
            }
            while (timeBurning <= randomAbilities.maxBurningTimer);

            yield return new WaitForSeconds(randomAbilities.maxBurningTimer);
            timeBurning = 0f;
            //Debug.Log("Ya no me quemo");
            isBurnActive = false;
        }
    }
    #endregion
    #endregion
}
