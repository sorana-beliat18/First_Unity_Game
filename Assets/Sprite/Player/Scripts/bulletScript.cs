using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.CompareTag("Enemy"))
        {
          

            Destroy(other.gameObject);      // distruge inamicul
            Destroy(gameObject, 0.1f);      // distruge glonțul DUPĂ sunet
        }
    }
}
