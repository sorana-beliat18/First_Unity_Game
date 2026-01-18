using UnityEngine;

public class MovingPlatform1_level3 : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;
    public bool moveHorizontal = true;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float movement = Mathf.PingPong(Time.time * speed, distance);

        if (moveHorizontal)
            transform.position = startPos + new Vector3(movement, 0, 0);
        else
            transform.position = startPos + new Vector3(0, movement, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.activeInHierarchy && collision.gameObject.activeInHierarchy)
            {
                collision.transform.SetParent(null);
            }
            else
            {
                collision.transform.parent = null;
            }
        }
    }
}