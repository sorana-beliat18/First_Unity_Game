using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;
    public bool reverse;   // 👈 nou

    private int i;

    void Start()
    {
        if (!reverse)
        {
            i = 0;
        }
        else
        {
            i = points.Length - 1;
        }

        transform.position = points[i].position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, points[i].position) < 0.01f)
        {
            if (!reverse)
            {
                i++;
                if (i >= points.Length)
                    i = 0;
            }
            else
            {
                i--;
                if (i < 0)
                    i = points.Length - 1;
            }
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            points[i].position,
            speed * Time.deltaTime
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
