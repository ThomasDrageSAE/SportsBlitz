using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyInputUI : MonoBehaviour, IUIElement
{
    private TextMeshProUGUI _textComponent;
    [SerializeField] private float _releaseKeyTime = 0.05f;
    private Image _keyImage;
    public bool isPressed { get; private set; } = false; // INFO: Logic to hnadle the pressed state

    // INFO: Things to be done when the element is created
    public void CreateElement()
    {
        _textComponent = GetComponentInChildren<TextMeshProUGUI>();
        _keyImage = GetComponentInChildren<Image>();


    }

    // INFO: Handle any cleanup if needed
    public void OnDestroy()
    {

    }

    // INFO: Called by the UI manager when the key is pressed
    public void Pressed(bool correctInput)
    {
        isPressed = true;
        transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);

        if (_keyImage != null && correctInput)
            _keyImage.color = Color.green;
        else
            _keyImage.color = Color.red;

        // INFO: Coroutine to undo the pressed effects.
        StartCoroutine(ReleaseKeyCoroutine(_releaseKeyTime));

    }

    // INFO: Coroutine to undo the pressed effects.
    private IEnumerator<WaitForSeconds> ReleaseKeyCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.localScale = new Vector3(1, 1, 1);

    }

    #region Helper Functions
    public GameObject GetGameObject() => gameObject;
    public void SetText(string text)
    {
        if (_textComponent == null) return;
        _textComponent.text = text;
    }
    #endregion

}