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

    [Header("Combat Settings")]
    public float attackRange = 4.0f; // Ajustat pentru a preveni împingerea
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

        // Căutăm automat jucătorul dacă nu este pus în Inspector
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!player || isDead) return;

        // Efectul de tremurat pentru ultimul stadiu (Rage 2)
        if (rageLevel == 2)
        {
            transform.localPosition += (Vector3)Random.insideUnitCircle * shakeIntensity;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // LOGICĂ: Dacă e aproape, atacă. Dacă e departe, merge spre el.
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
    }

    void MoveTowardsPlayer()
    {
        float direction = (player.position.x > transform.position.x) ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            Flip();
    }

    void StopMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        if (animator != null)
            animator.SetFloat("Speed", 0f);
    }

    void ExecuteAttack()
    {
        lastAttackTime = Time.time;

        // 1. Oprim orice altă viteză care ar putea forța trecerea la Run
        animator.SetFloat("Speed", 0);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // 2. Declanșăm parametrii
        animator.SetInteger("RageLevel", rageLevel);
        animator.SetTrigger("Attack");

        // 3. COMANDA SUPREMĂ: Dacă după trigger tot nu vrea, îi dăm "brânci"
        // Folosim CrossFade pentru a trece instant în animație, ignorând orice săgeată
        string animName = (rageLevel == 0) ? "FinalBoss_Attack3" :
                          (rageLevel == 1) ? "FinalBoss_Attack1" : "FinalBoss_Attack2";

        animator.CrossFadeInFixedTime(animName, 0.05f);

        Debug.Log("Am forțat vizual atacul: " + animName);

        // 4. Damage către Player
        if (Vector2.Distance(transform.position, player.position) <= attackRange + 1f)
        {
            player.SendMessage("TakeDamage", 10, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (animator != null) animator.SetTrigger("Hurt");

        // Schimbare stadii în funcție de viață
        if (currentHealth <= rage1Threshold && rageLevel == 0) ActivateRage1();
        else if (currentHealth <= rage2Threshold && rageLevel == 1) ActivateRage2();

        if (currentHealth <= 0) StartCoroutine(DieSequence());
    }

    void ActivateRage1()
    {
        rageLevel = 1;
        moveSpeed += 2f;
        transform.localScale *= 1.05f;
        if (spriteRenderer != null) spriteRenderer.color = new Color(1f, 0.4f, 0.4f);
        if (rageSound != null) rageSound.Play();
        animator.SetInteger("RageLevel", 1);
    }

    void ActivateRage2()
    {
        rageLevel = 2;
        moveSpeed += 1.5f;
        transform.localScale *= 1.1f;
        if (spriteRenderer != null) spriteRenderer.color = Color.red;
        if (rageSound != null) rageSound.Play();
        animator.SetInteger("RageLevel", 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detectare glonț (Colliderul glonțului trebuie să aibă scriptul bulletScript)
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
        rb.simulated = false; // Îl scoatem din fizică

        if (animator != null)
        {
            animator.SetTrigger("Dead");
            // Forțăm animația Dead în caz că Any State o blochează
            animator.Play("FinalBoss_Dead", 0, 0f);
        }

        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}