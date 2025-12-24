using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    public float speed = 2f;
    public float rayDist = 1f;
    public LayerMask groundLayer;
    public Transform groundDetection;

    private bool movingRight = true;

    void Update()
    {
        // mișcare în direcția corectă
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        transform.Translate(direction * speed * Time.deltaTime);

        // verificăm dacă mai e sol în față
        RaycastHit2D groundCheck = Physics2D.Raycast(
            groundDetection.position,
            Vector2.down,
            rayDist,
            groundLayer
        );

        // dacă nu mai e sol → întoarce
        if (!groundCheck.collider)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        // rotire vizuală
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
