using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public HeartBarScript healthBar;

    public bool isInvincible = false;
    public float shieldOpacity = 0.5f;
    public GameObject shieldVisual;

    private SpriteRenderer spriteRenderer;

    public AudioSource audioSource;
    public AudioClip deathSound;


    
    private ShieldPower[] shields;

    void Start()
    {
        currentLives = maxLives;
        healthBar.SetMaxHealth(maxLives);

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        
        shields = FindObjectsOfType<ShieldPower>(true);
    }


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
    public void KillInstant()
{
    Die();
}

    void Die()
    {
        if (audioSource != null && deathSound != null)
    {
        audioSource.PlayOneShot(deathSound);
    }
        Vector3 respawnPosition;

        if (Checkpoint.reached)
            respawnPosition = Checkpoint.respawnPoint;
        else
            respawnPosition = GameObject
                .FindGameObjectWithTag("StartPoint")
                .transform.position;

        transform.position = respawnPosition;

        
        currentLives = maxLives;
        healthBar.SetHealth(currentLives);

        
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
        RespawnShields();   
    }


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

    
    void RespawnShields()
    {
        foreach (ShieldPower s in shields)
        {
            if (s != null)
                s.gameObject.SetActive(true);
        }
    }

    
    public void ActivateShield(float duration)
    {
        StartCoroutine(ShieldCoroutine(duration));
    }

    IEnumerator ShieldCoroutine(float duration)
    {
        isInvincible = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        
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

        
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
    }
}
