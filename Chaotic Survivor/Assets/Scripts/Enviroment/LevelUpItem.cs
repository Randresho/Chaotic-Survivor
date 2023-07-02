using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpItem : MonoBehaviour
{
    private GameManager gameManager;
    public float addLevel = 0;
    [SerializeField] private Target target;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerActions>() != null)
        {
            FindObjectOfType<LevelManager>().playerLevelFloat += (addLevel += gameManager.porcentageLevel % 20);
            FindObjectOfType<LevelManager>().levelPlayer();
            FindObjectOfType<LevelManager>().SpawnLevelUpItem();
        }

        if (obj.GetComponent<HitObject>() != null)
            FindObjectOfType<LevelManager>().SpawnLevelUpItem();
    }
}
