using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance;

    [Header("UI")]
    public GameObject pausePanel;

    [Header("Input")]
    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;

    void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (pausePanel != null)
                pausePanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pausePanel == null) return;

        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        if (pausePanel == null) return;

        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        // Unpause
        Time.timeScale = 1f;

        
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (SportsBlitzGameManager.Instance != null)
        {
            SportsBlitzGameManager.Instance.ResetStats();
        }

        // Stop all audio 
        var sounds = FindObjectsOfType<AudioSource>();
        foreach (var s in sounds)
            s.Stop();
        
        SceneManager.LoadScene(0);
    }


    public void QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}