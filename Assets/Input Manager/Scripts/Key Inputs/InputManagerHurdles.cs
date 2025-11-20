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
    public class InputManagerHurdles : MonoBehaviour
    {

        // public static InputManager Instance { get; private set; }
        private Keyboard keyboardInputs;
        private bool _canAcceptInput = true;
        [SerializeField] private bool debug;

        #region Input Settings
        [SerializeField] private int _amountOfInputs = 3;
        [SerializeField] private bool _allowDuplicateInputs = false;
        [SerializeField] private bool _sortInputs = false;
        #endregion

        #region Letters to be used for inputs
        [SerializeField]
        private List<char> _inputLetters = new List<char>
        {
            'a','b','c','d','e','f','g','h','i','j','k','l','m',
            'n','o','p','q','r','s','t','u','v','w','x','y','z'
        };

        [SerializeField] private List<GameObject> _inputLettersPrefabs = new List<GameObject>();

        private Dictionary<char, GameObject> _letterPrefabDict = new Dictionary<char, GameObject>();
        #endregion

        // INFO: Store needed keys for game manager (Don't edit manually!)
        [HideInInspector] private List<char> _neededKeys;

        #region Unity Functions
        private void Awake()
        {
            _neededKeys = new List<char>();
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
            _letterPrefabDict = new Dictionary<char, GameObject>();

            int pairCount = Math.Min(_inputLetters.Count, _inputLettersPrefabs.Count);
            for (int i = 0; i < pairCount; i++)
            {
                char key = char.ToUpper(_inputLetters[i]);
                GameObject prefab = _inputLettersPrefabs[i];
                if (prefab == null) continue;

                if (!_letterPrefabDict.ContainsKey(key))
                    _letterPrefabDict.Add(key, prefab);
            }

            if (_inputLetters.Count != _inputLettersPrefabs.Count)
                Debug.LogWarning($"InputManager: _inputLetters.Count ({_inputLetters.Count}) != _inputLettersPrefabs.Count ({_inputLettersPrefabs.Count}). Excess items will be ignored.");

        }
        #endregion

        private void HandleInput()
        {
            if (!_canAcceptInput) return;

            if (keyboardInputs == null)
            {
                keyboardInputs = Keyboard.current;
                if (keyboardInputs == null) return;
            }

            foreach (UnityEngine.InputSystem.Controls.KeyControl key in keyboardInputs.allKeys)
            {
                if (key == null || !key.wasPressedThisFrame) continue;
                if (_neededKeys == null || _neededKeys.Count == 0) return;

                string pressed = key.keyCode.ToString().ToUpper();

                char expected = char.ToUpper(_neededKeys[0]);

                // INFO: Incorrect Key
                if (pressed != expected.ToString())
                {
                    if (debug) Debug.Log($"Incorrect key '{pressed}'. Resetting.");
                    EventManager.Instance.incorrectKeyInput?.Invoke(pressed);
                    StartCoroutine(DelayAndReset(2.1f));
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
                    StartCoroutine(DelayAndReset(0.5f));
                    return;
                }
            }
        }

        private IEnumerator DelayAndReset(float time)
        {
            _canAcceptInput = false; // blocks inputs
            yield return new WaitForSeconds(time);
            GetNewInputs();
            _canAcceptInput = true; // blocks inputs
        }

        #region Generate Random Inputs
        public List<char> GenerateRandomChars(int amountOfInputs, bool allowDuplicates = false, bool sortList = true)
        {
            // INFO: Validate input
            if (!allowDuplicates && amountOfInputs > _inputLetters.Count)
                return new List<char>();

            // INFO: Generate random chars
            List<char> result = new List<char>();
            List<char> temp = allowDuplicates ? _inputLetters : new List<char>(_inputLetters);

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

            List<char> randomChars = GenerateRandomChars(_amountOfInputs, _allowDuplicateInputs, _sortInputs);
            Debug.Log(string.Join(", ", randomChars));

            // Build per-letter prefab list (may contain nulls -> UIManager will fallback)
            List<GameObject> prefabsForLetters = new List<GameObject>(randomChars.Count);
            foreach (char c in randomChars)
            {
                prefabsForLetters.Add(GetPrefabForLetter(c));
            }

            uIManager.CreateUI(randomChars.Count, randomChars, prefabsForLetters);

            // INFO: Store needed keys for game manager
            _neededKeys = randomChars;

        }

        #endregion

        public List<char> GetNeededKeys() => _neededKeys;
        public void ClearNeededKeys() => _neededKeys?.Clear();

        // Helper to lookup a prefab for a given letter (returns null if none assigned)
        public GameObject GetPrefabForLetter(char letter)
        {
            if (_letterPrefabDict == null || _letterPrefabDict.Count == 0)
                return null;

            _letterPrefabDict.TryGetValue(char.ToUpper(letter), out var result);
            return result;
        }

    }
}
