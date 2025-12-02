using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SportsBlitz.Blake.Soccer
{
    public class SoccerInputManager : MonoBehaviour
    {

        #region  UI Elements
        [Header("UI Elements")]
        [SerializeField] private GameObject _sliderUI;
        [SerializeField] private TextMeshProUGUI _feedbackText;
        [SerializeField] private RectTransform _cursor;
        #endregion

        #region Movement Settings
        [Header("Movement Settings")]
        [SerializeField][Tooltip("Speed at which the player moves")] private float _movementSpeed = 1f;
        [SerializeField][Tooltip("The total distance the cursor travels (In Pixels)")] private float _travelDistance = 600f;
        #endregion

        #region Target Zone Settings
        [Header("Target Zone Settings")]
        [SerializeField][Tooltip("Centre of the target zone")] private TargetZoneCentre _targetZoneCentre = TargetZoneCentre.Center;
        [SerializeField][Tooltip("Target zone GameObject")] private GameObject _targetZone;
        private float _targetZoneWidth;
        #endregion

        #region  Debug Settings
        [Header("Debug Settings")]
        [SerializeField] private bool _debug = false;
        #endregion
        private float startTime;
        private Blake.EventManagerBlake _eventManagerBlake => Blake.EventManagerBlake.Instance;

        void Start()
        {
            startTime = Time.time;

            if (_feedbackText != null && _debug) _feedbackText.text = "Press SPACE to hit!";
            if(_targetZone != null) _targetZoneWidth = _targetZone.GetComponent<RectTransform>().rect.width;
            if (_sliderUI != null) _sliderUI.SetActive(true);

        }


        private void Update()
        {

            float t = (Time.time - startTime) * _movementSpeed;
            float pingPongValue = Mathf.PingPong(t, _travelDistance);
            float newX = pingPongValue - (_travelDistance / 2f);

            if (_cursor != null) _cursor.localPosition = new Vector3(newX, _cursor.localPosition.y, 0);

            if (Input.GetKey(KeyCode.Space)) CheckHit();

        }

        private void CheckHit()
        {
            if (_cursor == null) { Debug.LogError($"Cursor RectTransform is not assigned."); return; }
            float _currentX = _cursor.localPosition.x;

            // INFO: Calculate target centre and half width in pixels, with safe fallbacks
            RectTransform targetRect = (_targetZone != null) ? _targetZone.GetComponent<RectTransform>() : null;
            float centreX = 0f;
            if (targetRect != null)
                centreX = targetRect.localPosition.x;
            else
                centreX = (float)_targetZoneCentre * (_travelDistance / 2f);

            float halfWidth = (targetRect != null) ? (targetRect.rect.width / 2f) : (_targetZoneWidth / 2f);
            if (halfWidth <= 0f) halfWidth = 1f; // INFO: prevent divide by zero

            float _distanceToCentre = Mathf.Abs(_currentX - centreX);
            
            //  INFO: Normalise distance so 0 = perfect centre, 1 = at edge of target zone (clamped)
            float _normalisedDistance = Mathf.Clamp01(_distanceToCentre / halfWidth);

            if (_debug) Debug.Log($"normalisedDistance: {_normalisedDistance}");
            if (_normalisedDistance <= 0.5f)
            {
                if (_feedbackText != null && _debug) _feedbackText.text = "Perfect Hit!";
                _eventManagerBlake.gameWon?.Invoke();
            }
            else
            {
                if (_feedbackText != null && _debug) _feedbackText.text = "Miss!";
                _eventManagerBlake.gameLose?.Invoke();
            }

        }

        public GameObject GetSliderUI() => _sliderUI.gameObject;
    }

}

// INFO: Enum for zone centre (Unneeded but makes it easier to use in the inspector)
internal enum TargetZoneCentre
{
    Center = 0,
    Left = -1,
    Right = 1
}