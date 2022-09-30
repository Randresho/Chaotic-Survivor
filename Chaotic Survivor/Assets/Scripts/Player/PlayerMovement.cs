using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput m_PlayerInput;
    public Rigidbody2D m_rigidbody2D;
    public float m_PlayerSpeed = 0f;
    public float m_PlayerSpeedMax = 0f;
    [SerializeField] private Animator animator;
    [SerializeField] private LevelManager levelManager;
    private Vector2 moveInput;


    // Start is called before the first frame update
    void Awake()
    {
        m_PlayerInput = new PlayerInput();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.playerMovement = this;
        m_PlayerSpeedMax = m_PlayerSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = m_PlayerInput.PlayerMovement.Walk.ReadValue<Vector2>();
        SetVelocity(m_PlayerSpeed);

        animator.SetFloat("VelX", m_rigidbody2D.velocity.x);
        animator.SetFloat("VelY", m_rigidbody2D.velocity.y);
    }

    private void SetVelocity(float speed)
    {
        m_rigidbody2D.velocity = moveInput * speed;
    }

    #region Input Enable / Disable
    private void OnEnable()
    {
        m_PlayerInput.Enable();
    }
    private void OnDisable()
    {
        m_PlayerInput.Disable();
    }
    #endregion
}
