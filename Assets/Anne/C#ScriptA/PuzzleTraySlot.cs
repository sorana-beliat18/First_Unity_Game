using UnityEngine;
using UnityEngine.UI;

public class PuzzleTraySlot : MonoBehaviour
{
    public int trayIndex;   // 0..8
    public Image img;       // Icon Image (child)

    private DoorPuzzleUI doorUI;

    private void Awake()
    {
        
        doorUI = FindObjectOfType<DoorPuzzleUI>(true);

        
        if (img == null)
        {
            Transform iconTf = transform.Find("Icon");
            if (iconTf != null)
                img = iconTf.GetComponent<Image>();
        }

       
        if (img == null)
            img = GetComponentInChildren<Image>(true);

        if (img == null)
            Debug.LogError($"[{name}] Nu am găsit Image-ul Icon! Verifică să existe child 'Icon' cu componenta Image.");
    }

    public void OnClick()
    {
        if (doorUI == null)
        {
            Debug.LogWarning("DoorPuzzleUI nu a fost găsit!");
            return;
        }

        if (img == null)
        {
            Debug.LogWarning("Nu am referință la Icon Image!");
            return;
        }

        if (img.sprite == null)
        {
            Debug.Log($"Slot gol! ({name}) Icon sprite este NULL.");
            return;
        }

        Debug.Log($"Click tray {trayIndex}, sprite={img.sprite.name}");
        doorUI.SelectPieceFromTray(trayIndex, img.sprite);
    }
}

