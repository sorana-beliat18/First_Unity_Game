using System.Collections.Generic;
using UnityEngine;

public class PuzzleInventory : MonoBehaviour
{
    public static PuzzleInventory Instance;

    private HashSet<int> collected = new HashSet<int>();
    private PuzzleInventoryUI ui;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
      
        ui = FindObjectOfType<PuzzleInventoryUI>(true);
    }

    public void AddPiece(int id, Sprite sprite)
    {
       
        if (collected.Add(id))
        {
            if (ui != null)
            {
                ui.ShowPiece(id, sprite);
            }
        }
    }

    public bool HasPiece(int id)
    {
        return collected.Contains(id);
    }

    public int Count => collected.Count;
}

