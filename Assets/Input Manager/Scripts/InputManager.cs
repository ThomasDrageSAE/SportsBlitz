using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using SportsBlitz.Events;
using Steamworks;
using UnityEngine.InputSystem.Controls;
#if NaughtyAttributes
using NaughtyAttributes;
#endif

namespace SportsBlitz.Controls.Managers
{
    [RequireComponent(typeof(UIManager))]
    public class InputManager : MonoBehaviour
    {
        private Keyboard keyboardInputs;
        private bool _canAcceptInput = true;

        #region Debug Settings
        [SerializeField] private bool debug;
        #endregion

        #region Input Settings
        [Header("Input Generation Settings")]
        [SerializeField] private int _amountOfInputs = 3;
        [SerializeField] private bool _allowDuplicateInputs = false;
        [SerializeField] private bool _sortInputs = false;

        [Header("Key Settings")]
        [SerializeField] private bool _removeKeyAfterCorrectPress = false;
        [SerializeField] private bool _removeKeyAfterIncorrectPress = false;
        [SerializeField] private bool _keepSameKey = false;

        [Header("Delay Settings")]
        [SerializeField] private float delayAndResetSuccess = 0.25f;
        [SerializeField] private float delayAndResetFail = 0.25f;
        #endregion

        #region Unity Input System (Testing)
        [Header("Input Manager Settings")]
        public InputActionAsset InputActions;
        private InputAction m_BoxingAction;

        #endregion

        #region Letters to be used for inputs
        [Header("Letters and Prefabs Settings")]
        [SerializeField] private List<GameObject> _inputLettersPrefabs = new List<GameObject>();
        #endregion        
        // INFO: Store needed keys for game manager (Don't edit manually!)
        [HideInInspector] private List<string> _neededKeys;

        #region Unity Functions
        private void OnEnable()
        {
            InputActions.FindActionMap("Player").Enable();
            m_BoxingAction.performed += HandleNewInput;

        }
        private void OnDisable()
        {
            m_BoxingAction.performed -= HandleNewInput;
            InputActions.FindActionMap("Player").Disable();

        }

        private void Awake()
        {
            _neededKeys = new List<string>();
            m_BoxingAction = InputActions.FindAction("Box/Hurdles/TugOfWar/Fencing");

        }

        private void Start()
        {
            keyboardInputs = Keyboard.current;
            GetNewInputs();

        }
        #endregion

        // INFO: Remove after the key is pressed
        private void RemoveKeyAfterPress(string key)
        {
            for (int i = 0; i < _inputLettersPrefabs.Count; i++)
            {
                if (_inputLettersPrefabs[i] != null && _inputLettersPrefabs[i].GetComponent<KeyInputUI>() != null &&
                    _inputLettersPrefabs[i].GetComponent<KeyInputUI>().keyToDisplay.ToUpper() == key.ToUpper())
                {
                    _inputLettersPrefabs.RemoveAt(i);
                    break;
                }
            }
        }

        private void HandleNewInput(InputAction.CallbackContext ctx)
        {
            if (!_canAcceptInput) return;

            InputControl control = ctx.control;

            // INFO: Keyboard
            if (control is ButtonControl && control.device is Keyboard)
            {
                // Extract key name from "<Keyboard>/space"
                string path = control.path;
                string keyName = path.Substring(path.LastIndexOf('/') + 1);

                if (debug)
                    Debug.Log($"Keyboard key pressed: {keyName.ToUpper()}");

                HandleInput((KeyCode)Enum.Parse(typeof(KeyCode), keyName.ToUpper()));
                return;
            }


            // INFO: Gamepad (ignore sticks/triggers)
            if (control is ButtonControl button && control.device is Gamepad)
            {
                string buttonName = button.name;

                if (debug)
                    Debug.Log($"Gamepad button pressed: {buttonName}");

                // HandleInput(buttonName);
            }
        }


        private void HandleInput(KeyCode key)
        {
            if (_neededKeys == null || _neededKeys.Count == 0 || !_inputLettersPrefabs.Any(prefab => prefab.GetComponent<KeyInputUI>() != null && prefab.GetComponent<KeyInputUI>().keyToDisplay.ToUpper() == key.ToString().ToUpper())) return;

            string pressed = key.ToString().ToUpper();
            string expected = _neededKeys[0].ToUpper();

            // INFO: Incorrect Key
            if (pressed != expected)
            {
                if (debug) Debug.Log($"Incorrect key '{pressed}'. Resetting.");
                EventManager.Instance.incorrectKeyInput?.Invoke();
                if (_removeKeyAfterIncorrectPress) RemoveKeyAfterPress(expected.ToString());

                StartCoroutine(DelayAndReset(delayAndResetFail));
                return;
            }

            // INFO: Correct Key
            if (debug) Debug.Log($"Correct key '{pressed}' pressed.");
            EventManager.Instance?.correctKeyInput?.Invoke(pressed);
            _neededKeys.RemoveAt(0);

            // INFO: Check if all needed keys have been pressed
            if (_neededKeys.Count == 0)
            {
                if (debug) Debug.Log("Sequence complete.");
                EventManager.Instance?.correctKeySequence?.Invoke();
                if (_removeKeyAfterCorrectPress) RemoveKeyAfterPress(expected.ToString());
                StartCoroutine(DelayAndReset(delayAndResetSuccess));
                return;
            }

        }

        private IEnumerator DelayAndReset(float time)
        {
            _canAcceptInput = false; // blocks inputs
            yield return new WaitForSeconds(time);

            GetNewInputs();
            _canAcceptInput = true; //unblocks input
        }

        #region Generate Random Inputs
        public List<string> GenerateRandomChars(int amountOfInputs, bool allowDuplicates = false, bool sortList = true)
        {
            // INFO: Validate input
            if (!allowDuplicates && amountOfInputs > _inputLettersPrefabs.Count())
                return new List<string>();

            // INFO: Generate random chars
            List<string> result = new List<string>();
            List<GameObject> temp = new List<GameObject>(_inputLettersPrefabs);

            for (int i = 0; i < amountOfInputs; i++)
            {
                int index = UnityEngine.Random.Range(0, temp.Count);
                result.Add(temp[index].GetComponent<KeyInputUI>().keyToDisplay);

                // INFO: Remove selected letter to avoid duplicates
                if (!allowDuplicates)
                    temp.RemoveAt(index);
            }

            // INFO: Sort the list
            if (sortList)
                result.Sort();

            return result;
        }
        #endregion

        #region Get New Inputs
        string _previousKey = "";
        [ContextMenu("Generate New Inputs")]
        private void GetNewInputs()
        {
            UIManager uIManager = GetComponent<UIManager>();
            uIManager.ClearUI();

            List<string> randomChars = GenerateRandomChars(_amountOfInputs, _allowDuplicateInputs, _sortInputs);

            // Build per-letter prefab list (may contain nulls -> UIManager will fallback)
            List<GameObject> prefabsForLetters = new List<GameObject>(randomChars.Count);
            foreach (string c in randomChars)
            {
                prefabsForLetters.Add(GetPrefabForLetter(c));
            }

            if (_keepSameKey && !string.IsNullOrEmpty(_previousKey))
            {
                uIManager.CreateUI(1, new List<string> { _previousKey }, new List<GameObject> { GetPrefabForLetter(_previousKey) });
                _neededKeys = new List<string> { _previousKey };
                if (debug) Debug.Log($"Sequence: {string.Join(", ", _previousKey)}");
                return;
            }

            uIManager.CreateUI(randomChars.Count, randomChars, prefabsForLetters);
            if (debug) Debug.Log($"Sequence: {string.Join(", ", randomChars)}");
            if (string.IsNullOrEmpty(_previousKey)) _previousKey = randomChars[0];

            // INFO: Store needed keys for game manager
            _neededKeys = randomChars;

        }

        #endregion

        public List<string> GetNeededKeys() => _neededKeys;
        public void ClearNeededKeys() => _neededKeys?.Clear();

        // Helper to lookup a prefab for a given letter (returns null if none assigned)
        public GameObject GetPrefabForLetter(string letter)
        {
            for (int i = 0; i < _inputLettersPrefabs.Count; i++)
            {
                if (_inputLettersPrefabs[i] != null && _inputLettersPrefabs[i].GetComponent<KeyInputUI>() != null &&
                    _inputLettersPrefabs[i].GetComponent<KeyInputUI>().keyToDisplay.ToUpper() == letter.ToUpper())
                {
                    return _inputLettersPrefabs[i];
                }

            }

            if (debug) Debug.LogWarning($"No prefab found for letter '{letter}'");
            return null;

        }

    }
}
