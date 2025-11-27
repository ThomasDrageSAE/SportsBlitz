using System.Collections;
using Helper.Blake;
using UnityEngine;

namespace SportsBlitz.Blake.Soccer
{
    public class SoccerGameManager : Singleton<SoccerGameManager>
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
        private Blake.UIManager _soccerUIManager => Blake.UIManager.Instance;
        private TimerUIManager _timerUIManager => TimerUIManager.Instance;
        private Blake.EventManagerBlake _soccerEventManager => Blake.EventManagerBlake.Instance;
        [SerializeField] private SoccerInputManager _inputManager;
        #endregion

        #region Audio Settings
        [Header("Audio Settings")]
        [SerializeField] private AudioClip _ballKickSound;
        [Tooltip("Play ball kick sound when the player kicks the ball")][SerializeField] private bool _playBallKickSound = true;
        #endregion

        #region Player Settings
        [Header("Player Settings")]
        [Tooltip("Animator controller for the player character")]
        [SerializeField] private Animator _playerAnimationController;
        #endregion

        #region Ball Settings
        [Header("Ball Settings")]
        [SerializeField] private GameObject _ballObject;
        [SerializeField] private Animator _ballAnimationController;

        [Tooltip("Speed of the ball")]
        [SerializeField] private float _ballSpeed = 5f;
        private Rigidbody2D ballRB;
        #endregion

        #region Debug Settings
        [Header("Debug Settings")]
        [SerializeField] private bool _debug = false;
        #endregion

        #region Events
        private void OnEnable()
        {
            if (_soccerEventManager != null) _soccerEventManager.timeOver += GameLose;
            if (_soccerEventManager != null) _soccerEventManager.roundStart += RoundLogic;

            // INFO: Win/Lose
            if (_soccerEventManager != null) _soccerEventManager.gameLose += GameWin;
            if (_soccerEventManager != null) _soccerEventManager.gameWon += GameLose;
        }
        private void OnDisable()
        {
            if (_soccerEventManager != null) _soccerEventManager.timeOver -= GameLose;
            if (_soccerEventManager != null) _soccerEventManager.roundStart -= RoundLogic;

            // INFO: Win/Lose
            if (_soccerEventManager != null) _soccerEventManager.gameWon -= GameWin;
            if (_soccerEventManager != null) _soccerEventManager.gameLose -= GameLose;
        }
        #endregion

        private void Start()
        {
            _soccerEventManager.startGame?.Invoke();
            StartCoroutine(InstructionsCoroutine()); // INFO: Give the player time to read the instructions
            ballRB = _ballObject?.GetComponent<Rigidbody2D>();
            if (_playerAnimationController == null) Debug.LogWarning($"Player animation controller isn't assigned.");
        }

        #region Round Logic
        private void RoundLogic()
        {
            if (_gameLost && _gameStarted && _roundStarted) return;
            _roundStarted = true;
            if (_debug) Debug.Log($"Round started with duration: {_roundTime} seconds");
            _soccerEventManager.startTimer?.Invoke(_roundTime);
            if (_inputManager != null) _inputManager.gameObject.SetActive(true);
        }

        #endregion

        #region Instructions Delay
        private IEnumerator InstructionsCoroutine()
        {
            // INFO: Delay for instructions
            _soccerEventManager.startTimer?.Invoke(_instructionDelay);
            yield return new WaitForSeconds(_instructionDelay);

            // INFO: Start the game
            _gameStarted = true;
            _soccerEventManager.roundStart?.Invoke();

            // INFO: Play ball kick sound
            yield return new WaitForSeconds(0.1f); // INFO: Small delay to ensure the bell sound plays properly

        }
        #endregion

        private void FixedUpdate()
        {
            if (_gameLost && _ballObject != null) ballRB.MovePosition(ballRB.position + new Vector2(-1, 1).normalized * _ballSpeed * Time.fixedDeltaTime);
            if (_gameWon && _ballObject != null) ballRB.MovePosition(ballRB.position + Vector2.up * _ballSpeed * Time.fixedDeltaTime);

        }

        #region Win/Lose Logic
        // INFO: Game Win Function
        private void GameWin()
        {
            if (_gameLost) return;
            _gameLost = false;
            _gameWon = true;

            if (_playerAnimationController != null) _playerAnimationController.SetTrigger("Win");
            if (_ballAnimationController != null) _ballAnimationController.SetTrigger("Win");
            if (_ballKickSound != null && _playBallKickSound) AudioSource.PlayClipAtPoint(_ballKickSound, Vector3.zero);
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
            if (_ballAnimationController != null) _ballAnimationController.SetTrigger("Lose");
            if (_ballKickSound != null && _playBallKickSound) AudioSource.PlayClipAtPoint(_ballKickSound, Vector3.zero);
            StartCoroutine(EndDelay(0.5f));

            if (_debug) Debug.Log("Game Lost!");

            // INFO: The rest is handled by the Microgame Manager
        }
        #endregion

        // INFO: Allow for a delay at the end for animations, etc.
        private IEnumerator EndDelay(float time, bool isWin = false)
        {
            // INFO: Disable the input manager to prevent further inputs
            if (_inputManager == null) yield break;
            _inputManager.gameObject.GetComponent<SoccerInputManager>().enabled = false;
            _soccerEventManager.stopTimer?.Invoke();
            _soccerEventManager.gameEnd?.Invoke();

        }
    }

}