using UnityEngine;
using System.Collections;

public class DoubleJumpPowerUp : MonoBehaviour
{
    public float duration = 10f;    
    public int jumps = 1;              
    public float respawnTime = 30f;    

    private Collider2D col;
    private SpriteRenderer sr;

    private Coroutine respawnRoutine;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CharacterController2D controller =
            other.GetComponent<CharacterController2D>();

        if (controller != null)
        {
            controller.EnableDoubleJumpForSeconds(duration, jumps);
        }

        Hide();

        
        if (respawnRoutine != null)
            StopCoroutine(respawnRoutine);

        respawnRoutine = StartCoroutine(RespawnAfterTime());
    }

    IEnumerator RespawnAfterTime()
    {
        yield return new WaitForSeconds(respawnTime);
        Respawn();
    }


    public void Respawn()
    {
        if (respawnRoutine != null)
        {
            StopCoroutine(respawnRoutine);
            respawnRoutine = null;
        }

        col.enabled = true;
        sr.enabled = true;
    }

    private void Hide()
    {
        col.enabled = false;
        sr.enabled = false;
    }
}
