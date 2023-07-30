using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum enemyType { Ghost, Slime, Spider, Bat }
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
    [SerializeField] private Slider hpSlider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Space]
    [SerializeField] private float experience;
    [Space]
    public Collider2D collider;
    public float damagePlayer;
    [Space]
    public bool moveRight;
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
    public float electroShockDamage = 5f;
    private bool isElectroShocking;

    //Freeze
    [SerializeField] private Material freezeMaterial;
    private float timeFreezed;
    private bool isFreezeActive;

    //Burn
    [Space]
    private float timeBurning;
    private float burning;
    private bool isBurnActive;
    //Instant Kill

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
        //ActiveEnemy();
    }

    private void Start()
    {
        ActiveEnemy();
    }

    public void ActiveEnemy()
    {
        isBurnActive = false;
        isFreezeActive = false;


        transform.position = levelManager.outsideCam;

        hp = maxHp;
        hpSlider.maxValue = hp;
        hpSlider.value = hp;

        collider.enabled = true;
        m_rigidbodys.bodyType = RigidbodyType2D.Dynamic;

        deadObjVfx.SetActive(false);

        moveRight = false;

        spriteRenderer.material = originalMaterial;

        levelManager.enemyScriptables.Add(this);
        levelManager.UpdateCameraPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!levelManager.leveUp || !isFreezeActive)
        {
            speed = maxSpeed;
        }

        if (levelManager.leveUp || isFreezeActive)
        {
            speed = 0;
        }

        MoveEnemy();
    }

    #region Move
    private void MoveEnemy()
    {
        //Move
        if (!moveRight)
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
    #endregion

    #region Damage
    private void ReciveDamage(float damage)
    {
        hp -= damage;
        hpSlider.value = hp;

        if (!isFreezeActive)
            Flash();

        if (hp <= 0.05)
        {
            deadSound.Play();
            spriteRenderer.enabled = false;

            deadObjVfx.SetActive(true);
            deadVfx.Play("Anim");
            StartCoroutine(DestroyObj());
        }
    }

    public IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(timerToDestroy);
        levelManager.enemiesSpawned--;
        levelManager.enemiesKilled++;
        //levelManager.enemies.Remove(this);
        levelManager.playerLevelFloat += experience;
        levelManager.AddMana(1f);
        levelManager.levelPlayer();

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
        ActiveEnemy();
    }
    #endregion

    #region Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<BulletBehavior>() != null)
        {
            ReciveDamage(obj.GetComponent<BulletBehavior>().damage);
        }

        if(obj.GetComponent<RandomAbilities>() != null) 
        {
            if(randomAbilities.enemyCount <= randomAbilities.enemyCountLimit)
            {
                randomAbilities.enemyScripts.Add(this);
                randomAbilities.enemyCount++;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.GetComponent<RandomAbilities>() != null)
        {
            if (obj.GetComponent<RandomAbilities>().enemyScripts.Contains(this))
            {
                randomAbilities.enemyScripts.Remove(this);
                randomAbilities.enemyCount--;
            }
        }
    }
    #endregion

    #region Flash enemy
    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine(flashMaterial));
    }

    private IEnumerator FlashRoutine(Material flashing)
    {
        spriteRenderer.material = flashing;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
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

            case RandomAbilityEnum.InstantKill:
                StartCoroutine(InstantKill());
                break;
        }
    }

    #region Electro Shock
    private IEnumerator ElectroShocking()
    {
        while (electroShocktimer <= levelManager.maxElectroShockTimer)
        {
            electroShocktimer += Time.fixedDeltaTime;
            ReciveDamage(electroShockDamage);
            //Efectos de rayos
            //Sonido de rayos
            isFreezeActive = true;
        }
        yield return new WaitForSeconds(levelManager.maxElectroShockTimer);
        isFreezeActive = false;
    }
    #endregion

    #region Freeze
    private IEnumerator Freezed()
    {
        isFreezeActive = true;
        spriteRenderer.material = freezeMaterial;
        m_rigidbodys.bodyType = RigidbodyType2D.Static;
        //Efecto de congelacion
        //Sonido de congelado

        yield return new WaitForSeconds(levelManager.maxFreezeTimer);

        spriteRenderer.material = originalMaterial;

        isFreezeActive = false;

        m_rigidbodys.bodyType = RigidbodyType2D.Dynamic;
    }    
    #endregion

    #region Burn
    private IEnumerator ReciveBurnDamage()
    {
        while(isBurnActive)
        {       
            do
            {
                timeBurning += 5f;
                Debug.Log("Estoy apunto de quemarme");
                yield return new WaitForSeconds(levelManager.maxNextBurning);
                ReciveDamage(levelManager.burnDamage);
                Debug.Log("Me quemo");
                //Efecto de fuego
                //Sonido de quemadura
            }
            while (timeBurning <= levelManager.maxBurningTimer);

            yield return new WaitForSeconds(levelManager.maxBurningTimer);
            timeBurning = 0f;
            Debug.Log("Ya no me quemo");
            isBurnActive = false;
        }
    }
    #endregion

    #region Instant Kill
    private IEnumerator InstantKill()
    {
        //Sonido de instant kill
        StartCoroutine(DestroyObj());
        yield return null;
    }
    #endregion
    #endregion
}
