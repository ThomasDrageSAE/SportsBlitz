using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;   // ← NEW

public class SportsBlitzGameManager : MonoBehaviour
{
    public static SportsBlitzGameManager Instance;

    [Header("Stats")]
    public int gamesWon = 0;
    public int gamesLost = 0;
    public int health = 3;

    [Header("Goal")]
    public int targetWins = 5;

    [Header("Scenes")]
    public string tvSceneName = "TVScene";
    public string[] miniGameScenes;

    [Header("Timing")]
    public float postGamePause = 1.2f;

    private bool transitioning = false;

    // --- Random rotation state ---
    private List<int> remainingGameIndices = new List<int>(); // shuffled queue
    private int lastSceneIndex = -1; // last played minigame index

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (health <= 0) health = 3;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ---------------------------------------------------------
    public void RegisterWin()
    {
        if (transitioning) return;
        gamesWon++;
        StartCoroutine(ReturnToTV());
    }

    public void RegisterLoss()
    {
        if (transitioning) return;
        gamesLost++;
        health--;
        StartCoroutine(ReturnToTV());
    }

    // ---------------------------------------------------------
    public bool HasClearedRun()
    {
        return gamesWon >= targetWins;
    }

    public bool HasFailedRun()
    {
        return health <= 0;
    }

    // ---------------------------------------------------------
    IEnumerator ReturnToTV()
    {
        transitioning = true;
        yield return new WaitForSeconds(postGamePause);
        transitioning = false;
        SceneManager.LoadScene(tvSceneName);
    }

    // === RANDOM ROTATION LOGIC ======================================

    void InitializeGameOrder()
    {
        remainingGameIndices = new List<int>();

        // Fill with 0..N-1
        for (int i = 0; i < miniGameScenes.Length; i++)
            remainingGameIndices.Add(i);

        // Fisher–Yates shuffle
        for (int i = remainingGameIndices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = remainingGameIndices[i];
            remainingGameIndices[i] = remainingGameIndices[j];
            remainingGameIndices[j] = temp;
        }

        // Make sure we don't start the new cycle on the same scene as last time
        if (lastSceneIndex != -1 && remainingGameIndices.Count > 1 && remainingGameIndices[0] == lastSceneIndex)
        {
            int swapIndex = 1;
            int temp = remainingGameIndices[0];
            remainingGameIndices[0] = remainingGameIndices[swapIndex];
            remainingGameIndices[swapIndex] = temp;
        }
    }

    public void StartNextGame()
    {
        if (miniGameScenes == null || miniGameScenes.Length == 0)
        {
            Debug.LogWarning("No mini games assigned!");
            return;
        }

        // If we've used up the current shuffled list, build a new one
        if (remainingGameIndices == null || remainingGameIndices.Count == 0)
        {
            InitializeGameOrder();
        }

        // Take the next game in the shuffled queue
        int sceneIndex = remainingGameIndices[0];
        remainingGameIndices.RemoveAt(0);

        lastSceneIndex = sceneIndex;

        string sceneName = miniGameScenes[sceneIndex];
        SceneManager.LoadScene(sceneName);
    }

    // ---------------------------------------------------------
    public void ResetGame()
    {
        gamesWon = 0;
        gamesLost = 0;
        health = 3;

        // Optional: reset rotation when starting a fresh run
        remainingGameIndices.Clear();
        lastSceneIndex = -1;

        SceneManager.LoadScene(tvSceneName);
    }
}
