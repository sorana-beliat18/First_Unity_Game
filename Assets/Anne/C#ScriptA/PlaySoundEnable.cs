using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnEnable : MonoBehaviour
{
    private AudioSource a;

   
    public float playTime = 4.0f;

   
    public float fadeDuration = 0.3f;

    private float startVolume;

    private void Awake()
    {
        a = GetComponent<AudioSource>();
        a.playOnAwake = false;
        startVolume = a.volume;
    }

    private void OnEnable()
    {
        if (a == null || a.clip == null) return;

        a.Stop();
        a.volume = startVolume;
        a.Play();

        StopAllCoroutines();
        StartCoroutine(FadeOutAfterTime());
    }

    private IEnumerator FadeOutAfterTime()
    {
        yield return new WaitForSeconds(playTime);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            a.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        a.Stop();
        a.volume = startVolume;
    }
}
