using UnityEngine;

public class CameraFollower_Custom : MonoBehaviour
{
    public Transform target;
    public float FollowSpeed = 2f;

    [Header("Offsets")]
    public float xOffset = 2f;
    public float yOffset = 1.5f;

    [Header("Limita Hartii (Borders)")]
    public bool useLimits = true;
    public float minX = 0f;    //stânga
    public float maxX = 100f;  //dreapta
    public float minY = -2f;   //jos
    public float maxY = 15f;   //sus
    void LateUpdate()
    {
        if (target == null) return;
        float desiredX = target.position.x + xOffset;
        float desiredY = target.position.y + yOffset;
        if (useLimits)
        {
            desiredX = Mathf.Clamp(desiredX, minX, maxX);
            desiredY = Mathf.Clamp(desiredY, minY, maxY);
        }

        Vector3 newPos = new Vector3(desiredX, desiredY, -10);
        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}