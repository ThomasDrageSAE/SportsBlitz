using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class MiniGameTutorial : MonoBehaviour
{
    [Header("Timing")]
    public float displayTime = 5f;

    [Header("UI")]
    public TextMeshProUGUI tutorialText;

    
    public bool TutorialFinished { get; private set; }

    public bool TutorialEnd = false;


    void OnEnable()
    {
        Time.timeScale = 0;
        TutorialFinished = false;
        StartCoroutine(TutorialRoutine());
    }

    public IEnumerator TutorialRoutine()
    {
        // Show the tutorial panel
        gameObject.SetActive(true);
        // Wait X seconds
        yield return new WaitForSecondsRealtime(displayTime);

        // Hide and flag as done
        gameObject.SetActive(false);
        TutorialFinished = true;
        TutorialEnd = true;
        Time.timeScale = 1;
    }
}