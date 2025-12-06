using UnityEngine;
using TMPro;

public class HighScoreUI : MonoBehaviour
{
    public GameObject panel;
    public Transform scoreListParent;
    public TextMeshProUGUI scoreEntryPrefab;

    public TMP_InputField nameInput;
    public TextMeshProUGUI resultTitle;

    public UnityEngine.UI.Button submitButton;   // NEW
    public GameObject credits;

    private int finalScore;

    public void Show(int score, bool didWin)
    {
        finalScore = score;

        panel.SetActive(true);
        nameInput.text = "";

        // enable submit when the panel opens for a new run
        if (submitButton != null)
        {
            submitButton.interactable = true;
            submitButton.gameObject.SetActive(true);
        }

        resultTitle.text = didWin ? "You Win!" : "GAME OVER";

        RefreshScoreList();
    }

    public void SubmitScore()
    {
        string name = string.IsNullOrEmpty(nameInput.text) ? "AAA" : nameInput.text;

        Debug.Log($"Submitting score: {name} — {finalScore}");

        HighScoreManager.Instance.AddScore(name, finalScore);

        // hide submit button after score submission
        if (submitButton != null)
        {
            submitButton.interactable = false;
            submitButton.gameObject.SetActive(false); // hide entirely
        }

        RefreshScoreList();
    }

    public void ContinueButton()
    {
        panel.gameObject.SetActive(false);
        credits.SetActive(true);
    }
    public void CloseAndRestart()
    {
        panel.SetActive(false);
        SportsBlitzGameManager.Instance.ResetGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("TVScene");
    }

    public void RefreshScoreList()
    {
        // Clear list
        foreach (Transform child in scoreListParent)
            Destroy(child.gameObject);

        // Show top 10 scores
        int count = Mathf.Min(HighScoreManager.Instance.scores.Count, 10);

        for (int i = 0; i < count; i++)
        {
            var entry = HighScoreManager.Instance.scores[i];
            var row = Instantiate(scoreEntryPrefab, scoreListParent);

            row.gameObject.SetActive(true);
            row.text = $"{entry.playerName} — {entry.score}";
        }
    }
}
