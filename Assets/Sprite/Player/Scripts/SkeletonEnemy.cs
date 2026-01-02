using System.Collections;
using UnityEngine;

public class SkeletonEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float emergeSpeed = 2f;
    public float chaseSpeed = 2.5f;
    public float emergeHeight = 1f;

    [Header("Animation Timing")]
    public float emergeAnimDuration = 3f; // EXACT Length din Animation Clip

    private Vector3 hiddenPos;
    private Vector3 emergePos;

    private bool emerging = false;
    private bool chasing = false;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        // enemy inactiv la început
        rb.simulated = false;
        col.enabled = false;

        hiddenPos = transform.position;
        emergePos = hiddenPos + Vector3.up * emergeHeight;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // chemat din EnemyWakeUpTrigger
    public void WakeUp()
    {
        rb.simulated = true;
        emerging = true;

        anim.SetTrigger("Emerge");

        StartCoroutine(StartChaseAfterEmerge());
    }

    IEnumerator StartChaseAfterEmerge()
    {
        yield return new WaitForSeconds(emergeAnimDuration);

        emerging = false;
        chasing = true;
        col.enabled = true;
    }

    void FixedUpdate()
    {
        // 1️⃣ Iese din pământ
        if (emerging)
        {
            Vector2 pos = Vector2.MoveTowards(
                rb.position,
                emergePos,
                emergeSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(pos);
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        // 2️⃣ Urmărește player-ul
        else if (chasing)
        {
            float dir = Mathf.Sign(player.position.x - rb.position.x);
            rb.linearVelocity = new Vector2(dir * chaseSpeed, rb.linearVelocity.y);

            // 🔁 FLIP CORECT DUPĂ POZIȚIA PLAYER-ULUI
            Vector3 scale = transform.localScale;

            // Sprite-ul e desenat cu fața la STÂNGA
            if (player.position.x > rb.position.x)
                scale.x = -Mathf.Abs(scale.x); // privește dreapta
            else
                scale.x = Mathf.Abs(scale.x);  // privește stânga

            transform.localScale = scale;
        }
    }
}
