using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Beam : MonoBehaviour
{
    [Tooltip("Seconds the beam lives/fades out")]
    public float duration = 0.2f;

    private LineRenderer lr;
    private Color baseColor;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        // grab the initial material color (including alpha)
        baseColor = lr.material.color;
    }

    void Start()
    {
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // fade alpha 1 -> 0
            float a = Mathf.Lerp(1f, 0f, t);
            var c = new Color(baseColor.r, baseColor.g, baseColor.b, a);
            lr.startColor = c;
            lr.endColor = c;
            yield return null;
        }
        Destroy(gameObject);
    }
}