using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public HeartBarScript healthBar;

    // SHIELD
    public bool isInvincible = false;
    public float shieldOpacity = 0.5f;
    public GameObject shieldVisual; // optional

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentLives = maxLives;
        healthBar.SetMaxHealth(maxLives);

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    // APELAT DE INAMICI / CAPCANE
    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        healthBar.SetHealth(currentLives);

        if (currentLives <= 0)
        {
            Die();
        }
    }

    // ☠️ MOARTE + RESPAWN
    void Die()
    {
        Vector3 respawnPosition;

        if (Checkpoint.reached)
            respawnPosition = Checkpoint.respawnPoint;
        else
            respawnPosition = GameObject
                .FindGameObjectWithTag("StartPoint")
                .transform.position;

        transform.position = respawnPosition;

        // reset viata
        currentLives = maxLives;
        healthBar.SetHealth(currentLives);

        // reset / restore stele
        PlayerCollect pc = GetComponent<PlayerCollect>();
        if (pc != null)
        {
            if (Checkpoint.reached)
            {
                pc.RestoreStarsFromCheckpoint();
            }
            else
            {
                pc.stars = 0;
                PlayerCollect.starsAtCheckpoint = 0;
                pc.starUI.UpdateStars(0);
            }
        }

        RespawnStars();
        RespawnPowerUps();
    }

    // ⭐ RESPAWN STARS
    void RespawnStars()
    {
        StarRespawner[] stars = FindObjectsOfType<StarRespawner>(true);

        foreach (StarRespawner s in stars)
        {
            s.Respawn();
        }
    }

    // ⚡ RESPAWN POWER-UPS
    void RespawnPowerUps()
    {
        DoubleJumpPowerUp[] powerUps =
            FindObjectsOfType<DoubleJumpPowerUp>(true);

        foreach (DoubleJumpPowerUp p in powerUps)
        {
            p.Respawn();
        }
    }

    // ⚡ POWER-UP RESPAWN DUPĂ TIMP
    public void RespawnPowerUpAfterTime(DoubleJumpPowerUp powerUp, float time)
    {
        StartCoroutine(RespawnPowerUpCoroutine(powerUp, time));
    }

    IEnumerator RespawnPowerUpCoroutine(DoubleJumpPowerUp powerUp, float time)
    {
        yield return new WaitForSeconds(time);
        powerUp.Respawn();
    }

    // 🛡️ SHIELD / INVINCIBILITATE + OPACITATE
    public void ActivateShield(float duration)
    {
        StartCoroutine(ShieldCoroutine(duration));
    }

    IEnumerator ShieldCoroutine(float duration)
    {
        isInvincible = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        // opacitate mai mica
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = shieldOpacity;
            spriteRenderer.color = c;
        }

        yield return new WaitForSeconds(duration);

        isInvincible = false;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        // opacitate normala
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
    }
}
