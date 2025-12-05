using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public float FollowSpeed = 2f;//viteza cu care camera se misca
    public Transform target; //pentru a identifica pozitia caracterului
    public float yOffset = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y+yOffset, -10);//coordonatele caracterului
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.fixedDeltaTime);
    }
}
