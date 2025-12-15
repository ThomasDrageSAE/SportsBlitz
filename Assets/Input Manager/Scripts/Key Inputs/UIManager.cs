using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Helper.Blake;
using SportsBlitz.Events;

namespace SportsBlitz.Controls.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gridManager;
        [SerializeField] private GameObject _uiPrefab;
        [SerializeField] private List<IUIElement> _uiElements = new List<IUIElement>();

        #region Events
        void OnEnable()
        {
            if (EventManager.Instance == null) return;
            EventManager.Instance.incorrectKeyInput += OnIncorrectKeyInput;
            EventManager.Instance.correctKeyInput += OnCorrectKeyInput;
        }

        void OnDisable()
        {
            if (EventManager.Instance == null) return;
            EventManager.Instance.incorrectKeyInput -= OnIncorrectKeyInput;
            EventManager.Instance.correctKeyInput -= OnCorrectKeyInput;
        }
        #endregion

        #region Event Handlers
        // INFO: Event Handlers
        private void OnIncorrectKeyInput()
        {
            foreach (IUIElement element in _uiElements)
                element.Pressed(false);

        }

        private void OnCorrectKeyInput(string correctKey)
        {
            foreach (IUIElement element in _uiElements)
            {
                if (element.GetGameObject().name.ToString() == $"{correctKey.ToUpper()}_Key" && !element.isPressed)
                {
                    element.Pressed(true);
                    break;

                }
            }
        }
        #endregion


        // INFO: Create UI
        public void CreateUI(int amountOfElements, List<string> lettersToDisplay, List<GameObject> prefabs)
        {
            if (_gridManager == null) { Debug.LogError("GridManager is not assigned in the UIManager."); return; }
            if (_uiPrefab == null) { Debug.LogError("Default UIPrefab is not assigned in the UIManager."); return; }

            for (int i = 0; i < amountOfElements; i++)
            {
                GameObject prefabToUse = _uiPrefab;
                if (prefabs != null && i < prefabs.Count && prefabs[i] != null)
                    prefabToUse = prefabs[i];

                GameObject uiElement = Instantiate(prefabToUse, _gridManager.transform);
                uiElement.name = $"{lettersToDisplay[i].ToString().ToUpper()}_Key";
                IUIElement uiComp = uiElement.GetComponent<IUIElement>();
                uiComp?.CreateElement();
                uiComp?.SetText(lettersToDisplay[i].ToString());
                _uiElements?.Add(uiComp);
            }
        }

        [ContextMenu("Clear UI Elements")]
        public void ClearUI()
        {
            foreach (Transform child in _gridManager.transform)
                Destroy(child.gameObject);

            for (int i = 0; i < _uiElements.Count; i++)
                Destroy(_uiElements[i].GetGameObject());

            _uiElements.Clear();
        }

        // INFO: Get UI Elements
        public List<IUIElement> GetUIElements() => _uiElements;

    }
}