using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class FencingGameManager : MonoBehaviour
{
    [Header("Key Prefabs (UI)")]
    public GameObject keyQPrefab;
    public GameObject keyWPrefab;
    public GameObject keyEPrefab;
    public GameObject keyRPrefab;

    [Header("Layout Group Container")]
    public Transform keySequencePanel;

    [Header("Intro UI")]
    public TextMeshProUGUI introText;
    public TextMeshProUGUI perfectText;

    [Header("Gameplay Settings")]
    public int sequenceLength = 6;
    public float introDelay = 1f;
    public float endAnimDelay = 1.2f;

    [Header("Animators")]
    public Animator playerAnimator;
    public Animator opponentAnimator;

    [Header("Camera")]
    public Camera mainCamera;
    public float zoomInSize = 3.5f;
    public float zoomSpeed = 4f;
    public float shakeMagnitude = 0.1f;
    public float shakeDuration = 0.2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip inputSuccess;
    public AudioClip inputFail;
    public AudioClip attackSound;
    public AudioClip fallSound;
    public AudioClip perfectSound;

    private List<KeyCode> sequence = new List<KeyCode>();
    private List<GameObject> spawnedKeys = new List<GameObject>();
    private int currentIndex = 0;
    private bool gameActive = false;
    private bool madeMistake = false;

    private MinigameManager manager;
    private float defaultCameraSize;
    private Vector3 cameraOriginalPos;

    void Start()
    {
        manager = FindObjectOfType<MinigameManager>();
        if (mainCamera == null) mainCamera = Camera.main;

        defaultCameraSize = mainCamera.orthographicSize;
        cameraOriginalPos = mainCamera.transform.position;

        if (perfectText != null)
            perfectText.gameObject.SetActive(false);

        StartCoroutine(StartIntroSequence());
    }

    IEnumerator StartIntroSequence()
    {
        introText.gameObject.SetActive(true);
        introText.text = "READY...";
        yield return new WaitForSeconds(introDelay);

        introText.text = "FENCE!";
        yield return new WaitForSeconds(0.8f);

        introText.gameObject.SetActive(false);

        GenerateSequence();
        DisplaySequence();

        manager.ResetTimer(); // global timer start
        gameActive = true;
    }

    void Update()
    {
        if (!gameActive) return;

        
        if (manager.GetRemainingTime() <= 0)
        {
            gameActive = false;
            StartCoroutine(LoseAnimation());
            return;
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(sequence[currentIndex]))
            {
                HighlightKey(spawnedKeys[currentIndex], Color.green);
                PlaySound(inputSuccess);
                currentIndex++;

                if (currentIndex >= sequence.Count)
                {
                    gameActive = false;
                    StartCoroutine(WinAnimation());
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) ||
                     Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R))
            {
                ResetSequenceProgress();
            }
        }
    }

    void ResetSequenceProgress()
    {
        PlaySound(inputFail);
        currentIndex = 0;
        madeMistake = true;

        // Reset all icon colors
        foreach (GameObject icon in spawnedKeys)
        {
            Image img = icon.GetComponent<Image>();
            if (img != null)
                img.color = Color.white;
        }

        // Flash red feedback
        StartCoroutine(FlashIcons(Color.red, 0.1f));
    }

    IEnumerator WinAnimation()
    {
        if (!madeMistake && perfectText != null)
            StartCoroutine(ShowPerfectPopup());

        playerAnimator.SetTrigger("Attack");
        opponentAnimator.SetTrigger("Fall");
        PlaySound(attackSound);

        yield return StartCoroutine(CameraZoomAndShake());
        PlaySound(fallSound);

        yield return new WaitForSeconds(endAnimDelay);
        manager.Win();
    }

    IEnumerator LoseAnimation()
    {
        opponentAnimator.SetTrigger("Attack");
        playerAnimator.SetTrigger("Fall");
        PlaySound(attackSound);

        yield return StartCoroutine(CameraZoomAndShake());
        PlaySound(fallSound);

        yield return new WaitForSeconds(endAnimDelay);
        manager.Lose();
    }

    IEnumerator CameraZoomAndShake()
    {
        float startSize = mainCamera.orthographicSize;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * zoomSpeed;
            mainCamera.orthographicSize = Mathf.Lerp(startSize, zoomInSize, elapsed);
            yield return null;
        }

        float shakeEnd = Time.time + shakeDuration;
        while (Time.time < shakeEnd)
        {
            Vector3 offset = Random.insideUnitCircle * shakeMagnitude;
            mainCamera.transform.position = cameraOriginalPos + offset;
            yield return null;
        }

        mainCamera.transform.position = cameraOriginalPos;
        mainCamera.orthographicSize = startSize;
    }

    void GenerateSequence()
    {
        KeyCode[] keys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
        sequence.Clear();

        for (int i = 0; i < sequenceLength; i++)
            sequence.Add(keys[Random.Range(0, keys.Length)]);
    }

    void DisplaySequence()
    {
        foreach (Transform child in keySequencePanel)
            Destroy(child.gameObject);

        spawnedKeys.Clear();

        foreach (var key in sequence)
        {
            GameObject prefab = GetPrefabForKey(key);
            GameObject icon = Instantiate(prefab, keySequencePanel);
            icon.SetActive(true);
            spawnedKeys.Add(icon);
        }
    }

    GameObject GetPrefabForKey(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Q: return keyQPrefab;
            case KeyCode.W: return keyWPrefab;
            case KeyCode.E: return keyEPrefab;
            case KeyCode.R: return keyRPrefab;
            default: return keyQPrefab;
        }
    }

    void HighlightKey(GameObject icon, Color color)
    {
        Image img = icon.GetComponent<Image>();
        if (img != null)
            img.color = color;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    IEnumerator ShowPerfectPopup()
    {
        perfectText.gameObject.SetActive(true);
        perfectText.color = new Color(1, 1, 1, 1);
        perfectText.transform.localScale = Vector3.one * 0.6f;
        PlaySound(perfectSound);

        float duration = 1f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float scale = Mathf.Lerp(0.6f, 1.2f, t);
            perfectText.transform.localScale = Vector3.one * scale;

            float alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01((t - 0.6f) * 2f));
            perfectText.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        perfectText.gameObject.SetActive(false);
    }

    IEnumerator FlashIcons(Color flashColor, float duration)
    {
        foreach (GameObject icon in spawnedKeys)
        {
            Image img = icon.GetComponent<Image>();
            if (img != null) img.color = flashColor;
        }

        yield return new WaitForSeconds(duration);

        foreach (GameObject icon in spawnedKeys)
        {
            Image img = icon.GetComponent<Image>();
            if (img != null) img.color = Color.white;
        }
    }
}
