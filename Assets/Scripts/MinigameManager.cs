using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public float defaultTimeLimit = 8f;
    private float remainingTime;

    private ResultPopup popup;
    private bool isEnding = false;   // prevent double Win/Lose

    void Awake()
    {
        popup = FindObjectOfType<ResultPopup>(true);
    }

    public void ResetTimer()
    {
        remainingTime = defaultTimeLimit;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    void Update()
    {
        if (remainingTime > 0f)
            remainingTime -= Time.deltaTime;
    }

    public void Win()
    {
        if (isEnding) return;
        isEnding = true;

        // Show quick visual if popup exists
        if (popup != null)
            popup.ShowInstant("CLEAR!", Color.green);

        if (SportsBlitzGameManager.Instance != null)
        {
            SportsBlitzGameManager.Instance.RegisterWin();
        }
        else
        {
            Debug.LogError("No SportsBlitzGameManager");
        }
    }

    public void Lose()
    {
        if (isEnding) return;
        isEnding = true;

        if (popup != null)
            popup.ShowInstant("LOSE!", Color.red);

        if (SportsBlitzGameManager.Instance != null)
        {
            SportsBlitzGameManager.Instance.RegisterLoss();
        }
        else
        {
            Debug.LogError("No SportsBlitzGameManager");
        }
    }
}