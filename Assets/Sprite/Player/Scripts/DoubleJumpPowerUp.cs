using UnityEngine;

public class DoubleJumpPowerUp : MonoBehaviour
{
    [Header("Double Jump Settings")]
    public int bonusJumps = 1;   // câte jump-uri extra oferă (1 = double jump)
    public float duration = 10f; // durata în secunde

    private void OnTriggerEnter2D(Collider2D other)
    {
        // verificăm dacă obiectul este playerul
        if (!other.CompareTag("Player"))
            return;

        // luăm controllerul playerului
        CharacterController2D controller =
            other.GetComponent<CharacterController2D>();

        // activăm double jump-ul pe durată limitată
        if (controller != null)
        {
            controller.EnableDoubleJumpForSeconds(duration, bonusJumps);
            Destroy(gameObject); // power-up-ul dispare după colectare
        }
    }
}
