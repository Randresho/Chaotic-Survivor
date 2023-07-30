using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeAdder : MonoBehaviour
{
    [SerializeField] private  float addLife = 0;
    [SerializeField] private Target target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerActions>() != null)
        {
            obj.GetComponent<PlayerActions>().playerHP += addLife;
            FindObjectOfType<LevelManager>().SpawnLife();
        }

        if(obj.GetComponent<HitObject>() != null)
            FindObjectOfType<LevelManager>().SpawnLife();
    }
}
