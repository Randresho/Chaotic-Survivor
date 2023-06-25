using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

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
        if(enemiesHit > maxEnemiesHit)
        {
            DestroyNRemove();
        }

        Vector2 screenPos = cam.WorldToScreenPoint(rb.position);
        if (screenPos.y > Screen.height || screenPos.y < 0 || screenPos.x > Screen.width || screenPos.x < 0)
            DestroyNRemove();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if(obj.GetComponent<EnemyScriptableObject>() != null)
        {
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
        GameObject VFX = ObjectPoolVFX.instance.GetPooledObject();

        if (VFX != null)
        {
            VFX.transform.position = transform.position;
            VFX.transform.rotation = Quaternion.Euler(Vector2.up);
            VFX.SetActive(true);
        }
    }

    public void DestroyNRemove()
    {
        gameObject.SetActive(false);
        enemiesHit = 0;
    }
}
