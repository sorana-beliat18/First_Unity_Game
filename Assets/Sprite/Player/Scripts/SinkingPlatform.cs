using UnityEngine;

public class SinkingPlatform : MonoBehaviour
{
    public float sinkSpeed = 1f;      
    public float maxSinkDistance = 1f; 
    public float returnSpeed = 1f;    

    private Vector3 startPos;
    private bool playerOnPlatform;
    private float sunkAmount;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (playerOnPlatform && sunkAmount < maxSinkDistance)
        {
            float sinkStep = sinkSpeed * Time.deltaTime;
            transform.position += Vector3.down * sinkStep;
            sunkAmount += sinkStep;
        }
        else if (!playerOnPlatform && returnSpeed > 0 && sunkAmount > 0)
        {
            float returnStep = returnSpeed * Time.deltaTime;
            transform.position += Vector3.up * returnStep;
            sunkAmount -= returnStep;

            if (sunkAmount < 0)
                sunkAmount = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = true;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
            collision.transform.SetParent(null);
        }
    }
}
