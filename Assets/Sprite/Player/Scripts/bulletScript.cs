using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;
    void Start()
    {
    
        rb.linearVelocity = transform.right * speed;
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }

    

}
