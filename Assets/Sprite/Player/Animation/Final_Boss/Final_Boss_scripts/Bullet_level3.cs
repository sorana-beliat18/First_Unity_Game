using UnityEngine;

public class Bullet_level3 : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;

    void Update()
    {
        // Mișcare în față
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Distruge glonțul dacă atinge pământul sau un perete
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        // Dacă lovește inamicul (Boss-ul)
        if (collision.CompareTag("Enemy"))
        {
            // Boss-ul are deja OnCollision în scriptul lui care detectează glonțul
            // așa că aici doar îl distrugem după impact
            Destroy(gameObject);
        }
    }
}
