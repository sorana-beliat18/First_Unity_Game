using UnityEngine;

public class killzone : MonoBehaviour
{
    public Transform startPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Vector3 respawn;

        if (Checkpoint.reached)
            respawn = Checkpoint.respawnPoint;
        else
            respawn = startPoint.position;

        other.transform.position = respawn;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.ResetHealth();
        }
    }
}
