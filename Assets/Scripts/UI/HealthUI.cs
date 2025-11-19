using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update()
    {
        int hp = SportsBlitzGameManager.Instance.health;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < hp ? fullHeart : emptyHeart;
        }
    }
}