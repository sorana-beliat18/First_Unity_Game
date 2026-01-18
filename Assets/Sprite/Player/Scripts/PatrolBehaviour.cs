using UnityEngine;

public class PatrolBetweenPoints : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA; // stânga
    public Transform pointB; // dreapta

    [Header("Movement")]
    public float speed = 2f;

    private Rigidbody2D rb;
    private bool movingRight;

    void Start()
{
    rb = GetComponent<Rigidbody2D>();

    // Asigură ordinea punctelor
    if (pointA.position.x > pointB.position.x)
    {
        Transform temp = pointA;
        pointA = pointB;
        pointB = temp;
    }

    
    movingRight = true;


    Vector3 scale = transform.localScale;
    scale.x = -Mathf.Abs(scale.x); 
    transform.localScale = scale;
}


    void FixedUpdate()
    {
        Vector2 targetPos = rb.position;

        if (movingRight)
        {
            targetPos.x += speed * Time.fixedDeltaTime;

            if (targetPos.x >= pointB.position.x)
            {
                targetPos.x = pointB.position.x;
                movingRight = false;
                Flip();
            }
        }
        else
        {
            targetPos.x -= speed * Time.fixedDeltaTime;

            if (targetPos.x <= pointA.position.x)
            {
                targetPos.x = pointA.position.x;
                movingRight = true;
                Flip();
            }
        }

        rb.MovePosition(targetPos);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        if (pointA && pointB)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
