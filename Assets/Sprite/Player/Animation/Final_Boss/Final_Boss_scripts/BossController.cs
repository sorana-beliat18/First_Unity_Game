using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float jumpForce = 18f;
    private bool isFacingRight = true;
    private float currentMoveDir = 1f;
    public float changeDirThreshold = 1.5f;

    [Header("Detection Points")]
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform gapCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    [Header("Attack & Health Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;
    public int rageLevel = 0;

    [Header("AI Intelligence")]
    private float stuckTimer = 0f;
    private bool isAlternativeRouteActive = false;
    private float alternativeRouteTimer = 0f;
    private float altDirection = 1f;
    public float jumpScanDistance = 7f; // Mărit pentru a vedea platformele de departe

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!player) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        bool isWallInFront = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        if (distanceToPlayer <= attackRange)
        {
            StopBoss();
            TryAttack();
        }
        else
        {
            MoveTowardsPlayer(isGrounded, isWallInFront);
        }
    }

    void MoveTowardsPlayer(bool grounded, bool wall)
    {
        float horizontalDist = player.position.x - transform.position.x;
        float verticalDist = player.position.y - transform.position.y;

        // 1. DIRECȚIE CONSTANTĂ
        if (horizontalDist > changeDirThreshold) currentMoveDir = 1f;
        else if (horizontalDist < -changeDirThreshold) currentMoveDir = -1f;

        float finalMoveDir = currentMoveDir;

        // 2. DETECȚIE TAVAN ȘI SCANARE ÎN FAȚĂ
        bool isCeilingAbove = Physics2D.Raycast(transform.position, Vector2.up, 2.5f, groundLayer);

        // Scanăm în față pentru a găsi SOL (indiferent dacă e platformă fixă sau mobilă)
        Vector2 scanDirection = new Vector2(currentMoveDir, -0.5f).normalized;
        RaycastHit2D hit = Physics2D.Raycast(gapCheck.position, scanDirection, jumpScanDistance, groundLayer);
        Debug.DrawRay(gapCheck.position, scanDirection * jumpScanDistance, Color.cyan);

        bool isGapInFront = !Physics2D.OverlapCircle(gapCheck.position, checkRadius, groundLayer);

        // 3. LOGICA DE SĂRITURĂ/PRĂPASTIE (REPARATĂ)
        if (grounded && isGapInFront)
        {
            // DACĂ PLAYERUL E MAI SUS (indiferent dacă e prăpastie sau nu), SARE!
            if (verticalDist > 1.2f && !isCeilingAbove)
            {
                rb.linearVelocity = new Vector2(currentMoveDir * moveSpeed, jumpForce);
            }
            // DACĂ PLAYERUL E LA ACELAȘI NIVEL SAU MAI JOS, DAR AVEM O PLATFORMĂ ÎN FAȚĂ
            else if (hit.collider != null)
            {
                float distToLand = Vector2.Distance(gapCheck.position, hit.point);
                if (distToLand < 5f && !isCeilingAbove)
                {
                    rb.linearVelocity = new Vector2(currentMoveDir * moveSpeed, jumpForce);
                }
                else
                {
                    StopBoss(); // Așteaptă platforma mobilă
                    return;
                }
            }
            // DACĂ PLAYERUL E JOS ÎN PRĂPASTIE, NU SARE, DAR NICI NU ÎNGHEAȚĂ (Merge până la margine)
            else if (verticalDist < -2f)
            {
                // Îl lăsăm să meargă, va cădea natural după player
            }
            else
            {
                StopBoss();
                return;
            }
        }

        // 4. LOGICA DE PERETE
        if (wall && grounded && !isCeilingAbove)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 1.1f);
            stuckTimer += Time.deltaTime;
            if (stuckTimer > 0.5f)
            {
                isAlternativeRouteActive = true;
                alternativeRouteTimer = 0.8f;
                altDirection = -currentMoveDir;
                stuckTimer = 0;
            }
        }
        else { stuckTimer = 0; }

        if (isAlternativeRouteActive)
        {
            finalMoveDir = altDirection;
            alternativeRouteTimer -= Time.deltaTime;
            if (alternativeRouteTimer <= 0) isAlternativeRouteActive = false;
        }

        // 5. MIȘCAREA EFECTIVĂ
        rb.linearVelocity = new Vector2(finalMoveDir * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

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
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        if ((dir > 0 && !isFacingRight) || (dir < 0 && isFacingRight)) Flip();
        animator.SetInteger("RageLevel", rageLevel);
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    void Flip() { isFacingRight = !isFacingRight; transform.Rotate(0, 180, 0); }

    public void TakeHit()
    {
        animator.SetTrigger("Hurt");
        rageLevel = Mathf.Clamp(rageLevel + 1, 0, 2);
        lastAttackTime = Time.time + 0.5f;
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        this.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
            transform.SetParent(collision.transform, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform") && gameObject.activeInHierarchy)
            transform.parent = null;
    }
}