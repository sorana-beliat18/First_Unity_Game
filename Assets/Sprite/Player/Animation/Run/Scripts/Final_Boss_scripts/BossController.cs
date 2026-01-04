using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float jumpForce = 12f;
    private bool hasStarted = false;
    private bool isFacingRight = true;

    [Header("Detection Points")]
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;
    public int rageLevel = 0;

    // --- AICI PUNEM VARIABILELE NOI PENTRU INTELIGENȚĂ ---
    [Header("AI Intelligence")]
    public float obstacleDetectionDistance = 1.0f;
    private float stuckTimer = 0f;
    private bool isAlternativeRouteActive = false;
    private float alternativeRouteTimer = 0f;
    private float altDirection = 1f;

    [Header("Moving Platforms Support")]
    public Transform gapCheck; // Trage noul obiect aici
    private bool isOnPlatform = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!player) return;

        if (!hasStarted)
        {
            // ... (codul tău de start)
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Verificăm mediul
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        bool isWallInFront = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        // LOGICA DE ATAC (Dacă este aproape)
        if (distanceToPlayer <= attackRange)
        {
            StopBoss(); // Această funcție pune viteza pe 0 și Speed pe 0
            TryAttack();
        }
        // LOGICA DE MIȘCARE
        else
        {
            MoveTowardsPlayer(isGrounded, isWallInFront);
        }
    }

    // --- ACEASTA ESTE METODA NOUĂ ȘI DETALIATĂ ---
    void MoveTowardsPlayer(bool grounded, bool wall)
    {
        float directionToPlayer = Mathf.Sign(player.position.x - transform.position.x);
        float finalMoveDir = directionToPlayer;

        // --- LOGICA ÎMBUNĂTĂȚITĂ PENTRU PRĂPASTIE ---
        bool isGapInFront = !Physics2D.OverlapCircle(gapCheck.position, checkRadius, groundLayer);

        if (grounded && isGapInFront)
        {
            // 1. Dacă playerul e mai sus, SARE obligatoriu
            if (player.position.y > transform.position.y + 0.5f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            // 2. Dacă playerul e departe pe orizontală (mai mult de 3 unități), SARE înainte (Long Jump)
            else if (Mathf.Abs(player.position.x - transform.position.x) > 3f)
            {
                rb.linearVelocity = new Vector2(directionToPlayer * moveSpeed, jumpForce * 0.8f);
            }
            // 3. Dacă playerul e chiar sub el sau foarte aproape, se oprește (așteaptă platforma)
            else
            {
                StopBoss();
                return;
            }
        }
        // 1. LOGICA DE EVITARE PERETE
        if (wall)
        {
            if (grounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            stuckTimer += Time.deltaTime;
            if (stuckTimer > 0.5f)
            {
                isAlternativeRouteActive = true;
                alternativeRouteTimer = 1.0f;
                altDirection = -directionToPlayer;
                stuckTimer = 0;
            }
        }
        else
        {
            stuckTimer = 0;
        }

        // 2. APLICARE RUTĂ ALTERNATIVĂ (Dacă e blocat)
        if (isAlternativeRouteActive)
        {
            finalMoveDir = altDirection;
            alternativeRouteTimer -= Time.deltaTime;
            if (alternativeRouteTimer <= 0) isAlternativeRouteActive = false;
        }

        // 3. EXCEPȚIE: Dacă player-ul e deasupra, sare oricum (Jump up)
        if (grounded && player.position.y > transform.position.y + 2f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Aplicăm viteza finală
        rb.linearVelocity = new Vector2(finalMoveDir * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("Speed", moveSpeed);

        // Flip visual
        if ((finalMoveDir > 0 && !isFacingRight) || (finalMoveDir < 0 && isFacingRight))
        {
            Flip();
        }
    }
    void StopBoss()
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

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void TakeHit()
    {
        animator.SetTrigger("Hurt");
        rageLevel = Mathf.Clamp(rageLevel + 1, 0, 2);
        lastAttackTime = -999f;
        TryAttack();
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
        hasStarted = false;
        rb.linearVelocity = Vector2.zero;
        this.enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Când atinge o platformă mișcătoare, se "lipește" de ea
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = collision.transform;
            isOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Când pleacă de pe ea, redevine independent
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.parent = null;
            isOnPlatform = false;
        }
    }
}