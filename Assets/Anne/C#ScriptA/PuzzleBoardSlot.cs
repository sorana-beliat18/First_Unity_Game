using UnityEngine;
using UnityEngine.UI;

public class PuzzleBoardSlot : MonoBehaviour
{
    public int slotId; // 0..8
    public Image img;

    private DoorPuzzleUI doorUI;

    private void Awake()
    {
        if (img == null)
            img = GetComponent<Image>();

        doorUI = FindObjectOfType<DoorPuzzleUI>(true);
    }

    // chemat din Button OnClick
    public void OnClick()
    {
        if (doorUI == null)
        {
            Debug.LogWarning("DoorPuzzleUI nu a fost gãsit!");
            return;
        }

        doorUI.TryPlaceOnSlot(slotId, img);
    }
}
