using UnityEngine;

public class Star : MonoBehaviour
{
    private StarSpawner spawner;

    public void SetSpawner(StarSpawner s)
    {
        spawner = s;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCollect>()?.AddStar();
            spawner.SpawnStar();
            Destroy(gameObject);
        }
    }
}
