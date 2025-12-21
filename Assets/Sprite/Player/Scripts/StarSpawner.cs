using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;
    public float padding = 0.5f;
    public LayerMask obstacleLayer; // platforme / pereți

    void Start()
    {
        SpawnStar();
    }

    public void SpawnStar()
    {
        Camera cam = Camera.main;

        Vector3 bl = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 tr = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        for (int i = 0; i < 20; i++) // încearcă de max 20 ori
        {
            float x = Random.Range(bl.x + padding, tr.x - padding);
            float y = Random.Range(bl.y + padding, tr.y - padding);

            Vector2 pos = new Vector2(x, y);

            // verificăm dacă e ceva acolo
            Collider2D hit = Physics2D.OverlapCircle(pos, 0.3f, obstacleLayer);

            if (hit == null)
            {
                GameObject star = Instantiate(starPrefab, pos, Quaternion.identity);
                star.GetComponent<Star>().SetSpawner(this);
                return;
            }
        }

        Debug.LogWarning("NU s-a găsit spațiu liber pentru steluță");
    }
}
