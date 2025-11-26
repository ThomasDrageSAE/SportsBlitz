using UnityEngine;
using TMPro;
using System.Collections;

public class RacingTrafficLight : MonoBehaviour
{
    [Header("Light Sprites")]
    public Sprite redSprite;
    public Sprite yellowSprite;
    public Sprite greenSprite;

    [Header("GO Text")]
    public TextMeshProUGUI goText;  // Shows “GO!”

    [Header("Timing")]
    public float redTime = 0.8f;
    public float yellowTime = 0.8f;
    public float greenTime = 0.6f;

    [Header("Audio (READY / SET / GO)")]
    public AudioSource audioSource;
    public AudioClip readySound; 
    public AudioClip setSound;
    public AudioClip goSound;

    public bool sequenceFinished { get; private set; }

    private SpriteRenderer sr;
    private MiniGameTutorial tutorial;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tutorial = FindObjectOfType<MiniGameTutorial>();

        if (goText != null)
            goText.gameObject.SetActive(false);

        StartCoroutine(WaitForTutorialThenStart());
    }

    IEnumerator WaitForTutorialThenStart()
    {
        if (tutorial != null)
            while (!tutorial.TutorialFinished)
                yield return null;

        yield return StartCoroutine(LightSequence());
    }

    IEnumerator LightSequence()
    {
        sequenceFinished = false;

        // READY (RED)
        sr.enabled = true;
        sr.sprite = redSprite;
        Play(readySound);
        yield return new WaitForSeconds(redTime);

        // SET (YELLOW)
        sr.sprite = yellowSprite;
        Play(setSound);
        yield return new WaitForSeconds(yellowTime);

        // GO (GREEN)
        sr.sprite = greenSprite;
        Play(goSound);
        yield return new WaitForSeconds(greenTime);

        // SHOW GO TEXT
        sr.enabled = false;

        if (goText != null)
        {
            goText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            goText.gameObject.SetActive(false);
        }

        sequenceFinished = true;
    }

    void Play(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}
