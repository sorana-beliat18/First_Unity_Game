using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTimerWithInventoryUI : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimit = 60f;

    [Header("References")]
    public PuzzleInventoryUI inventoryUI;
    public TMP_Text timerText;

    private float timeRemaining;
    private bool running = true;

    private void Start()
    {
        timeRemaining = timeLimit;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (!running) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f) timeRemaining = 0f;

        UpdateTimerUI();

        if (timeRemaining <= 0f)
        {
            running = false;
            OnTimerFinished();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnTimerFinished()
    {
        if (inventoryUI == null)
        {
            Debug.LogWarning("Nu e setat inventoryUI in LevelTimerWithInventoryUI!");
            return;
        }

        if (!inventoryUI.IsFull())
        {
            RestartLevel();
        }
        else
        {
            Debug.Log("Inventar full - nivel complet!");
            // aici poți încărca next level dacă vrei
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

