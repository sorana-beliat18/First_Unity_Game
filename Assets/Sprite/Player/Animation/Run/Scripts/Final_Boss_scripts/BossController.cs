using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 12f;
    public float stopDistance = 1.5f;

    [Header("Attack")]
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;

    [Header("Rage")]
    public int rageLevel = 0;

    float lastAttackTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!player) return;

        float distance = Vector2.Distance(rb.position, player.position);

        if (distance > chaseRange)
        {
            Stop();
            return;
        }

        if (distance <= attackRange)
        {
            Stop();
            TryAttack();
            return;
        }

        float dir = Mathf.Sign(player.position.x - rb.position.x);
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        Flip(dir);
    }

    void Stop()
    {
        rb.linearVelocity = Vector2.zero;
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
        rageLevel = Mathf.Clamp(rageLevel + 1, 0, 2);
        lastAttackTime = -999f;
        TryAttack();
    }
}