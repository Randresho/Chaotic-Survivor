using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemCollector : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Rigidbody2D m_rigidbodys;
    [SerializeField] private float addLevelToPlayer;
    [SerializeField] private float speedMagnet;
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private float timerToDestroy;
    public Collider2D collider2D;

    [Header("Player")]
    [SerializeField] private Transform playerPos;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.items.Add(this); 
        m_rigidbodys = GetComponent<Rigidbody2D>();
        playerPos = FindObjectOfType<PlayerMovement>().transform;
        coinSound = GetComponent<AudioSource>();
        collider2D = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(m_rigidbodys.position);
        if (screenPos.y > Screen.height || screenPos.y < 0 || screenPos.x > Screen.width || screenPos.x < 0)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        /*if (timerToDestroy > 0)
            timerToDestroy -= Time.fixedDeltaTime;
        else
            Destroy(gameObject);*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            coinSound.Play();
            levelManager.playerLevelFloat += addLevelToPlayer;
            levelManager.coinsGrab++;

            levelManager.levelPlayer();
            //levelManager.items.Remove(this);

            StartCoroutine(DestroyObj());
        }

        if (obj.GetComponent<HitObject>() != null)
        {
            StartCoroutine(DestroyObj());
        }

        if(obj.GetComponent<MagnetItem>() != null)
        {
            Vector2 move = playerPos.position - transform.position;
            Vector2 velocity = move * speedMagnet * Time.fixedDeltaTime;
            m_rigidbodys.velocity = velocity;
        }
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(timerToDestroy);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<MagnetItem>() != null)
        {
            Vector2 move = playerPos.position - transform.position;
            Vector2 velocity = move * speedMagnet * Time.fixedDeltaTime;
            m_rigidbodys.velocity = velocity;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<MagnetItem>() != null)
        {
            Vector2 move = playerPos.position - transform.position;
            Vector2 velocity = move * 0;
            m_rigidbodys.velocity = velocity;
        }
    }
}
