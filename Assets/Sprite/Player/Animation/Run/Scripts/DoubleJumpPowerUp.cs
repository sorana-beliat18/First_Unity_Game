using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    public int bonusJumps = 1;
    public float duration = 10f;

    private Vector3 startPosition;
    private bool isScheduledForRespawn = false;

    void Start()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CharacterController2D controller =
            other.GetComponent<CharacterController2D>();

        PlayerHealth health =
            other.GetComponent<PlayerHealth>();

        if (controller != null && health != null && !isScheduledForRespawn)
        {
            Collect(controller);
            isScheduledForRespawn = true;

            // player-ul pornește timer-ul
            health.RespawnPowerUpAfterTime(this, 15f);
        }
    }

    public void Collect(CharacterController2D controller)
    {
        controller.EnableDoubleJumpForSeconds(duration, bonusJumps);
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        isScheduledForRespawn = false;
        transform.position = startPosition;
        gameObject.SetActive(true);
    }
}
