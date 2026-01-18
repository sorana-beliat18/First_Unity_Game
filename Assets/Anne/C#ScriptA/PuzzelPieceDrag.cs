using UnityEngine;

public class PuzzelPieceDrag : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;

    private void OnMouseDown()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        offset = transform.position - mouseWorldPos;
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    private void Update()
    {
        if (!dragging) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos + offset;
    }
}
