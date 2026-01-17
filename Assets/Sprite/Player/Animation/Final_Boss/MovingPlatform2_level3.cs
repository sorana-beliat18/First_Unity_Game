using UnityEngine;

public class MovingPlatform2_level3 : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;
    public bool moveHorizontal = true;
    public bool invert = false;

    private Vector3 startPos;

    void Start()
    {
        // Salvăm poziția de unde pleacă platforma la începutul jocului
        startPos = transform.position;
    }

    void Update()
    {
        // Calculăm mișcarea stânga-dreapta ca un pendul
        float movement = Mathf.PingPong(Time.time * speed, distance);

        if(invert) movement=-movement;

        if (moveHorizontal)
            transform.position = startPos + new Vector3(movement, 0, 0);
        else
            transform.position = startPos + new Vector3(0, movement, 0);
    }

    // Această funcție face ca jucătorul să se miște ODATĂ cu platforma când stă pe ea
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Boss"))
        {
            collision.transform.SetParent(transform);
        }
    }

    // Când jucătorul sare de pe platformă, nu mai este "purtat" de ea
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Boss"))
        {
            collision.transform.SetParent(null);
        }
    }
}
