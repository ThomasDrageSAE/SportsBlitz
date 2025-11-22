using UnityEngine;
using SportsBlitz.Events;
using System.Collections;

namespace SportsBlitz.Blake.Boxing
{
    public class Timer : MonoBehaviour
    {
        private Coroutine _timerCoroutine = null;
        [SerializeField] private bool _debug = false;

        #region Events
        private void OnEnable()
        {
            if (BoxingEventManager.Instance != null) BoxingEventManager.Instance.startTimer += StartTimer;
        }

        private void OnDisable()
        {
            if (BoxingEventManager.Instance != null) BoxingEventManager.Instance.startTimer -= StartTimer;
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
                BoxingEventManager.Instance.OnUpdateTimerText?.Invoke(remaining);
                yield return new WaitForSeconds(1f);
            }

            BoxingEventManager.Instance.OnUpdateTimerText?.Invoke(0f);
            _timerCoroutine = null;
            BoxingEventManager.Instance.timeOver?.Invoke("");
        }

    }
}
