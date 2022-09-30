using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    private AbilityScriptableObject abilityScript;

    public CircleCollider2D magnetCollider;

    // Start is called before the first frame update
    void Awake()
    {
        abilityScript = FindObjectOfType<AbilityScriptableObject>();
        abilityScript.SetMagnet(this);
        magnetCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
