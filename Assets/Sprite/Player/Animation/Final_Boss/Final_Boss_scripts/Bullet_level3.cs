using UnityEngine;

public class Bullet_level3 : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
