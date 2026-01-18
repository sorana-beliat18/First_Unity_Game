using UnityEngine;

public class TaskMenuControl : MonoBehaviour
{
    public GameObject taskMenu;

    [Header("Audio")]
    public AudioSource evilLaugh; 
    public AudioSource bgMusic;   

    void Awake()
    {
        Time.timeScale = 0f;
        if (taskMenu != null)
            taskMenu.SetActive(true);
    }

    void Update()
    {
        if (Time.timeScale == 0f && (Input.GetMouseButtonDown(0) || Input.anyKeyDown))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        if (evilLaugh != null)
        {
            evilLaugh.Play();
        }

        if (bgMusic != null && !bgMusic.isPlaying)
        {
            bgMusic.Play();
        }

        Time.timeScale = 1f;
        if (taskMenu != null)
            taskMenu.SetActive(false);
    }
}
