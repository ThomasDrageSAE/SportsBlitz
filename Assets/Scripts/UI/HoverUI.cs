using UnityEngine;

public class HoverUI : MonoBehaviour
{
    public float amplitude = 10f; // height of the hover in pixels
    public float frequency = 1f;  // speed of the hover

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        rectTransform.anchoredPosition = new Vector2(startPos.x, startPos.y + yOffset);
    }
}