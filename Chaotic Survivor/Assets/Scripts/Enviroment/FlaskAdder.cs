using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskAdder : MonoBehaviour
{
    #region Enums
    private enum FlaskSize { XS, Small, Normal, Medium, Big, }
    private enum FlaskType { Empty, Life, Level, Mana, }
    #endregion

    #region Scripts
    private GameManager gameManager;
    private LevelManager levelManager;
    #endregion

    [SerializeField] private FlaskType flaskType = FlaskType.Empty;
    [SerializeField] private FlaskSize flaskSize = FlaskSize.Normal;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;

        if (obj.GetComponent<PlayerActions>() != null)
        {
            switch(flaskType)
            {
                case FlaskType.Empty:
                    break;
                #region Life
                case FlaskType.Life:
                    #region Size flask
                    switch (flaskSize)
                    {
                        case FlaskSize.XS:
                            obj.GetComponent<PlayerActions>().playerHP += (obj.GetComponent<PlayerActions>().playerMaxHP * 0.1f);
                            break;
                        case FlaskSize.Small:
                            obj.GetComponent<PlayerActions>().playerHP += (obj.GetComponent<PlayerActions>().playerMaxHP * 0.25f);
                            break;
                        case FlaskSize.Normal:
                            obj.GetComponent<PlayerActions>().playerHP += (obj.GetComponent<PlayerActions>().playerMaxHP * 0.5f);
                            break;
                        case FlaskSize.Medium:
                            obj.GetComponent<PlayerActions>().playerHP += (obj.GetComponent<PlayerActions>().playerMaxHP * 0.75f);
                            break;
                        case FlaskSize.Big:
                            obj.GetComponent<PlayerActions>().playerHP += obj.GetComponent<PlayerActions>().playerMaxHP;
                            break;
                        default:
                            break;
                    }
                    #endregion
                    levelManager.SpawnLife();
                    break;
                #endregion
                #region Level
                case FlaskType.Level:
                    #region Size flask
                    switch (flaskSize)
                    {
                        case FlaskSize.XS:
                            levelManager.playerLevelFloat += (levelManager.playerLevelMaxFloat * 0.1f);
                            break;
                        case FlaskSize.Small:
                            levelManager.playerLevelFloat += (levelManager.playerLevelMaxFloat * 0.25f);
                            break;
                        case FlaskSize.Normal:
                            levelManager.playerLevelFloat += (levelManager.playerLevelMaxFloat * 0.5f);
                            break;
                        case FlaskSize.Medium:
                            levelManager.playerLevelFloat += (levelManager.playerLevelMaxFloat * 0.75f);
                            break;
                        case FlaskSize.Big:
                            levelManager.playerLevelFloat += levelManager.playerLevelMaxFloat;
                            break;
                        default:
                            break;
                    }
                    #endregion
                    levelManager.SpawnLevelUpItem();
                    break;
                #endregion
                #region Mana
                case FlaskType.Mana:
                    #region Size flask
                    switch (flaskSize)
                    {
                        case FlaskSize.XS:
                            levelManager.playerMana += (levelManager.playerMaxMana * 0.1f);
                            break;
                        case FlaskSize.Small:
                            levelManager.playerMana += (levelManager.playerMaxMana * 0.25f);
                            break;
                        case FlaskSize.Normal:
                            levelManager.playerMana += (levelManager.playerMaxMana * 0.5f);
                            break;
                        case FlaskSize.Medium:
                            levelManager.playerMana += (levelManager.playerMaxMana * 0.75f);
                            break;
                        case FlaskSize.Big:
                            levelManager.playerMana += (levelManager.playerMaxMana);
                            break;
                        default:
                            break;
                    }
                    #endregion
                    levelManager.SpawnMana();
                    break;
                #endregion
            }

        }

        if (obj.GetComponent<HitObject>() != null)
        {
            switch (flaskType)
            {
                case FlaskType.Empty:
                    break;
                case FlaskType.Life:
                    levelManager.SpawnLife();
                    break;
                case FlaskType.Level:
                    levelManager.SpawnLevelUpItem();
                    break;
                case FlaskType.Mana:
                    levelManager.SpawnMana();
                    break;
                default:
                    break;
            }
        }
    }
}
