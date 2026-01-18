using UnityEngine;
using UnityEngine.UI;

public class FlowerPickup : MonoBehaviour
{
    [Header("Setari UI")]
    public GameObject flowerIconHUD;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (flowerIconHUD != null)
            {
                flowerIconHUD.SetActive(true);
            }

            Debug.Log("Floare colectata cu succes!");

            Destroy(gameObject);
        }
    }
}