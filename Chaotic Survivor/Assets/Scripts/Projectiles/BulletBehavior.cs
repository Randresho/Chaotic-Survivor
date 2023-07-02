using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private LevelManager levelManager;
    private Rigidbody2D rb;

    [Header("Modificable")]
    public float damage;
    public int maxEnemiesHit;
    public float bulletSpeed;
    [Space]
    [SerializeField] private GameObject vfxPrefab;

    [SerializeField] private float timerToDestroy;
    private int enemiesHit;
    [HideInInspector] public Camera cam;

    [Header("Restore Data")]
    public float damageDefault = 5;
    public int maxEnemiesHitDefault = 3;
    public float bulletSpeedDefault = 10;

    // Start is called before the first frame update
    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.bullets.Add(this);

        rb = GetComponent<Rigidbody2D>();
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = -transform.right * bulletSpeed;
        

        Vector2 screenPos = cam.WorldToScreenPoint(rb.position);
        if (screenPos.y > Screen.height || screenPos.y < 0 || screenPos.x > Screen.width || screenPos.x < 0)
            DestroyNRemove();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if(obj.GetComponent<EnemyScriptableObject>() != null)
        {
            if (enemiesHit > maxEnemiesHit)
                DestroyNRemove();
            else
                enemiesHit++;
        }

        if(obj.GetComponent<HitObject>() != null)
        {
            VfxSpawner();
            DestroyNRemove();
        }
    }

    private void VfxSpawner()
    {
        Instantiate(vfxPrefab, transform.position, Quaternion.Euler(Vector2.up));
    }

    public void DestroyNRemove()
    {
        //levelManager.bullets.Remove(this);
        //Destroy(gameObject);
        gameObject.SetActive(false);
        enemiesHit = 0;
    }
}
