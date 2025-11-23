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
        [SerializeField][Tooltip("Width of the target zone (In Pixels)")] private float _targetZoneWidth = 25;
        #endregion

        #region  Debug Settings
        [Header("Debug Settings")]
        [SerializeField] private bool _debug = false;
        #endregion
        private float startTime;


        void Start()
        {
            startTime = Time.time;

            if (_feedbackText != null && _debug) _feedbackText.text = "Press SPACE to hit!";
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

            float halfWidth = _targetZoneWidth / 2f;
            float _minTarget = (float)_targetZoneCentre - halfWidth;
            float maxTarget = (float)_targetZoneCentre + halfWidth;

            float _distanceToCentre = Mathf.Abs(_currentX - (float)_targetZoneCentre);
            float _normalisedDistance = _distanceToCentre / halfWidth;

            if (_debug) Debug.Log($"normalisedDistance: {_normalisedDistance}");
            if (_normalisedDistance <= 0.5f && _normalisedDistance > 0f)
            {
                if (_feedbackText != null && _debug) _feedbackText.text = "Perfect Hit!";
                Blake.EventManager.Instance.Wongame?.Invoke();
            }
            else
            {
                Blake.EventManager.Instance.gameEnd?.Invoke();
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