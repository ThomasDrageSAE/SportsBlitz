using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;

namespace SportzBlitz.Controls.Managers
{
    public class InputManager : MonoBehaviour
    {

        // public static InputManager Instance { get; private set; }
        private Keyboard keyboardInputs;
        [SerializeField] private bool debug;

        #region Input Settings
        [Header("Input Generation Settings")]
        [SerializeField] private int _amountOfInputs = 3;
        [SerializeField] private bool _allowDuplicateInputs = false;
        [SerializeField] private bool _sortInputs = false;

        [Header("Key Removal Settings")]
        [SerializeField] private bool _removeKeyAfterCorrectPress = false;
        [SerializeField] private bool _removeKeyAfterIncorrectPress = false;
        #endregion

        #region Letters to be used for inputs
        [Header("Letters and Prefabs Settings")]
        [SerializeField]
        private List<string> _inputLetters = new List<string>()
        {
            "a","b","c","d","e","f","g","h","i","j","k","l","m",
            "n","o","p","q","r","s","t","u","v","w","x","y","z"
        };

        [SerializeField] private List<GameObject> _inputLettersPrefabs = new List<GameObject>();

        private Dictionary<string, GameObject> _letterPrefabDict = new Dictionary<string, GameObject>();
        #endregion

        // INFO: Store needed keys for game manager (Don't edit manually!)
        [HideInInspector] private List<string> _neededKeys;

        #region Unity Functions
        private void Awake()
        {
            _neededKeys = new List<string>();
            CreateDictionary();

        }

        private void Start()
        {
            keyboardInputs = Keyboard.current;
            GetNewInputs();

        }
        #endregion

        private void Update()
        {
            if (keyboardInputs != null && keyboardInputs.anyKey.wasPressedThisFrame) HandleInput();

        }

        #region Create Dictionary
        // INFO: Create the dictionary for letter to prefab lookup
        private void CreateDictionary()
        {
            // Build dictionary mapping letters to prefabs from the two serialized lists.
            _letterPrefabDict = new Dictionary<string, GameObject>();

            int pairCount = Math.Min(_inputLetters.Count(), _inputLettersPrefabs.Count);
            for (int i = 0; i < pairCount; i++)
            {
                string key = _inputLetters[i].ToUpper();
                GameObject prefab = _inputLettersPrefabs[i];
                if (prefab == null) continue;

                if (!_letterPrefabDict.ContainsKey(key))
                    _letterPrefabDict.Add(key, prefab);
            }

            if (_inputLetters.Count() != _inputLettersPrefabs.Count)
                Debug.LogWarning($"InputManager: _inputLetters.Count ({_inputLetters.Count()}) != _inputLettersPrefabs.Count ({_inputLettersPrefabs.Count}). Excess items will be ignored.");

        }
        #endregion

        // INFO: Remove after the key is pressed
        private void RemoveKeyAfterPress(string key)
        {
            if(_inputLetters.Contains(key.ToLower())) _inputLetters.Remove(key.ToLower());

        }

        private void HandleInput()
        {
            if (keyboardInputs == null)
            {
                keyboardInputs = Keyboard.current;
                if (keyboardInputs == null) return;
            }

            foreach (UnityEngine.InputSystem.Controls.KeyControl key in keyboardInputs.allKeys)
            {

                if (key == null || !key.wasPressedThisFrame) continue;
                if (_neededKeys == null || _neededKeys.Count == 0 || !_inputLetters.Contains(key.keyCode.ToString().ToLower())) return;

                string pressed = key.keyCode.ToString().ToUpper();
                string expected = _neededKeys[0].ToUpper();

                // INFO: Incorrect Key
                if (pressed != expected)
                {
                    if (debug) Debug.Log($"Incorrect key '{pressed}'. Resetting.");
                    EventManager.Instance.incorrectKeyInput?.Invoke(pressed);
                    if (_removeKeyAfterIncorrectPress) RemoveKeyAfterPress(expected.ToString());

                    StartCoroutine(DelayAndReset(0.1f));
                    continue;
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
                    StartCoroutine(DelayAndReset(0.1f));
                    break;
                }
            }
        }

        private IEnumerator DelayAndReset(float time)
        {
            yield return new WaitForSeconds(time);
            GetNewInputs();
        }

        #region Generate Random Inputs
        public List<string> GenerateRandomChars(int amountOfInputs, bool allowDuplicates = false, bool sortList = true)
        {
            // INFO: Validate input
            if (!allowDuplicates && amountOfInputs > _inputLetters.Count())
                return new List<string>();

            // INFO: Generate random chars
            List<string> result = new List<string>();
            List<string> temp = new List<string>(_inputLetters);

            for (int i = 0; i < amountOfInputs; i++)
            {
                int index = UnityEngine.Random.Range(0, temp.Count);
                result.Add(temp[index]);

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
        [ContextMenu("Generate New Inputs")]
        private void GetNewInputs()
        {

            UIManager uIManager = UIManager.Instance;
            uIManager.ClearUI();

            List<string> randomChars = GenerateRandomChars(_amountOfInputs, _allowDuplicateInputs, _sortInputs);
            if (debug) Debug.Log($"Sequence: {string.Join(", ", randomChars)}");

            // Build per-letter prefab list (may contain nulls -> UIManager will fallback)
            List<GameObject> prefabsForLetters = new List<GameObject>(randomChars.Count);
            foreach (string c in randomChars)
            {
                prefabsForLetters.Add(GetPrefabForLetter(c[0]));
            }

            uIManager.CreateUI(randomChars.Count, randomChars, prefabsForLetters);

            // INFO: Store needed keys for game manager
            _neededKeys = randomChars;

        }

        #endregion

        public List<string> GetNeededKeys() => _neededKeys;
        public void ClearNeededKeys() => _neededKeys?.Clear();

        // Helper to lookup a prefab for a given letter (returns null if none assigned)
        public GameObject GetPrefabForLetter(char letter)
        {
            if (_letterPrefabDict == null || _letterPrefabDict.Count == 0)
                return null;

            _letterPrefabDict.TryGetValue(letter.ToString().ToUpper(), out var result);
            return result;
        }

    }
}
