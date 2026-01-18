using UnityEngine;

public class Star : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

       

        PlayerCollect pc = other.GetComponent<PlayerCollect>();
        if (pc != null)
        {
            pc.AddStar();
        }
       

        
        gameObject.SetActive(false);

        
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
    }
}
