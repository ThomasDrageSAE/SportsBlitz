using UnityEngine;
using SportsBlitz.Events;
using System.Collections;

namespace SportsBlitz.Blake
{
    public class Timer : MonoBehaviour
    {
        private Blake.EventManagerBlake _eventManagerBlake => Blake.EventManagerBlake.Instance;
        private Coroutine _timerCoroutine;
        [SerializeField] private bool _debug = false;

        #region Events
        private void OnEnable()
        {
            if (_eventManagerBlake != null) _eventManagerBlake.startTimer += StartTimer;
            if (_eventManagerBlake != null) _eventManagerBlake.stopTimer += StopTimer;
        }

        private void OnDisable()
        {
            if (_eventManagerBlake != null) _eventManagerBlake.startTimer -= StartTimer;
            if (_eventManagerBlake != null) _eventManagerBlake.stopTimer -= StopTimer;
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
                _eventManagerBlake.OnUpdateTimerText?.Invoke(remaining);
                yield return new WaitForSeconds(1f);
            }

            _eventManagerBlake.OnUpdateTimerText?.Invoke(0f);
            _timerCoroutine = null;
            _eventManagerBlake.timeOver?.Invoke();
        }

    }
}
