using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : MonoBehaviour
{
    private GameManager gameManager;
    private AbilityScriptableObject abilityScriptableObject;

    [Header("Modificable")]
    public int currentBulletPoint = 8;
    public float timerToSpawn = 0;
    public float rotationSpeed;


    [Header("Bullets")]
    [SerializeField] private GameObject[] bulletsPrefabs = null;
    [SerializeField] private GameObject[] bulletPoint;
    [SerializeField] private Transform bulletsParent;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private bool[] spawnPoinws;
    [SerializeField] private float minShootTimer = 0.1f;

    private int maxBulletPoint = 8;
    private float timerToSpawnCurrent = 0;
    private float rotationPoint;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.SetSpinWeapon(this);
        abilityScriptableObject = FindObjectOfType<AbilityScriptableObject>();
        abilityScriptableObject.SetSpinWeapon(this);

        for (int i = 0; i < bulletPoint.Length; i++)
        {
            bulletPoint[i].SetActive(false);
        }

        maxBulletPoint = bulletPoint.Length;
        spawnPoinws = new bool[bulletPoint.Length];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotationPoint += rotationSpeed * Time.fixedDeltaTime;
        shootPoint.rotation = Quaternion.Euler(0, 0, rotationPoint);
        SpawnBullet(0);
    }

    public void SpawnBullet(int playerWeapon)
    {
        if(timerToSpawn < minShootTimer)
            timerToSpawn = minShootTimer;

        if (timerToSpawnCurrent > 0)
            timerToSpawnCurrent -= Time.fixedDeltaTime;
        else
        {
            if (currentBulletPoint >= maxBulletPoint)
                return;

            for (int i = currentBulletPoint; i < bulletPoint.Length; i++)
            {
                //Instantiate(bulletsPrefabs[playerWeapon], bulletPoint[i].transform.position, bulletPoint[i].transform.rotation, bulletsParent);
                GameObject bullet = ObjectoPoolSpinWeapon.instance.GetPooledObject();
                if (bullet != null)
                {
                    bullet.transform.position = bulletPoint[i].transform.position;
                    bullet.transform.rotation = bulletPoint[i].transform.rotation;
                    bullet.SetActive(true);
                }
            }
            timerToSpawnCurrent = timerToSpawn;
        }

        switch (currentBulletPoint)
        {
            case 0:
                spawnPoinws[0] = true;
                bulletPoint[0].SetActive(spawnPoinws[0]);
                break;
            case 1: 
                spawnPoinws[1] = true;
                bulletPoint[1].SetActive(spawnPoinws[1]);
                break;
            case 2:
                spawnPoinws[2] = true;
                bulletPoint[2].SetActive(spawnPoinws[2]);
                break;
            case 3:
                spawnPoinws[3] = true;
                bulletPoint[3].SetActive(spawnPoinws[3]);
                break;
            case 4:
                spawnPoinws[4] = true;
                bulletPoint[4].SetActive(spawnPoinws[4]);
                break;
            case 5:
                spawnPoinws[5] = true;
                bulletPoint[5].SetActive(spawnPoinws[5]);
                break;
            case 6:
                spawnPoinws[6] = true;
                bulletPoint[6].SetActive(spawnPoinws[6]);
                break;
            case 7:
                spawnPoinws[7] = true;
                bulletPoint[7].SetActive(spawnPoinws[7]);
                break;
            default:
                for (int i = 0; i < maxBulletPoint; i++)
                {
                    spawnPoinws[i] = false;
                    bulletPoint[i].SetActive(spawnPoinws[i]);
                }
                break;
        }
    }
}
