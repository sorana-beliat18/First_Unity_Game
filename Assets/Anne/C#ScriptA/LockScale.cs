using UnityEngine;

[ExecuteAlways]
public class LockScale : MonoBehaviour
{
    [SerializeField] private Vector3 lockedScale = Vector3.one;

    private void OnEnable()
    {
        Apply();
    }

    private void Update()
    {
        Apply(); // rulează și în editor, și în play
    }

    private void Apply()
    {
        if (transform.localScale != lockedScale)
            transform.localScale = lockedScale;
    }
}
