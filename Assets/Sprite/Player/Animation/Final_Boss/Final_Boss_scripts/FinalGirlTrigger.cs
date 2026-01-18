using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections; 

public class FinalGirlTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasPlayed = false;

    [Header("Referinte Iconite Canvas")]
    public GameObject iconitaItem1;
    public GameObject iconitaItem2;
    public GameObject iconitaItem3;

    [Header("Setari Victorie")]
    public string numeScenaWin = "Win_Screen"; 
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            if (CheckItemsCollected())
            {
                StartCoroutine(SequenceVictorie());
            }
        }
    }

    bool CheckItemsCollected()
    {
        if (iconitaItem1 == null || iconitaItem2 == null || iconitaItem3 == null) return false;
        return iconitaItem1.activeSelf && iconitaItem2.activeSelf && iconitaItem3.activeSelf;
    }
    IEnumerator SequenceVictorie()
    {
        hasPlayed = true;

        if (audioSource != null)
        {
            audioSource.Play();


            yield return new WaitForSeconds(audioSource.clip.length);
        }

        Debug.Log("Incarcam Winscreen...");
        SceneManager.LoadScene(numeScenaWin);
    }
}
