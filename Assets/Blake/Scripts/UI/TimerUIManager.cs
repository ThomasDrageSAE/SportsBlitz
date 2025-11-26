using UnityEngine;
using SportsBlitz.Events;
using TMPro;
using Helper.Blake;

namespace SportsBlitz.Blake
{
    public class TimerUIManager : Singleton<TimerUIManager>
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        #region Events
        private void OnEnable()
        {
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.OnUpdateTimerText += OnUpdateTimerText;
        }
        private void OnDisable()
        {
            if (Blake.EventManagerBlake.Instance != null) Blake.EventManagerBlake.Instance.OnUpdateTimerText -= OnUpdateTimerText;
        }
        #endregion

        private void OnUpdateTimerText(float time)
        {
            SetTimer(time);
        }

        public void SetTimer(float time)
        {
            if (_timerText == null) return;

            _timerText.text = FormatTime(time);

        }

        private string FormatTime(float time)
        {
            if (float.IsNaN(time) || float.IsInfinity(time) || time < 0f) time = 0f;
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            return $"{minutes:00}:{seconds:00}";
        }


    }
}