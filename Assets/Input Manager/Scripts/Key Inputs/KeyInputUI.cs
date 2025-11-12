using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyInputUI : MonoBehaviour, IUIElement
{
    private TextMeshProUGUI _textComponent;
    [SerializeField] private float _releaseKeyTime = 0.05f;

    // INFO: Things to be done when the element is created
    public void CreateElement()
    {
        _textComponent = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // INFO: Called when the element is destroyed by the UI manager
    public void DestroyElement()
    {
        Destroy(_textComponent.gameObject);
    }

    // INFO: Called by the UI manager when the key is pressed
    public void Pressed()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        StartCoroutine(ReleaseKeyCoroutine(_releaseKeyTime));

    }

    // INFO: Coroutine to undo the pressed effects.
    private IEnumerator<WaitForSeconds> ReleaseKeyCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.localScale = new Vector3(1, 1, 1);
    
    }

    public GameObject GetGameObject() => _textComponent.gameObject;
    public void SetText(string text) => _textComponent.text = text;
    
}