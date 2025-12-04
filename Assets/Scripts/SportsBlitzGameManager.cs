using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random; // ← NEW

public class SportsBlitzGameManager : MonoBehaviour
{
    public static SportsBlitzGameManager Instance;

    [Header("Stats")]
    public int gamesWon = 0;
    
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

    public void Update()
    {
        addHealth();
    }

    // 
    public void RegisterWin()
    {
        if (transitioning) return;
        gamesWon++;
        StartCoroutine(ReturnToTV());
    }

    public void RegisterLoss()
    {
        if (transitioning) return;
        
        health--;
        StartCoroutine(ReturnToTV());
    }

    // 
    public bool HasClearedRun()
    {
        return gamesWon >= targetWins;
    }

    public bool HasFailedRun()
    {
        return health <= 0;
    }

    // 
    IEnumerator ReturnToTV()
    {
        transitioning = true;
        yield return new WaitForSeconds(postGamePause);
        transitioning = false;
        SceneManager.LoadScene(tvSceneName);
    }

    //RANDOM ROTATION LOGIC

    void InitializeGameOrder()
    {
        remainingGameIndices = new List<int>();

        
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
        
        // differnt game cycle
        if (lastSceneIndex != -1 && remainingGameIndices.Count > 1 && remainingGameIndices[0] == lastSceneIndex)
        {
            int swapIndex = 1;
            int temp = remainingGameIndices[0];
            remainingGameIndices[0] = remainingGameIndices[swapIndex];
            remainingGameIndices[swapIndex] = temp;
        }
    }
    public void addHealth()
    {
        if (gamesWon == 5)
        {
            health++;
        }
        
        if (gamesWon == 10)
        {
            health++;
        }
    }

    public void StartNextGame()
    {
        if (miniGameScenes == null || miniGameScenes.Length == 0)
        {
            Debug.LogWarning("No mini games assigned!");
            return;
        }

        // new shuffle list
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
        health = 3;

        // reset rotation
        remainingGameIndices.Clear();
        lastSceneIndex = -1;

        SceneManager.LoadScene(tvSceneName);
    }
    public void ResetStats()
    {
        gamesWon = 0;
        health = 3;
    }

}
