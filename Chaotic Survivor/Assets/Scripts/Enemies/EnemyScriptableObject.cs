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

    // Start is called before the first frame update
    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        objectPolling = FindObjectOfType<ObjectPolling>();
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
        transform.position = levelManager.outsideCam;
        hp = maxHp;
        hpSlider.maxValue = hp;
        collider.enabled = true;
        deadObjVfx.SetActive(false);
        moveRight = false;
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
        
        EnemyLife();
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
        else
            MoveEnemy();

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

    public IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(timerToDestroy);
        levelManager.enemiesSpawned--;
        levelManager.enemiesKilled++;
        //levelManager.enemies.Remove(this);
        levelManager.playerLevelFloat += experience;
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
        ActiveEnemy();
        //Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<BulletBehavior>() != null)
        {
            hp -= obj.GetComponent<BulletBehavior>().damage;
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
}
