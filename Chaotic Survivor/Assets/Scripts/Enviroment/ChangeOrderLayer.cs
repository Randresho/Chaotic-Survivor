using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOrderLayer : MonoBehaviour
{
    LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if(obj.GetComponent<PlayerMovement>() != null)
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<Renderer>().sortingOrder -1;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<Renderer>().sortingOrder - 1;
            for (int i = 0; i < levelManager.enemies.Count; i++)
            {
                levelManager.enemies[i].moveRight = true;
            }
        }

        if (obj.GetComponent<EnemyScriptableObject>() != null)
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<Renderer>().sortingOrder - 1;
        }

        if(obj.GetComponent<PlayerActions>() != null)
        {
            obj.GetComponent<PlayerActions>().playerHP -= 5f * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = 12;

            for (int i = 0; i < levelManager.enemies.Count; i++)
            {
                levelManager.enemies[i].moveRight = false;
                levelManager.enemies[i].collider.enabled = true;
            }
        }

        if (obj.GetComponent<EnemyScriptableObject>() != null)
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = 8;
        }
    }
}
