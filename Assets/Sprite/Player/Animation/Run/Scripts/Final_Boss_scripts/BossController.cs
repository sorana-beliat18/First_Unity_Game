using UnityEngine;
using UnityEngine.AI; // OBLIGATORIU pentru NavMesh

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Rigidbody2D rb;
    public Animator animator;
    private NavMeshAgent agent; // Adăugat pentru GPS

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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>(); // Inițializăm agentul

        // Setări vitale pentru ca agentul să nu se bată cu fizica 2D
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position); // "Teleportează" agentul pe zona validă
        }
    }

    void Update() // Mutăm logica în Update pentru NavMesh
    {
        if (!player) return;

        if (!hasStarted)
        {
            // Boss-ul pornește doar când player-ul se mișcă prima dată
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                hasStarted = true;

            StopBoss();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            StopBoss();
            TryAttack();
        }
        else if (distance > stopDistance)
        {
            MoveWithNavMesh();
        }
        else
        {
            StopBoss();
        }

        // Controlăm Flip-ul în funcție de unde vrea agentul să meargă
        if (agent.velocity.x != 0)
        {
            Flip(Mathf.Sign(agent.velocity.x));
        }
    }

    void MoveWithNavMesh()
    {
        agent.isStopped = false; // Permitem mișcarea
        agent.SetDestination(player.position); // GPS-ul calculează drumul
        float currentSpeed = agent.velocity.sqrMagnitude;
        animator.SetFloat("Speed", currentSpeed>0.1f ? moveSpeed : 0f);
    }

    void StopBoss()
    {
        agent.isStopped = true; // Oprim agentul de NavMesh
        agent.velocity = Vector2.zero; // Resetăm viteza fizică
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
        animator.SetTrigger("Hurt");
        rageLevel = Mathf.Clamp(rageLevel + 1, 0, 2);
        lastAttackTime = -999f;
        TryAttack();
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
        hasStarted = false;
        agent.isStopped = true;
        this.enabled = false;
    }
}