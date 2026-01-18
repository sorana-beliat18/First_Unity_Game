using UnityEngine;

public class TaskMenuControl : MonoBehaviour
{
    public GameObject taskMenu;

    [Header("Audio")]
    public AudioSource evilLaugh;  // AudioSource-ul pentru râsul malefic
    public AudioSource bgMusic;    // AudioSource-ul pentru muzica de fundal

    void Awake()
    {
        // Îngheață jocul și arată papirusul
        Time.timeScale = 0f;
        if (taskMenu != null)
            taskMenu.SetActive(true);
    }

    void Update()
    {
        // La orice click sau tastă, pornim jocul
        if (Time.timeScale == 0f && (Input.GetMouseButtonDown(0) || Input.anyKeyDown))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        // 1. Redăm DOAR râsul malefic
        if (evilLaugh != null)
        {
            evilLaugh.Play();
        }

        // 2. Pornim muzica de fundal (dacă nu cânta deja)
        if (bgMusic != null && !bgMusic.isPlaying)
        {
            bgMusic.Play();
        }

        // 3. Pornim timpul și ascundem meniul
        Time.timeScale = 1f;
        if (taskMenu != null)
            taskMenu.SetActive(false);
    }
}
