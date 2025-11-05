using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    public TMP_Text statsText;

    void Update()
    {
        statsText.text = 
            $"Wins: {SportsBlitzGameManager.Instance.gamesWon}\n" +
            $"Losses: {SportsBlitzGameManager.Instance.gamesLost}";
    }
}