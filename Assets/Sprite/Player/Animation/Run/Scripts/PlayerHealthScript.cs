using UnityEngine;
using System.Collections;


public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public HeartBarScript healthBar;

    void Start()
    {
        currentLives = maxLives;
        healthBar.SetMaxHealth(maxLives);
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        healthBar.SetHealth(currentLives);

        if (currentLives <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        currentLives = maxLives;
        healthBar.SetHealth(currentLives);
    }

    void Die()
    {
        Vector3 respawnPosition;

        if (Checkpoint.reached)
        {
            respawnPosition = Checkpoint.respawnPoint;
        }
        else
        {
            respawnPosition = GameObject
                .FindGameObjectWithTag("StartPoint")
                .transform.position;
        }

        transform.position = respawnPosition;
        ResetHealth();
        RespawnPowerUps();
    }
    void RespawnPowerUps()
    {
    DoubleJumpPowerUp[] powerUps =
        FindObjectsOfType<DoubleJumpPowerUp>(true); // include inactive

    foreach (DoubleJumpPowerUp p in powerUps)
    {
        p.Respawn();
    }
   }
   public void RespawnPowerUpAfterTime(DoubleJumpPowerUp powerUp, float time)
{
    StartCoroutine(RespawnPowerUpCoroutine(powerUp, time));
}

IEnumerator RespawnPowerUpCoroutine(DoubleJumpPowerUp powerUp, float time)
{
    yield return new WaitForSeconds(time);
    powerUp.Respawn();
}





}
