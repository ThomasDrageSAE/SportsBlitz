using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SportzBlitz.Blake.Soccer
{
    public class SoccerInputManager : MonoBehaviour
    {

        [Header("Movement Settings")]
        [SerializeField][Tooltip("Speed at which the player moves")] private float _movementSpeed = 1f;
        [SerializeField][Tooltip("The total distance the cursor travels (In Pixels)")] private float _travelDistance = 500f;

        [Header("Target Zone Settings")]
        [SerializeField][Tooltip("Center of the target zone")] private TargetZoneCenter _targetZoneCentre = TargetZoneCenter.Center;
        [SerializeField][Tooltip("Width of the target zone (In Pixels)")] private float _targetZoneWidth = 100f;

        [SerializeField]private TextMeshProUGUI feedbackText;
        private RectTransform cursorRectTransform;
        private float startTime;
        private bool isGameActive = true;

        void Start()
        {
            cursorRectTransform = GetComponent<RectTransform>();
            startTime = Time.time;

            if (feedbackText == null) feedbackText.text = "Press SPACE to hit!";


        }


        private void Update()
        {
            if (!isGameActive) return;

            float t = (Time.time - startTime) * _movementSpeed;
            float pingPongValue = Mathf.PingPong(t, _travelDistance);
            float newX = pingPongValue - (_travelDistance / 2f);

            cursorRectTransform.localPosition = new Vector3(newX, cursorRectTransform.localPosition.y, 0);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                CheckHit();
            }
        }

        private void CheckHit()
        {
            float currentX = cursorRectTransform.localPosition.x;

            float halfWidth = _targetZoneWidth / 2f;
            float minTarget = (float)_targetZoneCentre - halfWidth;
            float maxTarget = (float)_targetZoneCentre + halfWidth;

            if (currentX < minTarget || currentX > maxTarget)
            {
                if (feedbackText != null) feedbackText.text = "Missed! Try Again!";
                return;
            }

            float distanceToCentre = Mathf.Abs(currentX - (float)_targetZoneCentre);

            float normalisedDistance = distanceToCentre / halfWidth;

            string message;

            if (normalisedDistance < 0.1f)
            {
                message = "Perfect Hit!";
            }
            else if (normalisedDistance < 0.3f)
            {
                message = "Great Hit!";
            }
            else
            {
                message = "Good Hit!";
            }
            
            if(feedbackText != null) feedbackText.text = message;
        }
    }

}

// INFO: Only used in this file
internal enum TargetZoneCenter
{
    Center = 0,
    Left = -1,
    Right = 1
}