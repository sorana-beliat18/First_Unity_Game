using System.Collections;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    public float speed = 10f;
    public float transparentTime = 0.5f;
    public int damage = 1;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 screenBounds;

    private bool isTransparent = false;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        originalColor = sr.color;
        rb.linearVelocity = new Vector2(-speed, 0);
    }

    void Update()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(Vector3.zero);

        if (transform.position.x < screenBounds.x - 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // DAMAGE PLAYER
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        // EFECT TRANSPARENȚĂ (o singură dată)
        if (!isTransparent)
        {
            StartCoroutine(TransparentEffect());
        }
    }

    IEnumerator TransparentEffect()
    {
        isTransparent = true;

        Color c = originalColor;
        c.a = 0.4f;
        sr.color = c;

        yield return new WaitForSeconds(transparentTime);

        sr.color = originalColor;
        isTransparent = false;
    }
}
