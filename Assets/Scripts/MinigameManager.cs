using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public void Win()
    {
        SportsBlitzGameManager.Instance.RegisterWin();
    }

    public void Lose()
    {
        SportsBlitzGameManager.Instance.RegisterLoss();
    }
}