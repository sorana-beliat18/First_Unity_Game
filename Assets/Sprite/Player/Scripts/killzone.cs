using UnityEngine;

public class killzone : MonoBehaviour
{
    public Transform startPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(health.currentLives); // omoară playerul
        }
    }
}
