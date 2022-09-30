using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBalance : MonoBehaviour
{
    [Header("Bullets")]
    public GameObject[] bulletsPrefabs;

    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private PlayerAbilities playerAbilities;

    [Header("Spin weapon")]
    [SerializeField] private SpinWeapon spinWeapon;

    [Header("Magnet")]
    [SerializeField] private MagnetItem magnetItem;

    [Header("Enemies")]
    [SerializeField] private GameObject[] enemiesPrefabs;

    [Header("Items")]
    [SerializeField] private GameObject[] itemsPrefabs;

    [Header("Level")]
    [SerializeField] private int levelToUpdate;

    // Start is called before the first frame update
    void Awake()
    {
        
    }
}
