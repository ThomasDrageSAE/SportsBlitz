using UnityEngine;

public class RacingGameManager : MonoBehaviour
{
    private MinigameManager manager;
    private MiniGameTutorial tutorial;
    private RacingTrafficLight trafficLight;

    public static bool RacingGameOver = false;
    private bool timerStarted = false;

    void Start()
    {
        RacingGameOver = false;

        manager = FindObjectOfType<MinigameManager>();
        tutorial = FindObjectOfType<MiniGameTutorial>();
        trafficLight = FindObjectOfType<RacingTrafficLight>();
    }

    void Update()
    {
        if (RacingGameOver)
            return;

        if (tutorial != null && !tutorial.TutorialFinished)
            return;

        if (trafficLight != null && !trafficLight.sequenceFinished)
            return;

        if (!timerStarted)
        {
            manager.ResetTimer();
            timerStarted = true;
        }

        if (manager.GetRemainingTime() <= 0)
        {
            RacingGameOver = true;
            manager.Win();
        }
    }
}