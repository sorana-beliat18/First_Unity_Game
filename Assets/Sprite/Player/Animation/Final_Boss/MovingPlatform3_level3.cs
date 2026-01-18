using UnityEngine;

public class MovingPlatform3_level3 : MonoBehaviour
{
    public float speed = 2f;
    public float distanceUp = 2f;    
    public float distanceDown = 3f;  
    public bool invert = false;      

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float pingPong = Mathf.PingPong(Time.time * speed, 1f);
        float yOffset = Mathf.Lerp(-distanceDown, distanceUp, pingPong);
        if (invert) yOffset = -yOffset;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Boss"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Boss"))
        {
            collision.transform.SetParent(null);
        }
    }
}