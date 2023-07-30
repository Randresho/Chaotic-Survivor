using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaAdderItem : MonoBehaviour
{
    [SerializeField] private float addMana = 0;
    [SerializeField] private Target target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerActions>() != null)
        {
            FindObjectOfType<LevelManager>().AddMana(addMana);
            FindObjectOfType<LevelManager>().SpawnMana();
        }

        if (obj.GetComponent<HitObject>() != null)
            FindObjectOfType<LevelManager>().SpawnMana();
    }
}
