using UnityEngine;

public class PatrolEnemyDamage : MonoBehaviour
{
    public int damage = 1;
    public float damageCooldown = 1f;

    private float lastDamageTime;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // cooldown – să nu scadă toate viețile instant
        if (Time.time - lastDamageTime < damageCooldown) return;

        PlayerHealth playerHealth =
            collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}
