using UnityEngine;

public class PortalOpenDoorPuzzleUI : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player a intrat in portal!");

        DoorPuzzleUI ui = FindObjectOfType<DoorPuzzleUI>(true);
        if (ui == null)
        {
            Debug.LogWarning("DoorPuzzleUI nu a fost gasit!");
            return;
        }
        gameObject.SetActive(false);

        Debug.Log("Deschid UI!");
        ui.Open();
    }
}
