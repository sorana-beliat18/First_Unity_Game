using UnityEngine;
using System.Collections;

public class ShieldPower : MonoBehaviour
{
    public float shieldDuration = 20f;
    //public float respawnTime = 30f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.ActivateShield(shieldDuration);
            }

            gameObject.SetActive(false);


            

        }

        
    }
}
