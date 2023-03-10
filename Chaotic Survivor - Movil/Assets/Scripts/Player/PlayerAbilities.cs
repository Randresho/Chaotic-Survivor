using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [System.Serializable]
    public class Abilities
    {
        public string abilityName;
        public bool abilityEnable;
        public GameObject ability;
        public bool[] isActive;
    }
    #region Scripts
    private LevelManager levelManager;
    private GameManager gameManager;
    private AbilityScriptableObject abilityScriptableObject;
    #endregion

    [Header("Modificable")]
    public float timerToSpawn = 0;
    public int shootType = 0;
    [Space]
    public bool[] enableAbilities;
    public AudioSource shootSound = null;

    [Header("Bullets")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject[] bulletsPrefabs = null;
    [SerializeField] private Transform bulletPoint;
    private float timerToSpawnCurrent = 0;
    [SerializeField] private Transform bulletsParent;

    [Header("Abilities")]
    public Abilities[] abilities;
    private PlayerActions playerActions;

    // Start is called before the first frame update
    void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();
        abilityScriptableObject = FindObjectOfType<AbilityScriptableObject>();
        abilityScriptableObject.SetPlayerAbilities(this);
        gameManager.SetPlayerAbilities(this);
        playerActions = FindObjectOfType<PlayerActions>();

        enableAbilities = new bool[abilities.Length];
        for (int i = 0; i < enableAbilities.Length; i++)
        {
            enableAbilities[i] = abilities[i].abilityEnable;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
    }

    public void SpawnBullet()
    {
        if (timerToSpawnCurrent > 0)
            timerToSpawnCurrent -= Time.fixedDeltaTime;
        else
        {
            shootSound.Play();
            //Instantiate(bulletsPrefabs[shootType], bulletPoint.position, shootPoint.rotation, bulletsParent);

            GameObject bullet = ObjectPoolNormalBullet.instance.GetPooledObject();
            if( bullet != null)
            {
                bullet.transform.position = bulletPoint.position;
                bullet.transform.rotation = playerActions.shootPoint.rotation;
                bullet.SetActive(true);
            }

            timerToSpawnCurrent = timerToSpawn;
        }
    }

    public void SpawnAbilities()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].ability.SetActive(abilities[i].abilityEnable);
        }

        for (int i = 0; i < enableAbilities.Length; i++)
        {
            abilities[i].abilityEnable = enableAbilities[i];
        }
    }
}
