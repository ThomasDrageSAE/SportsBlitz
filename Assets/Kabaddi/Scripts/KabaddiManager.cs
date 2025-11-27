using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class KabaddiManager : MonoBehaviour
{
    [SerializeField] private float time;
    public bool gameStart;
    public bool gameEnd;
    [SerializeField] private Text countdownTimer;
    [SerializeField] private float waitTime;
    private int countdownText;
    [SerializeField] private GameObject controlScreen;

    [SerializeField] MinigameManager manager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waitTime = 2;
        time = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            controlScreen.SetActive(true);
            waitTime -= Time.deltaTime;
        }

        if (waitTime <= 0)
        {
            controlScreen.SetActive(false);
            gameStart = true;
        }

       if (gameStart && time > 0 && !gameEnd)
        {
            time -= Time.deltaTime;
        }

       if (time <= 0)
        {
            Lose();  
        }

        countdownText = (int)time;

       countdownTimer.text = countdownText.ToString();
    }

    public void Lose()
    {
        gameEnd = true;
        /*loseScreen.SetActive(true);
        //lose
        Time.timeScale = 0;*/
        manager.Lose();
    }

    public void Win()
    {
        gameEnd = true;
        /*winScreen.SetActive(true);
        Time.timeScale = 0;*/
        manager.Win();
    }
    
}
