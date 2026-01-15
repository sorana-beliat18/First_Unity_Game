using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 respawnPoint;
    public static bool reached;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (!reached)
            respawnPoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !reached)
        {
            reached = true;
            respawnPoint = transform.position;

            // ⭐ SALVEAZĂ STELUȚELE
            PlayerCollect pc = other.GetComponent<PlayerCollect>();
            if (pc != null)
            {
                PlayerCollect.starsAtCheckpoint = pc.stars;
            }

            anim.SetBool("isActive", true);
        }
    }
}
