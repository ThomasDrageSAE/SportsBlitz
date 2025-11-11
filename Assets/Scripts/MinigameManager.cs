using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [Header("Global Minigame Settings")]
    public float defaultTimeLimit = 8f;

    private float timer;
    private bool timerActive = false;

    void Start()
    {
        timer = defaultTimeLimit;
        timerActive = true;
    }

    void Update()
    {
        if (!timerActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timerActive = false;
            TimeUp();
        }
    }

    public float GetRemainingTime()
    {
        return Mathf.Max(0, timer);
    }

    public void ResetTimer()
    {
        timer = defaultTimeLimit;
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    public void Win()
    {
        StopTimer();
        SportsBlitzGameManager.Instance.RegisterWin();
    }

    public void Lose()
    {
        StopTimer();
        SportsBlitzGameManager.Instance.RegisterLoss();
    }

    private void TimeUp()
    {
        Debug.Log("Time's up!");
        Lose();
    }
}