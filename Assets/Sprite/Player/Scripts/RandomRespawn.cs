using System.Collections;
using UnityEngine;

public class RandomRespawn : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float RandomRespawnTime = 15f;
    private Vector2 screenBounds;

    void Start()
    {
        StartCoroutine(ghostWave());
    }

    private void enemySpawn()
    {
        // recalculăm limitele camerei (pentru că se mișcă!)
        screenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );

        GameObject a = Instantiate(ghostPrefab);

        // spawn PUȚIN în dreapta camerei
        a.transform.position = new Vector2(
            screenBounds.x + 1.5f,
            Random.Range(-screenBounds.y, screenBounds.y)
        );
    }

    IEnumerator ghostWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(RandomRespawnTime);
            enemySpawn();
        }
    }
}
