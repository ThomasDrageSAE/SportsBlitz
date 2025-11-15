using UnityEngine;
using TMPro;
using System.Collections;

public class ResultPopup : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public CanvasGroup canvasGroup;

    public IEnumerator ShowResult(string message, Color color)
    {
        gameObject.SetActive(true);

        resultText.text = message;
        resultText.color = color;

        // fade in
        canvasGroup.alpha = 0f;
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * 2f;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // fade out
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * 2f;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}