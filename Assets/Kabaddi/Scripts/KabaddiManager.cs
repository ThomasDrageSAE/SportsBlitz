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

    private MinigameManager minigameManager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waitTime = 2;
        time = 5;
        minigameManager = GameObject.Find("MinigameManager").GetComponent<MinigameManager>();
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
        minigameManager.Lose();
        gameEnd = true;
        /*loseScreen.SetActive(true);
        //lose
        Time.timeScale = 0;*/
        
    }

    public void Win()
    {
        minigameManager.Win();
        gameEnd = true;
        /*winScreen.SetActive(true);
        Time.timeScale = 0;*/
        
    }
    
}
