using UnityEngine;

public class MovingPlatform4_level3 : MonoBehaviour
{
    public float speed = 2f;
    public float distanceUp = 2f;    // Câte blocuri merge în sus
    public float distanceDown = 3f;  // Câte blocuri merge în jos
    public bool invert = false;      // Dacă vrei să înceapă mișcarea invers

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Calculăm o valoare care merge de la 0 la 1 și înapoi la 0
        float pingPong = Mathf.PingPong(Time.time * speed, 1f);

        // Transformăm acea valoare într-o mișcare între -distanceDown și +distanceUp
        // Lerp face tranziția lină între cele două puncte
        float yOffset = Mathf.Lerp(-distanceDown, distanceUp, pingPong);

        if (invert) yOffset = -yOffset;

        transform.position = startPos + new Vector3(0, yOffset, 0);
    }

    // Păstrăm logica pentru a purta Player-ul/Boss-ul
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