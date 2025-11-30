using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GolfHoleManager : MonoBehaviour
{
    public BoxCollider2D golfHole;
    public CircleCollider2D golfBall;
    public TextMeshProUGUI winLoseConditionText;
    public TextMeshProUGUI timerText;

    public float time = 30f;

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
            timerText.text = "Time: " + time;
        }
        else if (time < 0)
        {
            time = 0;
            winLoseConditionText.text = "Times Up, You Lose!!!"; // Message displays when time is up.
            Debug.Log("Times Up, You Lose!");
            return;
        }
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.transform.CompareTag("Goal"))
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                timerText.text = "Time: " + time;
            }
            Debug.Log("Ball is not in Hole");
            return;
        }
        else
        {
            winLoseConditionText.text = "Hole In One, You Win!!!"; // Message will display in console when Ball goes into hole.
            Debug.Log("Ball has gone into Hole!"); 
            return;
        }
    }


}
