using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorPuzzleUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelVisual;   // copilul vizual (ex: PanelVisual)
    public Button closeButton;       // îl vom folosi ca NEXT

    [Header("Board slots (mijloc)")]
    public Image[] boardSlots;       // SlotID0..SlotID8 (imaginile de pe board)

    [Header("Tray icons (stânga)")]
    public Image[] trayIcons;        // Icon-urile din PiecesGrid: Slot0/Icon, Slot1/Icon, etc.

    [Header("Next Scene")]
    public string nextSceneName = "Level_2"; // <- pune Level_2

    private bool isOpen;

    // selecție piesă din tray
    private int selectedPieceIndex = -1;
    private Sprite selectedSprite = null;

    // progres puzzle
    private int correctPlacedCount = 0;
    private bool puzzleCompleted = false;

    private void Awake()
    {
        if (panelVisual != null)
            panelVisual.SetActive(false);

        // BUTONUL e ascuns la început
        if (closeButton != null)
            closeButton.gameObject.SetActive(false);

        isOpen = false;
    }

    private void Update()
    {
        // dacă nu mai avem buton de close, Escape poate închide UI
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void Open()
    {
        Debug.Log("OPEN UI");

        isOpen = true;

        if (panelVisual != null)
            panelVisual.SetActive(true);
        else
            Debug.LogWarning("panelVisual este NULL! Trage PanelVisual în Inspector.");

        // reset puzzle state la fiecare open
        ResetSelection();
        correctPlacedCount = 0;
        puzzleCompleted = false;

        // ascundem butonul până la completare
        if (closeButton != null)
            closeButton.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    public void Close()
    {
        isOpen = false;

        if (panelVisual != null)
            panelVisual.SetActive(false);

        Time.timeScale = 1f;

        ResetSelection();
    }

    private void ResetSelection()
    {
        selectedPieceIndex = -1;
        selectedSprite = null;
    }

    // chemat din PuzzleTraySlot.OnClick()
    public void SelectPieceFromTray(int trayIndex, Sprite sprite)
    {
        if (sprite == null)
        {
            Debug.Log("Sprite null, nu selectez nimic");
            return;
        }

        selectedPieceIndex = trayIndex;
        selectedSprite = sprite;

        Debug.Log($"Selectat piesa: index={trayIndex}, sprite={sprite.name}");
    }

    // chemat din PuzzleBoardSlot.OnClick()
    public void TryPlaceOnSlot(int slotId, Image slotImage)
    {
        if (selectedPieceIndex < 0 || selectedSprite == null)
        {
            Debug.Log("Nu ai selectat nicio piesă!");
            return;
        }

        if (slotImage == null) return;

        if (slotImage.sprite != null)
        {
            Debug.Log("Slot ocupat!");
            return;
        }

        // regula simplă: piesa merge doar pe slotId egal cu indexul ei
        if (selectedPieceIndex != slotId)
        {
            Debug.Log($"Greșit! Piesa {selectedPieceIndex} nu merge pe slot {slotId}");
            return;
        }

        // Plasare corectă pe board
        slotImage.sprite = selectedSprite;
        slotImage.preserveAspect = true;

        var c = slotImage.color;
        c.a = 1f;
        slotImage.color = c;

        // === NOU: șterge piesa din stânga (PiecesGrid) ===
        ClearTrayIcon(selectedPieceIndex);

        // deselect (după ce am folosit indexul)
        ResetSelection();

        Debug.Log("Piesa pusă corect pe slot " + slotId);

        // progres puzzle
        correctPlacedCount++;

        int total = (boardSlots != null && boardSlots.Length > 0) ? boardSlots.Length : 9;

        if (!puzzleCompleted && correctPlacedCount >= total)
        {
            OnPuzzleCompleted();
        }
    }

    private void ClearTrayIcon(int trayIndex)
    {
        if (trayIcons == null || trayIcons.Length == 0)
        {
            Debug.LogWarning("trayIcons nu este setat în Inspector! (DoorPuzzleUI)");
            return;
        }

        if (trayIndex < 0 || trayIndex >= trayIcons.Length)
            return;

        Image icon = trayIcons[trayIndex];
        if (icon == null) return;

        icon.sprite = null;

        // opțional: fă-l invizibil complet
        var col = icon.color;
        col.a = 0f;
        icon.color = col;

        // opțional: dezactivează raycast pe icon (nu e obligatoriu, click-ul e pe Slot)
        icon.raycastTarget = false;
    }

    private void OnPuzzleCompleted()
    {
        puzzleCompleted = true;
        Debug.Log("PUZZLE COMPLET! Arăt butonul NEXT.");

        if (closeButton == null)
        {
            Debug.LogWarning("closeButton este NULL!");
            return;
        }

        // afișăm butonul
        closeButton.gameObject.SetActive(true);

        // îl transformăm în NEXT
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(GoToNextScene);
    }

    private void GoToNextScene()
    {
        Time.timeScale = 1f;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("nextSceneName este gol! Setează Level_2 în Inspector.");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
