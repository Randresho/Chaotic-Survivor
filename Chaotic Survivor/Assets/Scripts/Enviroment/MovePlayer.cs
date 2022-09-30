using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float m_Speed = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            //Debug.Log(obj.name);
            //obj.GetComponent<PlayerMovement>().m_rigidbody2D.AddForce(new Vector2(0f, m_Speed * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            obj.GetComponent<PlayerMovement>().m_rigidbody2D.AddRelativeForce(new Vector2(0f, m_Speed));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
