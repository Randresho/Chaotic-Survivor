using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPolling : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string type;
        public GameObject[] gamePrefab;
        public int size;
    }

    public static ObjectPolling instance;

    [SerializeField] private List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    [Space]
    [SerializeField] private int repeat;
    [SerializeField] private Transform enemiesParent;
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        levelManager = FindObjectOfType<LevelManager>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                for (int j = 0; j < pool.gamePrefab.Length; j++)
                {
                    GameObject obj = Instantiate(pool.gamePrefab[j], levelManager.outsideCam, Quaternion.identity, enemiesParent);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
            }

            poolDictionary.Add(pool.type, objectPool);
        }
    }


    public GameObject SpawnFromPool(string type, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogError("Pool type: " + type + " doesn't exist");
            return null;
        }

        levelManager.outsideCam = levelManager.cameraMain.ViewportToWorldPoint(new Vector3(Random.Range(levelManager.outsideCamValues[0], levelManager.outsideCamValues[1]), Random.Range(levelManager.outsideCamValues[0], levelManager.outsideCamValues[1]), 10));
        GameObject objectToSpawn = poolDictionary[type].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[type].Enqueue(objectToSpawn);
        repeat++;
        return objectToSpawn;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
