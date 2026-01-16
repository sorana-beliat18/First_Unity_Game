using UnityEngine;

public class PortalTrigger2D : MonoBehaviour
{
    public LevelTimer2D timer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 👉 oprește și ascunde timerul
        if (timer != null)
            timer.StopAndHideTimer();

        // aici pui win / next level
    }
}

