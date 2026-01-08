using UnityEngine;

public class SlimeBounce : MonoBehaviour
{
    public float bounceForce = 15f; // Ajustează forța aici

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

            // --- ADAUGĂ ASTA ---
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null) audio.Play();
            // -------------------
        }
    }
}