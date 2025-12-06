using UnityEngine;
using TMPro;

public class ResultPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI resultText;

    public float displayTime = 1.2f;   // optional if you animate it elsewhere

    public void ShowInstant(string text, Color color)
    {
        if (canvasGroup == null || resultText == null)
        {
            Debug.LogWarning("ResultPopup: Missing references, cannot display result.");
            return;
        }

        resultText.text = text;
        resultText.color = color;

        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;

        // You can either:
        // 1) Let it disappear because the scene changes
        // 2) Or trigger an Animator here to fade it out
        //    e.g. animator.SetTrigger("Show");
    }
}