using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
}

[System.Serializable]
public class HighScoreWrapper
{
    public List<HighScoreEntry> list;
    public HighScoreWrapper(List<HighScoreEntry> list) { this.list = list; }
}

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;

    private const string PlayerPrefsKey = "SportsBlitzHighScores";

    public List<HighScoreEntry> scores = new List<HighScoreEntry>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add a new score to the leaderboard
    public void AddScore(string name, int score)
    {
        scores.Add(new HighScoreEntry { playerName = name, score = score });

        // Sort highest → lowest
        scores.Sort((a, b) => b.score.CompareTo(a.score));

        // Keep only top 10
        if (scores.Count > 10)
            scores.RemoveRange(10, scores.Count - 10);

        SaveScores();
    }

    // Save using JSON
    public void SaveScores()
    {
        string json = JsonUtility.ToJson(new HighScoreWrapper(scores));
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
    }

    // Load high scores from PlayerPrefs
    public void LoadScores()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            scores = new List<HighScoreEntry>();
            return;
        }

        string json = PlayerPrefs.GetString(PlayerPrefsKey);
        HighScoreWrapper wrapper = JsonUtility.FromJson<HighScoreWrapper>(json);
        scores = wrapper.list;
    }
}