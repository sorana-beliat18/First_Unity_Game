using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIPuzzleMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (audioSource.clip != null)
            audioSource.Play();
    }

    void OnDisable()
    {
        audioSource.Stop();
    }
}

