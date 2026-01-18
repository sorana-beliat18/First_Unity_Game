using UnityEngine;

public class PortalStopMusic : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (musicSource != null)
                musicSource.Stop();
        }
    }
}
