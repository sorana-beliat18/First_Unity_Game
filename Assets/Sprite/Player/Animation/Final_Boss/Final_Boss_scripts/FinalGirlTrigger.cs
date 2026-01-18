using UnityEngine;
using UnityEngine.SceneManagement; // Avem nevoie de asta pentru a schimba scenele
using System.Collections; // Avem nevoie de asta pentru secvența de așteptare

public class FinalGirlTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasPlayed = false;

    [Header("Referinte Iconite Canvas")]
    public GameObject iconitaItem1;
    public GameObject iconitaItem2;
    public GameObject iconitaItem3;

    [Header("Setari Victorie")]
    public string numeScenaWin = "WinScreen"; // Scrie aici numele EXACT al scenei de victorie

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

    // Aceasta este o "Corutina" - permite jocului sa astepte cateva secunde
    IEnumerator SequenceVictorie()
    {
        hasPlayed = true;

        if (audioSource != null)
        {
            audioSource.Play();

            // Asteptam pana cand sunetul se termina (ex: 2 secunde)
            // Poti pune exact durata sunetului tau "Yey"
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        // Dupa ce a trecut timpul, incarcam scena de Win
        Debug.Log("Incarcam Winscreen...");
        SceneManager.LoadScene(numeScenaWin);
    }
}
