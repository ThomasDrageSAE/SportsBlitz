using UnityEngine;
using SportsBlitz.Events;
using System.Collections;

namespace SportsBlitz.Blake
{
    public class Timer : MonoBehaviour
    {
        private Coroutine _timerCoroutine = null;
        [SerializeField] private bool _debug = false;

        #region Events
        private void OnEnable()
        {
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.startTimer += StartTimer;
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.stopTimer += StopTimer;
        }

        private void OnDisable()
        {
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.startTimer -= StartTimer;
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
    
        // INFO: Stop the timer
        private void StopTimer() => StopAllCoroutines();

        // INFO: Timer coroutine
        private IEnumerator TimerCoroutine(float time)
        {
            int remaining = Mathf.CeilToInt(time);

            for (; remaining > 0; remaining--)
            {
                Blake.EventManagerBlake.Instance.OnUpdateTimerText?.Invoke(remaining);
                yield return new WaitForSeconds(1f);
            }

            Blake.EventManagerBlake.Instance.OnUpdateTimerText?.Invoke(0f);
            _timerCoroutine = null;
            Blake.EventManagerBlake.Instance.timeOver?.Invoke();
        }

    }
}
