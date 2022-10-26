using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPath : MonoBehaviour
{
    [SerializeField] private Transform[] pointsMovers;
    [SerializeField] private Animator animator;
    [SerializeField] private float timerMax;
    [SerializeField] private int idxPos;
    [SerializeField] private Rigidbody2D m_rigidbodys;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Awake()
    {
        transform.position = pointsMovers[0].position;
        idxPos = Random.Range(1, pointsMovers.Length);
        timer = timerMax;
        animator.StopPlayback();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
        }
        else
        {
            CallMove();
        }
    }

    private void CallMove()
    {       
        if(transform.position != pointsMovers[idxPos].position)
        {
            Vector2 move = pointsMovers[idxPos].position - transform.position;
            Vector2 velocity = move * speed * Time.fixedDeltaTime;
            m_rigidbodys.velocity = velocity;
            animator.Play("SharkMove");
        }
        else
        {
            Awake();
        }
    }
}
