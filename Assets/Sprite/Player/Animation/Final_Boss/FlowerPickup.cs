using UnityEngine;
using UnityEngine.UI;

public class FlowerPickup : MonoBehaviour
{
    [Header("Setari UI")]
    public GameObject flowerIconHUD; // Trage aici obiectul FlowerIcon din Canvas

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificam daca cel care atinge floarea are tag-ul Player
        if (other.CompareTag("Player"))
        {
            // Verificam daca am legat iconita in Inspector ca sa nu dea eroare
            if (flowerIconHUD != null)
            {
                // Activeaza iconita din colt (o face sa apara)
                flowerIconHUD.SetActive(true);
            }

            Debug.Log("Floare colectata cu succes!");

            // Distruge floarea de pe jos
            Destroy(gameObject);
        }
    }
}