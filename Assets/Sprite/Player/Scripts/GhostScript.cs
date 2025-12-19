using System.Collections;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    public float speed = 10f;
    public float transparentTime = 0.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 screenBounds;

    private bool isTransparent = false;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // salvăm culoarea inițială
        originalColor = sr.color;

        rb.linearVelocity = new Vector2(-speed, 0);
    }

    void Update()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(0, 0, 0)
        );

        if (transform.position.x < screenBounds.x - 1f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // NU pornim din nou dacă deja e transparentă
        if (!isTransparent)
        {
            StartCoroutine(TransparentEffect());
        }
    }

    IEnumerator TransparentEffect()
    {
        isTransparent = true;

        // devine transparentă
        Color c = originalColor;
        c.a = 0.4f;
        sr.color = c;

        yield return new WaitForSeconds(transparentTime);

        // revine EXACT la culoarea inițială
        sr.color = originalColor;

        isTransparent = false;
    }
}
