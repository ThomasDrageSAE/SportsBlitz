using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkSpeed = 0.5f;
    private TMP_Text tmpText;
    private UnityEngine.UI.Text uiText;
    private bool isTMP;

    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
        if (tmpText != null) isTMP = true;
        else uiText = GetComponent<UnityEngine.UI.Text>();

        StartCoroutine(Blink());
    }

    System.Collections.IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkSpeed);

            if (isTMP) tmpText.enabled = !tmpText.enabled;
            else uiText.enabled = !uiText.enabled;
        }
    }
}