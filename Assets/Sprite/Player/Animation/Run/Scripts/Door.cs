using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string nextSceneName = "Level_3";

    public AudioSource audioSource;
    public AudioClip openSound;

    private Animator anim;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        anim.SetTrigger("Open");

        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpen) return;
        if (!other.CompareTag("Player")) return;

        SceneManager.LoadScene(nextSceneName);
    }
}
