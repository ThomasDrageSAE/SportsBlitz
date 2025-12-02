using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CRTFlicker : MonoBehaviour
{
    public float flickerSpeed = 4f;
    public float flickerAmount = 0.05f;

    private Image img;
    private float baseAlpha;

    void Awake()
    {
        img = GetComponent<Image>();
        baseAlpha = img.color.a;
    }

    void Update()
    {
        float flicker = Mathf.Sin(Time.time * flickerSpeed) * flickerAmount;
        Color c = img.color;
        c.a = baseAlpha + flicker;
        img.color = c;
    }
}
