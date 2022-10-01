using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeAdder : MonoBehaviour
{
    public float addLife = 0;
    [SerializeField] private Target target;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerActions>() != null)
        {
            obj.GetComponent<PlayerActions>().playerHP += addLife;
            Debug.Log("Se añadio " + addLife + " de vida");
            FindObjectOfType<LevelManager>().SpawnLife();
        }

        if(obj.GetComponent<HitObject>() != null)
            FindObjectOfType<LevelManager>().SpawnLife();
    }
}
