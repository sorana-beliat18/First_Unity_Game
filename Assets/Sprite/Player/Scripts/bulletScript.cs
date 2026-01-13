using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    void Start()
    {
    
        rb.linearVelocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // distruge inamicul
            Destroy(gameObject);            // distruge glonțul
        }
    }
    

    

}
