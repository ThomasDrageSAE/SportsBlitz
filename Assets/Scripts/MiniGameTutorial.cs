using UnityEngine;
using TMPro;
using System.Collections;

public class MiniGameTutorial : MonoBehaviour
{
    [Header("Timing")]
    public float displayTime = 5f;

    [Header("UI")]
    public TextMeshProUGUI tutorialText;

    // EXPOSED PUBLIC PROPERTY
    public bool TutorialFinished { get; private set; }

    void OnEnable()
    {
        TutorialFinished = false;
        StartCoroutine(TutorialRoutine());
    }

    IEnumerator TutorialRoutine()
    {
        // Show the tutorial panel
        gameObject.SetActive(true);

        // Wait X seconds
        yield return new WaitForSeconds(displayTime);

        // Hide and flag as done
        gameObject.SetActive(false);
        TutorialFinished = true;
    }
}