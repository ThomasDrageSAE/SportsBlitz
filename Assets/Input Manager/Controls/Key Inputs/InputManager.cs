using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace SportzBlitz.Controls.Managers
{
    public class InputManager : MonoBehaviour
    {
 
        // public static InputManager Instance { get; private set; }
        private Keyboard keyboardInputs;
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
        #endregion

        // INFO: Store needed keys for game manager (Don't edit manually!)
        [HideInInspector] private List<char> _neededKeys;

        private void Awake()
        {
            // #region Singleton Instance
            // if (Instance != null && Instance != this)
            // {
            //     Destroy(gameObject);
            // }
            // else
            // {
            //     Instance = this;
            // }
            // #endregion

        }


        private void Update()
        {
            if (keyboardInputs != null && keyboardInputs.anyKey.wasPressedThisFrame) HandleInput();

        }

        #region Handle the inputs
        private void HandleInput()
        {
            if (_neededKeys == null || _neededKeys.Count == 0) return;

            char expected = char.ToUpper(_neededKeys[0]);

            foreach (UnityEngine.InputSystem.Controls.KeyControl key in keyboardInputs.allKeys)
            {
                if (!key.wasPressedThisFrame) continue;

                string pressed = key.keyCode.ToString().ToUpper();

                // INFO: Failed Input
                if (pressed != expected.ToString())
                {
                    if (debug) Debug.Log($"Incorrect key '{pressed}'. Resetting.");
                    EventManager.Instance.incorrectKeyInput?.Invoke(pressed); // INFO: Events
                    return;
                }

                // INFO: Correct Input
                if (debug) Debug.Log($"Correct key '{pressed}' pressed.");

                EventManager.Instance.correctKeyInput.Invoke(_neededKeys[0].ToString()); // INFO: Events
                _neededKeys.RemoveAt(0);

                // INFO: Sequence Complete
                if (_neededKeys.Count == 0)
                {
                    if (debug) Debug.Log("Sequence complete.");
                    GetNewInputs();
                }
                return;
            }
        }
        #endregion

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
            uIManager.CreateUI(randomChars.Count, randomChars);

            // INFO: Store needed keys for game manager
            _neededKeys = randomChars;

        }

        private void Start()
        {
            keyboardInputs = Keyboard.current;
            GetNewInputs();
        
        }
        #endregion

        public List<char> GetNeededKeys() => _neededKeys;
        public void ClearNeededKeys() => _neededKeys.Clear();

    }
}
