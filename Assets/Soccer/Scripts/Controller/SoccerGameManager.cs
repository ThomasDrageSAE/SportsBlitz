using SportzBlitz.Animation;
using UnityEngine;

namespace SportzBlitz.Blake.Soccer
{
    public class SoccerGameManager : MonoBehaviour
    {
        private AnimationHandler animationHandler;
        private GameObject player;

         #region Events
        private void OnEnable()
        {
            // EventManager.Instance.correctKeySequence += HandleCorrectKeySequence;
            // EventManager.Instance.incorrectKeyInput += GameOver;
        }

        private void OnDisable()
        {
            // EventManager.Instance.correctKeySequence -= HandleCorrectKeySequence;
            // EventManager.Instance.incorrectKeyInput -= GameOver;
        }
        #endregion


        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            animationHandler = GetComponent<AnimationHandler>();

            if (animationHandler == null) { Debug.LogError($"AnimationHandler component not found on {gameObject.name}"); return; }
            if (player == null) { Debug.LogError($"Player animator reference is not set!"); return; }

            animationHandler.animator = player?.GetComponent<Animator>();

        }


        // INFO: Handle Correct Key Sequence
        private void HandleCorrectKeySequence()
        {
            Debug.Log("Correct key sequence detected! Goal scored!");
            animationHandler?.ActivateTrigger("KickBall");
            animationHandler?.ActivateTrigger("GameWin"); // INFO: Possibly add this?

        }

        // INFO: Will handle the game over state when manager is created
        private void GameOver(string key)
        {
            Debug.Log("Incorrect key input! Game Over!");
            animationHandler?.ActivateTrigger("GameOver");
        }

    }
}