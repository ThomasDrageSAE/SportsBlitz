using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GolfHoleManager : MonoBehaviour
{
    public BoxCollider2D golfHole;
    public CircleCollider2D golfBall;
    [SerializeField] TextMeshProUGUI winLoseConditionText;
    [SerializeField] TextMeshProUGUI timerText;

    public float time = 30f;
    public bool gameOver = false;

    private void Update()
    {
        Timer();      
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.transform.CompareTag("Goal"))
        {
            Debug.Log("Ball is not in Hole");
        }
        else
        {
            winLoseConditionText.text = "Hole In One, You Win!!!"; // Message will display in console when Ball goes into hole.
            Debug.Log("Ball has gone into Hole!");
            gameOver = true;
            Time.timeScale = 0;
            return;
        }
       
    }

    public void Timer()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timerText.text = "Time: " + time;
        }
        else if (time < 0)
        {
            time = 0;
            winLoseConditionText.text = "Times Up, You Lose!!!"; // Message displays when time is up.
            winLoseConditionText.color = Color.red;
            gameOver = true;
            Time.timeScale = 0;
            Debug.Log("Times Up, You Lose!");
            return;
        }

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}
