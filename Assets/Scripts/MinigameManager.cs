using UnityEngine;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    public float defaultTimeLimit = 8f;
    private float remainingTime;

    private ResultPopup popup;

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
        if (remainingTime > 0)
            remainingTime -= Time.deltaTime;
    }

    public void Win()
    {
        StartCoroutine(WinRoutine());
    }

    public void Lose()
    {
        StartCoroutine(LoseRoutine());
    }

    IEnumerator WinRoutine()
    {
        if (popup != null)
            yield return popup.ShowResult("CLEAR!", Color.green);

        SportsBlitzGameManager.Instance.RegisterWin();
    }

    IEnumerator LoseRoutine()
    {
        if (popup != null)
            yield return popup.ShowResult("LOSE!", Color.red);

        SportsBlitzGameManager.Instance.RegisterLoss();
    }
}