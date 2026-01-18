using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform target;
    public float yOffset = 1.5f;
        void Start()
    {
        
    }

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y+yOffset, -10);//coordonatele caracterului
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.fixedDeltaTime);
    }
}
