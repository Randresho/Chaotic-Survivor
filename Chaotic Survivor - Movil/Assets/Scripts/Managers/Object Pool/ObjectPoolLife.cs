using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolLife : MonoBehaviour
{
    private LevelManager levelManager;

    public static ObjectPoolLife instance;

    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private int amountToPool = 50;
    [SerializeField] private GameObject[] objectPrefab;
    [SerializeField] private Transform parentBullet;

    [SerializeField] private int random;

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
            for (int j = 0; j < objectPrefab.Length; j++)
            {
                GameObject obj = Instantiate(objectPrefab[j], parentBullet);

                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject()
    {

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            random = Random.Range(0, objectPrefab.Length);

            if (!pooledObjects[random].activeInHierarchy)
                return pooledObjects[random];
        }

        return null;
    }
}
