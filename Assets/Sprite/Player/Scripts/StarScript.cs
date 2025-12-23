using UnityEngine;

public class Star : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollect pc = other.GetComponent<PlayerCollect>();
            if (pc != null)
            {
                pc.AddStar();
            }

            Destroy(gameObject);
        }
    }
}
