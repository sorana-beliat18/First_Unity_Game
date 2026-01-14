using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [SerializeField] private Collider2D m_CrouchDisableCollider;

    const float k_GroundedRadius = .2f;
    const float k_CeilingRadius = .2f;

    private bool m_Grounded;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    // ===== DOUBLE JUMP =====
    public int extraJumps = 0;
    public int maxExtraJumps = 0;
    private bool doubleJumpActive = false;
    private Coroutine doubleJumpRoutine;

    [Header("Events")]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    public BoolEvent OnCrouchEvent;

    private bool m_wasCrouching = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            m_GroundCheck.position,
            k_GroundedRadius,
            m_WhatIsGround
        );

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;

                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();

                    // 🔑 REÎNCARCĂ DOUBLE JUMP DACĂ POWER-UP-UL E ACTIV
                    if (doubleJumpActive)
                        extraJumps = maxExtraJumps;
                }
            }
        }
    }

    // ===== POWER-UP TIMER =====
    public void EnableDoubleJumpForSeconds(float seconds, int jumps = 1)
    {
        if (doubleJumpRoutine != null)
            StopCoroutine(doubleJumpRoutine);

        doubleJumpRoutine = StartCoroutine(DoubleJumpTimer(seconds, jumps));
    }

    private IEnumerator DoubleJumpTimer(float seconds, int jumps)
    {
        doubleJumpActive = true;
        maxExtraJumps = jumps;
        extraJumps = jumps;

        yield return new WaitForSeconds(seconds);

        doubleJumpActive = false;
        maxExtraJumps = 0;
        extraJumps = 0;
        doubleJumpRoutine = null;
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!crouch)
        {
            if (Physics2D.OverlapCircle(
                m_CeilingCheck.position,
                k_CeilingRadius,
                m_WhatIsGround))
            {
                crouch = true;
            }
        }

        if (m_Grounded || m_AirControl)
        {
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                move *= m_CrouchSpeed;

                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            Vector3 targetVelocity = new Vector2(
                move * 10f,
                m_Rigidbody2D.linearVelocity.y
            );

            m_Rigidbody2D.linearVelocity = Vector3.SmoothDamp(
                m_Rigidbody2D.linearVelocity,
                targetVelocity,
                ref m_Velocity,
                m_MovementSmoothing
            );

            if (move > 0 && !m_FacingRight)
                Flip();
            else if (move < 0 && m_FacingRight)
                Flip();
        }

        // ===== JUMP + DOUBLE JUMP =====
        if (jump && (m_Grounded || extraJumps > 0))
        {
            if (!m_Grounded)
                extraJumps--;

            m_Grounded = false;

            m_Rigidbody2D.linearVelocity = new Vector2(
                m_Rigidbody2D.linearVelocity.x,
                0f
            );

            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
