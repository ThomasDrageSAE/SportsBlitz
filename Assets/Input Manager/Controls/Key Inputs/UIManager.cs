using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Helper.Blake;

namespace SportzBlitz.Controls.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject _gridManager;
        [SerializeField] private GameObject _uiPrefab;
        [SerializeField] private List<IUIElement> _uiElements = new List<IUIElement>();

        #region Events
        void OnEnable()
        {
            EventManager.Instance.incorrectKeyInput += OnIncorrectKeyInput;
            EventManager.Instance.correctKeyInput += OnCorrectKeyInput;
        }

        void OnDisable()
        {
            EventManager.Instance.incorrectKeyInput -= OnIncorrectKeyInput;
            EventManager.Instance.correctKeyInput -= OnCorrectKeyInput;
        }
        #endregion

        #region Event Handlers
        // INFO: Event Handlers
        private void OnIncorrectKeyInput(string incorrectKey)
        {
            // Debug.Log($"Incorrect key pressed: {incorrectKey}");
        }

        private void OnCorrectKeyInput(string correctKey)
        {
            foreach(IUIElement element in _uiElements)
            {
                if (element.GetGameObject().transform.parent.transform.parent.transform.parent.name.ToString().Contains($"_{correctKey}"))
                {
                    element.Pressed();
                    break;
                
                }
            }
        }
        #endregion

        public void CreateUI(int amountOfElements, List<char> lettersToDisplay)
        {
            if (_gridManager == null || _uiPrefab == null) { Debug.LogError("GridManager or UIPrefab is not assigned in the UIManager."); return; }

            for (int i = 0; i < amountOfElements; i++)
            {
                GameObject uiElement = Instantiate(_uiPrefab, _gridManager?.transform);
                uiElement.name = $"UIElement_{lettersToDisplay[i]}";
                uiElement?.GetComponent<IUIElement>()?.CreateElement();
                uiElement?.GetComponent<IUIElement>()?.SetText(lettersToDisplay[i].ToString());
                _uiElements?.Add(uiElement.GetComponent<IUIElement>());
            }
        }

        [ContextMenu("Clear UI Elements")]
        public void ClearUI()
        {
            foreach (Transform child in _gridManager.transform)
                Destroy(child.gameObject);

            for (int i = 0; i < _uiElements.Count; i++)
                _uiElements[i].DestroyElement();

            _uiElements.Clear();
        }
    }
}