using UnityEngine;

public class PuzzleCounter : MonoBehaviour
{
    public static PuzzleCounter Instance;

    [Header("Puzzle Settings")]
    public int totalPieces = 9;

    [Header("Portal")]
    public GameObject portalObject;  
    private int collected = 0;
    private bool portalSpawned = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddPiece()
    {
        collected++;

        if (PickupMessageUI.Instance != null)
        {
            if (collected >= totalPieces)
                PickupMessageUI.Instance.Show("Gaseste portalul!");
            else
                PickupMessageUI.Instance.Show($"Ai luat piesa {collected}/{totalPieces}");
        }

       
        if (!portalSpawned && collected >= totalPieces)
        {
            portalSpawned = true;

            if (portalObject != null)
                portalObject.SetActive(true);
            else
                Debug.LogWarning("[PuzzleCounter] portalObject nu e setat în Inspector!");
        }
    }

    public int Count => collected;
}

