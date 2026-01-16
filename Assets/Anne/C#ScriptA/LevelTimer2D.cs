using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTimer2D : MonoBehaviour
{
    public float timeLimit = 60f;
    public TMP_Text timerText;

    private float timeRemaining;
    private bool running = true;

    private void Start()
    {
        timeRemaining = timeLimit;
        UpdateUI();
    }

    private void Update()
    {
        if (!running) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f)
            timeRemaining = 0f;

        UpdateUI();

        if (timeRemaining <= 0f)
        {
            RestartLevel();
        }
    }

    // 👉 CHEMAT când atingi portalul
    public void StopAndHideTimer()
    {
        running = false;

        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
