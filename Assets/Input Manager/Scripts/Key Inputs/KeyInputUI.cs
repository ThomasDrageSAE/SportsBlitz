using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyInputUI : MonoBehaviour, IUIElement
{
    [SerializeField] private float _releaseKeyTime = 0.05f;
    [field: SerializeField] public string keyToDisplay { get; private set; }
    public bool isPressed { get; private set; } = false; // INFO: Logic to handle the pressed state
    private TextMeshProUGUI _textComponent;
    private Image _keyImage;

    #region Update Key Display in Editor
    private void OnValidate()
    {
        keyToDisplay = keyToDisplay?.ToUpper();
    }
    #endregion

    #region Handle Creation/Deletion
    // INFO: Things to be done when the element is created
    public void CreateElement()
    {
        _textComponent = GetComponentInChildren<TextMeshProUGUI>();
        _keyImage = GetComponentInChildren<Image>();
        if (_textComponent != null) _textComponent.text = keyToDisplay?.ToUpper();

    }

    // INFO: Handle any cleanup if needed
    public void OnDestroy()
    {

    }
    #endregion

    #region Pressed State
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
    #endregion

    #region Helper Functions
    public GameObject GetGameObject() => gameObject;
    public void SetText(string text)
    {
        if (_textComponent == null) return;
        _textComponent.text = text;
    }
    #endregion

}