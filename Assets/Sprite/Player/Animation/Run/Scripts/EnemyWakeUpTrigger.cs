using UnityEngine;

public class EnemyWakeUpTrigger : MonoBehaviour
{
    public SkeletonEnemy enemy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.WakeUp();
            Destroy(gameObject);
        }
    }
}
