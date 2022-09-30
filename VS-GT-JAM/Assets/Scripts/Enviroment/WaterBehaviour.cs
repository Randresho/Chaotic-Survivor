using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    public float speed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            obj.GetComponent<PlayerMovement>().m_PlayerSpeed = speed;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            obj.GetComponent<PlayerMovement>().m_PlayerSpeed = speed;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            obj.GetComponent<PlayerMovement>().m_PlayerSpeed = obj.GetComponent<PlayerMovement>().m_PlayerSpeedMax;
        }
    }
}
