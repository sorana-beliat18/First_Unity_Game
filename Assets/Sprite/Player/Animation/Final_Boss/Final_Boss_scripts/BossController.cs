using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource rageSound;
    public GameObject swordDrop; // Obiectul tău "gri" din hierarchy
    private SpriteRenderer spriteRenderer;

    [Header("Health & Stages")]
    public float maxHealth = 100f;
    private float currentHealth;
    public float rage1Threshold = 70f;
    public float rage2Threshold = 30f;
    public int rageLevel = 0;
    private bool isDead = false;

    [Header("Movement")]
    public float moveSpeed = 4f;
    private bool isFacingRight = true;

    [Header("Jumping Logic")]
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    public float jumpHeightThreshold = 2.5f;

    [Header("Advanced AI")]
    public float wallCheckDistance = 1.5f;
    public Transform ledgeCheck;
    public float ceilingCheckDistance = 3.0f;

    [Header("Combat Settings")]
    public float attackRange = 4.0f;
    public float attackCooldown = 2.0f;
    private float lastAttackTime = -999f;

    [Header("Effects")]
    public float shakeIntensity = 0.06f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Ne asigurăm că sabia este dezactivată la început
        if (swordDrop != null)
            swordDrop.SetActive(false);
    }

    void Update()
    {
        if (!player || isDead) return;

        // Pasul 1: Detecție mai mare (0.5f) pentru a evita blocarea animației
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);

        // Resetarea trigger-ului și forțarea revenirii din Jump dacă e pe sol
        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            animator.ResetTrigger("Jump");

            // Forțăm revenirea la Idle/Walk dacă a rămas blocat în ultimul cadru de Jump
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("FinalBoss_Jump"))
            {
                if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
                    animator.Play("FinalBoss_Walk");
                else
                    animator.Play("FinalBoss_Idle");
            }
        }

        if (rageLevel == 2)
        {
            transform.localPosition += (Vector3)Random.insideUnitCircle * shakeIntensity;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            StopMovement();
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                ExecuteAttack();
            }
        }
        else
        {
            MoveTowardsPlayer();
        }

        if (animator != null)
            animator.SetBool("IsGrounded", isGrounded);

        Debug.DrawRay(transform.position, isFacingRight ? Vector2.right * wallCheckDistance : Vector2.left * wallCheckDistance, Color.red);
        Debug.DrawRay(transform.position, Vector2.up * ceilingCheckDistance, Color.blue);
    }

    void MoveTowardsPlayer()
    {
        float direction = (player.position.x > transform.position.x) ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        Vector2 rayDir = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, rayDir, wallCheckDistance, groundLayer);

        bool isLedgeAhead = !Physics2D.OverlapCircle(ledgeCheck.position, 0.2f, groundLayer);

        RaycastHit2D ceilingCheck = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, groundLayer);
        bool isPathClearAbove = (ceilingCheck.collider == null);

        if (isGrounded && isPathClearAbove)
        {
            if ((player.position.y > transform.position.y + jumpHeightThreshold) || (wallCheck.collider != null) || isLedgeAhead)
            {
                Jump();
            }
        }

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            Flip();
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (animator != null)
            animator.SetTrigger("Jump");
    }

    void StopMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (animator != null)
            animator.SetFloat("Speed", 0f);
    }

    void ExecuteAttack()
    {
        if (isDead) return;
        lastAttackTime = Time.time;
        StopMovement();
        animator.SetTrigger("Attack");
        animator.SetInteger("RageLevel", rageLevel);

        string animName = (rageLevel == 0) ? "FinalBoss_Attack3" :
                          (rageLevel == 1) ? "FinalBoss_Attack1" : "FinalBoss_Attack2";

        animator.CrossFadeInFixedTime(animName, 0.1f);
        player.SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth < maxHealth / 2) animator.SetTrigger("Protect");
        else animator.SetTrigger("Hurt");

        if (currentHealth <= rage1Threshold && rageLevel == 0) ActivateRage1();
        else if (currentHealth <= rage2Threshold && rageLevel == 1) ActivateRage2();
        if (currentHealth <= 0) StartCoroutine(DieSequence());
    }

    void ActivateRage1()
    {
        rageLevel = 1;
        moveSpeed += 2f;
        jumpForce += 2f;
        transform.localScale *= 1.05f;
        if (spriteRenderer != null) spriteRenderer.color = new Color(1f, 0.4f, 0.4f);
        if (rageSound != null) rageSound.Play();
        animator.SetInteger("RageLevel", 1);
    }

    void ActivateRage2()
    {
        rageLevel = 2;
        moveSpeed += 1.5f;
        jumpForce += 2f;
        transform.localScale *= 1.1f;
        if (spriteRenderer != null) spriteRenderer.color = Color.red;
        if (rageSound != null) rageSound.Play();
        animator.SetInteger("RageLevel", 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<bulletScript>() != null)
        {
            TakeDamage(10f);
            Destroy(collision.gameObject);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    IEnumerator DieSequence()
    {
        isDead = true;

        // Oprim fizica imediat ca să nu mai alunece sau să sară
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        if (animator != null)
        {
            // Dezactivăm Any State prin oprirea Animatorului după ce pornește animația
            animator.Play("FinalBoss_Dead");
        }

        // Activăm sabia
        if (swordDrop != null)
        {
            swordDrop.SetActive(true);
            swordDrop.transform.position = transform.position;

            // IMPORTANT: Dacă tot nu se vede, forțăm SpriteRenderer-ul să fie în față
            SpriteRenderer swordSR = swordDrop.GetComponent<SpriteRenderer>();
            if (swordSR != null)
            {
                swordSR.sortingOrder = 10; // Îl pune deasupra restului hărții
            }
        }

        // Așteptăm 1 secundă să se vadă animația de moarte
        yield return new WaitForSeconds(1.0f);

        // Boss-ul dispare brusc
        Destroy(gameObject);
    }

    private float lastJumpTime = 0f;
    public float jumpCooldown = 0.5f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("JumpPoint"))
        {
            if (Time.time > lastJumpTime + jumpCooldown)
            {
                if (player.position.y > transform.position.y + 1.0f)
                {
                    JumpPointConfig config = other.GetComponent<JumpPointConfig>();

                    if (config != null)
                    {
                        // Săritură cu forță personalizată din JumpNode
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, config.customJumpForce);
                        animator.SetTrigger("Jump");
                        lastJumpTime = Time.time;
                    }
                    else
                    {
                        Jump();
                        lastJumpTime = Time.time;
                    }
                }
            }
        }
    }

    // Vizualizăm raza de detecție a solului în Editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.5f);
        }
    }
}