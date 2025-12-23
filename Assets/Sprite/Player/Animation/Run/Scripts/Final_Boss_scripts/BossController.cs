using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float stopDistance = 1.5f;
    private bool hasStarted = false;

    [Header("Attack")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;

    [Header("Rage")]
    public int rageLevel = 0; // 0 = Attack3, 1 = Attack1, 2 = Attack2

    [Header("Physics & Jumping")]
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform wallCheck;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!player) return;

        if (!hasStarted)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                hasStarted = true;

            Stop();
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        bool wallAhead = Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);

        if (isGrounded && wallAhead)
        {
            Jump();
        }

        float distance = Vector2.Distance(rb.position, player.position);

        if (distance <= attackRange)
        {
            Stop();
            TryAttack();
        }
        else if (distance > stopDistance)
        {
            Move(); // Acum functia aceasta exista mai jos!
        }
        else
        {
            Stop();
        }
    }

    // --- FUNCTIA CARE LIPSEA ---
    void Move()
    {
        float diffX = player.position.x - rb.position.x;

        // Evitam tremuratul daca e foarte aproape
        if (Mathf.Abs(diffX) < 0.1f)
        {
            Stop();
            return;
        }

        float dir = Mathf.Sign(diffX);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        // Setam animatia de mers
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        Flip(dir);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    void Stop()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetFloat("Speed", 0f);
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        animator.SetInteger("RageLevel", rageLevel);
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    void Flip(float dir)
    {
        if (dir == 0) return;
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (dir > 0 ? 1 : -1);
        transform.localScale = s;
    }

    public void TakeHit()
    {
        animator.SetTrigger("Hurt"); // Am adaugat si animatia de Hurt aici
        rageLevel = Mathf.Clamp(rageLevel + 1, 0, 2);
        lastAttackTime = -999f;
        TryAttack();
    }
    public void Die()
    {
        animator.SetTrigger("Dead");
        hasStarted = false; // Oprim urmarirea
        rb.linearVelocity = Vector2.zero; // Oprim orice miscare fizica
        this.enabled = false; // Dezactivam scriptul ca sa nu mai faca nimic
    }
}