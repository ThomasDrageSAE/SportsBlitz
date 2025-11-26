using System.Collections;
using Helper.Blake;
using SportsBlitz.Controls.Managers;
using SportsBlitz.Events;
using SportsBlitz.Blake;
using UnityEngine;

namespace SportsBlitz.Blake.Boxing
{
    public class BoxingGameManager : Singleton<BoxingGameManager>
    {

        #region Round Settings
        [Header("Round Settings")]
        [Tooltip("Duration of the round in seconds")][SerializeField] private float _roundTime = 10f;
        [Tooltip("Delay to show instructions before the round starts in seconds")][SerializeField] private float _instructionDelay = 5f;
        #endregion

        #region Game State
        // INFO:: Game state
        public bool _gameStarted { get; private set; } = false;
        public bool _roundStarted { get; private set; } = false;
        public bool _gameLost { get; private set; } = false;
        public bool _gameWon { get; private set; } = false;
        #endregion

        #region  Managers
        private Blake.UIManager _boxingUIManager => Blake.UIManager.Instance;
        private TimerUIManager _timerUIManager => TimerUIManager.Instance;
        private Blake.EventManager _boxingEventManager => Blake.EventManager.Instance;
        [SerializeField] private InputManager _inputManager;
        #endregion

        #region Audio Settings
        [Header("Audio Settings")]
        [SerializeField] private AudioClip _punchSound;
        #endregion

        #region Player Settings
        [Header("Player Settings")]
        [Tooltip("Animator controller for the player character")]
        [SerializeField] private Animator _playerAnimationController;
        #endregion

        #region Enemy Settings
        [Header("Enemy Settings")]
        [Tooltip("Animator controller for the enemy character")]
        [SerializeField] private Animator _enemyAnimationController;
        #endregion

        #region Debug Settings
        [Header("Debug Settings")]
        [SerializeField] private bool _debug = false;
        #endregion

        #region Events
        private void OnEnable()
        {
            if (EventManager.Instance != null) EventManager.Instance.timeOver += GameLose;
            if (EventManager.Instance != null) EventManager.Instance.roundStart += RoundLogic;

            // INFO: Win/Lose
            if (Events.EventManager.Instance != null) Events.EventManager.Instance.correctKeySequence += GameWin;
            if (Events.EventManager.Instance != null) Events.EventManager.Instance.incorrectKeyInput += GameLose;
        }
        private void OnDisable()
        {
            if (EventManager.Instance != null) EventManager.Instance.timeOver -= GameLose;
            if (EventManager.Instance != null) EventManager.Instance.roundStart -= RoundLogic;

            // INFO: Win/Lose
            if (Events.EventManager.Instance != null) Events.EventManager.Instance.correctKeySequence -= GameWin;
            if (Events.EventManager.Instance != null) Events.EventManager.Instance.incorrectKeyInput -= GameLose;
        }
        #endregion

        private void Start()
        {
            _boxingEventManager.startGame?.Invoke();
            StartCoroutine(InstructionsCoroutine()); // INFO: Give the player time to read the instructions


        }

        #region Round Logic
        private void RoundLogic()
        {
            if (_gameLost && _gameStarted && _roundStarted) return;
            _roundStarted = true;
            if (_debug) Debug.Log($"Round started with duration: {_roundTime} seconds");
            _boxingEventManager.startTimer?.Invoke(_roundTime);
            if (_inputManager != null) _inputManager.gameObject.SetActive(true);
        }

        #endregion

        #region Instructions Delay
        private IEnumerator InstructionsCoroutine()
        {
            // INFO: Delay for instructions
            _boxingEventManager.startTimer?.Invoke(_instructionDelay);
            yield return new WaitForSeconds(_instructionDelay);

            // INFO: Start the game
            _gameStarted = true;
            _boxingEventManager.roundStart?.Invoke();

            // INFO: Play bell sound
            yield return new WaitForSeconds(0.1f); // INFO: Small delay to ensure the bell sound plays properly

        }
        #endregion

        #region Win/Lose Logic
        // INFO: Game Win Function
        private void GameWin()
        {
            if (_gameLost) return;
            _gameLost = false;
            _gameWon = true;

            if (_playerAnimationController != null) _playerAnimationController.SetTrigger("Win");
            if (_punchSound != null) AudioSource.PlayClipAtPoint(_punchSound, Vector3.zero, 1.0f);
            if (_enemyAnimationController != null) _enemyAnimationController.SetTrigger("Lose");
            StartCoroutine(EndDelay(0.5f, true));
            if (_debug) Debug.Log("Game Won!");

            // INFO: The rest is handled by the Microgame Manager
        }

        // INFO: Game Lose Function
        private void GameLose()
        {
            if (_gameWon) return;
            _gameLost = true;
            _gameWon = false;

            if (_playerAnimationController != null) _playerAnimationController.SetTrigger("Lose");
            if (_enemyAnimationController != null) _enemyAnimationController.SetTrigger("Win");
            if (_punchSound != null) AudioSource.PlayClipAtPoint(_punchSound, Vector3.zero, 1.0f);

            StartCoroutine(EndDelay(0.5f));
            if (_debug) Debug.Log("Game Lost!");

            // INFO: The rest is handled by the Microgame Manager
        }
        #endregion

        // INFO: Allow for a delay at the end for animations, etc.
        private IEnumerator EndDelay(float time, bool isWin = false)
        {
            // INFO: Disable the input manager to prevent further inputs
            if (_inputManager != null) _inputManager.gameObject.GetComponent<InputManager>().enabled = false;
            _boxingEventManager.stopTimer?.Invoke();


            // INFO: Prevent the input UI from updating
            foreach (IUIElement element in FindFirstObjectByType<Controls.Managers.UIManager>()?.GetUIElements())
            {
                Debug.Log($"Disabling KeyInputUI for element: {element.GetGameObject().name}");
                element.GetGameObject().GetComponent<KeyInputUI>().enabled = false;
            }

            _boxingEventManager.gameEnd?.Invoke();

            yield return new WaitForSeconds(time);
        }
    }

}