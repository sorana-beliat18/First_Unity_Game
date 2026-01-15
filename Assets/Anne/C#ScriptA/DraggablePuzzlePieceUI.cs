using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class DraggablePuzzlePieceUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int pieceId;
    public bool IsLocked { get; private set; }

    private RectTransform rt;
    private CanvasGroup cg;
    private Canvas rootCanvas;

    private Vector2 homePos;
    private Transform homeParent;

    private void EnsureRefs()
    {
        if (rt == null) rt = GetComponent<RectTransform>();
        if (cg == null) cg = GetComponent<CanvasGroup>();
        if (rootCanvas == null) rootCanvas = GetComponentInParent<Canvas>();
    }

    private void Awake()
    {
        EnsureRefs();
    }

    public void SetHome(Transform parent)
    {
        EnsureRefs();

        if (parent == null)
        {
            Debug.LogError("[DraggablePuzzlePieceUI] SetHome a primit parent NULL!");
            return;
        }

        homeParent = parent;

        // punem piesa în slot
        transform.SetParent(homeParent, false);

        // poziție “centrată” în slot
        if (rt != null)
            rt.anchoredPosition = Vector2.zero;

        homePos = (rt != null) ? rt.anchoredPosition : Vector2.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        EnsureRefs();
        if (IsLocked) return;

        if (rootCanvas == null)
        {
            Debug.LogError("[DraggablePuzzlePieceUI] rootCanvas e NULL. Piesa nu e sub un Canvas?");
            return;
        }

        homeParent = transform.parent;
        homePos = rt.anchoredPosition;

        cg.blocksRaycasts = false;
        transform.SetParent(rootCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        EnsureRefs();
        if (IsLocked) return;
        if (rootCanvas == null) return;

        rt.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EnsureRefs();
        if (IsLocked) return;

        cg.blocksRaycasts = true;

        if (rootCanvas != null && transform.parent == rootCanvas.transform)
            ReturnToHome();
    }

    public void ReturnToHome()
    {
        EnsureRefs();

        if (homeParent == null)
        {
            Debug.LogWarning("[DraggablePuzzlePieceUI] ReturnToHome: homeParent NULL");
            return;
        }

        transform.SetParent(homeParent, false);
        rt.anchoredPosition = homePos;
    }

    public void SnapTo(RectTransform snapPoint)
    {
        EnsureRefs();

        if (snapPoint == null)
        {
            Debug.LogError("[DraggablePuzzlePieceUI] SnapTo: snapPoint NULL");
            return;
        }

        IsLocked = true;
        cg.blocksRaycasts = true;

        transform.SetParent(snapPoint, false);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;
        rt.localRotation = Quaternion.identity;
    }
}
