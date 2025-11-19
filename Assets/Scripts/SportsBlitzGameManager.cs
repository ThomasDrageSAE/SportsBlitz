using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SportsBlitzGameManager : MonoBehaviour
{
    public static SportsBlitzGameManager Instance;

    [Header("Stats")]
    public int gamesWon = 0;
    public int gamesLost = 0;
    public int health = 3;

    [Header("Scenes")]
    public string tvSceneName = "TVScene";
    public string[] miniGameScenes; 

    [Header("Timing")]
    public float postGamePause = 1.2f;

    private bool transitioning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (health <= 0) health = 3;
        }
        else Destroy(gameObject);
    }

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

        if (IsGameOver())
        {
            SceneManager.LoadScene("GameOverScene"); 
        }
        else
        {
            StartCoroutine(ReturnToTV());
        }
    }

    public bool IsGameOver()
    {
        return health <= 0;
    }

    IEnumerator ReturnToTV()
    {
        transitioning = true;
        yield return new WaitForSeconds(postGamePause);
        transitioning = false;
        SceneManager.LoadScene(tvSceneName);
    }

    public void StartNextGame()
    {
        if (miniGameScenes.Length == 0)
        {
            Debug.LogWarning("No mini games assigned!");
            return;
        }

        string scene = miniGameScenes[Random.Range(0, miniGameScenes.Length)];
        SceneManager.LoadScene(scene);
    }

    public void ResetGame()
    {
        gamesWon = 0;
        gamesLost = 0;
        health = 3;
        SceneManager.LoadScene(tvSceneName);
    }
}
