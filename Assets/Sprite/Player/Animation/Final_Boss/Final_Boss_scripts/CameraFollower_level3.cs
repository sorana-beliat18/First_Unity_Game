using UnityEngine;

public class CameraFollower_Custom : MonoBehaviour
{
    public Transform target;
    public float FollowSpeed = 2f;

    [Header("Offsets")]
    public float xOffset = 2f; // Pozitiv mută camera mai în față
    public float yOffset = 1.5f;

    [Header("Limita Hartii (Borders)")]
    public bool useLimits = true;
    public float minX = 0f;    // Limita stânga
    public float maxX = 100f;  // Limita dreapta
    public float minY = -2f;   // Limita jos
    public float maxY = 15f;   // Limita sus

    void LateUpdate()
    {
        if (target == null) return;

        // Calculăm poziția dorită cu offset-uri
        float desiredX = target.position.x + xOffset;
        float desiredY = target.position.y + yOffset;

        // Aplicăm limitele (Clamping) ca să nu mai vezi "golul"
        if (useLimits)
        {
            desiredX = Mathf.Clamp(desiredX, minX, maxX);
            desiredY = Mathf.Clamp(desiredY, minY, maxY);
        }

        Vector3 newPos = new Vector3(desiredX, desiredY, -10);
        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}