using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSlot : MonoBehaviour, IDropHandler
{
    public int slotId; // 0..8
    public bool occupied;

    private RectTransform snapPoint;

    private void Awake()
    {
        snapPoint = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (occupied) return;

        var piece = eventData.pointerDrag?.GetComponent<DraggablePuzzlePieceUI>();
        if (piece == null || piece.IsLocked) return;

        if (piece.pieceId != slotId)
        {
            piece.ReturnToHome();
            return;
        }

        piece.SnapTo(snapPoint);
        occupied = true;

        // aici poți incrementa un manager dacă vrei “Puzzle complet”
        // PuzzleManager.Instance?.OnPiecePlacedCorrectly();
    }
}
