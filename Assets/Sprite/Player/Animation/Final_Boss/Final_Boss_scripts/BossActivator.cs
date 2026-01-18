using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public BossController bossScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (bossScript != null)
            {
                bossScript.SetActivated(true);
                Debug.Log("Boss activat!");
            }
            Destroy(gameObject); 
        }
    }
}