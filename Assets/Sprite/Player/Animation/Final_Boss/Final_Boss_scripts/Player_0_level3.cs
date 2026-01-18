using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth_level3 : PlayerHealth
{
    [Header("Boss Level Sounds")]
    public AudioSource jumpSound;
    public AudioSource attackSound;
    public AudioSource hurtSound;
    public AudioSource deathSound; 

    private bool isDead = false; 

    private void Update()
    {
        if (isDead) return;

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpSound != null) jumpSound.Play();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (attackSound != null) attackSound.Play();
        }
    }

    public new void TakeDamage(int damage)
    {
        if (isInvincible || isDead) return;

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        if (healthBar != null)
            healthBar.SetHealth(currentLives);

        if (currentLives > 0 && hurtSound != null)
            hurtSound.Play();

        if (currentLives <= 0)
        {
            StartCoroutine(DieWithDelay()); 
        }
    }

    IEnumerator DieWithDelay()
    {
        isDead = true;
        Debug.Log("Playerul a murit. Resetăm tot nivelul în 2 secunde...");

        if (deathSound != null) deathSound.Play();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<MonoBehaviour>().enabled = false; 

        yield return new WaitForSeconds(2.0f); 

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}