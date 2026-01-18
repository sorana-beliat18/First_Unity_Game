using UnityEngine;

public class PuzzlePiecePickup : MonoBehaviour
{
    public int pieceId;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Sprite s = GetComponent<SpriteRenderer>().sprite;

        PuzzleInventoryUI ui = FindObjectOfType<PuzzleInventoryUI>(true);
        if (ui != null)
        {
            ui.AddToFirstEmptySlot(s);
        }

       
        if (PuzzleCounter.Instance != null)
        {
            PuzzleCounter.Instance.AddPiece();
        }

        Destroy(gameObject);
    }
}
