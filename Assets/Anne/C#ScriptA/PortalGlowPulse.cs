using UnityEngine;

public class PortalGlowPulse : MonoBehaviour
{
    [Header("Pulse")]
    public float scaleAmount = 0.05f;   // cât “pulseazã”
    public float speed = 2f;            // vitezã

    [Header("Alpha")]
    public float minAlpha = 0.75f;
    public float maxAlpha = 1f;

    private Vector3 startScale;
    private SpriteRenderer sr;

    private void Awake()
    {
        startScale = transform.localScale;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f; // 0..1

        // scale pulse
        float s = 1f + Mathf.Lerp(-scaleAmount, scaleAmount, t);
        transform.localScale = startScale * s;

        // alpha pulse
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
            sr.color = c;
        }
    }
}
