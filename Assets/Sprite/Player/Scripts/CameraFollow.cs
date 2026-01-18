using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform target; 
    
    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.fixedDeltaTime);
    }
    
}
