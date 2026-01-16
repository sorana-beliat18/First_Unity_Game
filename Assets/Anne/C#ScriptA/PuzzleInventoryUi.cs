using UnityEngine;
using UnityEngine.UI;

public class PuzzleInventoryUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;   // InventoryPanel (poți pune chiar acest GameObject)
    public Image[] slots;      // 9 sloturi

    private bool isOpen;

    private void Start()
    {
        if (panel == null)
            panel = gameObject;

        panel.SetActive(false);
        isOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            panel.SetActive(isOpen);
        }
    }

    // ===============================
    // VARIANTA VECHE (pe ID fix)
    // ===============================
    public void ShowPiece(int id, Sprite sprite)
    {
        if (id < 0 || id >= slots.Length)
            return;

        slots[id].sprite = sprite;
        slots[id].preserveAspect = true;

        Color c = slots[id].color;
        c.a = 1f;
        slots[id].color = c;
    }

    // ===============================
    // VARIANTA NOUĂ – în ordinea colectării
    // ===============================
    public int AddToFirstEmptySlot(Sprite sprite)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].sprite == null)
            {
                slots[i].sprite = sprite;
                slots[i].preserveAspect = true;

                Color c = slots[i].color;
                c.a = 1f;
                slots[i].color = c;

                return i;
            }
        }

        Debug.Log("Inventar plin!");
        return -1;
    }
    public bool IsFull()
    {
        for (int i = 0; i < slots.Length; i++)
            if (slots[i].sprite == null)
                return false;

        return true;
    }

    public int FilledCount()
    {
        int c = 0;
        for (int i = 0; i < slots.Length; i++)
            if (slots[i].sprite != null) c++;
        return c;
    }

}
