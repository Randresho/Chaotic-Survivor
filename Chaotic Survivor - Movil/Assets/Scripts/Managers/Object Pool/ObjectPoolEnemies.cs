using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolEnemies : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    public static ObjectPoolEnemies instance;

    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private int amountToPool = 50;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private Transform parentBullet;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        levelManager = FindObjectOfType<LevelManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectPrefab, parentBullet);
            levelManager.enemies.Add(obj.GetComponent<EnemyScriptableObject>());
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }

        return null;
    }
}
