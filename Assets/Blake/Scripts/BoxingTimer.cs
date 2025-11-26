using UnityEngine;
using SportsBlitz.Events;
using System.Collections;
using SportsBlitz.Blake.Soccer;
using SportsBlitz.Blake.Boxing;

namespace SportsBlitz.Blake
{
    public class BoxingTimer : MonoBehaviour
    {
        private Coroutine _timerCoroutine = null;
        [SerializeField] private bool _debug = false;

        #region Events
        private void OnEnable()
        {
            if (Blake.EventManager.Instance != null) Blake.EventManager.Instance.startTimer += StartTimer;
        }

        private void OnDisable()
        {
            if (Blake.EventManager.Instance != null) Blake.EventManager.Instance.startTimer -= StartTimer;
        }
        #endregion

        private void StartTimer(float time)
        {
            if (_debug) Debug.Log($"Starting Timer for {time} seconds.");

            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
            }

            _timerCoroutine = StartCoroutine(TimerCoroutine(time));
        }

        // INFO: Timer coroutine
        private IEnumerator TimerCoroutine(float time)
        {
            int remaining = Mathf.CeilToInt(time);

            for (; remaining > 0; remaining--)
            {
                // INFO: Pause timer if game is won or lost
                if (BoxingGameManager.Instance._gameLost || BoxingGameManager.Instance._gameWon)
                {
                    yield break;
                }

                Blake.EventManager.Instance.OnUpdateTimerText?.Invoke(remaining);
                yield return new WaitForSeconds(1f);
            }

            Blake.EventManager.Instance.OnUpdateTimerText?.Invoke(0f);
            _timerCoroutine = null;
            Blake.EventManager.Instance.timeOver?.Invoke();
        }

    }
}
