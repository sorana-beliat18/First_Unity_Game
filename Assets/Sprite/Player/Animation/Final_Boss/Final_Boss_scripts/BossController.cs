using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource rageSound;
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
    public Transform groundCheck;   // Obiect gol pus la picioarele boss-ului
    public LayerMask groundLayer;   // Stratul "Ground" pentru platforme
    private bool isGrounded;
    public float jumpHeightThreshold = 2.5f; // Cât de sus trebuie să fie playerul ca boss-ul să sară

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
    }

    void Update()
    {
        if (!player || isDead) return;

        // Verificăm dacă boss-ul atinge pământul
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

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

        // Sincronizăm starea de pământ cu animatorul (dacă ai parametrul IsGrounded)
        if (animator != null)
            animator.SetBool("IsGrounded", isGrounded);
    }

    void MoveTowardsPlayer()
    {
        float direction = (player.position.x > transform.position.x) ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // LOGICĂ SĂRITURĂ: Sare dacă e pe pământ și jucătorul e mai sus decât pragul setat
        if (isGrounded && player.position.y > transform.position.y + jumpHeightThreshold)
        {
            Jump();
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
        {
            animator.SetTrigger("Jump"); // Activează animația FinalBoss_Jump
        }
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

        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
        animator.SetInteger("RageLevel", rageLevel);

        string animName = (rageLevel == 0) ? "FinalBoss_Attack3" :
                          (rageLevel == 1) ? "FinalBoss_Attack1" : "FinalBoss_Attack2";

        animator.CrossFadeInFixedTime(animName, 0.1f);

        // Trimitere damage către player (ajustat la 1 pentru cele 3 vieți)
        player.SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < maxHealth / 2)
        {
            animator.SetTrigger("Protect");
        }
        else
        {
            animator.SetTrigger("Hurt");
        }

        if (currentHealth <= rage1Threshold && rageLevel == 0) ActivateRage1();
        else if (currentHealth <= rage2Threshold && rageLevel == 1) ActivateRage2();

        if (currentHealth <= 0) StartCoroutine(DieSequence());
    }

    void ActivateRage1()
    {
        rageLevel = 1;
        moveSpeed += 2f;
        jumpForce += 2f; // Sare mai sus în rage
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
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        if (animator != null)
        {
            animator.SetTrigger("Dead");
            animator.Play("FinalBoss_Dead", 0, 0f);
        }

        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}